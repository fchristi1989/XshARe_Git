using UnityEngine;
using System.Collections;

public class FPConfig : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The prefab used to represent a chair.")]
    public GameObject chairPrefab = null;

    [SerializeField]
    [Tooltip("The prefab used to represent a table.")]
    public GameObject tablePrefab = null;

    [SerializeField]
    [Tooltip("The prefab used to represent a shelf.")]
    public GameObject shelfPrefab = null;

    [SerializeField]
    [Tooltip("The prefab used to represent the selection panel.")]
    public GameObject pnlSelectPrefab = null;
}
