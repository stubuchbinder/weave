using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weave 
{
	private string _weavename;

    private List<PersonModel> _weavePeople;
	
	public Weave(string wname = "")
    {
		Debug.Log("In Weave Construct");
		
		_weavePeople = null;
		_weavename = wname;
	}
	public void setPersonList(List<PersonModel> plist)
    {
			_weavePeople = plist;
	}
	public List<PersonModel> getPersonList()
    {
		return _weavePeople;
	}	
	public void setName(string wname)
    {
			_weavename = wname;
	}
	public string getName() 
    {
		return _weavename;
	}		
}