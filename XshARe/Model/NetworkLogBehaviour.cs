using System;
using Mirror;
using UnityEngine;
/*
public class NetworkLogBehaviour : SyncDataContainer
{

    [SerializeField]
    private string lastLog = "";


    [SyncVar][SerializeField]
    private string nextLog = "";


    // Log called by client
    public void Log(int id, string message)
    {
        CmdLog(id, message);
    }


    // nextLog set on server for all clients
    [Command(ignoreAuthority=true)]
    private void CmdLog(int id, string message)
    {
        nextLog = System.DateTime.Now + " NetworkLog(id=" + id + "): " + message;
    }

    /*
    protected override void Start()
    {
        CmdLog(Components.NetworkAdapter.GetMirrorID(), "Server found by client");
        base.Start();
        
    }
    */
/*
    // called automatically on each client
    protected override void Update()
    {
        // if new log came in from server
        if (!lastLog.Equals(nextLog))
        {
            lastLog = nextLog;
            Debug.Log(lastLog);
        }

        base.Update();
    }

    
}
*/