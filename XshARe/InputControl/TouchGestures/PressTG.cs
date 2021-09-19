using UnityEngine.XR.ARFoundation;

public class PressTG : TouchGesture
{
    public PressTG(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }

    // not used yet

    protected override bool PerformOperation()
    {
        return false;
    }

    protected override bool GesturePerformed()
    {
        

        return false;
    }
}