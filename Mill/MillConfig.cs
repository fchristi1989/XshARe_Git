using UnityEngine;
using System.Collections;

public class MillConfig : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The prefab used to represent the board with tiles")]
    public GameObject boardPrefab = null;

    [SerializeField]
    [Tooltip("The prefab used to represent the game piece")]
    public GameObject gamePieceWhitePrefab = null;

    [SerializeField]
    [Tooltip("The prefab used to represent the game piece")]
    public GameObject gamePieceBlackPrefab = null;

    [SerializeField]
    [Tooltip("The prefab used to represent the player (empty, only for meta data)")]
    public GameObject millPlayerPrefab = null;

    [SerializeField]
    [Tooltip("The material used to represent the white pieces")]
    public Material whiteMat = null;

    [SerializeField]
    [Tooltip("The material used to represent the black pieces")]
    public Material blackMat = null;
}
