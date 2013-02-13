using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PersonModel //: MonoBehaviour
{
    private string fullName;
    private string photoURL;
    private WeaveConfigReader configReader;
    private Dictionary<string, string> answerList;

    public PersonModel(string pname = "")
    {
        answerList = new Dictionary<string, string>();

        fullName = pname;
    }
	
    public void SetAnswerForQuestion(string question, string answer)
    {
       

        if (String.Equals(question, "fullname", StringComparison.Ordinal))
        {
            fullName = answer;
        }

        if (String.Equals(question, "photofile", StringComparison.Ordinal))
        {
			//photoURL = DataModel.Instance.filePath  +DataModel.Instance.PhotoURL;
			
			photoURL = DataModel.Instance.PhotoURL;
			if (answer == "")
            {
                char[] delimiterChars = { ' ' };

                string[] names = fullName.Split(delimiterChars);
                string defaultfilename = "";
				
				//Debug.Log("*****NAMES = ");
                if (names.Length > 1)
                {
                    defaultfilename = names[0].ToLower() + "_" + names[1].ToLower() + ".png";
                }
                else
                {
                    defaultfilename = names[0].ToLower() + ".png";
                }
                photoURL += defaultfilename;
            }
            else
            {
                photoURL += answer;
            }
        }
        answerList[question] = answer;
    }
	
    public string GetAnswerForQuestion(string question)
    {
        return answerList[question];
    }

    public string GetName()
    {
        return fullName;
    }

    public string GetPhotoURL()
    {
        return photoURL;
    }
}