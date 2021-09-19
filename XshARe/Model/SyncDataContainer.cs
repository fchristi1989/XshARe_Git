using Mirror;
using UnityEngine;

public class SyncDataContainer : NetworkBehaviour
{
    

    [SerializeField]
    [SyncVar]
    private int id = -1;

    [SerializeField]
    [SyncVar]
    private int randomID = -1;

    
    [SerializeField]
    [SyncVar]
    private Vector3 localPos = new Vector3();

    [SerializeField]
    [SyncVar]
    private Vector3 scale = new Vector3();

    [SerializeField]
    [SyncVar]
    private Quaternion localRot = new Quaternion();



    public int ID
    {
        get { return id; }
        set { CmdSetID(value); }
    }

    /// <summary>
    /// A random generated id used to recover gameobjects gernerated on server-side
    /// </summary>
    public int RandomID
    {
        get { return randomID; }
        set { randomID = value; }
    }

    /// <summary>
    /// The local position is synchronized to hold the AR views consistent
    /// </summary>
    public Vector3 LocalPos
    {
        set
        {
            CmdMoveTo(new Vector3(value.x, value.y, value.z));
        }
    }

    [Command (ignoreAuthority = true)]
    private void CmdSetID(int value)
    {
        id = value;
    }

    /// <summary>
    /// Use this for initialization
    /// <\summary>
    protected virtual void Start()
    {
        gameObject.transform.parent = Components.Anchor.transform;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected virtual void Update()
    {
        if (!(localPos.x == 0 && localPos.y == 0 && localPos.z == 0))
        {
            gameObject.transform.parent = Components.Anchor.transform;
            gameObject.transform.localPosition = localPos;
        }

        if (!(localRot.x == 0 && localRot.y == 0 && localRot.z == 0 && localRot.w == 0))
        {
            gameObject.transform.parent = Components.Anchor.transform;
            gameObject.transform.localRotation = localRot;
        }

        if (!(scale.x == 0 && scale.y == 0 && scale.z == 0))
        {
            gameObject.transform.parent = Components.Anchor.transform;
            gameObject.transform.localScale = scale;
        }

    }


    /// <summary>
    /// Rotate the gameobject around the y-axis
    /// </summary>
    /// <param name="degrees">The amount of degrees to rotate</param>
    public void RotateY(int degrees)
    {
        gameObject.transform.parent = Components.Anchor.transform;

        gameObject.transform.Rotate(0, degrees, 0, Space.Self);
        Quaternion lRot = new Quaternion(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y, gameObject.transform.localRotation.z, gameObject.transform.localRotation.w);

        CmdRotate(lRot);
    }
    


    [Command(ignoreAuthority = true)]
    private void CmdRotate(Quaternion rotation)
    {
        localRot = rotation;
    }

    /// <summary>
    /// Scale the gameobject
    /// </summary>
    /// <param name="ratio">The amount of ratio to scale the gameobject</param>
    public void Scale(float ratio)
    {
        gameObject.transform.parent = Components.Anchor.transform;
        gameObject.transform.localScale *= ratio;

        Vector3 sc = new Vector3(gameObject.transform.lossyScale.x, gameObject.transform.lossyScale.y, gameObject.transform.lossyScale.z);

        CmdScale(sc);
    }

    [Command(ignoreAuthority = true)]
    private void CmdScale(Vector3 sc)
    {
        scale = sc;
    }

    /// <summary>
    /// Move the gameobject to a new position
    /// </summary>
    /// <param name="position">The global position for the client's coordinate system</param>
    public void MoveTo(Vector3 position)
    {
        gameObject.transform.parent = Components.Anchor.transform;
        gameObject.transform.position = position;
        Vector3 lPos = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

        CmdMoveTo(lPos);
    }

    [Command(ignoreAuthority = true)]
    private void CmdMoveTo(Vector3 lPos)
    {
        localPos = lPos;
    }

    /// <summary>
    /// Destroy a gameobject on client-side. It is recommended to use the MirrorAdapter method instead to destroy on server-side and synchronize
    /// </summary>
    public void Delete()
    {
        Object.Destroy(gameObject);
    }

}