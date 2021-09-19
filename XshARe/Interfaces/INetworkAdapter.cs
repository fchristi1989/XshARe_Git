using System;
using UnityEngine;

public interface INetworkAdapter
{
    int CreateSyncGameObjectRelToAnchor(GameObject prefab, Vector3 localPosition, Quaternion localRotation);
    int CreateSyncGameObject(GameObject prefab, Vector3 pos, Quaternion rotation);
    GameObject CreateAsyncGameObjectRelToCenter(GameObject prefab, Vector3 position);
    void SetIntValue(int randomID, string v, int roomNumber);
    void CallServerMethod(int variant, int[] parameters);
    void Delete(SyncDataContainer selected);
    int GetMirrorID();
    //void SetAnchor(GameObject anchor);
}
