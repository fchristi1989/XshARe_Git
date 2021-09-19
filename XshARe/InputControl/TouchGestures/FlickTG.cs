using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FlickTG : TouchGesture
{
    public FlickTG(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }

    protected override bool PerformOperation()
    {
        ModelLogic model = base.ModelLogic;


        if (model == null)
            return false;

        model.DeleteSelected();

        return true;
    }

    protected override bool GesturePerformed()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                 return (Math.Abs(Input.GetTouch(0).deltaPosition.x) > base.SwipeThreshold);
            }
        }

        return false;
    }
}