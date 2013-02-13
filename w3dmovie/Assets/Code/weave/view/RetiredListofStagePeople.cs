using UnityEngine;
using System.Collections;

public class RetiredListofStagePeople : MonoBehaviour 
{
    //private Component[] _stageNumbers;
    public void RemoveRetiredStars()
    {
        Component[] _stageNumbers;
       GameObject _listOfStarsGO =
           GameObject.Find("4ListOfStagePeople");
            StartCoroutine("Pause");
            _stageNumbers =
                GetComponentsInChildren<StagePerson>();

            foreach (StagePerson number in _stageNumbers)
            {
                number.transform.parent = _listOfStarsGO.transform;
            }

        }

    IEnumerator Pause()
    {
    yield return new WaitForSeconds(10f);
    }

}