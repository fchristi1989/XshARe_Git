using UnityEngine;
using System.Collections;

public class SelectionPanelHandler : MonoBehaviour
{
    public void OnDelete()
    {
        Components.ModelLogic.DeleteSelected();
    }
}
