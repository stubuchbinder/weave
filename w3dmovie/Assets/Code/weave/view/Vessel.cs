using UnityEngine;
using System.Collections;

public class Vessel : MonoBehaviour
{
    private int VesselNumber;
    public void setVesselNumber(int number)
    {
        VesselNumber = number;
    }

    public int getVesselNumber()
    {
        return VesselNumber;
    }
}