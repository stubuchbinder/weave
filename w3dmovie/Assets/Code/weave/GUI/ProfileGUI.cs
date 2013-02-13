using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class ProfileGUI : MonoBehaviour
{

    private bool isOn = false;
    public GUISkin Skin;
    private float hieght;
    private float width;
    private double orginalWidth = 1920;
    private double orginalHeight = 1200;
    private double startWidth;
    private double startHeight;
    private double orginalArea;
    private double startArea;
    private double textScale;
    private double startprecentage;
    private float CalcHeight = 0;
    private float PixelDef;
    private int s;
    private double x;
    private double y;
    private double z;
    private int spaceA;
    private int spaceB;
    private float sizeheight;
    private float sizewidth;
    private float textspeed = 12.5f;
    private GameObject _nameGO;
    private GameObject _weaveAppStarterGO;
    private GameObject _stageDirectorGO;
    private GameObject star;
    private WeaveConfigSet _nameConfig;
    private WeaveConfigSet _answerConfig;
    private ConfigAnswerFormat _answerConfigFormatter;
    private WeaveConfigSet _questionConfig;
    private string Question1;//
    private string Question2;//
    private string Question3;//
    private string Answer1;//
    private string Answer2;//
    private string Answer3;//
    GameObject animatedSpriteGameObject;
    tk2dAnimatedSprite animatedSprite;
    private bool isWarm = false;
    private float ProperPlayTime = 12.5f;

    void Start()
    {
        startHeight = Screen.height;
        startWidth = Screen.width;
        orginalArea = orginalHeight * orginalWidth;
        startArea = startHeight * startWidth;
        textScale = (startArea * 100) / orginalArea;
        textScale = Math.Truncate(textScale);
        startprecentage = textScale;
        _stageDirectorGO = GameObject.FindWithTag("TagStageDirector");
        _weaveAppStarterGO = GameObject.FindWithTag("TagWeaveAppStarter");
        animatedSpriteGameObject = GameObject.Find("AnimatedSprite");
        animatedSprite = animatedSpriteGameObject.GetComponent<tk2dAnimatedSprite>();
        animatedSprite.Play("CardAnimation");
        
    }

    void OnGUI()
    {
        GUI.skin = Skin;
        GUIStyle Question = Skin.customStyles[0];
        GUIStyle Answer = Skin.customStyles[1];
        Answer.clipping = TextClipping.Overflow;
        double currentprecentage = textScale;

        if (isWarm == true)
        {
            x = (45 * currentprecentage) / 100;
            int s = Convert.ToInt32(x);
            if (s >= 43)
            {
                s = 43;
            }
            if (s <= 15)
            {
                s = 15;
            }
            Question.fontSize = s - 7;
            Answer.fontSize = s + 4;
             y = (5 * currentprecentage) / 100; //10
             spaceA = Convert.ToInt32(y);
             z = (50 * currentprecentage) / 100; //20
             spaceB = Convert.ToInt32(z);
            CalcHeight = (spaceA * 3) + (spaceB * 2);
            hieght = ((Screen.height / 4));
            width = (Screen.width / 1.9f);
            sizeheight = hieght * 3;
            sizewidth = (int)Math.Round(width * .81f, 0) - 35;
            CalcHeight = CalcHeight + Question.CalcHeight(new GUIContent(Question1), sizewidth);
            CalcHeight = CalcHeight + Question.CalcHeight(new GUIContent(Question2), sizewidth);
            CalcHeight = CalcHeight + Question.CalcHeight(new GUIContent(Question3), sizewidth);
            CalcHeight = CalcHeight + Answer.CalcHeight(new GUIContent(Answer1), sizewidth);
            CalcHeight = CalcHeight + Answer.CalcHeight(new GUIContent(Answer2), sizewidth);
            CalcHeight = CalcHeight + Answer.CalcHeight(new GUIContent(Answer3), sizewidth);
            //Debug.Log("Height = " + CalcHeight);

            PixelDef = (CalcHeight * 150) / 675;

            string playTime = DataModel.Instance.PlayTime;
            ProperPlayTime = float.Parse(playTime);
            //ProperPlayTime = ProperPlayTime + 2;
        }
        
        if (isOn == true)
        {
           
            
            GUILayout.BeginArea(new Rect(width, hieght + 20, sizewidth, sizeheight));
            GUILayout.Label(Question1, Question);
            GUILayout.Space(spaceA);
            GUILayout.Label(Answer1, Answer);
            GUILayout.Space(spaceB);
            GUILayout.Label(Question2, Question);
            GUILayout.Space(spaceA);
            GUILayout.Label(Answer2, Answer);
            GUILayout.Space(spaceB);
            GUILayout.Label(Question3, Question);
            GUILayout.Space(spaceA);
            GUILayout.Label(Answer3, Answer);
            
           
            
            GUILayout.EndArea();
            
        }
    }

    public void WarmUp(GameObject person)
    {
        star = person;
        _questionConfig = _weaveAppStarterGO.GetComponent<WeaveAppStarter>().GetWeaveConfigForQuestion(0);
        _answerConfig = _weaveAppStarterGO.GetComponent<WeaveAppStarter>().GetWeaveConfigForQuestion(0);
        _nameConfig = _weaveAppStarterGO.GetComponent<WeaveAppStarter>().GetWeaveConfigForQuestion(0);
        string qText = _questionConfig.GetVal("questiontext");
        string qName = _questionConfig.GetVal("qname");
        string ftextInit = "";
        Question1 = qText;
        ftextInit += qText;
        _questionConfig = _weaveAppStarterGO.GetComponent<WeaveAppStarter>().GetWeaveConfigForQuestion(1);
        _answerConfigFormatter = new ConfigAnswerFormat(_answerConfig);
        PersonModel thestar = star.GetComponent<StagePerson>().GetPersonModel();

        string answerToDisplay = _answerConfigFormatter.GetFormattedAnswer(thestar);
        string aName = _answerConfig.GetVal("aname");
        ftextInit = "";
        Answer1 = answerToDisplay;
        ftextInit += answerToDisplay;
        //yield return new WaitForSeconds(.1f);
        _answerConfig = _weaveAppStarterGO.GetComponent<WeaveAppStarter>().GetWeaveConfigForQuestion(1);

        qText = _questionConfig.GetVal("questiontext");
        qName = _questionConfig.GetVal("qname");
        ftextInit = "";
        Question2 = qText;
        ftextInit += qText;
        //Debug.Log(ftextInit);
        _questionConfig = _weaveAppStarterGO.GetComponent<WeaveAppStarter>().GetWeaveConfigForQuestion(2);

        _answerConfigFormatter = new ConfigAnswerFormat(_answerConfig);
        answerToDisplay = _answerConfigFormatter.GetFormattedAnswer(thestar);
        aName = _answerConfig.GetVal("aname");
        ftextInit = "";
        Answer2 = answerToDisplay;
        ftextInit += answerToDisplay;
        _answerConfig = _weaveAppStarterGO.GetComponent<WeaveAppStarter>().GetWeaveConfigForQuestion(2);

        qText = _questionConfig.GetVal("questiontext");
        qName = _questionConfig.GetVal("qname");
        ftextInit = "";
        Question3 = qText;
        ftextInit += qText;

        _answerConfigFormatter = new ConfigAnswerFormat(_answerConfig);
        answerToDisplay = _answerConfigFormatter.GetFormattedAnswer(thestar);
        aName = _answerConfig.GetVal("aname");
        ftextInit = "";
        Answer3 = answerToDisplay;

        isWarm = true;
    }

    IEnumerator DrawIt()
    {
        GameObject Line1 = GameObject.Find("Line1");
        StartCoroutine("turnonline1");
        Hashtable Line1Scale = new Hashtable();
        Line1Scale.Add("x", 20);
        Line1Scale.Add("time", .5);
        Line1Scale.Add("easetype", "linear");
        Line1Scale.Add("delay", .5);
        iTween.ScaleTo(Line1, Line1Scale);

        Hashtable Line1Move = new Hashtable();
        Line1Move.Add("x", -1);
        Line1Move.Add("time", .5);
        Line1Move.Add("easetype", "linear");
        Line1Move.Add("delay", .5);
        iTween.MoveTo(Line1, Line1Move);

        GameObject Line2 = GameObject.Find("Line2");
        StartCoroutine("turnonline2");

        Hashtable Line2Scale = new Hashtable();
        Line2Scale.Add("x", 13);
        Line2Scale.Add("time", .5);
        Line2Scale.Add("easetype", "linear");
        Line2Scale.Add("delay", 1);
        iTween.ScaleTo(Line2, Line2Scale);

        Hashtable Line2Move = new Hashtable();
        Line2Move.Add("x", 13.4);
        Line2Move.Add("y", 10.95);
        Line2Move.Add("time", .5);
        Line2Move.Add("easetype", "linear");
        Line2Move.Add("delay", 1);
        iTween.MoveTo(Line2, Line2Move);

        GameObject ProfileCardBG = GameObject.Find("ProfileCardBG");
        StartCoroutine("turnonpcbg");

        Hashtable PCBGScale1 = new Hashtable();
        PCBGScale1.Add("x", 59);
        PCBGScale1.Add("time", 1);
        PCBGScale1.Add("easetype", "linear");
        PCBGScale1.Add("delay", 1.5);
        iTween.ScaleTo(ProfileCardBG, PCBGScale1);

        Hashtable PCBGMove1 = new Hashtable();
        PCBGMove1.Add("x", 47.33);
        PCBGMove1.Add("time", 1);
        PCBGMove1.Add("easetype", "linear");
        PCBGMove1.Add("delay", 1.5);
        iTween.MoveTo(ProfileCardBG, PCBGMove1);
        
        float yOrginalPos = 15.5f;
        float yOrginalScale = .5f;
        float yScaleFactor = PixelDef;
        float yNewScale = yOrginalScale * yScaleFactor;
        float yNewPos = yOrginalPos - (yNewScale / 2) + (yOrginalScale / 2);

        Hashtable PCBGScale2 = new Hashtable();
        PCBGScale2.Add("y", yNewScale);
        PCBGScale2.Add("time", .5);
        PCBGScale2.Add("easetype", "linear");
        PCBGScale2.Add("delay", 3);
        iTween.ScaleTo(ProfileCardBG, PCBGScale2);

        Hashtable PCBGMove2 = new Hashtable();
        PCBGMove2.Add("y", yNewPos);
        PCBGMove2.Add("time", .5);
        PCBGMove2.Add("easetype", "linear");
        PCBGMove2.Add("delay", 3);
        iTween.MoveTo(ProfileCardBG, PCBGMove2);

        yield return new WaitForSeconds(3);

        string name = star.GetComponent<StagePerson>().getName();
        string ftextInit = "";
        float scale;
        ftextInit += name;
        FlyingText.anchor = TextAnchor.LowerCenter;
        _nameGO = FlyingText.GetObject(ftextInit);
        _nameGO.layer = 8;
        scale = animatedSpriteGameObject.transform.renderer.bounds.size.x / _nameGO.renderer.bounds.size.x / 2;
        if (scale >= 2.7f)
        {
            scale = 2.7f;
        }
        _nameGO.transform.localScale = new Vector3(_nameGO.transform.localScale.x * scale, _nameGO.transform.localScale.y * scale, _nameGO.transform.localScale.z);
        _nameGO.transform.position = new Vector3(44.5f, 24, 0);

        yield return new WaitForSeconds(.5f);
        isOn = true;
        yield return new WaitForSeconds(ProperPlayTime);
        isOn = false;
        //yield return new WaitForSeconds(2);

        Hashtable PCBGScale2A = new Hashtable();
        PCBGScale2A.Add("y", .5);
        PCBGScale2A.Add("time", .5);
        PCBGScale2A.Add("easetype", "linear");
        PCBGScale2A.Add("delay", 0);
        iTween.ScaleTo(ProfileCardBG, PCBGScale2A);

        Hashtable PCBGMove2A = new Hashtable();
        PCBGMove2A.Add("y", 15.5);
        PCBGMove2A.Add("time", .5);
        PCBGMove2A.Add("easetype", "linear");
        PCBGMove2A.Add("delay", 0);
        iTween.MoveTo(ProfileCardBG, PCBGMove2A);

        Hashtable PCBGScale1A = new Hashtable();
        PCBGScale1A.Add("x", .5);
        PCBGScale1A.Add("y", .5);
        PCBGScale1A.Add("time", .5);
        PCBGScale1A.Add("easetype", "linear");
        PCBGScale1A.Add("delay", .5);
        iTween.ScaleTo(ProfileCardBG, PCBGScale1A);

        Hashtable PCBGMove1A = new Hashtable();
        PCBGMove1A.Add("x", 18.09);
        PCBGMove1A.Add("y", 15.5);
        PCBGMove1A.Add("time", .5);
        PCBGMove1A.Add("easetype", "linear");
        PCBGMove1A.Add("delay", .5);
        iTween.MoveTo(ProfileCardBG, PCBGMove1A);

        StartCoroutine("turnoffpcbg");

        Hashtable Line2ScaleA = new Hashtable();
        Line2ScaleA.Add("x", 1);
        Line2ScaleA.Add("time", .5);
        Line2ScaleA.Add("easetype", "linear");
        Line2ScaleA.Add("delay", 1);
        iTween.ScaleTo(Line2, Line2ScaleA);

        Hashtable Line2MoveA = new Hashtable();
        Line2MoveA.Add("x", 9.19);
        Line2MoveA.Add("y", 6.45);
        Line2MoveA.Add("time", .5);
        Line2MoveA.Add("easetype", "linear");
        Line2MoveA.Add("delay", 1);
        iTween.MoveTo(Line2, Line2MoveA);
        
        StartCoroutine("turnoffline2");

        Hashtable Line1ScaleA = new Hashtable();
        Line1ScaleA.Add("x", .5);
        Line1ScaleA.Add("time", .5);
        Line1ScaleA.Add("easetype", "linear");
        Line1ScaleA.Add("delay", 1.5);
        iTween.ScaleTo(Line1, Line1ScaleA);

        Hashtable Line1MoveA = new Hashtable();
        Line1MoveA.Add("x", -11);
        Line1MoveA.Add("time", .5);
        Line1MoveA.Add("easetype", "linear");
        Line1MoveA.Add("delay", 1.5);
        iTween.MoveTo(Line1, Line1MoveA);

        StartCoroutine("turnoffline1");
        animatedSpriteGameObject.GetComponent<MeshRenderer>().enabled = false;
        animatedSprite.renderer.enabled = false;
    }

    IEnumerator turnonline1()
    {
        yield return new WaitForSeconds(.5f);
        GameObject Line1 = GameObject.Find("Line1");
        Line1.renderer.enabled = true;

    }

    IEnumerator turnonline2()
    {
        yield return new WaitForSeconds(1.15f);
        GameObject Line2 = GameObject.Find("Line2");
        Line2.renderer.enabled = true;

    }

    IEnumerator turnonpcbg()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject PCBG = GameObject.Find("ProfileCardBG");
        PCBG.renderer.enabled = true;

    }

    IEnumerator turnoffpcbg()
    {
        yield return new WaitForSeconds(1);
        GameObject PCBG = GameObject.Find("ProfileCardBG");
        PCBG.renderer.enabled = false;
        Destroy(_nameGO);
    }

    IEnumerator turnoffline2()
    {
        yield return new WaitForSeconds(1.45f);
        GameObject Line2 = GameObject.Find("Line2");
        Line2.renderer.enabled = false;

    }

    IEnumerator turnoffline1()
    {
        yield return new WaitForSeconds(1.75f);
        star.GetComponent<MMFocus>().Closer();
        yield return new WaitForSeconds(.2f);
        GameObject Line1 = GameObject.Find("Line1");
        Line1.renderer.enabled = false;
        
    }

    public float MathStuff()
    {
        return PixelDef;
    }
}
