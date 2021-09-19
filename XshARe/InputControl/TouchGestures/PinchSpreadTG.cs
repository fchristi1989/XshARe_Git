using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PinchSpreadTG : TouchGesture
{
    private const int THRESHOLD = 25;

    private float oldDistanceY = 0;
    private float distanceY = 0;

    public PinchSpreadTG(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }

    protected override bool PerformOperation()
    {
        ModelLogic model = base.ModelLogic;


        if (model == null)
            return false;

        model.ScaleSelected(distanceY > oldDistanceY);

        return true;
    }

    protected override bool GesturePerformed()
    {
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (oldDistanceY == 0)
                {
                    oldDistanceY = Math.Abs(Input.GetTouch(0).position.y - Input.GetTouch(1).position.y);

                    return false;
                }
                else
                {
                    oldDistanceY = distanceY;
                    distanceY = Math.Abs(Input.GetTouch(0).position.y - Input.GetTouch(1).position.y);

                    if (distanceY < oldDistanceY - THRESHOLD)
                    {
                        return true;
                    }

                    if (distanceY > oldDistanceY + THRESHOLD)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        oldDistanceY = 0;
        distanceY = 0;

        return false;
    }
}