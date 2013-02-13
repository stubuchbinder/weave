// FlyingText3D 1.2.2
// ©2012 Starscene Software. All rights reserved. Redistribution without permission not allowed.

using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

namespace FlyingText3D {

public class TTFFontInfo {
	const byte ON_CURVE = (1 << 0);
	const byte X_SHORT =  (1 << 1);
	const byte Y_SHORT =  (1 << 2);
	const byte REPEAT =   (1 << 3);
	const byte X_SAME =   (1 << 4);
	const byte Y_SAME =   (1 << 5);
	
	const byte HORIZONTAL =    (1 << 0);
	const byte MINIMUM =       (1 << 1);
	const byte CROSS_STREAM =  (1 << 2);
	const byte OVERRIDE =      (1 << 3);
	const byte KERN_VERTICAL = (1 << 7);
	
	const ushort ARG_1_AND_2_ARE_WORDS =    (1 << 0);
	const ushort ARGS_ARE_XY_VALUES =       (1 << 1);
	const ushort ROUND_XY_TO_GRID =         (1 << 2);
	const ushort WE_HAVE_A_SCALE =          (1 << 3);
	const ushort MORE_COMPONENTS =          (1 << 5);
	const ushort WE_HAVE_AN_X_AND_Y_SCALE = (1 << 6);
	const ushort WE_HAVE_A_TWO_BY_TWO =     (1 << 7);
	const ushort WE_HAVE_INSTRUCTIONS =     (1 << 8);
	const ushort USE_MY_METRICS =           (1 << 9);
	
	private byte[] _ttfData;
	private bool _isAvailable;
	private string _name;
	public string name {
		get {return _name;}
	}
	private int _unitsPerEm;
	public int unitsPerEm {
		get {return _unitsPerEm;}
	}
	private int _ascent;
	private int _descent;
	private int _lineHeight;
	public int lineHeight {
		get {return _lineHeight;}
	}
	private uint _glyphDataOffset;
	private int _cmapFormat;
	private int[] _glyphIndexArray;
	private int[] _endCodeArray;
	private int[] _startCodeArray;
	private int[] _idDeltaArray;
	private int[] _idRangeOffsetArray;
	private int[] _glyphIdArray;
	private uint[] _locationIndexArray;
	private int[] _advanceArray;
	public int[] advanceArray {
		get {return _advanceArray;}
	}	
	private bool _hasKerning;
	public bool hasKerning {
		get {return _hasKerning;}
	}
	private Dictionary<KernPair, short> _kernDictionary;
	public Dictionary<KernPair, short> kernDictionary {
		get {return _kernDictionary;}
	}
	private Dictionary<char, GlyphData> _glyphDictionary;
	public Dictionary<char, GlyphData> glyphDictionary {
		get {return _glyphDictionary;}
	}
	
	public bool SetGlyphData (char character) {
		if (!_isAvailable) {
			Debug.LogError ("Font information not available");
			return false;
		}
		
		if (_glyphDictionary.ContainsKey (character)) return true;

		int charCode = Convert.ToInt32 (character);
		try {
			uint idx = _glyphDataOffset;
			int glyphIndex = 0;
			if (_cmapFormat == 4) {
				int end = _endCodeArray.Length;
				for (int i = 0; i < end; i++) {
					if (_endCodeArray[i] >= charCode && _startCodeArray[i] <= charCode) {
						if (_idRangeOffsetArray[i] == 0) {
							glyphIndex = _idDeltaArray[i] + charCode;
						}
						else {
							// A bit complicated since the offset is the number of bytes away from the current actual location in
							// _idRangeOffsetArray itself, which "overflows" into _glyphIdArray, since _glyphIdArray immediately follows
							// _idRangeOffsetArray in the truetype file.
							int glyphIdIndex = ((_idRangeOffsetArray[i] + 2 * (charCode - _startCodeArray[i])) / 2) - (end - i);
							glyphIndex = _glyphIdArray[glyphIdIndex];
						}
						break;
					}
				}
			}
			else if (_cmapFormat == 6) {
				glyphIndex = _glyphIndexArray[charCode];
			}
			else if (_cmapFormat == 0) {
				if (charCode > 255) {
					Debug.LogWarning ("Character code " + charCode + " (" + Convert.ToChar(charCode) + ") not found in font");
					_glyphDictionary.Add (character, new GlyphData(null, null, 0, 0, 0, 0, _unitsPerEm, glyphIndex));
					return true;
				}
				glyphIndex = _glyphIndexArray[charCode];
			}
			
			// Space and null characters; don't bother with any glyphs that exist for these
			if (glyphIndex >= 1 && glyphIndex <= 3) {
				_glyphDictionary.Add (character, new GlyphData(null, null, 0, 0, 0, 0, _unitsPerEm, glyphIndex));
				return true;
			}
			
			idx += _locationIndexArray[glyphIndex];
			int numberOfContours = GetShort (_ttfData, ref idx);
			int xMin = GetShort (_ttfData, ref idx);
			int yMin = GetShort (_ttfData, ref idx);
			int xMax = GetShort (_ttfData, ref idx);
			int yMax = GetShort (_ttfData, ref idx);
			
			// Simple glyph
			if (numberOfContours > 0) {
				var pointsList = new List<Vector2[]>(numberOfContours);
				var onCurvesList = new List<bool[]>(numberOfContours);
				ReadGlyphData (idx, numberOfContours, pointsList, onCurvesList, 0, 0);
				_glyphDictionary.Add (character, new GlyphData(pointsList, onCurvesList, xMin, yMin, xMax, yMax, _unitsPerEm, glyphIndex));
				return true;
			}
			// Compound glpyh
			else if (numberOfContours < 0) {
				ushort flags;
				uint glyphIdx = idx;
				var pointsList = new List<Vector2[]>();
				var onCurvesList = new List<bool[]>();
				int useGlyphIndex = glyphIndex;
				int xValue = 0, yValue = 0;
//				int point1, point2;
				
				do {
					flags = GetUshort (_ttfData, ref glyphIdx);
					glyphIndex = GetUshort (_ttfData, ref glyphIdx);
					if ((flags & USE_MY_METRICS) != 0) {
						useGlyphIndex = glyphIndex;
					}
					
					if ((flags & ARG_1_AND_2_ARE_WORDS) != 0) {
						if ((flags & ARGS_ARE_XY_VALUES) != 0) {
							xValue = GetShort (_ttfData, ref glyphIdx);
							yValue = GetShort (_ttfData, ref glyphIdx);
						}
						else {
//							Debug.Log ("point");
//							point1 = GetUshort (_ttfData, ref glyphIdx);
//							point2 = GetUshort (_ttfData, ref glyphIdx);
						}
					}
					else {
						if ((flags & ARGS_ARE_XY_VALUES) != 0) {
							xValue = (sbyte)_ttfData[glyphIdx++];
							yValue = (sbyte)_ttfData[glyphIdx++];
						}
						else {
//							Debug.Log ("point");
//							point1 = _ttfData[glyphIdx++];
//							point2 = _ttfData[glyphIdx++];
						}
					}
					
					if ((flags & WE_HAVE_A_SCALE) != 0) {
//						Debug.Log ("scale");
						glyphIdx += 2;
					}
					else if ((flags & WE_HAVE_AN_X_AND_Y_SCALE) != 0) {
//						Debug.Log ("x and y scale");
						glyphIdx += 4;
					}
					else if ((flags & WE_HAVE_A_TWO_BY_TWO) != 0) {
//						Debug.Log ("2 x 2");
						glyphIdx += 8;
					}
					
					idx = _glyphDataOffset + _locationIndexArray[glyphIndex];
					numberOfContours = GetShort (_ttfData, ref idx);
					idx += 8;
					ReadGlyphData (idx, numberOfContours, pointsList, onCurvesList, xValue, yValue);
					
				} while ((flags & MORE_COMPONENTS) != 0);
				
				_glyphDictionary.Add (character, new GlyphData(pointsList, onCurvesList, xMin, yMin, xMax, yMax, _unitsPerEm, useGlyphIndex));
				return true;
			}
			// No contours
			else {
				_glyphDictionary.Add (character, new GlyphData(null, null, 0, 0, 0, 0, _unitsPerEm, glyphIndex));
				return true;
			}
		}
		catch (Exception err) {
			Debug.LogError ("charCode: " + charCode + " (" + character + "), " + err.Message);
			return false;
		}
	}
	
	private void ReadGlyphData (uint idx, int numberOfContours, List<Vector2[]> pointsList, List<bool[]> onCurvesList, int xOffset, int yOffset) {
		var endPtsOfContours = GetUshortArray (_ttfData, ref idx, numberOfContours);
		uint instructionLength = GetUshort (_ttfData, ref idx);
		idx += instructionLength;	// Skip instructions
		
		// Flags array
		int numberOfPoints = endPtsOfContours[numberOfContours-1] + 1;
		var allOnCurveList = new List<bool>(numberOfPoints);
		var flagList = new List<byte>(numberOfPoints);
		int pointCount = 0;
		while (pointCount < numberOfPoints) {
			byte thisByte = _ttfData[idx++];
			flagList.Add (thisByte);
			allOnCurveList.Add ((thisByte & ON_CURVE) != 0);
			pointCount++;
			if ((thisByte & REPEAT) != 0) {
				int repeatCount = _ttfData[idx++];
				for (int i = 0; i < repeatCount; i++) {
					flagList.Add (thisByte);
					allOnCurveList.Add ((thisByte & ON_CURVE) != 0);
					pointCount++;
				}
			}
		}
		bool[] allOnCurves = allOnCurveList.ToArray();
		byte[] flags = flagList.ToArray();
		
		var allCoords = new Vector2[numberOfPoints];
		// X coords
		int currentPos = 0;
		for (int i = 0; i < numberOfPoints; i++) {
			int x = 0;
			bool xShort = ((flags[i] & X_SHORT) != 0);
			bool xSame = ((flags[i] & X_SAME) != 0);
			
			if (xShort) {
				x = xSame? _ttfData[idx++] : -_ttfData[idx++];
			}
			else if (!xSame) {
				x = GetShort (_ttfData, ref idx);
			}
			
			currentPos += x;
			allCoords[i].x = currentPos + xOffset;
		}
		// Y coords
		currentPos = 0;
		for (int i = 0; i < numberOfPoints; i++) {
			int y = 0;
			bool yShort = ((flags[i] & Y_SHORT) != 0);
			bool ySame = ((flags[i] & Y_SAME) != 0);
			
			if (yShort) {
				y = ySame? _ttfData[idx++] : -_ttfData[idx++];
			}
			else if (!ySame) {
				y = GetShort (_ttfData, ref idx);
			}
			
			currentPos += y;
			allCoords[i].y = currentPos + yOffset;
		}
		
		if (numberOfContours > 1) {
			// All contours are in one array, so split that into arrays for each contour
			int start = 0;
			for (int i = 0; i < numberOfContours; i++) {
				int coordsLength = endPtsOfContours[i] + 1 - start;
				var coords = new Vector2[coordsLength];
				var onCurves = new bool[coordsLength];
				Array.Copy (allCoords, start, coords, 0, coordsLength);
				Array.Copy (allOnCurves, start, onCurves, 0, coordsLength);
				
				pointsList.Add (coords);
				onCurvesList.Add (onCurves);
				start = endPtsOfContours[i] + 1;
			}
		}
		else {
			// One contour, so no need to split anything
			pointsList.Add (allCoords);
			onCurvesList.Add (allOnCurves);
		}
	}
	
	public TTFFontInfo (byte[] ttfData) {
		_ttfData = ttfData;
		_glyphDictionary = new Dictionary<char, GlyphData>();
		
		try {
			if (_ttfData.Length < 10) {
				throw new Exception ("No data found in file");
			}
			uint idx = 0;
			uint length = 0;
			var id = GetUint (_ttfData, ref idx);
			if (id != 0x00010000 && id != 0x74727565) {
				throw new Exception ("Not a supported TTF file");
			}
			bool macTTF = (id == 0x74727565);
			
			// Name table, get font name
			if (!TagSearch (_ttfData, "name", ref idx, ref length)) {
				throw new Exception ("name tag not found");
			}
			_name = GetName (_ttfData, ref idx);
			if (_name == "") {
				throw new Exception ("Name not retrieved");
			}
			
			// Cmap table, get glyph index array
			if (!TagSearch (_ttfData, "cmap", ref idx, ref length)) {
				throw new Exception ("cmap tag not found");
			}
			uint baseIdx = idx;
			idx += 2;
			int cmapSubtables = GetUshort (_ttfData, ref idx);
			var platformIds = new int[cmapSubtables];
			var platformSpecificIds = new int[cmapSubtables];
			var cmapOffsets = new uint[cmapSubtables];
			for (int i = 0; i < cmapSubtables; i++) {
				platformIds[i] = GetUshort (_ttfData, ref idx);
				platformSpecificIds[i] = GetUshort (_ttfData, ref idx);
				cmapOffsets[i] = GetUint (_ttfData, ref idx);
			}
			
			var gotIndexArray = false;
			int unicodeIndex = Array.IndexOf (platformIds, 0);
			if (unicodeIndex != -1 && platformSpecificIds[unicodeIndex] == 3) {
				idx = baseIdx + cmapOffsets[unicodeIndex];
				int format = GetUshort (_ttfData, ref idx);
				// Format 4
				if (format == 4) {
					gotIndexArray = true;
					SetFormat4 (ref idx);
				}
			}
			if (!gotIndexArray) {
				int winIndex = Array.IndexOf (platformIds, 3);
				if (winIndex != -1 && platformSpecificIds[winIndex] == 1) {
					idx = baseIdx + cmapOffsets[winIndex];
					int format = GetUshort (_ttfData, ref idx);
					// Format 4
					if (format == 4) {
						gotIndexArray = true;
						SetFormat4 (ref idx);
					}
				}
			}
			if (!gotIndexArray) {
				int macIndex = Array.IndexOf (platformIds, 1);
				if (macIndex != -1 && platformSpecificIds[macIndex] == 0) {
					idx = baseIdx + cmapOffsets[macIndex];
					int format = GetUshort (_ttfData, ref idx);
					// Format 0
					if (format == 0) {
						gotIndexArray = true;
						_cmapFormat = 0;
						_glyphIndexArray = new int[256];
						int j = 0;
						for (uint i = idx+4; i < idx+260; i++) {
							_glyphIndexArray[j++] = _ttfData[i];
						}
					}
					// Format 6
					else if (format == 6) {
						gotIndexArray = true;
						_cmapFormat = 6;
						idx += 4;
						ushort firstCode = GetUshort (_ttfData, ref idx);
						int entryCount = GetUshort (_ttfData, ref idx);
						_glyphIndexArray = new int[entryCount];
						for (int i = 0; i < entryCount; i++) {
							_glyphIndexArray[i] = GetUshort (_ttfData, ref idx) - firstCode;
						}
					}
				}
			}
			if (!gotIndexArray) {
				throw new Exception ("Didn't get cmap index array");
			}
			
			// Head table, get units per em and indexToLocFormat (short or long glyph data offset)
			if (!TagSearch (_ttfData, "head", ref idx, ref length)) {
				throw new Exception ("head tag not found");
			}
			idx += 18;
			_unitsPerEm = GetUshort (_ttfData, ref idx);
			idx += 30;
			int indexToLocFormat = GetShort (_ttfData, ref idx);
			
			// Maxp table, get number of glyphs
			if (!TagSearch (_ttfData, "maxp", ref idx, ref length)) {
				throw new Exception ("maxp tag not found");
			}
			idx += 4;
			int numGlyphs = GetUshort (_ttfData, ref idx);
			
			// Loca table, get glyph location indices
			_locationIndexArray = new uint[numGlyphs];
 			if (!TagSearch (_ttfData, "loca", ref idx, ref length)) {
				throw new Exception ("loca tag not found");
			}
			if (indexToLocFormat == 0) {
				for (int i = 0; i < numGlyphs; i++) {
					_locationIndexArray[i] = (uint)(GetUshort (_ttfData, ref idx) * 2);
				}
			}
			else {
				for (int i = 0; i < numGlyphs; i++) {
					_locationIndexArray[i] = GetUint (_ttfData, ref idx);
				}
			}
			
			// Glyf table, just need to get the byte offset to look up glyph indices elsewhere (_glyphDataOffset)
			if (!TagSearch (_ttfData, "glyf", ref _glyphDataOffset, ref length)) {
				throw new Exception ("glyf tag not found");
			}
			
			// Hhea table, get possibly wrong ascent/descent/lineGap values, plus the number of horizontal metrics
			if (!TagSearch (_ttfData, "hhea", ref idx, ref length)) {
				throw new Exception ("hhea tag not found");
			}
			idx += 4;	// Skip version
			_ascent = GetShort (_ttfData, ref idx);
			_descent = GetShort (_ttfData, ref idx);
			int lineGap = GetShort (_ttfData, ref idx);
			idx += 24;
			int numOfLongHorMetrics = GetShort (_ttfData, ref idx);
			
			// OS/2 table, if long enough, to get correct ascent/descent/lineGap values
			if (TagSearch (_ttfData, "OS/2", ref idx, ref length)) {
				if (length > 68) {
					idx += 68;
					_ascent = GetShort (_ttfData, ref idx);
					_descent = GetShort (_ttfData, ref idx);
					lineGap = GetShort (_ttfData, ref idx);
				}
			}
			
			_lineHeight = _ascent - _descent + lineGap;
			
			// Hmtx table, horizontal metrics
			if (!TagSearch (_ttfData, "hmtx", ref idx, ref length)) {
				throw new Exception ("hmtx tag not found");
			}
			if (numOfLongHorMetrics > 10) {
				_advanceArray = new int[numOfLongHorMetrics];
				for (int i = 0; i < numOfLongHorMetrics; i++) {
					_advanceArray[i] = GetUshort (_ttfData, ref idx);
					idx += 2;	// Skip left side bearing
				}
			}
			// Monospaced font
			else {
				_advanceArray = new int[numGlyphs];
				int advanceWidth = GetUshort (_ttfData, ref idx);
				for (int i = 0; i < numGlyphs; i++) {
					_advanceArray[i] = advanceWidth;
				}
			}
			
			// Kern table, if it exists
			_hasKerning = false;
			if (TagSearch (_ttfData, "kern", ref idx, ref length)) {
				var horizontal = false;
				byte format = 0;
				if (macTTF) {
					idx += 4;	// Skip version
					int subtables = (int)GetUint (_ttfData, ref idx);
					for (int i = 0; i < subtables; i++) {
						length = GetUint (_ttfData, ref idx);
						byte coverage = _ttfData[idx++];
						format = _ttfData[idx++];
						idx += 2;
						if (format == 0) {
							if ((coverage & KERN_VERTICAL) == 0) {
								horizontal = true;
							}
							break;
						}
						idx += length - 8;
					}
				}
				else {
					idx += 2;	// Skip version
					int subtables = GetUshort (_ttfData, ref idx);
					for (int i = 0; i < subtables; i++) {
						idx += 2;	// Skip version
						length = GetUshort (_ttfData, ref idx);
						format = _ttfData[idx++];
						byte coverage = _ttfData[idx++];
						if (format == 0) {
							if ((coverage & MINIMUM) == 0) {
								horizontal = true;
							}
							break;
						}
						idx += length - 6;
					}
				}
				if (format == 0 && horizontal) {
					_hasKerning = true;
					int nPairs = GetUshort (_ttfData, ref idx);
					idx += 6;
					_kernDictionary = new Dictionary<KernPair, short>(nPairs);
					for (var i = 0; i < nPairs; i++) {
						_kernDictionary.Add (new KernPair (GetUshort (_ttfData, ref idx), GetUshort (_ttfData, ref idx)), GetShort (_ttfData, ref idx));
					}
				}
			}			
		}
		catch (Exception err) {
			Debug.LogError (err.Message);
			_isAvailable = false;
			return;
		}
		_isAvailable = true;
	}
	
	private void SetFormat4 (ref uint idx) {
		_cmapFormat = 4;
		int subtableLength = GetUshort (_ttfData, ref idx);
		idx += 2;	// Skip language
		int segCount = GetUshort (_ttfData, ref idx) / 2;
		idx += 6;	// Skip stuff that could be computed trivially anyway
		_endCodeArray = GetUshortToIntArray (_ttfData, ref idx, segCount);
		idx += 2;	// Always 0
		_startCodeArray = GetUshortToIntArray (_ttfData, ref idx, segCount);
		_idDeltaArray = GetShortToIntArray (_ttfData, ref idx, segCount);
		_idRangeOffsetArray = GetUshortToIntArray (_ttfData, ref idx, segCount);
		int glyphIdArrayLength = (subtableLength - (16 + 4*(segCount*2))) / 2;
		_glyphIdArray = GetUshortToIntArray (_ttfData, ref idx, glyphIdArrayLength);
	}
	
	public static string GetFontName (byte[] fontBytes) {
		uint idx = 0;
		uint length = 0;
		var id = GetUint (fontBytes, ref idx);
		if (id != 0x00010000 && id != 0x74727565) {
			return "Not a supported TTF file";
		}
		if (TagSearch (fontBytes, "name", ref idx, ref length)) {
			return GetName (fontBytes, ref idx);
		}
		return "";
	}
	
	private static string GetName (byte[] ttfData, ref uint idx) {
		uint baseIdx = idx;
		idx += 2;
		int count = GetUshort (ttfData, ref idx);
		uint stringOffset = GetUshort (ttfData, ref idx) + baseIdx;
		string name = "";
		for (int i = 0; i < count; i++) {
			int platformID = GetUshort (ttfData, ref idx);
			int platformSpecificID = GetUshort (ttfData, ref idx);
			idx += 2;	// Skip language
			int nameID = GetUshort (ttfData, ref idx);
			int stringLength = GetUshort (ttfData, ref idx);
			int offset = GetUshort (ttfData, ref idx);
			if (nameID == 4 && platformID == 1 && platformSpecificID == 0) {
				idx = (uint)(stringOffset + offset);
				name = new ASCIIEncoding().GetString (GetByteArray (ttfData, ref idx, stringLength));
				break;
			}
			else if (nameID == 4 && platformID == 3 && platformSpecificID == 1) {
				idx = (uint)(stringOffset + offset);
				name = new UnicodeEncoding().GetString (GetLittleEndianByteArray (ttfData, ref idx, stringLength));
				break;
			}
		}
		return name;
	}
	
	// Search font directory for a specific tag and get the byte offset and length for it
	private static bool TagSearch (byte[] ttfData, string tag, ref uint offset, ref uint length) {
		uint idx = 4;
		int numberOfTables = GetUshort (ttfData, ref idx);
		idx = 12;
		for (int i = 0; i < numberOfTables; i++) {
			if (GetTag (ttfData, ref idx) == tag) {
				idx += 4;
				offset = GetUint (ttfData, ref idx);
				length = GetUint (ttfData, ref idx);
				return true;
			}
			idx += 12;
		}
		return false;
	}
	
	private static string GetTag (byte[] ttfData, ref uint idx) {
		if (idx < ttfData.Length) {
			char[] chars = {(char)ttfData[idx], (char)ttfData[idx+1], (char)ttfData[idx+2], (char)ttfData[idx+3]};
			idx += 4;
			return new string(chars);
		}
		return null;
	}
	
	private static uint GetUint (byte[] ttfData, ref uint idx) {
		uint number = (uint)((ttfData[idx]<<24) | (ttfData[idx+1]<<16) | (ttfData[idx+2]<<8) | ttfData[idx+3]);
		idx += 4;
		return number;
	}

	private static ushort GetUshort (byte[] ttfData, ref uint idx) {
		ushort number = (ushort)((ttfData[idx]<<8) | ttfData[idx+1]);
		idx += 2;
		return number;
	}

	private static short GetShort (byte[] ttfData, ref uint idx) {
		short number = (short)((ttfData[idx]<<8) | ttfData[idx+1]);
		idx += 2;
		return number;
	}
	
	private static ushort[] GetUshortArray (byte[] ttfData, ref uint idx, int arrayLength) {
		var array = new ushort[arrayLength];
		for (int i = 0; i < arrayLength; i++) {
			array[i] = (ushort)((ttfData[idx]<<8) | ttfData[idx+1]);
			idx += 2;
		}
		return array;
	}
	
	private static int[] GetUshortToIntArray (byte[] ttfData, ref uint idx, int arrayLength) {
		var array = new int[arrayLength];
		for (int i = 0; i < arrayLength; i++) {
			array[i] = (ushort)((ttfData[idx]<<8) | ttfData[idx+1]);
			idx += 2;
		}
		return array;
	}

	private static int[] GetShortToIntArray (byte[] ttfData, ref uint idx, int arrayLength) {
		var array = new int[arrayLength];
		for (int i = 0; i < arrayLength; i++) {
			array[i] = (short)((ttfData[idx]<<8) | ttfData[idx+1]);
			idx += 2;
		}
		return array;
	}

	private static short[] GetShortArray (byte[] ttfData, ref uint idx, int arrayLength) {
		var array = new short[arrayLength];
		for (int i = 0; i < arrayLength; i++) {
			array[i] = (short)((ttfData[idx]<<8) | ttfData[idx+1]);
			idx += 2;
		}
		return array;
	}

	private static byte[] GetByteArray (byte[] ttfData, ref uint idx, int arrayLength) {
		var array = new byte[arrayLength];
		Array.Copy (ttfData, idx, array, 0, arrayLength);
		idx += (uint)arrayLength;
		return array;
	}

	private static byte[] GetLittleEndianByteArray (byte[] ttfData, ref uint idx, int arrayLength) {
		var array = new byte[arrayLength];
		for (int i = 0; i < arrayLength; i += 2) {
			array[i+1] = ttfData[idx];
			array[i] = ttfData[idx+1];
			idx += 2;
		}
		return array;
	}
}

}