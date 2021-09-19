using UnityEngine;
using System.Collections;

public class UserAugmentation : MonoBehaviour
{

    [SerializeField]
    private GameObject augmentationPrefab = null;

    [SerializeField]
    private float startDelay = 1.0f;

    [SerializeField]
    private float repeatRate = 1.0f;

    private int augmentationID = -1;
    private SyncDataContainer augmentation = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ModelLogic model = Components.ModelLogic;

        if (augmentationID == -1 && model.SessionState == SessionState.Running)
        {
            Camera camera = Camera.current;
            augmentationID =  model.CreateSyncGameObjectRelToCenter(augmentationPrefab, camera.transform.position, camera.transform.rotation);
        }

        if (augmentationID != -1 && augmentation == null)
        {
            foreach (SyncDataContainer sdc in FindObjectsOfType(typeof(SyncDataContainer)))
            {
                if (sdc.RandomID == augmentationID)
                {
                    augmentation = sdc;

                    InvokeRepeating("UpdateAugmentation", startDelay, repeatRate);
                }
            }
        }
    }

    private void UpdateAugmentation()
    {
        if (augmentation == null)
            return;

        augmentation.MoveTo(Camera.current.transform.position);
        //TODO: rotation missing
    }
}
