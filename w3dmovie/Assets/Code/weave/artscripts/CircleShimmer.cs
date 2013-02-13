using UnityEngine;
using System.Collections;

public class CircleShimmer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ShimmerDown();
	}

    IEnumerator ShimmerUp(){
        Hashtable fadeparams =
            new Hashtable();
        fadeparams.Add("alpha", 10);
        fadeparams.Add("time", 2.5);
        fadeparams.Add("delay", 2);
        iTween.FadeTo(this.gameObject, fadeparams);
        yield return new WaitForSeconds(5);
         ShimmerDown();
    }

    public void ShimmerDown()
    {
        Hashtable fadeparams =
            new Hashtable();
        fadeparams.Add("alpha", .1);
        //fadeparams.Add("delay", 2);
        fadeparams.Add("time", 2.5);
        iTween.FadeTo(this.gameObject, fadeparams);
        StartCoroutine("ShimmerUp");
    }

}
