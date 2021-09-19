using System;
using Mirror;
using UnityEngine;

public class RoomSDC : SyncDataContainer
{
    [SerializeField][SyncVar]
    private int roomNumber = -1;

    public int RoomNumber
    {
        get
        {
            return roomNumber;
        }

        set
        {
            roomNumber = value;
        }
    }

}
