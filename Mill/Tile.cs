using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    private int id = -1;

    private int posX;
    private int posY;

    private List<Row> rows;

    public int ID
    {
        set { id = value; }
        get { return id; }
    }

    public int PosX
    {
        get
        {
            return posX;
        }
    }

    public int PosY
    {
        get
        {
            return posY;
        }
    }

    public List<Row> Rows
    {
        get { return rows; }
        set { rows = value; }
    }

    private void Start()
    {
        GameObject bl = GameObject.Find(Paths.Custom);
        MillLogic ml = bl.GetComponent<MillLogic>();
        ml.AddTile(this);

        posX = int.Parse(gameObject.name.Split('_')[1]);
        posY = int.Parse(gameObject.name.Split('_')[2]);

        rows = new List<Row>();
    }

    
}
