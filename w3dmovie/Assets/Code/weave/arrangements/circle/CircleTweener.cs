using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleTweener 
{
	 private float 		innerRadius;
	 private float 		outerRadius;
	 private float 		dotRadius;
	 private Vector3 	scaleVector;
	 private float 		centerX;
	 private float 		centerY;
	 private float 		centerZ;
	 private float 		tweenTime;
	 private int 		numberOfRings;
	 private int 		numberOfDots;
	 private float[] 	ringRadius;
	 private float[] 	ringCircum;
	 private int[] 		dotsPerRing;
     private GameObject pergo;
	
	public CircleTweener()
    {
		SetDefaultVariables();
	}
	public void SetDefaultVariables()
    {
		centerX =
            0;
		centerY =
            0;
		centerZ =
            0;
		numberOfRings =
            3;
		scaleVector =
            new Vector3(1,1,1);
		innerRadius =
            2;
		outerRadius =
            25;
		dotRadius =
            1;
		tweenTime =
            2;
	}

	public void SetNumberOfRings(int numrings)
    {
		numberOfRings =
            numrings;		
	}

	public void SetTweenTime(float twtime)
    {	
		tweenTime =
            twtime;		
	}

	public void SetInOutRadius(float inrad, float outrad)
    {
		innerRadius =
            inrad;
		outerRadius =
            outrad;
	}

	public void SetCenterPos(float xc, float yc, float zc)
    {
		centerX =
            xc;
		centerY =
            yc;
		centerZ =
            zc;
	}

	public void SetScale(float scale)
    {
		scaleVector =
            new Vector3(scale,scale,scale);
	}

	public void TweenThePeople(List<GameObject> personGOList)
    {
		Debug.Log(">>> Circle - tweenThePeople");
		numberOfDots =
            personGOList.Count;
		DistributeDots();
		int numDotsOnFilledRings =
            0;
		int currentRingNumber =
            0;
		int counter =
            0;
		
        foreach (GameObject pgo in personGOList)
        {
			TweenOneDot( pgo, counter - numDotsOnFilledRings, 	dotsPerRing[currentRingNumber] , 
																ringRadius[currentRingNumber]  );
			if ((counter - numDotsOnFilledRings +1) >= dotsPerRing[currentRingNumber])
            {
				// new ring
				numDotsOnFilledRings += dotsPerRing[currentRingNumber];
				currentRingNumber++;
			}
            counter++;
        }
	}

    private void TweenOneDot(GameObject pDot, int clockPos, int totalNumber, float radius)
    {
        float angleRot =
            (float)((((float)clockPos / (float)totalNumber) * 360.0) - 90.0) * (float)(Mathf.PI / 180);
        float xpos = centerX + (radius * Mathf.Cos(angleRot) * 1);
        float ypos = centerY + (radius * Mathf.Sin(angleRot) * 1);
        float zpos = centerZ;
        Hashtable moveparams =
            new Hashtable();
        moveparams.Add("x", xpos);
        moveparams.Add("y", ypos);
        moveparams.Add("z", zpos);
        moveparams.Add("time", tweenTime);
        moveparams.Add("easetype", "easeInOutSine");
        iTween.MoveTo(pDot, moveparams);
        Hashtable scaleparams =
            new Hashtable();
        scaleparams.Add("amount", scaleVector);
        scaleparams.Add("delay", 0);
        scaleparams.Add("time", tweenTime);
        scaleparams.Add("easetype", "easeInOutSine");
        iTween.ScaleBy(pDot, scaleparams);
    }

	public void DistributeDots()
    {
			float totalAvailableCirc =
                0;
			float totalDotCirc =
                0;
			int dotsOnRings =
                0;
			float gapBetweenDots =
                0;
			float spacePerDot =
                0;
			float spaceBetweenRings =
                0;
			ringRadius  =
                new float[numberOfRings];
			ringCircum  =
                new float[numberOfRings];
			dotsPerRing =
                new int[numberOfRings];

			if (numberOfRings == 1)
            {
				spaceBetweenRings =
                    (outerRadius - innerRadius) / (numberOfRings + 1);
			}

            else
            {
				spaceBetweenRings = (outerRadius - innerRadius) / (numberOfRings);
			}

			for (int i = 0; i < numberOfRings; i++)
            {
				if (numberOfRings ==1)
                {
					ringRadius[i] =
                        innerRadius + spaceBetweenRings;
				}

                else
                {
					ringRadius[i] =
                        innerRadius + spaceBetweenRings/2 + (spaceBetweenRings * i);
				}

				ringCircum[i] =
                    2 * Mathf.PI * ringRadius[i];
				totalAvailableCirc += ringCircum[i];
			}

			totalDotCirc =
                dotRadius * numberOfDots;
			gapBetweenDots =
                (totalAvailableCirc - totalDotCirc) / numberOfDots;
			spacePerDot =
                dotRadius + gapBetweenDots;
			
			for (int j = 0; j < numberOfRings; j++)
            {
				dotsPerRing[j] =
                    (int)Mathf.Round(ringCircum[j] / spacePerDot);
				dotsOnRings += dotsPerRing[j];
			}

			// add extra dots to outside ring due to rounding down
			dotsPerRing[numberOfRings-1] += (numberOfDots - dotsOnRings);
		}

}