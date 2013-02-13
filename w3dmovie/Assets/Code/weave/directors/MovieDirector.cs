using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovieDirector 
{
    public MovieDirector()
    {   }

    public void RollMovie(List<GameObject> allPersonObjectsList)
    {
        List<GameObject> _allStagePersonObjectsList;
        _allStagePersonObjectsList = allPersonObjectsList;
        
        Debug.Log(">> MovieDirector:RollMovie");
    }

    public void RandomPersonCycle()
    {
        GameObject stagepersonlistgo = GameObject.FindWithTag("TagListOfStagePeople");
        stagepersonlistgo.GetComponent<ListOfStagePeople>().BeginMovieMode();
    }
}