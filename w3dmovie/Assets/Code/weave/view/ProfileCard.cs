using UnityEngine;
using System.Collections;

public class ProfileCard : MonoBehaviour {

    private GameObject star;

    void Start()
    {
      //  this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        Hashtable fadeparams =
            new Hashtable();
        fadeparams.Add("alpha", .5);
        fadeparams.Add("time", 1);
        iTween.FadeTo(this.gameObject, fadeparams);
    }

    public void LaunchPC(GameObject person)
    {
        star = person;
        StartCoroutine("EndShow"); 
    }

    IEnumerator EndShow()
    {
        //gameObject.GetComponent<Text>().WarmUp(star);
    	yield return new WaitForSeconds(0.5f);
    	//gameObject.GetComponent<Text>().ActivatePersonCard(star);
    }

    public void CloseProfileCard()
    {
        Hashtable moveparams = new Hashtable();
        Vector3 scalevector1 = new Vector3(2, 1, .05f);
        Hashtable scaleparams1 =  new Hashtable();

		moveparams.Add("x", 26);
        moveparams.Add("y", -25);
        moveparams.Add("time", 2);
       // moveparams.Add("delay", 3);
        moveparams.Add("easetype", "easeInOutSine");
		iTween.MoveTo(this.gameObject, moveparams);
        
		scaleparams1.Add("scale", scalevector1);
        scaleparams1.Add("time", 2);
        //scaleparams1.Add("delay", 3);
        scaleparams1.Add("easetype", "easeInOutSine");
        iTween.ScaleTo(this.gameObject, scaleparams1);
        
		star.GetComponent<MMFocus>().Closer();
        StartCoroutine("KillCard");
    }

    IEnumerator KillCard()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

}