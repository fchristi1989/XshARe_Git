using System;
using Mirror.Discovery;
using UnityEngine;

public class Components
{
    public static SpatialAnchorsCoordinator ARCoordinator
    {
        get
        {
            GameObject go = GameObject.Find(Paths.AzureSpatialAnchors);

            if (go == null)
                return null;

            return go.GetComponent<SpatialAnchorsCoordinator>();
        }
    }

    public static INetworkAdapter NetworkAdapter
    {
        get
        {
            GameObject go = GameObject.Find(Paths.NetworkModel);
            return go.GetComponent<INetworkAdapter>();
        }
    }

    /*
    public static MirrorServerBehaviour MirrorServer
    {
        get
        {
            GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject go in gos)
            {
                if (go.name == Paths.NetworkServer)
                    return go.GetComponent<MirrorServerBehaviour>();
            }

            return null;
        }
    }
    */

    public static ModelLogic ModelLogic
    {
        get
        {
            GameObject go = GameObject.Find(Paths.Custom);
            return go.GetComponent<ModelLogic>();
        }
    }

    public static NetworkDiscovery NetworkDiscovery
    {
        get
        {
            GameObject go = GameObject.Find(Paths.NetworkModel);
            return go.GetComponent<NetworkDiscovery>();
        }
    }

    /*
    public static GameObject Anchor
    {
        get
        {
            //Mirror automatically disables initial Gameobjects in scene with Network Identity;

            //GameObject go = GameObject.Find(Paths.Anchor);
            GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject go in gos)
            {
                if (go.name == Paths.Anchor)
                    return go;
            }

            return null;
        }
    }
    */

    public static GameObject Anchor
    {
        get
        {
            GameObject anchor = GameObject.Find(Paths.Anchor);

            if (anchor == null)
                Debug.Log("Error: Non-Existing anchor requested");

            return anchor;
        }
    }

}
