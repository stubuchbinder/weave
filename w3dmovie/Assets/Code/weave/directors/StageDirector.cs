using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDirector : MonoBehaviour
{
    public GameObject stagePersonToCopy;
    private float peopleCounter = 0;
    private int stagePersonCounter = 0;
    private int questionNumber;
    private WeaveConfigReader configReader;
    public List<PersonModel> personData;
    private List<GameObject> allStagePersonObjectsList;
    public MovieDirector movieDirector;
	
	public bool logoIsLoaded = false;

    void Start()
    {
    }

	public void CreateLogo(string logoURL)
	{
		GameObject logoSpaceGameObject = GameObject.Find("LogoGameObject");
		logoSpaceGameObject.GetComponent<PhotoGrabber>().url = logoURL;
        logoSpaceGameObject.GetComponent<PhotoGrabber>().enabled = true;
        string scale = DataModel.Instance.LogoSize;
        float properScale = float.Parse(scale);
        Vector3 pscale = new Vector3(properScale, properScale, properScale);
        logoSpaceGameObject.transform.localScale = pscale;
		logoSpaceGameObject.renderer.enabled = true;
		
		logoIsLoaded = true;

        StartCoroutine("positionLogo");
	}

    private IEnumerator positionLogo()
    {
        yield return new WaitForSeconds(3);
        GameObject Logo = GameObject.Find("LogoSpace");
        Hashtable moveparams2 = new Hashtable();
        moveparams2.Add("x", 15);
        moveparams2.Add("Y", -6);
        moveparams2.Add("z", -51.5);
        moveparams2.Add("time", 3.5f);
        moveparams2.Add("easetype", "linear");
        iTween.MoveTo(Logo, moveparams2);
       
    }

    private void CreateStagePerson(PersonModel person)
    {
		
		//Debug.Log("CreateStagePerson");
        Vector3 position = new Vector3((3f - (peopleCounter * 1.5f)), 0f, 0f);

        //Debug.Log("stagePersonToCopy = " + stagePersonToCopy);
        GameObject newStagePerson = (GameObject)Instantiate(stagePersonToCopy, position, Quaternion.identity);
        GameObject _listOfStagePeople = GameObject.Find("4ListOfStagePeople");
        //pull name
        string name = person.GetName();
        string url = person.GetPhotoURL();
		/*if (name == "\n")
        {
            Destroy(newStagePerson);
        }
        else
        {*/
      //  Debug.Log("**** url = " + url);
		
        newStagePerson.transform.FindChild("StagePersonGO").GetComponent<StagePerson>().setName(name);
        newStagePerson.transform.FindChild("StagePersonGO").GetComponent<PhotoGrabber>().url = url; // REFATOR NO PUBLIC VARIABLES!!
        newStagePerson.transform.FindChild("StagePersonGO").GetComponent<StagePerson>().SetPersonModel(person);

        stagePersonCounter = stagePersonCounter + 1;

      //  Debug.Log("CreateStagePerson >> " + name + " _stagepersoncounter: " + stagePersonCounter);

        allStagePersonObjectsList.Add(newStagePerson); //Stage Person Added
        Transform per = newStagePerson.transform.FindChild("StagePersonGO");
        GameObject rings = GameObject.Find("MiniRingV2");
        string tagger = "rings" + stagePersonCounter.ToString();
        rings.name = tagger;
        per.GetComponent<MeshRenderer>().enabled = false;
        _listOfStagePeople.GetComponent<ListOfStagePeople>().addToList(per);
        newStagePerson.name = stagePersonCounter.ToString();
        newStagePerson.GetComponent<Vessel>().setVesselNumber(stagePersonCounter);
        newStagePerson.GetComponentInChildren<StagePerson>().setVesselName(stagePersonCounter.ToString());
        newStagePerson.transform.parent = _listOfStagePeople.transform;
        Transform ring = per.transform.FindChild("Ring");
    }
    //}

    public void HereIsTheWeaveData(Weave weaveData)
    {
        personData = weaveData.getPersonList();
        allStagePersonObjectsList = new List<GameObject>();

        foreach (PersonModel person in personData)
        {
           
                CreateStagePerson(person);
                peopleCounter++;
            
        }

        ArrangeStart();
        ArrangeInCircleDefault();
    }

    public void ArrangeStart()
    {
        Debug.Log("ArrangeStart - Not a circle");

        foreach (GameObject pgo in allStagePersonObjectsList)
        {
            Hashtable moveparams = new Hashtable();
            moveparams.Add("x", 10);
            moveparams.Add("y", 10);
            moveparams.Add("z", 40);
            moveparams.Add("time", .1);
            iTween.MoveTo(pgo, moveparams);
        }
    }

    public void ArrangeInCircleDefault()
    {
        CircleArrangement testCircleArranger = new CircleArrangement();
        testCircleArranger.ArrangeThePeopleDefaultPos(allStagePersonObjectsList);

    }

    public void ArrangeInCircleHere(float xc, float yc, float zc, float inrad, float outrad)
    {
        CircleArrangement testCircleArranger = new CircleArrangement();
        testCircleArranger.ArrangeThePeopleHere(allStagePersonObjectsList, xc, yc, zc, inrad, outrad);
    }

    public void ArrangeInRing()
    {
        SingleRingArrangement ringArranger = new SingleRingArrangement();
        ringArranger.ArrangeThePeople(allStagePersonObjectsList);
    }

    public void RollMovie()
    {
        GameObject sr = GameObject.Find("Rings");
        movieDirector = new MovieDirector();

        movieDirector.RollMovie(allStagePersonObjectsList);
        movieDirector.RandomPersonCycle();
        sr.GetComponent<SelectorRings>().MovieMode();
    }

    internal void ArrangeInRing(int p, int p_2, int p_3, int p_4, int p_5)
    {
        throw new System.NotImplementedException();
    }

    public void ArrangeOnGroupMap()
    {
        GroupArrangement testGArranger = new GroupArrangement();
        testGArranger.ArrangeThePeopleDefaultPos(allStagePersonObjectsList);
    }
	
	

}