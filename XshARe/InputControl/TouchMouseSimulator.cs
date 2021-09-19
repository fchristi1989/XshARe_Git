using System;
using UnityEngine;

// Replaces Input Control for Testing on Desktop (Tap, Press and Double Tap)
// Other Gestures can be simulated via buttons
public class TouchMouseSimulator : MonoBehaviour
{
    private const float DOUBLECLICKTIME = 1;
    private int framesDown;
    private float lastClick;
    private Vector3 startPosition;

    private ModelLogic modelLogic;

    void Start()
    {
        framesDown = 0;
        lastClick = 0;
        startPosition = new Vector3();

        GameObject custom = GameObject.Find(Paths.Custom);
        modelLogic = custom.GetComponent<ModelLogic>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject target = RaycastTarget();

            if (target != null)
            {
                // DoubleClick: Create new
                if (Time.time - lastClick < DOUBLECLICKTIME)
                {
                    modelLogic.AddGameObject(target.transform.position, target.transform.rotation, target);
                }
                // Click: Select
                else
                {
                    modelLogic.Select(target);
                }
            }

            lastClick = Time.time;
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            framesDown = 0;

            //Drag: Move
            if (!startPosition.Equals(Input.mousePosition))
                modelLogic.MoveSelected(Input.mousePosition, RaycastTarget());
        }

        if (Input.GetMouseButton(0))
        {
            framesDown++;
        }
    }

    private GameObject RaycastTarget()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray raycast = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(raycast, out raycastHit))
        {
            GameObject result = raycastHit.collider.gameObject;

            return result;
        }

        return null;
    }
}
