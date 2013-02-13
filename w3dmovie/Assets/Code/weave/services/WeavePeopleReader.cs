using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using utils.CSVWeaveReader;

public class WeavePeopleReader 
{
	public WeavePeopleReader()
    {
		Debug.Log("In WeavePeopleReader Construct");
	}

	public List<PersonModel> readPeopleFromCSV(string csvfilename)
    {
		Debug.Log("readPeopleFromCSV2 = " + csvfilename);

	
		List<PersonModel> weavePeople;
		weavePeople = CSVWeaveReader.readPeopleFromWeaveCSVFile(csvfilename);
		return weavePeople;
	}

}