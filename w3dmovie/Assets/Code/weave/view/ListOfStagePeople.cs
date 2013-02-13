using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ListOfStagePeople : MonoBehaviour                                                  
{                                                                                                                             
    public List<Transform> listOfStagePeople;
    public List<GameObject> listOfRings;
    public List<Transform> listOfStars;
    public List<GameObject> listOfRetirees;
    private Component[] meshRenderer;                                                                                                                                                                              
    private int spaceCounter = -9;
    private int numberOfStars = 5;
    private int peopleLeft;
    private bool devmode;
    private float textspeed = 19.5f;//18
    private WeaveConfigReader configReader;
    private GameObject ProfileGui;
    

    void Start()                                                                                //As soon as the program starts running it will do all this code
    {
        ProfileGui = GameObject.Find("GUIProfileCard");

		/*
		string numberofstars1 = DataModel.Instance.NumberOfStars;
		
		Debug.Log("numberofstars1: " +numberofstars1);
		numberOfStars = Int32.Parse(numberofstars1);
		*/	

   }
	
	public int NumberOfStars
	{
		set
		{
			numberOfStars = value;	
		}
		get
		{
			return numberOfStars;	
		}
	}

    void Update()
    {

        if (devmode == true)
        {
            textspeed = 10f;
        }

    }
    public void devmodeOn()
    {
        devmode = true;
    }

    public void BeginMovieMode()                                                                //Method used to start the Movie Mode
    {
            StartCoroutine("RandomShow");                                                       //Runs the RandomShow
    }

    public void addToList(Transform person)
    {
        listOfStagePeople.Add(person);
    }

    private Transform ListofStagePeopletoPickFrom()
    {

        int randomNumber = UnityEngine.Random.Range(0, listOfStagePeople.Count - 1);
        Transform star = listOfStagePeople[randomNumber];
        listOfStagePeople.Remove(star);
        //Debug.Log("Random Person Selected = " + star.name + " : " + randomNumber);
        star.tag = "star";
        Transform glow = star.transform.FindChild("Glow");
        Transform gray = star.transform.FindChild("Gray");
        string number = "rings" + star.GetComponent<StagePerson>().getVesselName();
        GameObject Rings = GameObject.Find(number);
        Hashtable scaleParm = new Hashtable();
        scaleParm.Add("x", 3.4);
        scaleParm.Add("y", 3.4);
        scaleParm.Add("time", 1);
        scaleParm.Add("easetype", "spring");
        iTween.ScaleBy(Rings, scaleParm);
        gray.gameObject.SetActive(true);
        glow.gameObject.SetActive(true);
        return star;
    }

    IEnumerator RandomShow()                                                                    
    {
        string playTime = DataModel.Instance.PlayTime;
        textspeed = float.Parse(playTime);
        textspeed = textspeed + 6.25f;

        int check = listOfStagePeople.Count;
        if (check <= numberOfStars)
        {
            numberOfStars = check;
        }
        yield return new WaitForSeconds(2);

        for (int i = 0; i < numberOfStars; i++ )
        {
            //yield return new WaitForSeconds(1);
           	Transform startohighlight = ListofStagePeopletoPickFrom();
            //Debug.Log("Star Count = " + i);
            startohighlight.GetComponent<StagePerson>().setStarNumber(i);
            string starN = "star" + (i.ToString());
            startohighlight.name = starN;

            yield return new WaitForSeconds(1);
            startohighlight.tag = null;
            startohighlight.transform.parent = null;
           listOfStars.Add(startohighlight);
       
        }
        StartCoroutine("RecedeTheWeave");
    }

    IEnumerator RecedeTheWeave()
    {
        Debug.Log("Recede");
        GameObject logo = GameObject.Find("LogoSpace");
        GameObject selectorring = GameObject.Find("Rings");
        Hashtable moveparams = new Hashtable();
        moveparams.Add("z", -1);
        moveparams.Add("time", .5);
        moveparams.Add("easetype", "easeInOutSine");
        iTween.MoveTo(selectorring, moveparams);
        yield return new WaitForSeconds(.5f);
        Hashtable moveparams1 = new Hashtable();
        moveparams1.Add("z", 30);
        moveparams1.Add("time", 1);
        moveparams1.Add("easetype", "easeInOutSine");
        iTween.MoveTo(this.gameObject, moveparams1);
        logo.transform.parent = this.gameObject.transform;
        selectorring.transform.parent = this.gameObject.transform;
        yield return new WaitForSeconds(1);
        StartCoroutine("ShiftTheWeave");
    }

    IEnumerator ShiftTheWeave()
    {
        Debug.Log("Shift");
        Hashtable moveparams2 = new Hashtable();
        moveparams2.Add("x", 175);
        moveparams2.Add("y", -60);
        moveparams2.Add("time", 1);
        moveparams2.Add("easetype", "easeInOutSine");
        iTween.MoveTo(this.gameObject, moveparams2);
        yield return new WaitForSeconds(1);
        StartCoroutine("AlignTheStars");
    }

    IEnumerator AlignTheStars()
    {
        Debug.Log("Align");
        int x = listOfStars.Count-1;
        for (int i = 0; i <= x; i++)
        {
            string starN = "star" + (i.ToString());
            GameObject person = GameObject.Find(starN);
            string position = i.ToString();
            int location = int.Parse(position);
            location = location * 5;
            Hashtable moveparams3 = new Hashtable();
            moveparams3.Add("x", location);
            moveparams3.Add("y", -20);
            moveparams3.Add("time", 1);
            moveparams3.Add("easetype", "easeInOutSine");
           iTween.MoveTo(person, moveparams3);

            if (i == x)
            {
                StartCoroutine("BottomPosition");
            }
        }
       yield return new WaitForSeconds(.5f);

    }

    IEnumerator BottomPosition()
    {
        yield return new WaitForSeconds(1);
        int j = listOfStars.Count-1;
        for (int i = 0; i <= j; i++)
        {
            string starN = "star" + (i.ToString());
            GameObject person = GameObject.Find(starN);
            Hashtable moveparams1 = new Hashtable();
            int position = -23 + (i * 5);
            moveparams1.Add("x", position);
            moveparams1.Add("y", -34);
            moveparams1.Add("z", -21);
            moveparams1.Add("time", .5);
            moveparams1.Add("easetype", "easeInOutSine");
            iTween.MoveTo(person, moveparams1);
        }

        int x = listOfStars.Count - 1;
        for (int i = 0; i <= x; i++)
        {
            string starN = "star" + (i.ToString());
            GameObject person = GameObject.Find(starN);
            Hashtable scalepamrs = new Hashtable();
            scalepamrs.Add("x", .45);
            scalepamrs.Add("y", .25);
            scalepamrs.Add("z", .45);
            scalepamrs.Add("time", .5);
            scalepamrs.Add("easetype", "easeInOutSine");
            iTween.ScaleTo(person, scalepamrs);
        }

        StartCoroutine("CycleProfileCard");
    }
    
    IEnumerator CycleProfileCard()
    {
        GameObject selectorring = GameObject.Find("Rings");

        selectorring.transform.parent = null;
        yield return new WaitForSeconds(.5f);
        int x = listOfStars.Count - 1;
        for (int i = 0; i <= x; i++)
        {
            string starN = "star" + (i.ToString());
            GameObject person = GameObject.Find(starN);
            person.transform.parent = null;
            string number = "rings" + person.GetComponent<StagePerson>().getVesselName();
            GameObject Rings = GameObject.Find(number);
            Hashtable scaleParm = new Hashtable();
            scaleParm.Add("x", .29);
            scaleParm.Add("y", .29);
            scaleParm.Add("time", 1);
            scaleParm.Add("easetype", "spring");
            iTween.ScaleBy(Rings, scaleParm);
        }
        
        for (int i = 0; i <= x; i++)
        {
            string starN = "star" + (i.ToString());
            GameObject person = GameObject.Find(starN);
            person.GetComponent<StagePerson>().setPosition();
            Vector3 propersize = person.transform.localScale;
            //Move Selector Ring
            Vector3 position = person.GetComponent<StagePerson>().getPosition();
            Hashtable moveparams = new Hashtable();
            moveparams.Add("position", position);
            moveparams.Add("time", .5);
            moveparams.Add("easetype", "easeInOutSine");
            iTween.MoveTo(selectorring, moveparams);
            
            //Scale Selector Ring
            Vector3 scaleVector2 = new Vector3(1, 1, 1);
            Hashtable scaleparams2 = new Hashtable();
            scaleparams2.Add("scale", scaleVector2);
            scaleparams2.Add("time", .5);
            scaleparams2.Add("easetype", "easeInOutSine");
            iTween.ScaleTo(selectorring, scaleparams2);

            GameObject InnerRing = GameObject.Find("Rings.InnerRing");
            GameObject MiddleRing = GameObject.Find("Rings.MiddleRing");
            GameObject OuterRing = GameObject.Find("Rings.OuterRing");
            Vector3 scaleRingsVector = new Vector3(3.8f, 3.8f, 3.8f);
            Hashtable scaleRings = new Hashtable();
            scaleRings.Add("scale", scaleRingsVector);
            scaleRings.Add("time", 1);
            scaleRings.Add("delay", .5);
            scaleRings.Add("easetype", "easeInOutSine");
            iTween.ScaleTo(InnerRing, scaleRings);
            scaleRings.Remove("scale");
            scaleRingsVector = new Vector3(3.6f, 3.6f, 3.6f);
            scaleRings.Add("scale", scaleRingsVector);
            iTween.ScaleTo(MiddleRing, scaleRings);
            scaleRings.Remove("scale");
            scaleRingsVector = new Vector3(3.7f, 3.7f, 3.7f);
            scaleRings.Add("scale", scaleRingsVector);
            iTween.ScaleTo(OuterRing, scaleRings);
            yield return new WaitForSeconds(.5f);
            Transform glow = person.transform.FindChild("Glow");
            glow.GetComponent<MeshRenderer>().enabled = true;
            person.GetComponentInChildren<MMFocus>().MovieMode();
            selectorring.GetComponent<SelectorRings>().MoveToFocus();
            ProfileGui.GetComponent<ProfileGUI>().WarmUp(person);
            yield return new WaitForSeconds(1);
            ProfileGui.GetComponent<ProfileGUI>().StartCoroutine("DrawIt");

            for (int y = 1 + i; y <= x; y++) //Shifts People Over
            {
                string personLeftOver = "star" + (y.ToString());
                GameObject miniringLeftOver = GameObject.Find(personLeftOver);
                Hashtable moveparams3 = new Hashtable();
                moveparams3.Add("x", 5);
                moveparams3.Add("time", .5);
                moveparams3.Add("easetype", "easeInOutSine");
                iTween.MoveAdd(miniringLeftOver, moveparams3);
            }

            yield return new WaitForSeconds(textspeed); ///18

            spaceCounter = spaceCounter + 5;
            Hashtable moveparams1 = new Hashtable();
            moveparams1.Add("x", -3);
            moveparams1.Add("y", -34);
            moveparams1.Add("z", -21);
            moveparams1.Add("time", .5);
            moveparams1.Add("easetype", "easeInOutSine");
            iTween.MoveTo(person, moveparams1);
           
            listOfRetirees.Add(person);
            int count = listOfRetirees.Count;

            for (int z = 0; z < count; z++) //Shifts second group over
            {
                string personRetOver = "star" + (z.ToString());
                GameObject Retiree = GameObject.Find(personRetOver);
                Hashtable moveparams3 = new Hashtable();
                moveparams3.Add("delay", 1.5);
                moveparams3.Add("x", 5);
                moveparams3.Add("time", .5);
                moveparams3.Add("easetype", "easeInOutSine");
                iTween.MoveAdd(Retiree, moveparams3);
            }
            Transform gray = person.transform.FindChild("Gray");
            gray.GetComponent<MeshRenderer>().enabled = true;
            glow.GetComponent<MeshRenderer>().enabled = true;
            selectorring.transform.parent = null;
            glow.GetComponent<MeshRenderer>().enabled = false;
        }

        if (listOfStagePeople.Count > 0)
        {
            int tally = listOfRetirees.Count;
            for (int z = 0; z < tally; z++) //Shifts second group over
            {
                string personRetOver = "star" + (z.ToString());
                GameObject Retiree = GameObject.Find(personRetOver);
                Transform gray =  Retiree.transform.FindChild("Gray");
                gray.gameObject.SetActive(false);
            }

            listOfRetirees.Clear();
            spaceCounter = -9;

            Hashtable moveparams3 = new Hashtable();
            moveparams3.Add("x", 0);
            moveparams3.Add("y", 0);
            moveparams3.Add("z", 0);
            moveparams3.Add("time", 1);
            moveparams3.Add("easetype", "easeInOutSine");
            iTween.MoveTo(this.gameObject, moveparams3);
            yield return new WaitForSeconds(1);
            ReturnToHome();
            StartCoroutine("RandomShow");
            GameObject Rings = GameObject.Find("Rings");
            Rings.GetComponent<SelectorRings>().MovieMode();
        }

        if (listOfStagePeople.Count <= 4)
        {
            int tally = listOfRetirees.Count;
            for (int z = 0; z < tally; z++) //Shifts second group over
            {
                string personRetOver = "star" + (z.ToString());
                GameObject Retiree = GameObject.Find(personRetOver);
                Transform gray = Retiree.transform.FindChild("Gray");
                gray.gameObject.SetActive(false);
            }

           
            listOfRetirees.Clear();
            spaceCounter = -9;

            Hashtable moveparams3 = new Hashtable();
            moveparams3.Add("x", 0);
            moveparams3.Add("y", 0);
            moveparams3.Add("z", 0);
            moveparams3.Add("time", 1);
            moveparams3.Add("easetype", "easeInOutSine");
            iTween.MoveTo(this.gameObject, moveparams3);
            yield return new WaitForSeconds(1);
            ReturnToHome();
            GameObject Rings = GameObject.Find("Rings");
            Rings.GetComponent<SelectorRings>().MovieMode();
        }
    }

    private void TurnOffGray()
    {
    }

    private Vector3 Vector3(float p, float p_2, float p_3)
    {
        throw new NotImplementedException();
    }

    private void ReturnToHome()
    {
        int x = listOfStars.Count;
        for (int y = 0; y < x; y++)
        {
            string starN = "star" + (y.ToString());
            GameObject person = GameObject.Find(starN);
            person.GetComponent<MMFocus>().RetreatToVessel();
            listOfStars.Remove(person.transform);

            person.name = "Retiree";
        }
    }

   }