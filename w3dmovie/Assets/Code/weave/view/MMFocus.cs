using UnityEngine;
using System.Collections;

public class MMFocus : MonoBehaviour 
{
    public GameObject ProfileCard;
    private GameObject stagePersonGO;
    private float posx;
    private float posy;
    private float posz;
    private Vector3 propersize;
    // Use this for initialization
	void Start () 
    {
        stagePersonGO = this.gameObject;
	}

    public void MovieMode()
    {
        StartCoroutine("FirstHalf");
    }

    IEnumerator FirstHalf()
    {
        propersize = stagePersonGO.transform.localScale;

        Hashtable scaleparams = new Hashtable();
		Hashtable moveparams = new Hashtable();
        scaleparams.Add("time", 1);
        scaleparams.Add("x", 3.5);
        scaleparams.Add("z", 3.5);
        scaleparams.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(stagePersonGO, scaleparams);
		moveparams.Add("x", -24);
		moveparams.Add("y", 7.5);
        moveparams.Add("z", -10.25);
        moveparams.Add("time", .5);
        moveparams.Add("easetype", "easeInOutSine");
        iTween.MoveTo(stagePersonGO, moveparams);
        yield return new WaitForSeconds(1);
    }

    public void Closer()
    {
        Hashtable scaleparams = new Hashtable();
		GameObject selectorring = GameObject.Find("Rings");

		selectorring.GetComponent<SelectorRings>().StartRotation();
        scaleparams.Add("scale",propersize);
        scaleparams.Add("time", .5f);
        scaleparams.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(stagePersonGO, scaleparams);

        GameObject InnerRing = GameObject.Find("Rings.InnerRing");
        GameObject MiddleRing = GameObject.Find("Rings.MiddleRing");
        GameObject OuterRing = GameObject.Find("Rings.OuterRing");
        Vector3 scaleRingsVector = new Vector3(1, 1, 1);
        Hashtable scaleRings = new Hashtable();
        scaleRings.Add("scale", scaleRingsVector);
        scaleRings.Add("time", .5f);
        scaleRings.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(InnerRing, scaleRings);
        iTween.ScaleTo(MiddleRing, scaleRings);
        iTween.ScaleTo(OuterRing, scaleRings);
       
		//Scale Selector Ring
        Vector3 scaleVector2 = new Vector3(1, 1, 1);
        Hashtable scaleparams2 = new Hashtable();
        scaleparams2.Add("scale", scaleVector2);
        scaleparams2.Add("time", .5);
        scaleparams2.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(selectorring, scaleparams2);

        Hashtable movefarm = new Hashtable();
        movefarm.Add("x",-24.25);
        movefarm.Add("y", 7.7);
        movefarm.Add("time", .5);
        iTween.MoveTo(this.gameObject, movefarm);
    }

    public void RetreatToVessel()
    {
        this.gameObject.GetComponent<StagePerson>().setVesselPosition();
        Vector3 position = this.gameObject.GetComponent<StagePerson>().getVesselPosition();

        Hashtable moveparams = new Hashtable();
        moveparams.Add("position", position);
        moveparams.Add("delay", .5f);
        moveparams.Add("time", 1);
        moveparams.Add("easetype", "easeInOutSine");
        iTween.MoveTo(stagePersonGO, moveparams);

        StartCoroutine("Parent");
    }

    IEnumerator Parent()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.GetComponent<StagePerson>().Parent();
        Hashtable reset = new Hashtable();
        reset.Add("x", .18);
        reset.Add("y", .1);
        reset.Add("z", .18);
        reset.Add("time", .1);
        reset.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(this.gameObject, reset);
    }

}