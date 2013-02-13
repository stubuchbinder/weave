using UnityEngine;
using System.Collections;
using System;

public class MenuGui : MonoBehaviour
{
    private bool movieModeStatus = false;
    public bool devMode = true;
    private Component[] meshRenderer;
    public GameObject GUIProfile;
    public GUISkin WeaveSkin;
    public Texture btntexture;
    public Texture btntexture2;
    public Rect btnRec = new Rect(-100, 10, 150, 1000);
    public Rect btnRec2 = new Rect(10, 10, 150, 100);

    void OnGUI()
    {
        GUI.skin = WeaveSkin;
        GameObject stageDirector = GameObject.Find("2StageDirector");
        
        /*if (GUI.Button(btnRec, " "))
        {
            Hashtable value = new Hashtable();
            value.Add("from", btnRec);
            value.Add("to", btnRec2);
            value.Add("onupdate", "resize");
            value.Add("time", .5);
            value.Add("easetype", "spring");
            iTween.ValueTo(gameObject, value);
            if (GUI.Button(new Rect(200,200,150,150),btntexture))
            {
                stageDirector.GetComponent<StageDirector>().RollMovie();
                GameObject Logo = GameObject.Find("LogoSpace");
                Hashtable moveparams2 =
                       new Hashtable();
                moveparams2.Add("x", 18);
                moveparams2.Add("Y", -5);
                moveparams2.Add("z", 0);
                moveparams2.Add("time", 3.5f);
                moveparams2.Add("easetype", "linear");
                iTween.MoveTo(Logo, moveparams2);
                movieModeStatus = true;

                GameObject StagePeople = GameObject.Find("4ListOfStagePeople");
                meshRenderer = StagePeople.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mesh in meshRenderer)
                {
                    mesh.enabled = true;
                }
            }
        }*/


        
        if (devMode == false)
        {
           if (GUI.Button(new Rect(10, Screen.height - 100, 150, 100), btntexture2))
            {
            devMode = true;
            GameObject StagePeople = GameObject.Find("4ListOfStagePeople");
            StagePeople.GetComponent<ListOfStagePeople>().devmodeOn();
            }

        }
      //  if (movieModeStatus == false)
        //{
       
            if (GUI.Button(btnRec,btntexture))
            {

                Hashtable value = new Hashtable();
                value.Add("from", btnRec);
                value.Add("to", btnRec2);
                value.Add("onupdate", "resize");
                value.Add("time", .5);
                value.Add("easetype", "spring");
                iTween.ValueTo(gameObject, value);

               /* stageDirector.GetComponent<StageDirector>().RollMovie();
                GameObject Logo = GameObject.Find("LogoSpace");
                Hashtable moveparams2 =
                       new Hashtable();
                moveparams2.Add("x", 18);
                moveparams2.Add("Y", -5);
                moveparams2.Add("z", 0);
                moveparams2.Add("time", 3.5f);
                moveparams2.Add("easetype", "linear");
                iTween.MoveTo(Logo, moveparams2);
                movieModeStatus = true;

                GameObject StagePeople = GameObject.Find("4ListOfStagePeople");
               meshRenderer = StagePeople.GetComponentsInChildren<MeshRenderer>();
               foreach (MeshRenderer mesh in meshRenderer)
               {
                   mesh.enabled = true;
               }

            }/*
            string exitbutton = DataModel.Instance.ExitButton;
            bool ebutton = Convert.ToBoolean(exitbutton);

            if (ebutton == true)
            {
                if (GUI.Button(new Rect(10, Screen.height - 60, 100, 40), "Exit"))
                {
                    Application.ExternalEval("closeWin");
                }
            }*/
        }
     
    }

    void resize(Rect size)
    {
        btnRec = size;
    }
	
}