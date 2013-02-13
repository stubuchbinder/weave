using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupArrangement 
{
	public GroupArrangement()
    {
		Debug.Log(">GArr> In GroupArrangement Construct");
	}

	public void enterStage()
    {	}

	public void ArrangeThePeopleDefaultPos(List<GameObject> personGOList)
    {
		Debug.Log(">GArr> ArrangeThePeople in Default Group");
		Debug.Log(">GArr> Screen.width = " + Screen.width);
	}

}