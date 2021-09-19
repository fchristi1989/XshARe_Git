using UnityEngine.XR.ARFoundation;

public abstract class HMDGesture : Gesture
{
    protected HMDGesture(ARRaycastManager arrm, ModelLogic model) : base(arrm, model)
    {
    }
}