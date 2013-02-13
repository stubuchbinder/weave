using UnityEngine;
using System.Collections;

public class Shuttle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void WeaveMovementUp()
    {
        this.gameObject.GetComponent<SelectorRings>().enabled = false;
        Hashtable rotateparams =
            new Hashtable();
        rotateparams.Add("z", 0);
        rotateparams.Add("time", .5);
        rotateparams.Add("easetype", "linear");
        iTween.RotateTo(this.gameObject, rotateparams);

        Hashtable moveparams1 =
           new Hashtable();
        moveparams1.Add("x", 16);
        moveparams1.Add("y", -60);
        moveparams1.Add("delay", .5);
        moveparams1.Add("time", .5);
        moveparams1.Add("easetype", "easeInOutSine");
        iTween.MoveTo(this.gameObject, moveparams1);

        StartCoroutine("PauseUp");
    }

    private void WeaveMovementUpLoop()
    {
        Hashtable moveparams2 =
          new Hashtable();
        moveparams2.Add("x", 16);
        moveparams2.Add("y", 45);
        moveparams2.Add("delay", .5);
        moveparams2.Add("time", .5);
        moveparams2.Add("easetype", "easeInOutSine");
        moveparams2.Add("looptype", "pingpong");
        iTween.MoveTo(this.gameObject, moveparams2);
    }

    IEnumerator PauseUp()
    {
        yield return new WaitForSeconds(1);
        WeaveMovementUpLoop();
    }

    public void WeaveMovementSideWays()
    {
        this.gameObject.GetComponent<SelectorRings>().enabled = false;
        Hashtable rotateparams =
            new Hashtable();
        rotateparams.Add("z", 0);
        rotateparams.Add("time", .5);
        rotateparams.Add("easetype", "linear");
        iTween.RotateTo(this.gameObject, rotateparams);

        Hashtable moveparams1 =
           new Hashtable();
        moveparams1.Add("x", 123);
        moveparams1.Add("y", -8);
        moveparams1.Add("delay", 1.5);
        moveparams1.Add("time", .5);
        moveparams1.Add("easetype", "easeInOutSine");
        iTween.MoveTo(this.gameObject, moveparams1);

        StartCoroutine("PauseSideWays");
    }

    private void WeaveMovementSideWaysLoop()
    {
        GameObject lz1 = GameObject.Find("LZ1");
        GameObject lz2 = GameObject.Find("LZ2");
        Hashtable moveparams2 =
          new Hashtable();
        moveparams2.Add("x", -4);
        moveparams2.Add("y", -8);
        moveparams2.Add("delay", .5);
        moveparams2.Add("time", .5);
        moveparams2.Add("easetype", "easeInOutSine");
        moveparams2.Add("looptype", "pingpong");
        iTween.MoveTo(this.gameObject, moveparams2);

        Hashtable rotateparams =
           new Hashtable();
        rotateparams.Add("y", 90);
        rotateparams.Add("delay", .5);
        rotateparams.Add("looptype", "pingpong");
        rotateparams.Add("time", .5);
        rotateparams.Add("easetype", "linear");
        iTween.RotateTo(lz1, rotateparams);
        iTween.RotateTo(lz2, rotateparams);
    }

    IEnumerator PauseSideWays()
    {
        yield return new WaitForSeconds(1.5f);
        WeaveMovementSideWaysLoop();
    }
}