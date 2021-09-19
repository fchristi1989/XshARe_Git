using System;
using System.Collections.Generic;
using UnityEngine;

public class ModelLogic : MonoBehaviour
{
    /// <summary>
    /// The ratio to scale a GameObject per frame
    /// </summary>
    protected const float SCALERATIO = 0.05f;

    /// <summary>
    /// The degress to rotate a GameObject per frame
    /// </summary>
    protected const int ROTATEDEGREES = 2;

    private SessionState state = SessionState.Initial;

    private INetworkAdapter networkManager;

    private GameObject prefabToCreate = null;
    private List<PlayerSDC> players;

    private SyncDataContainer selected = null;

    // Instantiated before Start() to prevent NullReferenceException
    private List<IView> views = new List<IView>();

    /// <summary>
    /// Add a view, like the Input view, to receive notifications after model changes
    /// </summary>
    /// <param name="view">A component to view model data</param>
    public void AddView(IView view)
    {
        views.Add(view);
    }

    /// <summary>
    // non-synchronized information: the currently selected item on client-side
    /// </summary>
    public SyncDataContainer SelectedObject
    {
        get
        {
            return selected;
        }
        set { selected = value; }
    }

    


    /// <summary>
    /// The client's current session state
    /// </summary>
    public SessionState SessionState
    {
        get
        {
            return state;
        }

        set
        {
            
            state = value;

        }
    }

    /// <summary>
    /// The prefab used for creating a GameObject via AddGameObject
    /// </summary>
    public GameObject DefaultPrefab
    {
        set
        {
            prefabToCreate = value;
        }
    }

    /// <summary>
    /// Called on initialization
    /// </summary>
    protected virtual void Start()
    {
        networkManager = Components.NetworkAdapter;

        players = new List<PlayerSDC>();

        
    }

    /// <summary>
    /// Returns true, if the respective GameObject is currently selected
    /// </summary>
    /// <param name="target">The GameObject or a child GameObject of it</param>
    /// <returns></returns>
    public bool IsSelected(GameObject target)
    {
        if (selected == null)
            return false;

        GameObject root = RootOf(target);

        return root == selected.gameObject;
    }

    private GameObject RootOf(GameObject child)
    {
        if (child == null)
            return null;

        if (child.GetComponent<SyncDataContainer>() != null)
        {
            return child;
        }

        if (child.transform.parent != null)
            return RootOf(child.transform.parent.gameObject);

        return null;
    }

    /// <summary>
    /// Add a SyncDataContainer to a list. Not necessary and deprecated after commitment to the new network component.
    /// </summary>
    /// <param name="sdc">The SyncFataContainer to add</param>
    public virtual void AddDataContainer(SyncDataContainer sdc)
    {
        if (sdc.ID == -1)
        {
            sdc.ID = sdc.GetInstanceID() - 1;
            Boolean idUnique = false;

            while (!idUnique)
            {
                sdc.ID = sdc.ID + 1;
                idUnique = true;

                foreach (SyncDataContainer dataContainer in FindObjectsOfType(typeof(SyncDataContainer)))
                {
                    if (dataContainer.ID == sdc.ID)
                        idUnique = false;
                }
            }
        }

        else if (sdc is PlayerSDC)
        {
            players.Add(sdc as PlayerSDC);
        }


        
    }

    /// <summary>
    /// Rotate the selected GameObject around the y-axis
    /// </summary>
    /// <param name="clockwise">true, if rotation is intended clockwise</param>
    public virtual void RotateSelectedY(bool clockwise)
    {
        if (state == SessionState.Running && selected != null)
        {
            if (clockwise)
                selected.RotateY(-ROTATEDEGREES);
            else
                selected.RotateY(ROTATEDEGREES);

            foreach (IView view in views)
            {
                view.Notify();
            }
        }
    }

    /// <summary>
    /// Delete the selected GameObject
    /// </summary>
    public virtual void DeleteSelected()
    {

        if (state == SessionState.Running && selected != null)
        {
            networkManager.Delete(selected);

            selected.Delete();
            selected = null;

            foreach (IView view in views)
            {
                view.Notify();
            }


        }
    }

    /// <summary>
    /// Scale the selected GameObject in all three dimensions
    /// </summary>
    /// <param name="grow">true, if positive scaling is intended</param>
    public virtual void ScaleSelected(bool grow)
    {
        if (state == SessionState.Running && selected != null)
        {
            if (grow)
            {
                selected.Scale(1 + SCALERATIO);
            }
            else
            {
                selected.Scale(1 - SCALERATIO);
            }

            foreach (IView view in views)
            {
                view.Notify();
            }
        }
    }

    /// <summary>
    /// Manipulate any field on any SnycDataContainer on server-side for synchronization. Use Commands in SyncDataContainers instead, whenever possible
    /// </summary>
    /// <param name="randomID">ID to find the SyncDataContainer</param>
    /// <param name="fieldName">The name of the field</param>
    /// <param name="value">The new value</param>
    internal void SetIntValue(int randomID, string fieldName, int value)
    {
        networkManager.SetIntValue(randomID, fieldName, value);
    }


    /// <summary>
    /// Move the selected GameObject
    /// </summary>
    /// <param name="position">The client's global target position</param>
    /// <param name="target">The GameObject to move. The selected GameObject is moved anyway</param>
    public virtual void MoveSelected(Vector3 position, GameObject target)
    {
        //Target not used in this base method

        if (state == SessionState.Running && selected != null)
        {
            selected.MoveTo(position);

            foreach (IView view in views) {
                view.Notify();
            }
        }
    }

    /// <summary>
    /// Select a or deselect GameObject
    /// </summary>
    /// <param name="hit">The gameObject to select, or a chil GameObject of it</param>
    public virtual void Select(GameObject hit)
    {

        // In case of composited GameObjects
        hit = RootOf(hit);

        if (state == SessionState.Running && hit != null)
        {
            foreach (SyncDataContainer sdc in FindObjectsOfType(typeof(SyncDataContainer)))
            {
                if (sdc.gameObject == hit)
                {
                    if (selected == sdc)
                        selected = null;
                    else
                        selected = sdc;

                    foreach (IView view in views)
                    {
                        view.Notify();
                    }
                    
                }
            }

            
        }
    }

    /// <summary>
    /// Call the method ServerMethod on server-side.
    /// </summary>
    /// <param name="variant">Determines between workflows to run</param>
    /// <param name="parameters">Further parameters</param>
    internal void CallServerMethod(int variant, int[] parameters)
    {
        networkManager.CallServerMethod(variant, parameters);
    }

    /// <summary>
    /// Create a new synchronized GameObject. Called by InputControl
    /// </summary>
    /// <param name="pos">The client's global target position</param>
    /// <param name="rotation">The client's global target rotation</param>
    /// <param name="target">The target GameObect, the new one is placed on. Null, if new GemaObject is placed in the free room</param>
    /// <returns>The randomID to recover the created GameObject on client-side</returns>
    public virtual int AddGameObject(Vector3 pos, Quaternion rotation, GameObject target)
    {
        if (state == SessionState.Running)
        {
            int randomID = CreateSyncGameObject(prefabToCreate, pos, rotation);
            return randomID;
        }

        return -1;
    }

    /// <summary>
    /// Create a new synchronized GameObject. Called by AddGameObject
    /// </summary>
    /// <param name="prefab">The prefab for the new GameObject</param>
    /// <param name="position">The client's global target rotation</param>
    /// <param name="rotation">The client's global target rotation</param>
    /// <returns>TThe randomID to recover the created GameObject on client-side</returns>
    protected int CreateSyncGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Debug.Log("CreateSyncGameObject called, SessionState:");
        Debug.Log(SessionState.Running);

        if (state == SessionState.Running)
        {
            int randomID = networkManager.CreateSyncGameObject(prefab, position, rotation);
            return randomID;
        }

        return -1;
    }

    /// <summary>
    /// Create a new local unsynchronized GameObject.
    /// </summary>
    /// <param name="prefab">The prefab for the new GameObject</param>
    /// <param name="position">The client's global target rotation</param>
    /// <param name="rotation">The client's global target rotation</param>
    /// <returns>The created GameObject</returns>
    protected GameObject CreateAsyncGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (state == SessionState.Running)
            return Instantiate(prefab, position, rotation);

        return null;
    }


    /// <summary>
    /// Create a new synchronized GameObject with posiotn relative to the anchor.
    /// </summary>
    /// <param name="prefab">The prefab for the new GameObject</param>
    /// <param name="position">The client's global target rotation</param>
    /// <param name="rotation">The client's global target rotation</param>
    /// <returns>TThe randomID to recover the created GameObject on client-side</returns>
    public int CreateSyncGameObjectRelToCenter(GameObject prefab, Vector3 position, Quaternion rotation)
    {

        if (state == SessionState.Running)
            return networkManager.CreateSyncGameObjectRelToAnchor(prefab, position, rotation);

        return -1;
    }


    /// <summary>
    /// Create a new local unsynchronized GameObject relative to the anchor.
    /// </summary>
    /// <param name="prefab">The prefab for the new GameObject</param>
    /// <param name="position">The client's global target rotation</param>
    /// <returns>The created GameObject</returns>
    public GameObject CreateAsyncGameObjectRelToCenter(GameObject prefab, Vector3 position)
    {
        if (networkManager == null)
            networkManager = Components.NetworkAdapter;

        if (state == SessionState.Running)
            return networkManager.CreateAsyncGameObjectRelToCenter(prefab, position);

        return null;
    }

    /// <summary>
    /// Override this method to initialize an AR scene
    /// </summary>
    public virtual void CreateStartObjects()
    {
        // Empty; to be overridden.
    }

    /// <summary>
    /// Override this method and implement a switch-case branching for various server-side workflows. This method is intended to be called on server-side only.
    /// </summary>
    /// <param name="variant">Represents a case to run a specific workflow</param>
    /// <param name="parameters">Eventual paramters</param>
    public virtual void ServerMethod(int variant, int[] parameters)
    {
        // Empty; to be overridden.
    }






}
