using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class TapTG : TouchGesture
{
    private const int THRESHOLD = 10;
    private int framesPressed = 0;

    public TapTG(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }

    protected override bool PerformOperation()
    {
        ModelLogic model = base.ModelLogic;

        if (model == null)
            return false;

        Raycast raycast = new Raycast(base.ARRaycastManager, Input.GetTouch(0).position);

        if (raycast.Target == null)
            return false;

        model.Select(raycast.Target);
        return true;

        
    }

    protected override bool GesturePerformed()
    {

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                framesPressed++;
                return false;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (framesPressed < THRESHOLD && framesPressed > 0)
                {
                    framesPressed = 0;
                    return true;
                }

                framesPressed = 0;
                return false;
            }

            framesPressed = 0;
            return false;
        }

        framesPressed = 0;
        return false;
    }
}