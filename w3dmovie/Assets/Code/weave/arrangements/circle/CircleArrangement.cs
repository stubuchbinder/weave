using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CircleArrangement 
{
	private CircleTweener myTweener;

	public CircleArrangement()
    {	
        string numberofrings1 = DataModel.Instance.NumberOfRings; 
        int numberofrings = Int32.Parse(numberofrings1);
		//Debug.Log("In CircleArrangement Construct");
		myTweener = new CircleTweener();
        myTweener.SetNumberOfRings(numberofrings);
	}

	public void enterStage()
    {	}

	public void ArrangeThePeopleDefaultPos(List<GameObject> personGOList)
    {
		//Debug.Log(">> ArrangeThePeople in Default Circle");
		//Debug.Log(">> Screen.width = " + Screen.width);
        string innerradi = DataModel.Instance.InnerCircleRadius;
        string outerradi = DataModel.Instance.OuterCircleRadius;
        string scale = DataModel.Instance.PeopleSize;
        int inner = int.Parse(innerradi);
        int outer = int.Parse(outerradi);
        float size = float.Parse(scale);
		myTweener.SetCenterPos( 18,  -5,  0);
		myTweener.SetInOutRadius( inner,  outer);
        myTweener.SetScale(size);
		myTweener.TweenThePeople(personGOList);
	}

	public void ArrangeThePeopleHere(List<GameObject> personGOList, float xc, float yc, float zc , float inrad, float outrad)
    {
		//Debug.Log(">> ArrangeThePeople in Circle");
		myTweener.SetCenterPos( xc,  yc,  zc);
		myTweener.SetInOutRadius( inrad,  outrad);
		myTweener.TweenThePeople(personGOList);
	}

}