using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using utils.CSVWeaveReader;
using utils.CSVPeopleReader;
using System.IO;


public class WeaveAppLoader : MonoBehaviour {

	private string _filePath;
	
	public bool ctrlDown = false;
	public bool rDown = false;

	void Start () 
	{
		Debug.Log("WeaveAppLoader -> Start");
		_filePath = DataModel.Instance.filePath;
		StartCoroutine(LoadConfig());
	}
	
	
	
	IEnumerator LoadConfig()
	{
		Debug.Log("WeaveAppLoader -> LoadConfig()");
		
		XmlDocument config = new XmlDocument();
		
		if(_filePath == "") {
			// Load from Resources	
			
			Debug.Log("Loading From Resources");
			TextAsset textAsset = (TextAsset)Resources.Load("config/weave_config");  
			config.LoadXml(textAsset.text);
			
		} else {
			string url = _filePath + "config/weave_config.xml";
			WWW www = new WWW(url);
			yield return www;
			
			if(www.error == null) {
				Debug.Log("Loading weave_config.xml from filePath");
				string xml = www.text;
				config.LoadXml(xml);
			} else {
				Debug.Log("Error: " +www.error +" - loading from resources");	
		
				TextAsset textAsset = (TextAsset)Resources.Load("config/weave_config.xml");  
				config.LoadXml(textAsset.text);
			}
		}
		
		DataModel.Instance.Config = config;
		
		StartCoroutine(LoadPeople());
	}

	IEnumerator LoadPeople()
	{
		Debug.Log("WeaveAppLoader -> LoadPeople()");
		
		byte[] people;
		
		string url = _filePath +DataModel.Instance.PeopleFile;
		
		if(_filePath == "") {
			Debug.Log("Loading people file '" +DataModel.Instance.PeopleFile +" ' from Resources");
			TextAsset textAsset = (TextAsset)Resources.Load(DataModel.Instance.PeopleFile); 
			people = textAsset.bytes;
		} else {
			WWW www = new WWW(url);
			yield return www;
			
			if(www.error == null) {
				
				Debug.Log("Loading weave_people.csv from filePath");
				people = www.bytes;
			} else {
				Debug.Log("Error: " +www.error +" - loading from resources");	
				
				TextAsset textAsset = (TextAsset)Resources.Load(DataModel.Instance.PeopleFile); 
				people = textAsset.bytes;
	
			}
		}
		
	
		BinaryReader reader = new BinaryReader(new MemoryStream(people));
		
		List<PersonModel> persons = CSVPeopleReader.readPeopleFromBinaryReader(reader);
		yield return persons;
		
		DataModel.Instance.People = persons;

		LoadComplete();		
	
	}
	
	private void LoadComplete()
	{
		Debug.Log("WeaveAppLoader -> LoadComplete()");	
		WeaveAppStarter appStarter = GameObject.FindGameObjectWithTag("TagWeaveAppStarter").GetComponent<WeaveAppStarter>();
		appStarter.startWeave();
	}
	
	

	void FixedUpdate()
	{
		if(Input.GetKey(KeyCode.LeftControl)) {
			ctrlDown = true;	
		} else if (Input.GetKeyUp(KeyCode.LeftControl)){
			ctrlDown = false;	
		}
		
		if(Input.GetKey(KeyCode.R)){
			rDown = true;	
		} else if (Input.GetKeyUp(KeyCode.R)) {
			rDown = false;
		}
	}
	
	void LateUpdate()
	{
		if(rDown && ctrlDown)
			Application.LoadLevel(0);
	}
}
