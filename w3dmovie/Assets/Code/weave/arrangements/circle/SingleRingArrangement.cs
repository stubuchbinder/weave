using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SingleRingArrangement
{
    private CircleTweener myTweener;
    public SingleRingArrangement()
    {
        Debug.Log("In SingleRingArrangement Construct");
        myTweener =
            new CircleTweener();
    }

    public void enterStage()
    {

    }

    public void ArrangeThePeople(List<GameObject> personGOList, float tweentime = 2)
    {
        Debug.Log(">> One Ring to rule them all!");
        myTweener.SetNumberOfRings(1);
        myTweener.SetScale(1f);
        myTweener.SetTweenTime(tweentime);
        myTweener.SetCenterPos(18, -5, 0);
        myTweener.SetInOutRadius(35, 35);
        myTweener.TweenThePeople(personGOList);
    }

}