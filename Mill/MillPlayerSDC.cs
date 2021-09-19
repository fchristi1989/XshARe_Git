using UnityEngine;
using System.Collections;
using Mirror;

public class MillPlayerSDC : SyncDataContainer
{
    [SerializeField]
    [SyncVar]
    private Role role = Role.Observer;

    [SerializeField]
    [SyncVar]
    private MillState state = MillState.Observe;

    [SerializeField]
    [SyncVar]
    private int piecesToPlace = 9;
    

    public Role Role
    {
        get { return role; }
        set { CmdSetRole(value); }
    }

    public MillState State
    {
        set { CmdSetState(value); }
        get { return state; }
    }

    public int PiecesToPlace
    {
        get { return piecesToPlace; }
        set { CmdSetPiecesToPlace(value); }
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetRole(Role value)
    {
        role = value;
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetState(MillState value)
    {
        state = value;
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetPiecesToPlace(int value)
    {
        piecesToPlace = value;
    }

    protected override void Start()
    {
        MillPlayerSDC[] players = FindObjectsOfType(typeof(MillPlayerSDC)) as MillPlayerSDC[];

        if (Role == Role.Observer)
        {
            if (players.Length == 1)
            {
                {
                    Role = Role.White;
                    State = MillState.PlacePiece;
                }
            }
            else if (players.Length == 2)
            {
                Role = Role.Black;
                State = MillState.Wait;
            }

            ID = players.Length;
        }

        base.Start();
    }

}
