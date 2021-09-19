using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DoubleTapTG : TouchGesture
{
    public DoubleTapTG(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }

    protected override bool PerformOperation()
    {
        ARRaycastManager arrm = base.ARRaycastManager;
        ModelLogic model = base.ModelLogic;

        if (model == null || arrm == null)
            return false;

        Raycast raycast = new Raycast(arrm, Input.GetTouch(0).position);

        model.AddGameObject(raycast.TargetPosition, raycast.TargetRotation, raycast.Target);

        return true;
    }


    protected override bool GesturePerformed()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).tapCount == 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    return true;
                }
            }
        }

        return false;
    }
}