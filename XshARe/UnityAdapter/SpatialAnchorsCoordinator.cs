using UnityEngine;
using System.Collections;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System;
using Microsoft.Azure.SpatialAnchors.Unity;

public class SpatialAnchorsCoordinator : AzureSpatialAnchorsSharedAnchorDemoScript
{
    public static Text textBox;

    #region Unity Inspector Variables
    [SerializeField]
    [Tooltip("The prefab used to represent a game piece.")]
    private GameObject gamePiecePrefab = null;

    [SerializeField]
    [Tooltip("Prefab for Azure Spatial Anchors Room Information.")]
    private GameObject roomPrefab = null;

    [SerializeField]
    [Tooltip("Camera coordinates of users are synchronized.")]
    private bool syncCamera = false;

    [SerializeField]
    [Tooltip("Prefab attached to camera positions.")]
    private GameObject cameraPrefab = null;

    [SerializeField]
    [Tooltip("Prefab to visualize user input.")]
    private GameObject inputPrefab = null;

    [SerializeField]
    [Tooltip("Prefab to visualize user selection panel.")]
    private GameObject selectionPrefab = null;

    /*
    [SerializeField]
    [Tooltip("Prefab to hold children positioned to an anchor.")]
    private GameObject anchorBasedRoot = null;
    */
    #endregion Unity Inspector Variables

    private InputControl inputControl = null;
    private InputView inputViewer = null;

    private ModelLogic modelLogic = null;
    private INetworkAdapter networkManager = null;
    private GameObject anchor = null;

    private GameObject cameraAttached = null;

    private AnchorState anchorState = AnchorState.Initial;

    private Button btnCreateAnchor = null;
    private Button btnLocateAnchor = null;
    private Button btnHost = null;
    private Button btnClient = null;



    // Use this for initialization
    public override void Start()
    {
        base.Start();

        textBox = base.feedbackBox;

        inputControl = new InputControl(base.arRaycastManager);
        inputViewer = new InputView(selectionPrefab, inputPrefab, base.arRaycastManager);

        modelLogic = Components.ModelLogic;
        networkManager = Components.NetworkAdapter;
        anchor = Components.Anchor;

        // ->ModelLogic
        modelLogic.DefaultPrefab = gamePiecePrefab;

        if (syncCamera)
            InvokeRepeating("CreateCameraAttached", 5.0f, 5.0f);


        GameObject uxParent = GameObject.Find("UXParent");
        GameObject mobileUX = uxParent.transform.Find("MobileUX").gameObject;
        //Canvas canvas = mobileUX.GetComponent<Canvas>();
        Button[] buttons = mobileUX.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.name == "CreateFlowButton")
                btnCreateAnchor = button;
            else if (button.name == "LocateFlowButton")
                btnLocateAnchor = button;
            else if (button.name == "ExtendedHostButton")
                btnHost = button;
            else if (button.name == "ExtendedClientButton")
                btnClient = button;
        }

    }

    // Update is called once per frame
    public override void Update()
    {
        try
        {
            
            base.Update();
            inputControl.Update();
            inputViewer.Update();

            if (modelLogic.SessionState == SessionState.Initial && anchorState == AnchorState.Initial)
            {
                modelLogic.SessionState = SessionState.Waiting;
                anchorState = AnchorState.HostChoice;

            }
            else if (base.CAppState == AppState.DemoStepChooseFlow && anchorState == AnchorState.HostChoice)
            {
                Debug.Log("Changing buttons");

                btnCreateAnchor.gameObject.SetActive(false);
                btnLocateAnchor.gameObject.SetActive(false);
                btnHost.gameObject.SetActive(true);
                btnClient.gameObject.SetActive(true);

                Debug.Log("Changed buttons");

                textBox.text = "Establish Connection";

                modelLogic.SessionState = SessionState.Waiting;

            }
            else if (base.CAppState == AppState.DemoStepStopSession && anchorState == AnchorState.AnchorChoice)
            {
                anchorState = AnchorState.SaveAnchor;
                UpdateSpatialAnchorsRoom(base.latestAnchorNumber);
            }
            else if ((base.CAppState == AppState.DemoStepStopSessionForQuery) && modelLogic.SessionState == SessionState.Waiting)
            {

                //GameObject anchor = Instantiate(anchorBasedRoot);
                anchor = Components.Anchor;
                anchor.AddComponent<CloudNativeAnchor>();
                MoveAnchoredObject(anchor, new Vector3(), new Quaternion(), currentCloudAnchor);
                //networkManager.SetAnchor(anchor);

                if (syncCamera)
                    CreateCameraAttached();

                modelLogic.SessionState = SessionState.Running;
                modelLogic.CreateStartObjects();

                textBox.text = "Session Running";
            }
            else if (base.CAppState == AppState.DemoStepInputAnchorNumber)
            {
                SetAnchorToFind();
                base.CDemoFlow = DemoFlow.LocateFlow;
                base.CAppState = AppState.DemoStepCreateSession;
            }


            if (syncCamera)
                UpdateCameraAttached();

            
        }
        catch (Exception e)
        {
            textBox.text = e.GetType().ToString();
            textBox.text += e.StackTrace;
        }

        //textBox.text = base.CAppState.ToString();

    }

    internal void ConnectedSuccessfully()
    {
        anchorState = AnchorState.AnchorChoice;
        btnCreateAnchor.gameObject.SetActive(true);
        btnLocateAnchor.gameObject.SetActive(true);
        btnHost.gameObject.SetActive(false);
        btnClient.gameObject.SetActive(false);
    }

    internal void UpdateClientButton(string ip)
    {
        Debug.Log("UPDATECLIENTBUTTON");
        Debug.Log(anchorState);
        if (anchorState == AnchorState.HostChoice || anchorState == AnchorState.HostFound)
        {
            Text text = btnClient.GetComponentInChildren<Text>();
            text.text = "Connect to Host";
            textBox.text = "Found Host: " + ip;
            anchorState = AnchorState.HostFound;
        }

        Debug.Log("UPDATECLIENTBUTTON finisches");

    }

    //TODO:AugmentedUserSDC
    private void UpdateCameraAttached()
    {
        Camera camera = Camera.current;

        if (camera != null && cameraAttached != null)
        {
            Vector3 position = camera.transform.position;

            cameraAttached.transform.position = new Vector3(position.x, position.y, position.z);

            cameraAttached.transform.rotation = camera.transform.rotation;

        }

    }


    //TODO:AugmentedUserSDC
    private void CreateCameraAttached()
    {
        Camera camera = Camera.current;


        if (cameraAttached != null)
        {
            Destroy(cameraAttached);
            cameraAttached = null;
        }


        if (camera != null)
        {

            if (cameraPrefab != null)
            {
                Vector3 position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);

                cameraAttached = GameObject.Instantiate(cameraPrefab, position, camera.transform.rotation);
                //cameraAttached.transform.parent = networkModel.transform;
                // TODO: Anders machen
            }
        }
    }

    private async void SetAnchorToFind()
    {
        string key = "";
        int number = -1;

        //RoomSDC room = modelLogic.Room;
        RoomSDC room = FindObjectOfType<RoomSDC>();

        if (room != null)
        {
            number = room.RoomNumber;
            base.AnchorNumberToFind = number;

#if !UNITY_EDITOR
            key = await anchorExchanger.RetrieveAnchorKey((long)number);
#endif

            base.AnchorKeyToFind = key;
        }

    }

    private void UpdateSpatialAnchorsRoom(long roomNumber)
    {
        RoomSDC room = FindObjectOfType<RoomSDC>();

        if (room == null)
        {
            //room = networkManager.CreateSyncGameObject(roomPrefab, new Vector3(), new Quaternion()) as RoomSDC;
            int randomID = networkManager.CreateSyncGameObject(roomPrefab, new Vector3(), new Quaternion());

            //GameObject room = Instantiate(roomPrefab);
            //RoomSDC dataContainer = room.GetComponent<RoomSDC>();

            /*
            if (room != null)
            {
                room.RoomNumber = (int)roomNumber;
                //modelLogic.Room = dataContainer;
            }
            */

            if (randomID == -1)
                Debug.Log("Error: No room created");
            else
                networkManager.SetIntValue(randomID, "RoomNumber", (int)roomNumber);

            //room.transform.parent = networkModel.transform;

        }
    }

    
  
}
