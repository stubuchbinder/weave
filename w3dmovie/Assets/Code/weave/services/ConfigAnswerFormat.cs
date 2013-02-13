using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ConfigAnswerFormat 
{
	private WeaveConfigSet _questionConfig;
	
	public ConfigAnswerFormat ( WeaveConfigSet questionConfig )
    {	
		_questionConfig = questionConfig;
	}
	public string GetFormattedAnswer(PersonModel person)
    { 
		string answertext = "";
		string answerformat = _questionConfig.GetVal("answerformat");

		char[] delimiterChars = { '~' }; 

		string[] answerbits = answerformat.Split(delimiterChars);
		
		bool isKeyText = false;
		
		for(int i=0; i< answerbits.Length; i++) 
        {
			if (!isKeyText)
            {
				// regular text just add it to the answertext
				answertext += answerbits[i];
			}
            else
            {
				string keyval =  answerbits[i];
				
				if(keyval == "newline")
                {
					answertext += "\n";
				}
                else if (keyval == "space")
                {
					answertext += " ";
					
				}
                else
                {
					// this is a key for a question
					answertext += person.GetAnswerForQuestion(keyval);
				}
			}
			isKeyText = !isKeyText; // alternate between key and non-key text
        }
		return answertext;		
	}
}