using UnityEngine;
using System.Collections;

public class TimingController : MonoBehaviour 
{
    private float PreShowPause = 8.5f;
    private float SetStarPause = 0.5f;
    private float MoveStarPause = 1.5f;
    private float ScaleStarPause = 8.5f;
    private float UnScaleStarPause = 2.5f;
    private float RetreatStarPause = 2.5f;
    private float NamePause = 3.0f;
    private float DisplayNamePause = 10.0f;
    private float AnswerPause = 2.0f;
    private float CreateAnswerPause = 1.5f;
    private float DisplayAnswerPause = 8.0f;
    private float ApplyForcePause = 2.5f;
    private float HideBubblePause = 0.5f;
    private float DisplayStarPause = 10.0f;
	void Start () 
    {
     PreShowPause = 3f;
     SetStarPause = 0.5f;
     MoveStarPause = 2f;
     ScaleStarPause = 8.5f;
     UnScaleStarPause = 1f;
     RetreatStarPause = 2f;
     NamePause = 6f;
     DisplayNamePause = 6f;
     AnswerPause = 5.0f;
     CreateAnswerPause = 2f;
     DisplayAnswerPause = 5.0f;
     ApplyForcePause = 0.0f;
     HideBubblePause = 4.0f;
     DisplayStarPause = 15.0f;
	}
    public float getPreShowPause()
    {
        return PreShowPause;
    }
    public float getSetStarPause()
    {
        return SetStarPause;
    }
    public float getMoveStarPause()
    {
        return MoveStarPause;
    }
    public float getScaleStarPause()
    {
        return ScaleStarPause;
    }
    public float getUnScaleStarPause()
    {
        return UnScaleStarPause;
    }
    public float getRetreatStarPause()
    {
        return RetreatStarPause;
    }
    public float getNamePause()
    {
        return NamePause;
    }
    public float getDisplayNamePause()
    {
        return DisplayNamePause;
    }
    public float getAnswerPause()
    {
        return AnswerPause;
    }
    public float getCreateAnswerPause()
    {
        return CreateAnswerPause;
    }
    public float getDisplayAnswerPause()
    {
        return DisplayAnswerPause;
    }
    public float getApplyForcePause()
    {
        return ApplyForcePause;
    }
    public float getHideBubblePause()
    {
        return HideBubblePause;
    }
    public float getDisplayStarPause()
    {
        return DisplayStarPause;
    }

}