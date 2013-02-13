using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {
   
    public void StartRotation()
    {
        
        Hashtable rotateparams =
            new Hashtable();
        rotateparams.Add("z", 360);
        rotateparams.Add("time", 10);
        rotateparams.Add("looptype", "loop");
        rotateparams.Add("easetype", "linear");
        iTween.RotateAdd(this.gameObject, rotateparams);
    }
}
