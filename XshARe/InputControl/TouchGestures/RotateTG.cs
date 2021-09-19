using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RotateTG : TouchGesture
{
    private const int THRESHOLD = 5;

    private Vector2 delta1 = new Vector2(0, 0);
    private Vector2 delta2 = new Vector2(0, 0);

    public RotateTG(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }

    protected override bool PerformOperation()
    {
        ModelLogic model = base.ModelLogic;

        if (model == null)
            return false;

        model.RotateSelectedY(delta1.x > 0 && delta2.x < 0);

        return true;
    }

    protected override bool GesturePerformed()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {

                if (touch1.position.y > touch2.position.y)
                {
                    Touch buffer = touch1;
                    touch1 = touch2;
                    touch2 = buffer;
                }

                delta1 = touch1.deltaPosition;
                delta2 = touch2.deltaPosition;

                if (delta1.x > THRESHOLD && delta2.x < -THRESHOLD)
                    return true;

                if (delta1.x < -THRESHOLD && delta2.x > THRESHOLD)
                    return true;

          
            }

        }

        return false;
    }
}