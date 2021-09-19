using UnityEngine;
using System.Collections;
using Mirror;

public class GamePieceSDC : SyncDataContainer
{
    [SerializeField]
    [SyncVar]
    private int tileID = -1;

    [SerializeField]
    [SyncVar]
    private int playerID = -1;

    public int TileID
    {
        get { return tileID; }
        set { CmdSetTileID(value); }
    }

    public int PlayerID
    {
        get { return playerID; }
        set { CmdSetPlayerID(value); }
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetTileID(int value)
    {
        tileID = value;
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetPlayerID(int value)
    {
        playerID = value;
    }

}
