using UnityEngine;
using System.Collections;

public class SelectorRings : MonoBehaviour
{
	private GameObject 		starGameObject;
	private GameObject 		innerRingGameObject;
	private GameObject 		middleRingGameObject;
	private GameObject 		outerRingGameObject;
    public bool             isMiniRing = false;
	private string 		 	starName;
	static private bool		shouldRotate;

	public void Start ()
	{
        if (isMiniRing == false)
        {
            shouldRotate = true;
            innerRingGameObject = GameObject.Find("Rings.InnerRing");
            middleRingGameObject = GameObject.Find("Rings.MiddleRing");
            outerRingGameObject = GameObject.Find("Rings.OuterRing");
        }
        
	}

	public void Update ()
	{
        if (isMiniRing == false)
        {
            if (shouldRotate)
            {
               // Debug.Log("Rotating");
                innerRingGameObject.transform.Rotate(Vector3.forward * (Time.deltaTime * 20));
                middleRingGameObject.transform.Rotate(Vector3.back * (Time.deltaTime * 10));
                outerRingGameObject.transform.Rotate(Vector3.forward * (Time.deltaTime * 5));
            }
        }
        else
        { 
        }

        
	}

	public void StartRotation()
	{
		shouldRotate = true;
	}

	public void MovieMode()
    {
    	Debug.Log("SelectorRings -> MovieMode");
        Vector3 scaleVector1 = new Vector3(10, 10, 1);

        GameObject InnerRing = GameObject.Find("Rings.InnerRing");
        GameObject MiddleRing = GameObject.Find("Rings.MiddleRing");
        GameObject OuterRing = GameObject.Find("Rings.OuterRing");
        Vector3 scaleRingsVector = new Vector3(1, 1, 1);
        Hashtable scaleRings = new Hashtable();
        scaleRings.Add("scale", scaleRingsVector);
        scaleRings.Add("time", 1);
        scaleRings.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(InnerRing, scaleRings);
        iTween.ScaleTo(MiddleRing, scaleRings);
        iTween.ScaleTo(OuterRing, scaleRings);
		
        Hashtable scaleparams1 = new Hashtable();
        Hashtable moveparams = new Hashtable();

        moveparams.Add("x", 16.5);
        moveparams.Add("y", -5.5);
        moveparams.Add("z" , -26);
        moveparams.Add("easetype", "linear");
        moveparams.Add("time", 1);
        iTween.MoveTo(this.gameObject, moveparams);

		scaleparams1.Add("scale", scaleVector1);
        scaleparams1.Add("time", 3);
        scaleparams1.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(this.gameObject, scaleparams1);
  
    }

    public void MoveToStar()
    {
		
        GameObject star = GameObject.FindGameObjectWithTag("star");
        Vector3 scaleVector2 = new Vector3(.6f, .6f, .6f);
        Hashtable scaleparams2 = new Hashtable();
        star.GetComponent<StagePerson>().setVesselPosition();
        Vector3 position = star.GetComponent<StagePerson>().getVesselPosition();
        Hashtable moveparams = new Hashtable();

		shouldRotate = true;

		scaleparams2.Add("scale", scaleVector2);
        scaleparams2.Add("time", .5);
        scaleparams2.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(this.gameObject, scaleparams2);
        
        star.GetComponent<StagePerson>().setVesselPosition();
        moveparams.Add("position", position);
        moveparams.Add("time", 1);
        moveparams.Add("easetype", "easeInOutSine");
        iTween.MoveTo(this.gameObject, moveparams);  
    }
	
	
    public void MovetoCenter()
    {
		
		Debug.Log("SelectorRings -> MovetoCenter");
        Hashtable moveparams1 = new Hashtable();
		Vector3 scaleVector2 = Vector3.one;
        Hashtable scaleparams2 = new Hashtable();

		moveparams1.Add("x", 16);
        moveparams1.Add("y", -5);
        moveparams1.Add("z", -15);
        moveparams1.Add("delay", 1);
        moveparams1.Add("time", 3);
        moveparams1.Add("easetype", "easeInOutSine");
        iTween.MoveTo(this.gameObject, moveparams1);
        
		scaleparams2.Add("scale", scaleVector2);
        scaleparams2.Add("delay", 1);
        scaleparams2.Add("time", 3);
        scaleparams2.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(this.gameObject, scaleparams2);
    }
	
    public void StartPositions()
    {
       float posx = this.gameObject.transform.position.x;
       float posy = this.gameObject.transform.position.y;
       float posz = this.gameObject.transform.position.z;
    }

    public void MoveToFocus()
    {
        Debug.Log("Selector Rings: move to focus");
        Hashtable moveparams1 = new Hashtable();
		moveparams1.Add("x", -25.25);
		moveparams1.Add("y", 7.75);
        moveparams1.Add("z", -9);
        moveparams1.Add("time", .5);
        moveparams1.Add("easetype", "easeInOutSine");
        iTween.MoveTo(this.gameObject, moveparams1);
		StartCoroutine("ScaleRing");

    }

    IEnumerator ScaleRing()
    {
		Vector3 scaleVector2 = new Vector3(2,2,2);
        Hashtable scaleparams2 = new Hashtable();
        scaleparams2.Add("scale", scaleVector2);
        scaleparams2.Add("time", .5);
        scaleparams2.Add("easetype", "easeInOutSine");
		//shouldRotate = false;
        iTween.ScaleTo(this.gameObject, scaleparams2);
        yield return new WaitForSeconds(.5f);
    }

}