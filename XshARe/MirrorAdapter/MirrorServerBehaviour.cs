using UnityEngine;
using System.Collections;
using Mirror;
using System;
using System.Reflection;


// This Behaviour holds the server commands
// It needs to be attached together with a NetworkIdentity
public class MirrorServerBehaviour : NetworkBehaviour
{
    


    private GameObject anchor;



    // Use this for initialization
    void Start()
    {
        anchor = Components.Anchor;
        anchor.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command(ignoreAuthority = true)]
    public void CmdCreateObject(int prefabIndex, Vector3 localPosition, Quaternion rotation, int randomID)
    {
        
        Debug.Log("Starting CmdCreateObject");

        Debug.Log("Get prefab from index");
        GameObject prefab = NetworkManager.singleton.spawnPrefabs[prefabIndex];


        Debug.Log("get world position");
        //Vector3 position = anchor.transform.position + localPosition;

        Debug.Log("Instantiate");
        GameObject go = Instantiate(prefab);

        Debug.Log("get SDC and randomID");
        SyncDataContainer sdc = go.GetComponent<SyncDataContainer>();
        sdc.RandomID = randomID;

        Debug.Log("Spawn GameObject");
        NetworkServer.Spawn(go);

        go.transform.parent = anchor.transform;
        go.transform.localPosition = localPosition;
        sdc.MoveTo(go.transform.position);

        /*
        Debug.Log("At NetworkTransformChild");
        NetworkTransformChild ntc = anchor.AddComponent<NetworkTransformChild>();
        ntc.target = go.transform;
        */

        /*
        //Frage: Muss das nicht ans ende?
        Debug.Log("Spawn GameObject");
        NetworkServer.Spawn(go);
        */
        
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetIntValue(int randomID, string propName, int value)
    {
        foreach (SyncDataContainer sdc in FindObjectsOfType(typeof(SyncDataContainer)))
        {
            if (sdc.RandomID == randomID)
            {
                Type type = sdc.GetType();
                PropertyInfo info = type.GetProperty(propName);
                Debug.Log(propName);
                Debug.Log(info == null);
                info.SetValue(sdc, value);
            }
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdCallModelMethod(int variant, int[] parameters)
    {
        ModelLogic model = Components.ModelLogic;

        /*
        MethodInfo mi = model.GetType().GetMethod(methodName);

        Parameter[] paramObjects = new Parameter[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            paramObjects[i] = new Parameter(parameters[i]);
        }

        mi.Invoke(model, paramObjects);
        */

        model.ServerMethod(variant, parameters);
    }

    [Command(ignoreAuthority = true)]
    public void CmdDelete(int selectedRandomID)
    {
        foreach (SyncDataContainer sdc in FindObjectsOfType(typeof(SyncDataContainer)))
        {
            if (sdc.RandomID == selectedRandomID)
                NetworkServer.Destroy(sdc.gameObject);
        }

    }
}
