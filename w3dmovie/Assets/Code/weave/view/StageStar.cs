using UnityEngine;
using System.Collections;

public class StageStar : MonoBehaviour
{
    private GameObject stageStarGO;
    public void GetPosition()
    {
       float posx =
           stageStarGO.transform.position.x;
       float posy =
           stageStarGO.transform.position.y;
       float posz =
           stageStarGO.transform.position.z;
    }

    public void MovieMode()
    {
        StartCoroutine("StartMovie");
    }

    IEnumerator StartMovie()
    {
        GameObject timingcontrollergo =
            GameObject.Find("8TimingController");
        float setstarpause =
            timingcontrollergo.GetComponent<TimingController>().getSetStarPause();
        yield return new WaitForSeconds(setstarpause);
        stageStarGO =
            this.gameObject;
        stageStarGO.GetComponentInChildren<MMFocus>().MovieMode();
    }

	public PersonModel GetPersonInStar()
    {
		return stageStarGO.GetComponentInChildren<StagePerson>().GetPersonModel();	
	}

}