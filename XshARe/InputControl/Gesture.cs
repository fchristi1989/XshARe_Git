using UnityEngine.XR.ARFoundation;

public abstract class Gesture
{
    protected const int SWIPETHRESHOLD = 100;

    private ARRaycastManager arrm;
    private ModelLogic modelLogic;


    protected int SwipeThreshold
    {
        get
        {
            return SWIPETHRESHOLD;
        }
    }

    protected ARRaycastManager ARRaycastManager
    {
        get { return arrm; }
    }

    protected ModelLogic ModelLogic
    {
        get { return modelLogic; }
    }

    public Gesture(ARRaycastManager arrm, ModelLogic modelLogic)
    {
        this.arrm = arrm;
        this.modelLogic = modelLogic;
    }

    public void Perform()
    {
        if (GesturePerformed())
            PerformOperation();
    }

    protected abstract bool GesturePerformed();
    protected abstract bool PerformOperation();

}