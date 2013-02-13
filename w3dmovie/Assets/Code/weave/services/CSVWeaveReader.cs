/* CSVReader - a simple open source C# class library to read CSV data
 * by Andrew Stellman - http://www.stellman-greene.com/CSVReader
 * 
 * CSVReader.cs - Class to read CSV data from a string, file or stream
 * 
 * download the latest version: http://svn.stellman-greene.com/CSVReader
 * 
 * (c) 2008, Stellman & Greene Consulting
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of Stellman & Greene Consulting nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY STELLMAN & GREENE CONSULTING ''AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL STELLMAN & GREENE CONSULTING BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace utils.CSVWeaveReader
{
    /// <summary>
    /// Read CSV-formatted data from a file or TextReader
    /// </summary>
    public class CSVWeaveReader : IDisposable
    {
        public const string NEWLINE = "\r";
        /// <summary>
        /// This reader will read all of the CSV data
        /// </summary>
        private BinaryReader _binaryReader;
        /// <summary>
        /// Read CSV-formatted data from a file
        /// </summary>
        /// <param name="filename">Name of the CSV file</param>
        public CSVWeaveReader (TextAsset csvTextAsset)
		{
            //Debug.Log("csvTextAsset: " + csvTextAsset);
			TextAsset textAsset;
            //Debug.Log("textAsset: "+ textAsset);
			if (csvTextAsset == null) {
					throw new ArgumentNullException ("Null FileInfo passed to CSVReader");
			}
			this._binaryReader = new BinaryReader(new MemoryStream(csvTextAsset.bytes));
		}
		string currentLine = "";
        /// <summary>
        /// Read the next row from the CSV data
        /// </summary>
        /// <returns>A list of objects read from the row, or null if there is no next row</returns>
        public List<string> readOneRow()
        {
            // ReadLine() will return null if there's no next line
            if (_binaryReader.BaseStream.Position >= _binaryReader.BaseStream.Length)
                return null;
            StringBuilder builder = new StringBuilder();
            // Read the next line
            while ((_binaryReader.BaseStream.Position < _binaryReader.BaseStream.Length) && (!builder.ToString().EndsWith(NEWLINE)))
            {
                char c = _binaryReader.ReadChar();
                builder.Append(c);
            }
            currentLine = builder.ToString();
            if (currentLine.EndsWith(NEWLINE))
                currentLine = currentLine.Remove(currentLine.IndexOf(NEWLINE), NEWLINE.Length);
			
            // Build the list of objects in the line
            List<string> objects = new List<string>();
            while (currentLine != "")
                objects.Add(ReadNextObject());
            return objects;
        }
        /// <summary>
        /// Read the next object from the currentLine string
        /// </summary>
        /// <returns>The next object in the currentLine string</returns>
        private string ReadNextObject()
        {
            if (currentLine == null)
                return null;
            // Check to see if the next value is quoted
            bool quoted = false;
            if (currentLine.StartsWith("\""))
                quoted = true;
            // Find the end of the next value
            string nextObjectString = "";
            int i = 0;
            int len = currentLine.Length;
            bool foundEnd = false;
            while (!foundEnd && i <= len)
            {
                // Check if we've hit the end of the string
                if ((!quoted && i == len) // non-quoted strings end with a comma or end of line
                    || (!quoted && currentLine.Substring(i, 1) == ",")
                    // quoted strings end with a quote followed by a comma or end of line
                    || (quoted && i == len - 1 && currentLine.EndsWith("\""))
                    || (quoted && currentLine.Substring(i, 2) == "\","))
                    foundEnd = true;
                else
                    i++;
            }
            if (quoted)
            {
                if (i > len || !currentLine.Substring(i, 1).StartsWith("\""))
                    throw new FormatException("Invalid CSV format: " + currentLine.Substring(0, i));
                i++;
            }
            nextObjectString = currentLine.Substring(0, i).Replace("\"\"", "\"");

            if (i < len)
                currentLine = currentLine.Substring(i + 1);
            else
                currentLine = "";

            if (quoted)
            {
                if (nextObjectString.StartsWith("\""))
                    nextObjectString = nextObjectString.Substring(1);
                if (nextObjectString.EndsWith("\""))
                    nextObjectString = nextObjectString.Substring(0, nextObjectString.Length - 1);
                return nextObjectString;
            }
            else
            {
                //object convertedValue;
                //StringConverter.ConvertString(nextObjectString, out convertedValue);
				// PPPPP removed string converter
				
                return nextObjectString;
            }
        }
		
        /// <summary>
        /// Read the row data read using repeated readOneRow() calls and build a DataColumnCollection with types and column names
        /// </summary>
        /// <param name="headerRow">True if the first row contains headers</param>
        /// <returns>System.Data.DataTable object populated with the row data</returns>
        public List<PersonModel> readAllRows(bool headerRow)
        {
			// Read the CSV data into rows
            List<List<string>> rows = new List<List<string>>();
            List<string> readRow = null;
            List<string> columnNames = new List<string>();
			List<PersonModel> weavePeople = new List<PersonModel>();
            bool headerRead = false;

			while ((readRow = readOneRow()) != null)
            {
                rows.Add(readRow);
			}
			
            // The names (if headerRow is true) will be stored in these lists
            // Read the column names from the header row (if there is one)
            if (headerRow){
                foreach (object name in rows[0]){
                    columnNames.Add(name.ToString());
				}
			}

			// Add the data from the rows
            foreach (List<string> row in rows){
                if (headerRead || !headerRow)
                {
					PersonModel newWeavePerson = new PersonModel();
					
                    for (int i = 0; i < row.Count; i++)
                    {
						newWeavePerson.SetAnswerForQuestion(columnNames[i], row[i]);
		 				Debug.Log(">> " + i + "Answer <" + columnNames[i] + "> =" + row[i] );
					}
					weavePeople.Add(newWeavePerson);
                }
                else{
                    headerRead = true;
				}
			}

            return weavePeople;
        }
        /// <summary>
        /// Read a CSV file into a table
        /// </summary>
        /// <param name="filename">Filename of CSV file</param>
        /// <param name="headerRow">True if the first row contains column names</param>
        /// <returns>System.Data.DataTable object that contains the CSV data</returns>
        public static List<PersonModel> readPeopleFromCSVFile(string filename, bool headerRow)
        {
            using (CSVWeaveReader reader = new CSVWeaveReader((TextAsset)Resources.Load(filename)))
                return reader.readAllRows(headerRow);
        }

        /// <summary>
        /// Read a CSV file into a table
        /// </summary>
        /// <param name="filename">Filename of CSV file</param>
        /// <param name="headerRow">True if the first row contains column names</param>
        /// <returns>System.Data.DataTable object that contains the CSV data</returns>
        public static List<PersonModel> readPeopleFromWeaveCSVFile(string filename)
        {
			//Debug.Log(">>>> text asset = " + (TextAsset)Resources.Load(filename));

            TextAsset textAsset = (TextAsset)Resources.Load(filename); 
          
			using (CSVWeaveReader reader = new CSVWeaveReader(textAsset))
                return reader.readAllRows(true);
        }

        public void Dispose()
        {
            if (_binaryReader != null)
            {
                try
                {
                    // Can't call BinaryReader.Dispose due to its protection level
                    _binaryReader.Close();
                }
                catch { }
            }
        }

    }
}