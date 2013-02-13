using UnityEngine;
using System.Collections;

public class StagePerson : MonoBehaviour 
{
    public int starNumber;
    private int stageNumber;
    public string VesselName;
    private string naMe;
    public PersonModel personModel;
    private float vposx;
    private float vposy;
    private float vposz;
    private float posx;
    private float posy;
    private float posz;
    
    public void setVesselName(string name)
    {
        VesselName = name;
    }

    public string getVesselName()
    {
        return VesselName;
    }

    public void SetPersonModel(PersonModel persondata)
    {
        personModel = persondata;
    }

    public PersonModel GetPersonModel()
    {
        return personModel;
    }

    public string getName()
    {
        return naMe;
    }

    public void setName(string _name)
    {
        naMe = _name;
    }

    public void setVesselPosition()
    {
        GameObject Vessel = GameObject.Find(VesselName);
        //Vessel.transform.parent = null;

       	vposx = Vessel.transform.position.x;
        vposy = Vessel.transform.position.y;
        vposz = Vessel.transform.position.z;
            //Debug.Log(Vessel.name);
	}

    public void setPosition()
    {
        posx = this.gameObject.transform.position.x;
        posy = this.gameObject.transform.position.y;
        posz = this.gameObject.transform.position.z;

    }

    public Vector3 getPosition()
    {
        return new Vector3(posx, posy, posz);
    }

    public Vector3 getVesselPosition()
    {
        return new Vector3(vposx,vposy,vposz);
    }


	public void setStarNumber(int number)
    {
        starNumber = number;
    }

    public int getStarNumber()
    {
        return starNumber;
    }

    public void Parent()
    {
        GameObject Vessel = GameObject.Find(VesselName);
        this.gameObject.transform.parent = Vessel.transform;
    }
}