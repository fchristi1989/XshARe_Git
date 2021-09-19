using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using System;
using System.Threading;

public class MirrorAdapter : MonoBehaviour, INetworkAdapter
{
    [SerializeField]
    [Tooltip("The prefab used to represent the server.")]
    private GameObject mirrorServerPrefab = null;

    /*
    [SerializeField]
    private GameObject networkLoggerPrefab = null;
    */

    [SerializeField]
    [Tooltip("The prefab used to represent the the anchor based root.")]
    private GameObject anchorPrefab = null;

    //private GameObject anchor;

    private MirrorServerBehaviour mServer;

    private bool serverFound = false;

    //readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    private NetworkDiscovery networkDiscovery;
    private SpatialAnchorsCoordinator arCoordinator;

    private ServerResponse serverResponse = null;

    private SyncDataContainer result = null;

    private int mirrorID = -1;


    public int GetMirrorID()
    {
            return mirrorID;
    }

    void Start()
    {
        // Mirror disables anchor in the beginning 
        //anchor = Components.Anchor;
        //anchor.SetActive(true);

        //mServer = Components.MirrorServer;
        //mServer.gameObject.SetActive(true);

        networkDiscovery = Components.NetworkDiscovery;
        arCoordinator = Components.ARCoordinator;

        double randomDouble = new System.Random().NextDouble() * 10000000;
        mirrorID = Convert.ToInt32(randomDouble);
    }

    /*
    public GameObject CreateAnchor(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject anchor = Instantiate(prefab, position, rotation);
        anchors.Add(anchor);
        return anchor;
    }
    */

    /*
    public void SetAnchor()
    {
        anchors.Add(anchor);
    }
    */

    /*
    public SyncDataContainer CreateSyncGameObject(GameObject  prefab, Vector3 pos, Quaternion rotation)
    {
        if (anchors.Count > 0)
        {
            GameObject go = GameObject.Instantiate(prefab, pos, rotation);
            go.transform.parent = anchors[0].transform;

            NetworkTransformChild tnc = anchors[0].AddComponent<NetworkTransformChild>();
            tnc.target = go.transform;

            return go.GetComponent<SyncDataContainer>();
        }

        return null;
    }
    */

    public int CreateSyncGameObject(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        GameObject anchor = Components.Anchor;

        int prefabIndex = PrefabIndex(prefab);

        if (anchor == null || prefabIndex == -1)
            return -1;

        //Vector3 localPosition = pos - anchor.transform.position;
        GameObject dummy = new GameObject();
        dummy.transform.parent = Components.Anchor.transform;
        dummy.transform.position = new Vector3(pos.x, pos.y, pos.z);
 
        Vector3 localPosition = new Vector3(dummy.transform.localPosition.x, dummy.transform.localPosition.y, dummy.transform.localPosition.z);
        Destroy(dummy);

        //uint netID = CmdCreateObject(prefabIndex, pos, rotation);

        int randomID = CreateSyncGameObjectRelToAnchor(prefab, localPosition, rotation);

        /*
        foreach (SyncDataContainer sdc in FindObjectsOfType(typeof(SyncDataContainer)))
        {
            if (sdc.RandomID == randomID)
            {
                sdc.MoveTo(pos);

            }
        }
        */

        return randomID;
    }


    /*
    public SyncDataContainer CreateSyncGameObjectRelToAnchor(GameObject prefab, Vector3 localPosition, Quaternion localRotation)
    {
        if(anchors.Count > 0)
        {
            GameObject go = Instantiate(prefab);
            go.transform.parent = anchors[0].transform;

            go.transform.localPosition = localPosition;
            go.transform.localRotation = localRotation;

            NetworkTransformChild tnc = anchors[0].AddComponent<NetworkTransformChild>();
            tnc.target = go.transform;

            return go.GetComponent<SyncDataContainer>();
        }

        return null;
    }
    */

    public int CreateSyncGameObjectRelToAnchor(GameObject prefab, Vector3 localPosition, Quaternion rotation)
    {
        Debug.Log("Debug: Begin CreateSyncGameObjectRelToAnchor");

        mServer = FindObjectOfType<MirrorServerBehaviour>();
        GameObject anchor = Components.Anchor;

        int prefabIndex = PrefabIndex(prefab);

        if (anchor == null || prefabIndex == -1)
            return -1;

        double randomDouble = new System.Random().NextDouble() * 10000000;
        int randomID = Convert.ToInt32(randomDouble);

        
        Debug.Log("Call CmdCreateObject; prefabIndex: " + prefabIndex + ", randomID: " + randomID);
        //Components.MirrorServer.CmdCreateObject(prefabIndex, localPosition, rotation, randomID);

        mServer.CmdCreateObject(prefabIndex, localPosition, rotation, randomID);

        /*
        result = null;

        //wait until result has arrived
        while (result == null)
        {
            foreach (GameObject go in FindObjectsOfType<GameObject>())
            {
                SyncDataContainer sdc = go.GetComponent<SyncDataContainer>();

                if (sdc != null)
                {
                    if (sdc.RandomID == randomID)
                        result = sdc;
                }
            }
        }
        



        //Test: Just use TestCommand
        //OnTestCommand();
        //SyncDataContainer result = null;


        return result;
        */


        foreach (SyncDataContainer sdc in FindObjectsOfType(typeof(SyncDataContainer)))
        {
            if (sdc.RandomID == randomID)
            {
                Debug.Log("RandomID already found");

            }
        }

        return randomID;
    }
    

    public GameObject CreateAsyncGameObjectRelToCenter(GameObject prefab, Vector3 position)
    {
        GameObject go = Instantiate(prefab);

        GameObject anchor = Components.Anchor;


        if (anchor != null)
        {
            go.transform.parent = anchor.transform;
            go.transform.localPosition = position;

            //go.transform.position = anchor.transform.position + position;
            go.transform.rotation = anchor.transform.rotation;
        }
        else
        {
            go.transform.position = position;
        }


        return go;
    }

    private int PrefabIndex(GameObject prefab)
    {
        // Exactly one SyncDataContainer attached to identify GameObject
        //SyncDataContainer prefabSDC = prefab.GetComponent<SyncDataContainer>();

        

        for (int i = 0; i < NetworkManager.singleton.spawnPrefabs.Count; i++)
        {
            GameObject spawnPrefab = NetworkManager.singleton.spawnPrefabs[i];

            /*
            SyncDataContainer spawnSDC = spawnPrefab.GetComponent<SyncDataContainer>();

            if (spawnSDC != null)
            {
                if (prefabSDC.GetType() == spawnSDC.GetType())
                    return i;
            }

            */

            if (prefab.name.Equals(spawnPrefab.name))
                return i;
        }

        return -1;
    }

    

    public void OnDiscoveredServer(ServerResponse info)
    {
        Debug.Log("ONDISCOVEREDSERVER");

        serverFound = true;
        serverResponse = info;

        /*
        foreach (ServerResponse info in discoveredServers.Values)
        {
            // get last server in list
            server = info;
        }
        */

        // TODO: Lösche discovered server

        arCoordinator.UpdateClientButton(serverResponse.EndPoint.Address.ToString());
        //Connect(server);

    }

    public void OnHostClick()
    {
        //discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();

        StartServer();

        arCoordinator.ConnectedSuccessfully();

    }

    public void OnClientClick()
    {
        if (serverFound)
        {
            NetworkManager.singleton.StartClient(serverResponse.uri);

            //StartClient();

            arCoordinator.ConnectedSuccessfully();
            //NetworkLogBehaviour log = FindObjectOfType(typeof(NetworkLogBehaviour)) as NetworkLogBehaviour;
            //log.Log(mirrorID, "Client joined successfully");

        }
        else
        {
            //discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }

        serverFound = false;
    }


    // This method is required to establish the MirrorServer object on all devices, which is the interface to give server commands
    // This method is supposed to be called by the server only, as soon as it knows to be the server
    public void StartServer()
    {
        GameObject mirrorServer = Instantiate(mirrorServerPrefab);
        NetworkServer.Spawn(mirrorServer);
        mServer = mirrorServer.GetComponent<MirrorServerBehaviour>();

        GameObject anchor = Instantiate(anchorPrefab);
        NetworkServer.Spawn(anchor);

        /*
        GameObject networkLog = Instantiate(networkLoggerPrefab);
        NetworkServer.Spawn(networkLog);
        networkLog.GetComponent<NetworkLogBehaviour>().Log(mirrorID, "Server started, Log Spawned");
        */
    }

    /*
    // This method is supposed to be called by all clients after the server is established
    public void StartClient()
    {
        Debug.Log("Getting anchor from server");
        mServer = FindObjectOfType<MirrorServerBehaviour>();
        //anchor = Components.Anchor;
    }
    */

    public void OnTestCommand()
    {
        mServer = FindObjectOfType<MirrorServerBehaviour>();
        mServer.CmdCreateObject(0, new Vector3(), new Quaternion(), 5);
    }

    public void SetIntValue(int randomID, string fieldName, int value)
    {
        mServer = FindObjectOfType<MirrorServerBehaviour>();
        mServer.CmdSetIntValue(randomID, fieldName, value);
    }

    public void CallServerMethod(int variant, int[] parameters)
    {
        mServer = FindObjectOfType<MirrorServerBehaviour>();
        mServer.CmdCallModelMethod(variant, parameters);
    }

    public void Delete(SyncDataContainer selected)
    {
        mServer = FindObjectOfType<MirrorServerBehaviour>();
        mServer.CmdDelete(selected.RandomID);
    }
}


