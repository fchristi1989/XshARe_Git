using UnityEngine;
using System.Collections;

public class DesktopSetup : MonoBehaviour
{
    private ModelLogic modelLogic;

    // Use this for initialization
    public void CreateStartObjects()
    {
        
        modelLogic = Components.ModelLogic;
        modelLogic.SessionState = SessionState.Running;
        modelLogic.CreateStartObjects();
        Debug.Log("Desktop Setup: Created Start Objects");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
