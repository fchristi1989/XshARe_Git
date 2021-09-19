using UnityEngine;
using System.Collections;

public class SelectionHandler : MonoBehaviour
{
    
    public void AddChair()
    {
        Debug.Log("SelectionHandler.AddChair called");
        Logic.AddChair(this.gameObject);
    }

    public void AddTable()
    {
        Debug.Log("SelectionHandler.AddTable called");
        Logic.AddTable(this.gameObject);

    }

    public void AddShelf()
    {
        Debug.Log("SelectionHandler.AddShelf called");
        Logic.AddShelf(this.gameObject);

    }

    private FPLogic Logic
    {
        get
        {
            return Components.ModelLogic as FPLogic;
        }
    }
}
