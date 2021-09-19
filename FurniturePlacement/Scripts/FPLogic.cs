using System;
using UnityEngine;

public class FPLogic : ModelLogic
{
    FPConfig config;

    protected override void Start()
    {
        config = this.gameObject.GetComponent<FPConfig>();

        //mandatory
        base.Start();
    }

    public override int AddGameObject(Vector3 pos, Quaternion rotation, GameObject target)
    {
        if (base.SessionState == SessionState.Running)
            GameObject.Instantiate(config.pnlSelectPrefab, pos, rotation);

        return -1;
    }

    internal void AddChair(GameObject panel)
    {
        base.CreateSyncGameObject(config.chairPrefab, panel.transform.position, panel.transform.rotation);
        Destroy(panel);
    }

    internal void AddShelf(GameObject panel)
    {
        base.CreateSyncGameObject(config.shelfPrefab, panel.transform.position, panel.transform.rotation);
        Destroy(panel);
    }

    internal void AddTable(GameObject panel)
    {
        base.CreateSyncGameObject(config.tablePrefab, panel.transform.position, panel.transform.rotation);
        Destroy(panel);
    }

    public override void ScaleSelected(bool grow)
    {
        // Do nothing
    }
}
