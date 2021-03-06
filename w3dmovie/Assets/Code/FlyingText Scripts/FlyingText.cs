// FlyingText3D 1.2.2
// ©2012 Starscene Software. All rights reserved. Redistribution without permission not allowed.

using UnityEngine;
using System;
using System.Globalization;
using System.Collections.Generic;
using FlyingText3D;

[AddComponentMenu("FlyingText3D/FlyingText")]
public class FlyingText : MonoBehaviour {
		
	public List<FontData> m_fontData;
	public int m_defaultFont = 0;
	public Material m_defaultMaterial;
	public Color m_defaultColor = Color.white;
	public int m_defaultResolution = 5;
	public float m_defaultSize = 2.0f;
	public float m_defaultDepth = 0.25f;
	public float m_defaultLetterSpacing = 1.0f;
	public float m_defaultLineSpacing = 1.0f;
	public Justify m_defaultJustification = Justify.Left;
	public bool m_includeBackface = true;
	public bool m_texturePerLetter = false;
	public TextAnchor m_anchor = TextAnchor.UpperLeft;
	public ZAnchor m_zAnchor = ZAnchor.Front;
	public ColliderType m_colliderType = ColliderType.None;
	public bool m_addRigidbodies = false;
	public PhysicMaterial m_physicsMaterial;
	public float m_smoothingAngle = 50.0f;
	
	public static int defaultFont;
	public static Material defaultMaterial;
	public static Color defaultColor;
	public static int defaultResolution;
	public static float defaultSize;
	public static float defaultDepth;
	public static float defaultLetterSpacing;
	public static float defaultLineSpacing;
	public static Justify defaultJustification;
	public static bool includeBackface;
	public static bool texturePerLetter;
	public static TextAnchor anchor;
	public static ZAnchor zAnchor;
	public static ColliderType colliderType;
	public static bool addRigidbodies;
	public static PhysicMaterial physicsMaterial;
	public static float smoothingAngle;
	
	private static FlyingText _instance;
	public static FlyingText instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType(typeof (FlyingText)) as FlyingText;
			}
			return _instance;
		}
	}
	private static bool _initialized = false;
	private static TTFFontInfo[] _fontInfo;
	private static string[] _fontNames;
	private static char[] _removeChars = {' ', '\n', '\r', '"', '\'', '\t'};
	private static Dictionary<string, Color> _colorDictionary;
	
	private const int PREPROCESS = 2;
	private const int BUILDMESH = 1;
	
	private void Awake () {
		if (FindObjectsOfType (typeof(FlyingText)).Length > 1) {
			Destroy (this);
			return;
		}
		if (!_initialized) {
			Initialize();
		}
	}
	
	public void Initialize () {
		defaultFont = m_defaultFont;
		defaultMaterial = m_defaultMaterial;
		defaultColor = m_defaultColor;
		defaultResolution = m_defaultResolution;
		defaultSize = m_defaultSize;
		defaultDepth = m_defaultDepth;
		defaultLetterSpacing = m_defaultLetterSpacing;
		defaultLineSpacing = m_defaultLineSpacing;
		defaultJustification = m_defaultJustification;
		includeBackface = m_includeBackface;
		texturePerLetter = m_texturePerLetter;
		anchor = m_anchor;
		zAnchor = m_zAnchor;
		colliderType = m_colliderType;
		addRigidbodies = m_addRigidbodies;
		physicsMaterial = m_physicsMaterial;
		smoothingAngle = m_smoothingAngle;
		if (defaultMaterial == null) {
			var mat = Resources.Load ("VertexColored") as Material;
			if (mat) {	
				defaultMaterial = mat;
			}
			else {
				var shader = Shader.Find ("Diffuse");
				if (shader) {
					defaultMaterial = new Material(shader);
				}
			}
		}
		
		if (m_fontData.Count == 0) {
			_initialized = false;
			return;
		}
		_fontInfo = new TTFFontInfo[m_fontData.Count];
		_fontNames = new string[m_fontData.Count];
		
		for (int i = 0; i < m_fontData.Count; i++) {
			if (m_fontData[i].ttfFile != null) {
				_fontInfo[i] = new TTFFontInfo (m_fontData[i].ttfFile.bytes);
				var name = _fontInfo[i].name;
				name = name.Replace(" ", "");
				name = name.ToLower();
				_fontNames[i] = name;
			}
		}

		_colorDictionary = new Dictionary<string, Color>(){{"red", Color.red}, {"green", Color.green}, {"blue", Color.blue}, {"white", Color.white}, {"black", Color.black}, {"yellow", Color.yellow}, {"cyan", Color.cyan}, {"magenta", Color.magenta}, {"gray", Color.gray}, {"grey", Color.grey}};
		DontDestroyOnLoad (this);
		_initialized = true;
	}
	
	private static bool CheckSetup () {
		if (!_initialized) {
			Debug.LogError ("FlyingText hasn't been initialized yet...use script execution order to make sure your Awake functions run first, or use Start");
			return false;
		}
		return true;
	}

	public static void PrimeText (string text) {
		if (!CheckSetup()) return;
		GetObject (text, defaultMaterial, defaultSize, defaultDepth, defaultResolution, defaultLetterSpacing, defaultLineSpacing, true, true);
	}
	
	public static void PrimeText (string text, float size, float extrudeDepth, int resolution) {
		if (!CheckSetup()) return;
		GetObject (text, defaultMaterial, size, extrudeDepth, resolution, defaultLetterSpacing, defaultLineSpacing, true, true);
	}
	
	public static GameObject GetObject (string text) {
		if (!CheckSetup()) return null;
		return GetObject (text, defaultMaterial, defaultSize, defaultDepth, defaultResolution, defaultLetterSpacing, defaultLineSpacing, false, false);
	}
	
	public static GameObject GetObject (string text, Material material, float size, float extrudeDepth, int resolution) {
		if (!CheckSetup()) return null;
		return GetObject (text, material, size, extrudeDepth, resolution, defaultLetterSpacing, defaultLineSpacing, false, false);
	}
	
	public static GameObject GetObject (string text, Material material, float size, float extrudeDepth, int resolution, float characterSpacing, float lineSpacing) {
		if (!CheckSetup()) return null;
		return GetObject (text, material, size, extrudeDepth, resolution, characterSpacing, lineSpacing, false, false);
	}

	public static GameObject GetObjects (string text) {
		if (!CheckSetup()) return null;
		return GetObject (text, defaultMaterial, defaultSize, defaultDepth, defaultResolution, defaultLetterSpacing, defaultLineSpacing, false, true);
	}
	
	public static GameObject GetObjects (string text, Material material, float size, float extrudeDepth, int resolution) {
		if (!CheckSetup()) return null;
		return GetObject (text, material, size, extrudeDepth, resolution, defaultLetterSpacing, defaultLineSpacing, false, true);
	}
	
	public static GameObject GetObjects (string text, Material material, float size, float extrudeDepth, int resolution, float characterSpacing, float lineSpacing) {
		if (!CheckSetup()) return null;
		return GetObject (text, material, size, extrudeDepth, resolution, characterSpacing, lineSpacing, false, true);
	}
	
	private static GameObject GetObject (string s, Material material, float size, float extrudeDepth, int resolution, float characterSpacing, float lineSpacing, bool prime, bool separate) {
		if (!_initialized) {
			Debug.LogError ("No font information available");
			return null;
		}
		if (s == null || s.Length < 1) {
			Debug.LogError ("String can't be null");
			return null;
		}
		if (material == null) {
			material = defaultMaterial;
		}
		if (resolution < 1) {
			resolution = 1;
		}
		if (size < 0.001f) {
			size = 0.001f;
		}
		if (extrudeDepth < 0.0f) {
			extrudeDepth = 0.0f;
		}
		if (characterSpacing < 0.0f) {
			characterSpacing = 0.0f;
		}
		defaultFont = Mathf.Clamp (defaultFont, 0, _fontInfo.Length-1);
		
		List<CommandData> commandData;
		int commandIndex = 0;
		char[] chars = ParseString (s, out commandData);		
		int charCount = chars.Length;
		var glyphIndices = new int[charCount];
		int totalVertCount = 0;
		int totalTriCount = 0;
		bool extrude = (extrudeDepth > 0.0f);
		float spacePercent = (characterSpacing < 1.0f)? characterSpacing : 1.0f;
		float spaceAdd = (characterSpacing > 1.0f)? characterSpacing - 1.0f : 0.0f;
		var thisFont = _fontInfo[defaultFont];
		int fontNumber = defaultFont;
				
		// Get total vertex and triangle count, initializing glyph data as necessary
		for (int i = 0; i < charCount; i++) {
			var thisCommand = commandData[commandIndex];
			while (thisCommand.index == i) {
				if (thisCommand.command == Command.Font) {
					int thisFontNumber = (int)thisCommand.data;
					if (thisFontNumber >= 0 && thisFontNumber < _fontInfo.Length) {
						fontNumber = thisFontNumber;
						thisFont = _fontInfo[fontNumber];
					}
				}
				thisCommand = commandData[++commandIndex];
			}
			
			if (thisFont == null) {
				Debug.LogError ("Font is null");
				return null;
			}
			
			// Set up glyphs if they haven't been previously initialized
			var character = chars[i];
			if (!thisFont.glyphDictionary.ContainsKey (character)) {
				if (!thisFont.SetGlyphData (character)) return null;
			}
			
			var glyphData = thisFont.glyphDictionary[character];
			glyphIndices[i] = glyphData.glyphIndex;
			
			if (glyphData.isVisible) {
				if (glyphData.resolution != resolution) {
					if (!glyphData.SetMeshData (resolution)) {
						Debug.LogWarning ("Triangulation failed for char code " + Convert.ToInt32(character) + " (" + character + ")");
						continue;
					}
					if (!extrude) {
						glyphData.SetFrontData();
					}
					else {
						if (includeBackface) {
							glyphData.SetData();
						}
						else {
							glyphData.SetFrontAndEdgeData();
						}
					}
				}
				if (!separate) {
					totalVertCount += glyphData.vertexCount;
					totalTriCount += glyphData.triCount;
				}
			}
		}
		
		if (totalVertCount > 65534) {
			Debug.LogError ("Too many points...use fewer characters or reduce resolution");
			return null;
		}
		if (!separate && totalVertCount == 0) {
			Debug.LogError ("No usable characters in string");
			return null;
		}
		
		if (prime) return null;
		GameObject goParent = separate? new GameObject() : null;
		
		// Use vertex colors if anything other than Color.white is specified anywhere
		Color thisColor = defaultColor;
		var useColors = false;		
		if (thisColor == Color.white) {
			for (var i = 0; i < commandData.Count; i++) {
				if (commandData[i].command == Command.Color && (Color)commandData[i].data != Color.white) {
					useColors = true;
					break;
				}
			}
		}
		else {
			useColors = true;
		}
		
		var totalVerts = new Vector3[totalVertCount];
		var totalTris = new int[totalTriCount];
		var meshUVs = new Vector2[totalVertCount];
		var meshColors = new Color[totalVertCount];
		int vertIndex = 0;
		int triIndex = 0;
		float baseScale = 1.0f / thisFont.unitsPerEm;
		bool uvsPerLetter = separate? true : texturePerLetter;
		Vector3[] thisVerts;
		int[] thisTris;
		var kernPair = new KernPair();
		float smallestX = float.MaxValue;
		float largestX = -float.MaxValue;
		float smallestY = float.MaxValue;
		float largestY = -float.MaxValue;
		List<float> lineLengths = null;
		List<Justify> lineJustifies = null;
		int loopType = BUILDMESH;
		bool hasMultipleLines = false;
		int lineCount = 0;
		float longestLength = 0.0f;
		var thisJustify = defaultJustification;
		
		if (Array.IndexOf (chars, '\n') != -1) {
			lineLengths = new List<float>();
			lineJustifies = new List<Justify>();
			loopType = PREPROCESS;
			hasMultipleLines = true;
		}
		
		while (loopType > 0) {
			float horizontalPosition = 0.0f;
			float verticalPosition = 0.0f;
			float zPosition = 0.0f;
			float thisSize = size;
			thisFont = _fontInfo[defaultFont];
			commandIndex = 0;
			if (hasMultipleLines && loopType == BUILDMESH) {
				thisJustify = lineJustifies[0];
			}
			var thisCommand = commandData[0];
			for (int i = 0; i < charCount; i++) {
				while (thisCommand.index == i) {
					switch (thisCommand.command) {
						case Command.Size:
							thisSize = (float)thisCommand.data;
							if (thisSize < .001f) thisSize = .001f;
							break;
						case Command.Color:
							thisColor = (Color)thisCommand.data;
							break;
						case Command.Font:
							fontNumber = (int)thisCommand.data;
							if (fontNumber >= 0 && fontNumber < _fontInfo.Length) {
								thisFont = _fontInfo[fontNumber];
								baseScale = 1.0f / thisFont.unitsPerEm;
							}
							break;
						case Command.Zpos:
							zPosition = (float)thisCommand.data;
							break;
						case Command.Depth:
							extrudeDepth = (float)thisCommand.data;
							if (extrudeDepth < 0.0f) extrudeDepth = 0.0f;
							break;
						case Command.Space:
							horizontalPosition += (float)thisCommand.data * thisSize;
							break;
						case Command.Justify:
							if (loopType == PREPROCESS) {
								thisJustify = ((Justify)thisCommand.data);
							}
							break;
					}
					thisCommand = commandData[++commandIndex];
				}
				
				float scaleFactor = baseScale * thisSize;
				var character = chars[i];
				if (character == '\0') {
					continue;
				}
				if (character == '\n') {
					if (loopType == PREPROCESS) {
						lineLengths.Add (horizontalPosition);
						lineJustifies.Add (thisJustify);
					}
					else if (hasMultipleLines) {
						if (++lineCount < lineJustifies.Count) {
							thisJustify = lineJustifies[lineCount];
						}
					}
					verticalPosition -= thisFont.lineHeight * lineSpacing * scaleFactor;
					horizontalPosition = 0.0f;
					continue;
				}
				int thisGlyphIdx = glyphIndices[i];
				
				// Kerning
				if (thisFont.hasKerning && i > 0) {
					kernPair.left = glyphIndices[i-1];
					kernPair.right = thisGlyphIdx;
					if (thisFont.kernDictionary.ContainsKey (kernPair)) {
						horizontalPosition += thisFont.kernDictionary[kernPair] * scaleFactor;
					}
				}
				var glyphData = thisFont.glyphDictionary[character];
				
				// Copy tris/verts to combined mesh
				int vertexCount = glyphData.vertexCount;
				if (vertexCount > 0 && loopType == BUILDMESH) {
					if (glyphData.scaleFactor != scaleFactor) {
						glyphData.ScaleVertices (scaleFactor, extrude, includeBackface);
					}
					
					if (extrude && glyphData.extrudeDepth != extrudeDepth) {
						glyphData.SetExtrudeDepth (extrudeDepth, includeBackface);
					}
					
					thisVerts = glyphData.vertices;
					
					if (separate) {
						totalVerts = new Vector3[vertexCount];
						totalTris = new int[glyphData.triCount];
						meshUVs = new Vector2[vertexCount];
						if (useColors) {
							meshColors = new Color[vertexCount];
						}
						vertIndex = 0;
						triIndex = 0;
					}
					
					// Get min/max bounds (for UVs if not per-letter, plus anchor position)
					float max = glyphData.xMax * scaleFactor + horizontalPosition;
					float min = glyphData.xMin * scaleFactor + horizontalPosition;
					if (max > largestX) {
						largestX = max;
					}
					if (min < smallestX) {
						smallestX = min;
					}
					max = glyphData.yMax * scaleFactor + verticalPosition;
					min = glyphData.yMin * scaleFactor + verticalPosition;
					if (max > largestY) {
						largestY = max;
					}
					if (min < smallestY) {
						smallestY = min;
					}
					
					if (uvsPerLetter) {
						float xMax = glyphData.xMax * scaleFactor;
						float xMin = glyphData.xMin * scaleFactor;
						float yMax = glyphData.yMax * scaleFactor;
						float yMin = glyphData.yMin * scaleFactor;
						float xRange = xMax - xMin;
						float yRange = yMax - yMin;
						for (int j = 0; j < vertexCount; j++) {
							meshUVs[j + vertIndex].x = (thisVerts[j].x - xMin) / xRange;
							meshUVs[j + vertIndex].y = (thisVerts[j].y - yMin) / yRange;
						}
					}
					
					if (useColors) {
						for (int j = 0; j < vertexCount; j++) {
							meshColors[j + vertIndex] = thisColor;
						}
					}
					
					thisTris = glyphData.triangles;
					int triCount = glyphData.triCount;
					for (int j = 0; j < triCount; j += 3) {
						totalTris[triIndex  ] = thisTris[j  ] + vertIndex;
						totalTris[triIndex+1] = thisTris[j+1] + vertIndex;
						totalTris[triIndex+2] = thisTris[j+2] + vertIndex;
						triIndex += 3;
					}
					
					// Set vertices with appropriate line justification
					if (hasMultipleLines && thisJustify != Justify.Left && longestLength != lineLengths[lineCount]) {
						float addSpace = (thisJustify == Justify.Right)? longestLength - lineLengths[lineCount] :
																		 (longestLength - lineLengths[lineCount]) / 2 ; 
						if (!separate) {
							for (int j = 0; j < vertexCount; j++) {
								totalVerts[vertIndex  ].x = thisVerts[j].x + horizontalPosition + addSpace;
								totalVerts[vertIndex  ].y = thisVerts[j].y + verticalPosition;
								totalVerts[vertIndex++].z = thisVerts[j].z + zPosition;
							}
						}
						else {
							for (int j = 0; j < vertexCount; j++) {
								totalVerts[vertIndex  ].x = thisVerts[j].x + addSpace;
								totalVerts[vertIndex  ].y = thisVerts[j].y;
								totalVerts[vertIndex++].z = thisVerts[j].z + zPosition;
							}
						}
					}
					else {
						if (!separate) {
							for (int j = 0; j < vertexCount; j++) {
								totalVerts[vertIndex  ].x = thisVerts[j].x + horizontalPosition;
								totalVerts[vertIndex  ].y = thisVerts[j].y + verticalPosition;
								totalVerts[vertIndex++].z = thisVerts[j].z + zPosition;
							}
						}
						else {
							for (int j = 0; j < vertexCount; j++) {
								totalVerts[vertIndex  ].x = thisVerts[j].x;
								totalVerts[vertIndex  ].y = thisVerts[j].y;
								totalVerts[vertIndex++].z = thisVerts[j].z + zPosition;
							}
						}
					}
					
					// Create mesh and game object for individual letters
					if (separate) {
						var charMesh = new Mesh();
						charMesh.name = character.ToString();
						charMesh.vertices = totalVerts;
						charMesh.uv = meshUVs;
						if (useColors) {
							charMesh.colors = meshColors;
						}
						charMesh.triangles = totalTris;
						charMesh.RecalculateNormals();
						
						var charGo = new GameObject(character.ToString(), typeof(MeshFilter), typeof(MeshRenderer));
						charGo.GetComponent<MeshFilter>().mesh = charMesh;
						if (colliderType == ColliderType.Mesh || colliderType == ColliderType.ConvexMesh) {
							var meshCollider = charGo.AddComponent<MeshCollider>();
							meshCollider.sharedMesh = charMesh;
							meshCollider.convex = (colliderType == ColliderType.ConvexMesh);
							meshCollider.sharedMaterial = physicsMaterial;
						}
						else if (colliderType == ColliderType.Box) {
							var boxCollider = charGo.AddComponent<BoxCollider>();
							boxCollider.sharedMaterial = physicsMaterial;
						}
						if (addRigidbodies) {
							charGo.AddComponent(typeof(Rigidbody));
						}
						charGo.renderer.sharedMaterial = material;
						charGo.transform.parent = goParent.transform;
						charGo.transform.position = new Vector3(horizontalPosition, verticalPosition, zPosition);
					}
				}
				horizontalPosition += (thisFont.advanceArray[thisGlyphIdx] + spaceAdd / baseScale) * (scaleFactor * spacePercent);
			}
			
			if (loopType-- == PREPROCESS) {
				lineLengths.Add (horizontalPosition);
				lineJustifies.Add (thisJustify);
				longestLength = lineLengths[0];
				for (int i = 1; i < lineLengths.Count; i++) {
					if (lineLengths[i] > longestLength) {
						longestLength = lineLengths[i];
					}
				}
			}
		}
				
		// Set UVs for complete mesh, if not per-letter
		if (!uvsPerLetter) {
			float xRange = largestX - smallestX;
			float yRange = largestY - smallestY;
			for (int i = 0; i < totalVertCount; i++) {
				meshUVs[i].x = (totalVerts[i].x - smallestX) / xRange;
				meshUVs[i].y = (totalVerts[i].y - smallestY) / yRange;
			}
		}
		
		var add = Vector3.zero;
		switch (anchor) {
			case TextAnchor.UpperLeft:
				add.y = largestY;
				break;
			case TextAnchor.UpperCenter:
				add.x = (largestX - smallestX) * .5f;
				add.y = largestY;
				break;
			case TextAnchor.UpperRight:
				add.x = largestX - smallestX;
				add.y = largestY;
				break;
			case TextAnchor.MiddleLeft:
				add.y = (smallestY - largestY) * .5f + largestY;
				break;
			case TextAnchor.MiddleCenter:
				add.x = (largestX - smallestX) * .5f;
				add.y = (smallestY - largestY) * .5f + largestY;
				break;
			case TextAnchor.MiddleRight:
				add.x = largestX - smallestX;
				add.y = (smallestY - largestY) * .5f + largestY;
				break;
			case TextAnchor.LowerLeft:
				add.y = (smallestY - largestY) + largestY;
				break;
			case TextAnchor.LowerCenter:
				add.x = (largestX - smallestX) * .5f;
				add.y = (smallestY - largestY) + largestY;
				break;
			case TextAnchor.LowerRight:
				add.x = largestX - smallestX;
				add.y = (smallestY - largestY) + largestY;
				break;
		}
		if (extrude) {
			switch (zAnchor) {
				case ZAnchor.Middle:
					add.z = defaultDepth * .5f;
					break;
				case ZAnchor.Back:
					add.z = defaultDepth;
					break;
			}
		}
		if (!separate) {
			for (int i = 0; i < totalVertCount; i++) {
				totalVerts[i] -= add;
			}
		}
		else {
			var gos = goParent.GetComponentsInChildren<Transform>();
			for (int i = 0; i < gos.Length; i++) {
				if (gos[i].gameObject == goParent.gameObject) continue;
				gos[i].position -= add;
			}
		}
		
		// Get gameobject name from string, and create mesh and game object if not separate
		var charString = new string(chars);
		var name = charString.Substring(0, Mathf.Min(20, charString.Length));
		name = name.Replace("\n", " ");
		name = name.Replace("\0", "");
		
		if (separate) {
			goParent.name = "3DText " + name;
			return goParent;
		}
		
		var mesh = new Mesh();
		mesh.name = name;
		mesh.vertices = totalVerts;
		mesh.uv = meshUVs;
		if (useColors) {
			mesh.colors = meshColors;
		}
		mesh.triangles = totalTris;
		mesh.RecalculateNormals();
		
		var textGo = new GameObject("3DText " + name, typeof(MeshFilter), typeof(MeshRenderer));
		textGo.GetComponent<MeshFilter>().mesh = mesh;
		textGo.renderer.sharedMaterial = material;
		if (colliderType == ColliderType.Box) {
			var boxCollider = textGo.AddComponent<BoxCollider>();
			boxCollider.sharedMaterial = physicsMaterial;
		}
		if (addRigidbodies) {
			textGo.AddComponent<Rigidbody>();
		}
		return textGo;
	}
	
	private static char[] ParseString (string s, out List<CommandData> commandData) {
		commandData = new List<CommandData>();
		
		s = s.Replace ("\0", "");
		s = s.Replace ("<<", "\01");
		s = s.Replace (">>", "\02");
		s = s.Replace ("<br>", "\n");
		s = s.Replace ("<BR>", "\n");		
		
		int startIndex = 0;
		int i = s.IndexOf ("<", startIndex);
		while (i != -1) {
			int j = s.IndexOf (">", startIndex);
			if (j == -1 || j < i) break;
			
			string tagData = s.Substring (i+1, j-i-1);
			s = s.Remove (i, j-i+1);
			startIndex = i;
			string tag, data;
			if (GetTagData (ref tagData, out tag, out data)) {
				tag = tag.ToLower();
				switch (tag) {
					case "size":
						float thisVal;
						if (Single.TryParse (data, out thisVal)) {
							commandData.Add (new CommandData(i, Command.Size, thisVal));
						}
						break;
					case "color":
						Color thisColor;
						if (TryParseColor (ref data, out thisColor)) {
							commandData.Add (new CommandData(i, Command.Color, thisColor));
						}
						else {
							data = data.ToLower();
							if (_colorDictionary.ContainsKey (data)) {
								commandData.Add (new CommandData(i, Command.Color, _colorDictionary[data]));
							}
						}
						break;
					case "font":
						int fontNumber;
						if (Int32.TryParse (data, out fontNumber)) {
							commandData.Add (new CommandData(i, Command.Font, fontNumber));
						}
						else {
							data = data.ToLower();
							for (int k = 0; k < _fontNames.Length; k++) {
								if (data == _fontNames[k]) {
									commandData.Add (new CommandData(i, Command.Font, k));
									break;
								}
							}
						}
						break;
					case "zpos":
						if (Single.TryParse (data, out thisVal)) {
							commandData.Add (new CommandData(i, Command.Zpos, thisVal));
						}
						break;
					case "depth":
						if (Single.TryParse (data, out thisVal)) {
							commandData.Add (new CommandData(i, Command.Depth, thisVal));
						}
						break;
					case "space":
						if (Single.TryParse (data, out thisVal)) {
							commandData.Add (new CommandData(i, Command.Space, thisVal));
						}
						break;
					case "justify":
						if (data == "left") {
							commandData.Add (new CommandData(i, Command.Justify, Justify.Left));
						}
						else if (data == "right") {
							commandData.Add (new CommandData(i, Command.Justify, Justify.Right));							
						}
						else if (data == "center" || data == "centre") {
							commandData.Add (new CommandData(i, Command.Justify, Justify.Center));
						}
						break;
					default:
						Debug.LogWarning ("Unknown tag: " + tag);
						break;
				}
			}
			
			i = s.IndexOf ("<", startIndex);
		}
		
		commandData.Add (new CommandData(-1, Command.None, null));
		s = s.Replace ("\01", "\0<");
		s = s.Replace ("\02", "\0>");

		var charArray = new char[s.Length + 1];
		for (i = 0; i < s.Length; i++) {
			charArray[i] = s[i];
		}
		charArray[s.Length] = '\0';
		return charArray;
	}
	
	private static bool GetTagData (ref string s, out string tag, out string data) {
		if (s.IndexOfAny (_removeChars) != -1) {
			s = string.Join("", s.Split(_removeChars)); // Prefer not to use Regex for just this one thing, given how much size it adds
		}
		var strings = s.Split('=');
		if (strings.Length != 2) {
			tag = ""; data = "";
			return false;
		}
		tag = strings[0];
		data = strings[1];
		return true;
	}
	
	private static bool TryParseColor (ref string s, out Color color) {
		color = Color.white;
		if (s.Length != 7 || !s.StartsWith("#")) {
			return false;
		}
		int value;
		if (Int32.TryParse (s.Substring(1, 6), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value)) {
			color = new Color((value >> 16) / 255.0f, ((value >> 8) & 255) / 255.0f, (value & 255) / 255.0f);
			return true;
		}
		return false;
	}
}