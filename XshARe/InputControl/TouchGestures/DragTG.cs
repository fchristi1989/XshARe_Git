using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DragTG : TouchGesture
{

    private bool dragBegan = false;
    private bool dragFromObject = false;
    private float dragDelta;

    public DragTG(ARRaycastManager arrm, ModelLogic modelLogic) : base(arrm, modelLogic)
    {
    }

    protected override bool PerformOperation()
    {
        ARRaycastManager arrm = base.ARRaycastManager;
        ModelLogic model = base.ModelLogic;


        if (model == null || arrm == null)
            return false;

        if (dragFromObject)
        {
            Raycast raycast = new Raycast(arrm, Input.GetTouch(0).position);
            model.MoveSelected(raycast.TargetPosition, raycast.Target);

            
        }
        else
        {
            model.RotateSelectedY(dragDelta > 0);
        }

        return false;
    }

    protected override bool GesturePerformed()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Raycast raycast = new Raycast(base.ARRaycastManager, Input.GetTouch(0).position);

                dragFromObject = false;

                if (raycast.Target != null && base.ModelLogic.SelectedObject != null)
                {

                    if (base.ModelLogic.IsSelected(raycast.Target))
                        dragFromObject = true;  
                }
                
            }


            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                dragBegan = true;

                if (dragFromObject)
                    return false;
                else
                {
                    dragDelta = Input.GetTouch(0).deltaPosition.x;
                    return true;
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && dragBegan)
            {
                dragBegan = false;
                return true;
            }
            
        }

        return false;
    }
}