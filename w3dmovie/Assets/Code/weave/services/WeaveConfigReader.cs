using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class WeaveConfigReader 
{
	private XmlDocument _xmlConfig;
	public WeaveConfigReader()
    {
		Debug.Log("In WeaveConfigReader Construct");
	}
	
	
	public XmlDocument XMLConfig
	{
		get
		{
			return _xmlConfig;	
		}
		set
		{
			_xmlConfig = value;	
		}
	}
	

	
	public void loadXMLConfigFromFile(string xmlfilename)
    {
		Debug.Log("readConfigFromXML = " + xmlfilename);

		TextAsset textAsset = (TextAsset)Resources.Load(xmlfilename);  
		_xmlConfig = new XmlDocument();
		_xmlConfig.LoadXml(textAsset.text);
	}
	


	public string getTopLevelConfigVal(string toplevelkey)
    {
		string configPath = "weave/";
		string toplevelvalue = "";
		configPath += toplevelkey;
		XmlNode nodeToFind = _xmlConfig.SelectSingleNode(configPath);
		if (nodeToFind != null)
        {
			toplevelvalue = nodeToFind.InnerText;
			Debug.Log("XML Config " + "  ( " + configPath + " )  = " + toplevelvalue );
		} 
        else 
        {
			Debug.Log("XML Config " + " >>  " + configPath + " >> NODE NotFoundException");
		}
		return toplevelvalue;	
	}

	public WeaveConfigSet GetQuestionConfig(int qnumber)
    {
		WeaveConfigSet qconfig = null;
		XmlNode qconfignode;
 		XmlNodeList questionNodes = _xmlConfig.GetElementsByTagName( "question" );
		// returns all question tags no matter where they are in the hierarchy
		Debug.Log("XML Config >> questionNodes.Count = " + questionNodes.Count );
		if ( qnumber < questionNodes.Count){
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