using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace utils.CSVPeopleReader
{
	public class CSVPeopleReader : IDisposable {
		
		private BinaryReader _binaryReader;
		
		public const string NEWLINE = "\r";
		
		public CSVPeopleReader (BinaryReader reader)
		{
			this._binaryReader = reader;
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
		 			//	Debug.Log(">> " + i + "Answer <" + columnNames[i] + "> =" + row[i] );
					}
					weavePeople.Add(newWeavePerson);
                }
                else{
                    headerRead = true;
				}
			}

            return weavePeople;
        }

        public static List<PersonModel> readPeopleFromBinaryReader(BinaryReader reader)
        {
			
			using (CSVPeopleReader peopleReader = new CSVPeopleReader(reader))
				return peopleReader.readAllRows(true);
        }
		
	
       

        public void Dispose()
        {
            if (_binaryReader != null)
            {
                try
                {
                    _binaryReader.Close();
                }
                catch { }
            }
        }
		
	
	
	}
}
