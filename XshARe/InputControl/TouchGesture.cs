using System;
using UnityEngine.XR.ARFoundation;

public abstract class TouchGesture : Gesture
{
    private ARRaycastManager arrm;

    protected TouchGesture(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }
}