using System.Collections.Generic;

public class Row
{
    private List<Tile> tiles;

    public Row()
    {
        tiles = new List<Tile>();
    }

    public List<Tile> Tiles
    {
        get { return tiles; }
    }

    public void Sort()
    {
        List<Tile> sortedList = new List<Tile>();

        while (tiles.Count > 0)
        {
            Tile next = tiles[0];

            foreach (Tile tile in tiles)
            {
                if (tile.PosY < next.PosY || tile.PosX < next.PosX)
                {
                    next = tile;
                }
            }

            tiles.Remove(next);
            sortedList.Add(next);
        }

        tiles = sortedList;
    }
}