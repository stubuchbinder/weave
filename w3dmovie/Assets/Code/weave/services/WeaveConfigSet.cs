using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class WeaveConfigSet 
{
	private XmlNode _xmlConfigNode;
	
	public WeaveConfigSet(XmlNode thisNode)
    {
		_xmlConfigNode = thisNode;
	}
	public string GetVal( string qkey)
    {
		string qvalue = "";

		XmlNode knode = _xmlConfigNode.SelectSingleNode(qkey);
		
		if (knode != null)
        {	
			qvalue = knode.InnerText;
			
			//Debug.Log("XML Config Set >> ( qkey = " + qkey + " ) = " + qvalue );
		}
        else
        {
			//Debug.Log("XML Config Set >> Node Not Found ( qkey = " + qkey + " )" );
		}
		return qvalue;		
	}
}