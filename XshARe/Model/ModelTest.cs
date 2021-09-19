using UnityEngine;
using System.Collections;

public class ModelTest : MonoBehaviour
{
    private GameObject objectToSelect = null;

    private ModelLogic ModelLogic
    {
        get
        {
            GameObject custom = GameObject.Find(Paths.Custom);
            return custom.GetComponent<ModelLogic>();
        }
    }

   
    


    //TODO: Erweitern bei Bedarf


    //Important:
    public void CheckSessionState()
    {
        SessionState value = ModelLogic.SessionState;
        Debug.Log(value);
    }

    public void SetSessionState()
    {
        SessionState value = SessionState.Running;
        ModelLogic.SessionState = value;
        CheckSessionState();
    }


    public void CallRotateSelectedY()
    {
        ModelLogic.RotateSelectedY(true);
    }

    public void CallDeleteSelected()
    {
        ModelLogic.DeleteSelected();
    }

    public void CallScaleSelected()
    {
        ModelLogic.ScaleSelected(true);
    }

    public void CallMoveSelected()
    {
        ModelLogic.MoveSelected(new Vector3(1, 1, 1), null);
    }

    public void CallSelect()
    {
        ModelLogic.Select(objectToSelect); ;
    }

    /*
    public void CallAddGameObject()
    {
        
        Debug.Log("Call AddGameObject called");
        Debug.Log(ModelLogic);
        GameObject value = ModelLogic.AddGameObject(new Vector3(), new Quaternion(), null);
        objectToSelect = value;
        Debug.Log(value);
    }
    */
}
