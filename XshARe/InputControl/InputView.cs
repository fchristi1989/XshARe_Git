using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System;

public class InputView : IView
{
    private const float FLOATING = 0.3f;

    private GameObject selectionPrefab;
    private GameObject inputPrefab;
    private ARRaycastManager arRaycastManager;
    private ModelLogic modelLogic;
    private GameObject objectSelection;

    private List<GameObject> activeSelections;

    public InputView(GameObject selectionPrefab, GameObject inputPrefab, ARRaycastManager arRaycastManager)
    {
        this.selectionPrefab = selectionPrefab;
        this.inputPrefab = inputPrefab;
        this.arRaycastManager = arRaycastManager;
        modelLogic = Components.ModelLogic;

        activeSelections = new List<GameObject>();
        modelLogic.AddView(this);

        objectSelection = null;
    }

    public void Notify()
    {
        // Delete object selection if no object selected
        if (modelLogic.SelectedObject == null && objectSelection != null)
        {
            GameObject.Destroy(objectSelection);
            objectSelection = null;
        }
        else
        {
            // Create object selection if needed
            if (objectSelection == null)
            {
                objectSelection = GameObject.Instantiate(selectionPrefab);
            }

            // Place and scale selection adequately to selected object
            GameObject selectedObject = modelLogic.SelectedObject.gameObject;
            

            objectSelection.transform.rotation = selectedObject.transform.rotation;
            objectSelection.transform.position = new Vector3(selectedObject.transform.position.x, MaxY(selectedObject) + FLOATING, selectedObject.transform.position.z);
        }
    }

    private float MaxY(GameObject parent)
    {
        float result = parent.transform.position.y;

        foreach (Transform child in parent.transform.GetComponentsInChildren<Transform>())
        {
            if (child != parent.transform)
                result = Math.Max(result, MaxY(child.gameObject));
        }
       
        return result;
    }

    public void Update()
    {
        int count = Input.touchCount;

        // Create new selections, if required
        while (activeSelections.Count < count)
        {
            activeSelections.Add(GameObject.Instantiate(inputPrefab));
        }

        // Delete extend selections, if existing
        while (activeSelections.Count > count)
        {
            GameObject deleteObject = activeSelections[activeSelections.Count - 1];
            activeSelections.Remove(deleteObject);
            GameObject.Destroy(deleteObject);
        }

        // Map selection positon to each raycast hit
        for (int i = 0; i < count; i++)
        {
            Touch touch = Input.GetTouch(i);

            Raycast raycast = new Raycast(arRaycastManager, touch.position);
            activeSelections[i].transform.position = raycast.TargetPosition;

      
        }
    }
}
