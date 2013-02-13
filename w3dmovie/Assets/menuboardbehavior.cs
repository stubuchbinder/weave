using UnityEngine;
using System.Collections;

public class menuboardbehavior : MonoBehaviour
{
    private Component[] meshRenderer;
    private bool ShowHideStatus = false;
    public bool IsSubButton = false;
    public bool IsPlayButton = false;
    public bool IsFFButton = false;
    public Texture PrimaryTexture;
    public Texture SecondaryTexture;
    private float CurrentTimeScale = 1;
    private int PlayCounter = 0;

    void Start()
    {
        StartCoroutine("FirstPop");
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Vector3 postion = new Vector3();
            if (ShowHideStatus == false)
            {
                postion = new Vector3(-57, -51, 0);
                MoveMenu(postion);
                ShowHideStatus = true;
            }
            else
            {
                postion = new Vector3(-74, -55, 0);
                MoveMenu(postion);
                ShowHideStatus = false;
            }
        }
    }

    IEnumerator FirstPop()
    {
        yield return new WaitForSeconds(5);
        Vector3 postion = new Vector3();
        postion = new Vector3(-57, -51, 0);
        MoveMenu(postion);
        ShowHideStatus = true;

    }

    private void OnMouseDown()
    {
        Vector3 postion = new Vector3();
        if (IsSubButton == false)
        {
            

            if (ShowHideStatus == false)
            {
                postion = new Vector3(-57, -51, 0);
                MoveMenu(postion);
                ShowHideStatus = true;
            }
            else
            {
                postion = new Vector3(-74, -55, 0);
                MoveMenu(postion);
                ShowHideStatus = false;
            }
 
        }

        if (IsSubButton == true && IsPlayButton == true)
        {
            if (PlayCounter == 0) //play
            {
                GameObject stageDirector = GameObject.Find("2StageDirector");
                stageDirector.GetComponent<StageDirector>().RollMovie();
                GameObject Logo = GameObject.Find("LogoSpace");
                Hashtable moveparams2 = new Hashtable();
                moveparams2.Add("x", 18);
                moveparams2.Add("Y", -5);
                moveparams2.Add("z", 0);
                moveparams2.Add("time", 4);
                moveparams2.Add("easetype", "linear");
                iTween.MoveTo(Logo, moveparams2);
                GameObject StagePeople = GameObject.Find("4ListOfStagePeople");
                meshRenderer = StagePeople.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mesh in meshRenderer)
                {
                    mesh.enabled = true;
                }

                postion = new Vector3(-74, -55, 0);
                this.gameObject.renderer.material.mainTexture = SecondaryTexture;
                MoveMenu(postion);

                PlayCounter++;
            }
            else if (PlayCounter % 2 == 0) //unpause
            {
                Time.timeScale = CurrentTimeScale;
                this.gameObject.renderer.material.mainTexture = SecondaryTexture;
                PlayCounter++;
                postion = new Vector3(-71, -55, 0);
                MoveMenu(postion);
            }

            else //pause
            {
                Time.timeScale = 0;
                this.gameObject.renderer.material.mainTexture = PrimaryTexture;
                PlayCounter++;
            }

        }

        if (IsSubButton == true && IsFFButton == true)
        {
            CurrentTimeScale = CurrentTimeScale + .1f;
            if (CurrentTimeScale >= 2)
            {
                CurrentTimeScale = 2;
                this.gameObject.renderer.material.mainTexture = SecondaryTexture;
            }
            Time.timeScale = CurrentTimeScale;
            Debug.Log(Time.timeScale);
        }

    }

    private void MoveMenu(Vector3 Pos)
    {
        GameObject MenuBoard = GameObject.Find("MenuBoard");
        Hashtable posParameters = new Hashtable();
        posParameters.Add("position", Pos);
        posParameters.Add("time", .5);
        posParameters.Add("easetype", "spring");
        iTween.MoveTo(MenuBoard, posParameters);
    }
}