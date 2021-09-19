using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Raycast
{
    private ARRaycastManager aRRaycastManager;

    private GameObject target;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector2 screenPosition;

    public Raycast(ARRaycastManager aRRaycastManager, Vector2 screenPosition)
    {
        this.aRRaycastManager = aRRaycastManager;
        this.screenPosition = screenPosition;

        Ray raycast = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(raycast, out raycastHit))
        {
            target = raycastHit.collider.gameObject;
            targetPosition = raycastHit.point;
            targetRotation = raycastHit.collider.gameObject.transform.rotation;
        }
        else
        {
            List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

            if (aRRaycastManager.Raycast(screenPosition, aRRaycastHits) && aRRaycastHits.Count > 0)
            {
                target = null;
                targetPosition = aRRaycastHits[0].pose.position;
                targetRotation = aRRaycastHits[0].pose.rotation;
            }
        }
    }

    public GameObject Target
    {
        get
        {
            return target;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
    }

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    public Vector2 ScreenPosition
    {
        get
        {
            return screenPosition;
        }
    }
}
