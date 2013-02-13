using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class DataModel {

	private static DataModel _instance;
	public string filePath;

	public DataModel ()
	{
		if (_instance != null) {
			Debug.LogError ("Cannot have two instances of singleton.");
			return;
		}

		_instance = this;
		Init();
	}
	
	public static DataModel Instance 
	{
		get 
		{
			if (_instance == null)
			{
				new DataModel();
			}
			
			return _instance;
		}

	}
	
	
	private void Init() 
	{
		Debug.Log("DataModel -> Init");
		
		string dataPath = Application.dataPath;
		// Initialize filePath
		string prefix = Application.absoluteURL.Split("://"[0])[0];
		if(Application.isEditor) {
			Debug.Log("Application running in Editor");
			filePath = "";	
		} else {
			
			
			if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer) {
				Debug.Log("Application running in StandAlone");

				if(Application.platform == RuntimePlatform.OSXPlayer) {
					string assetsPath = Application.dataPath;
					string configPath = assetsPath.Substring(0,assetsPath.LastIndexOf("weave.app" +'/'));
					filePath = "file://" +configPath;
				} else {
					filePath = Application.dataPath;
				}
				
			} else {
				Debug.Log("Application running in WebPlayer");
				
				filePath = Application.dataPath +"/";
			}
		}	
		Debug.Log(" filePath: " +filePath);
	}
	
	
	private XmlDocument _config;
	public XmlDocument Config
	{
		get 
		{
			return _config;
		}
		
		set
		{
			_config = value;
		}
	}
	


	
	
	
	private List<PersonModel> _people;
	public List<PersonModel> People
	{
		get
		{
			return _people;	
		}
		set
		{
			_people = value;
		}
	}
    
    public string PlayTime
    {
        get
        {
            

            XmlNode PlayTimeNode = _config.SelectSingleNode("weave/playtime");
            return PlayTimeNode.InnerText;
        }
    }
	
	public string LogoURL
	{
		get
		{
			XmlNode logoNode = _config.SelectSingleNode("weave/logo");
			return logoNode.InnerText;	
		}
	}
	
	public string PhotoURL
	{
		get
		{
			XmlNode photoNode = _config.SelectSingleNode("weave/photo");
			return photoNode.InnerText;
		}
	}
	
	public string PeopleFile
	{
		get
		{
			XmlNode peopleNode = _config.SelectSingleNode("weave/peoplefile");
			return peopleNode.InnerText;
		}
	}
	
	public string NumberOfRings
	{
		get
		{
			XmlNode ringsNode = _config.SelectSingleNode("weave/numofrings");
			return ringsNode.InnerText;
		}
	}
	
	public string NumberOfStars
	{
		get
		{
			XmlNode starsNode = _config.SelectSingleNode("weave/numofstars");
			return starsNode.InnerText;
		}
		
	}

    public string ExitButton
    {
        get
        {
            XmlNode exitButton = _config.SelectSingleNode("weave/exitbutton");
            return exitButton.InnerText;
        }

    }

    public string PeopleSize
    {
        get
        {
          XmlNode peopleSizeNode = _config.SelectSingleNode("weave/sizeofpeople");
          return peopleSizeNode.InnerText;
        }

    }

    public string LogoSize
    {
        get
        {
            XmlNode logoSizeNode = _config.SelectSingleNode("weave/logosize");
            return logoSizeNode.InnerText;
        }

    }

    public string InnerCircleRadius
    {
        get
        {
            XmlNode innercircleradius = _config.SelectSingleNode("weave/innerradius");
            return innercircleradius.InnerText;
        }

    }

    public string OuterCircleRadius
    {
        get
        {
            XmlNode outercircleradius = _config.SelectSingleNode("weave/outerradius");
            return outercircleradius.InnerText;
        }

    }

	public WeaveConfigSet GetQuestionConfig(int qnumber)
	{
		WeaveConfigSet qconfig = null;
		XmlNode qconfignode;
		
		XmlNodeList questionNodes = _config.GetElementsByTagName("question");
		
		if(qnumber < questionNodes.Count) {
			qconfignode = questionNodes[qnumber];
			qconfig = new WeaveConfigSet(qconfignode);
		}
		else 
        {
			Debug.Log("XML Config >> Not that many questions  .... questionNodes.Count = " + questionNodes.Count );
		}
		return qconfig;	

	}
		
}
