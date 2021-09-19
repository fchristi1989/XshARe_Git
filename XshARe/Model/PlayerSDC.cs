using System;
public class PlayerSDC : SyncDataContainer
{
    protected bool stillActive = true;

    public bool StillActive
    {
        get { return stillActive; }
        set { stillActive = value; }
    }

}
