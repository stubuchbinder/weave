using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class WeaveAppStarter : MonoBehaviour 
{
	private WeaveConfigReader configReader;
	private WeavePeopleReader peopleReader;
    private Weave weaveData;

	public WeaveConfigSet GetWeaveConfigForQuestion(int qnumber)
    {
		return DataModel.Instance.GetQuestionConfig(qnumber);	
	}

	public void startWeave()
	{
		Debug.Log("WeaveAppStarter -> startWeave");
		
		string photoURL = DataModel.Instance.PhotoURL;
		string logoURL = DataModel.Instance.LogoURL;
        string peoplesize = DataModel.Instance.PeopleSize;
	
		ListOfStagePeople listOfStagePeople = GameObject.FindGameObjectWithTag("TagListOfStagePeople").GetComponent<ListOfStagePeople>();
		listOfStagePeople.NumberOfStars = Int32.Parse(DataModel.Instance.NumberOfStars);
		
		// Create The Weave Object From Config
		weaveData = new Weave("Test Weave One");
		weaveData.setPersonList(DataModel.Instance.People);
		
		// Initialize StageDirector
		StageDirector sd = GameObject.FindGameObjectWithTag("TagStageDirector").GetComponent<StageDirector>();

		sd.CreateLogo(logoURL);
		sd.HereIsTheWeaveData(weaveData);

	}
	


}