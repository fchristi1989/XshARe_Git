using UnityEngine;
using System.Collections;

public class UNMAdapter : MonoBehaviour, INetworkAdapter
{

    public int CreateSyncGameObjectRelToAnchor(GameObject prefab, Vector3 localPosition, Quaternion localRotation)
    {
        GameObject go = Instantiate(prefab);
        go.transform.parent = gameObject.transform;

        go.transform.localPosition = localPosition;
        go.transform.localRotation = localRotation;

        //return go.GetComponent<SyncDataContainer>();
        return -1;
    }


    public int CreateSyncGameObject(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        // Create the prefab
        GameObject go = GameObject.Instantiate(prefab, pos, rotation);
        go.transform.parent = gameObject.transform;

        // return go.GetComponent<SyncDataContainer>();
        return -1;
    }

    public GameObject CreateAsyncGameObjectRelToCenter(GameObject prefab, Vector3 position)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = gameObject.transform.position + position;
        go.transform.rotation = gameObject.transform.rotation;
        

        return go;
    }

    

    public void SetAnchor(GameObject anchor)
    {
        throw new System.NotImplementedException();
    }

    public void Spawn(GameObject go)
    {
        throw new System.NotImplementedException();
    }

    public void SetIntValue(int randomID, string v, int roomNumber)
    {
        throw new System.NotImplementedException();
    }

    public void CallServerMethod(int variant, int[] parameters)
    {
        throw new System.NotImplementedException();
    }

    public void Delete(SyncDataContainer selected)
    {
        throw new System.NotImplementedException();
    }

    public int GetMirrorID()
    {
        throw new System.NotImplementedException();
    }


    /*
    public GameObject CreateAnchor(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        throw new System.NotImplementedException();
    }
    */
}
