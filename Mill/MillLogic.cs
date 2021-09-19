using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MillLogic : ModelLogic
{
    //not synchronized
    private List<Tile> tiles;
    private List<Row> rows;

    private MillPlayerSDC self;
    private int selfRandomID;

    private MillConfig config; 

    //synchronized
    //private List<MillPlayerSDC> millPlayers;
    //private List<GamePieceSDC> pieces;

    private Text stateText = null;


    public MillPlayerSDC Self
    {
        get { return self; }
    }

    protected override void Start()
    {
        base.Start();

        tiles = new List<Tile>();
        rows = new List<Row>();
        //millPlayers = new List<MillPlayerSDC>();
        //pieces = new List<GamePieceSDC>();

        config = this.gameObject.GetComponent<MillConfig>();

        //enable for desktop testing
        /*
        base.SessionState = SessionState.Running;
        CreateStartObjects();
        */

        selfRandomID = -1;

    }


    private void Update()
    {
        if (base.SessionState == SessionState.Running)
        {
            if (rows.Count == 0)
                CreateRows();

            if (self != null)
                SpatialAnchorsCoordinator.textBox.text = self.State.ToString() + "\n(" + self.PiecesToPlace + " pieces left)";
        }

        if (self == null)
        {
            foreach (MillPlayerSDC player in FindObjectsOfType(typeof(MillPlayerSDC)))
            {
                if (selfRandomID == player.RandomID)
                {
                    Debug.Log("player self found");
                    self = player;
                }
            }
        }
    }

    public void AddTile(Tile tile)
    {
        tiles.Add(tile);
        tile.ID = tiles.Count - 1;
    }

    private void CreateRows()
    {
        CreateFourRows(new int[] { 0, 1, 0, 2, 0, 3 });
        CreateFourRows(new int[] { -3, 3, 0, 3, 3, 3 });
        CreateFourRows(new int[] { -2, 2, 0, 2, 2, 2 });
        CreateFourRows(new int[] { -1, 1, 0, 1, 1, 1});

    }

    private void CreateFourRows (int[] coordinates1)
    {
        int[] coordinates2 = new int [coordinates1.Length];
        int[] coordinates3 = new int[coordinates1.Length];
        int[] coordinates4 = new int[coordinates1.Length];


        for (int i = 0; i < coordinates1.Length; i++)
        {
            coordinates2[i] = coordinates1[i] * -1;

            coordinates3[0] = coordinates1[1];
            coordinates3[1] = coordinates1[0];
            coordinates3[2] = coordinates1[3];
            coordinates3[3] = coordinates1[2];
            coordinates3[4] = coordinates1[5];
            coordinates3[5] = coordinates1[4];

            coordinates4[i] = coordinates3[i] * -1;
        }

        CreateRow(coordinates1);
        CreateRow(coordinates2);
        CreateRow(coordinates3);
        CreateRow(coordinates4);


    }

    

    private void CreateRow(int[] coordinates)
    { 

        Row row = new Row();

        Tile tile1 = FindTile(coordinates[0], coordinates[1]);
        Tile tile2 = FindTile(coordinates[2], coordinates[3]);
        Tile tile3 = FindTile(coordinates[4], coordinates[5]);

        if (tile1 == null)
            return;

        row.Tiles.Add(tile1);
        row.Tiles.Add(tile2);
        row.Tiles.Add(tile3);

        Debug.Log("Row: " + rows.Count);
        Debug.Log(tile1);
        Debug.Log(tile2);
        Debug.Log(tile3);
        Debug.Log("");

        tile1.Rows.Add(row);
        tile2.Rows.Add(row);
        tile3.Rows.Add(row);

        // important to find neighbours
        row.Sort();

        rows.Add(row);
    }

    private Tile FindTile(int x, int y)
    {
        foreach (Tile tile in tiles)
        {
            if (tile.PosX == x && tile.PosY == y)
                return tile;
        }

        return null;
    }

    public override int AddGameObject(Vector3 pos, Quaternion rotation, GameObject target)
    {
        if (base.SessionState == SessionState.Running && target != null)
        {
            // state == Running -> self != null
            if (self.State == MillState.PlacePiece && self.PiecesToPlace > 0)
            {
                // Idea: GamePieces are larger than tiles -> as hit tile means an empty tile
                Tile targetTile = target.GetComponent<Tile>();

                if (targetTile != null)
                {
                    if (GetOwnerID(targetTile) == -1)
                    {
                        /*
                        GamePieceSDC addedPiece = base.CreateSyncGameObject(config.gamePiecePrefab, target.transform.position, target.transform.rotation) as GamePieceSDC;
                        addedPiece.PlayerID = self.ID;
                        addedPiece.TileID = targetTile.ID;
                        */

                        GameObject prefab = null;

                        if (self.Role == Role.White)
                            prefab = config.gamePieceWhitePrefab;
                        else if (self.Role == Role.Black)
                            prefab = config.gamePieceBlackPrefab;

                        int randomID = base.CreateSyncGameObject(prefab, target.transform.position, target.transform.rotation);
                        base.SetIntValue(randomID, "PlayerID", self.ID);
                        base.SetIntValue(randomID, "TileID", targetTile.ID);

                        /*
                        if (self.Role == Role.White)
                            addedPiece.gameObject.GetComponent<Renderer>().material = config.whiteMat;
                        else if (self.Role == Role.Black)
                            addedPiece.gameObject.GetComponent<Renderer>().material = config.blackMat;
                        */


                        self.PiecesToPlace -= 1;

                        //ServerMethod 1: Update States after turn
                        base.CallServerMethod(1, new int[] { self.ID, randomID });

                        


                        //return addedPiece.gameObject;
                        return randomID;
                    }
                }
            }
        }

        return -1;
    }


    //Called on serverside -> self and selected are invalid
    public override void ServerMethod(int variant, int[] parameters)
    {
        // variant 1: UpdateStates after turn
        if (variant == 1)
        {
            int playerID = parameters[0];
            int pieceRandomID = parameters[1];

            MillPlayerSDC[] millPlayers = FindObjectsOfType(typeof(MillPlayerSDC)) as MillPlayerSDC[];
            MillPlayerSDC activePlayer = null;

            foreach (MillPlayerSDC player in millPlayers)
            {
                if (player.ID == playerID)
                    activePlayer = player;
            }



            GamePieceSDC addedPiece = null;

            foreach (GamePieceSDC piece in FindObjectsOfType(typeof(GamePieceSDC)))
            {
                if (piece.RandomID == pieceRandomID)
                    addedPiece = piece;
            }

            if (RowFilled(addedPiece))
                activePlayer.State = MillState.RemovePiece;
            else
                UpdateStatesAfterTurn();

        }
    }


    public override void DeleteSelected()
    {
        if (base.SessionState == SessionState.Running && self.State == MillState.RemovePiece)
        {
            GamePieceSDC selectedPiece = base.SelectedObject as GamePieceSDC;

            if (selectedPiece != null)
            {
                if (selectedPiece.PlayerID != self.ID)
                {
                    //pieces.Remove(selectedPiece);
                    base.DeleteSelected();
                    UpdateStatesAfterTurn();
                }
            }
        }
            
    }

    public override void MoveSelected(Vector3 position, GameObject target)
    {
        if (base.SessionState == SessionState.Running && self.State == MillState.MovePiece && target != null)
        {
            GamePieceSDC selectedPiece = base.SelectedObject as GamePieceSDC;
            Tile targetTile = target.GetComponent<Tile>();

            
            if (selectedPiece != null && targetTile != null)
            {
                if (selectedPiece.PlayerID == self.ID && GetOwnerID(targetTile) == -1 && Neighbours(tiles[selectedPiece.TileID], targetTile))
                {
                    selectedPiece.gameObject.transform.position = target.transform.position;
                    selectedPiece.LocalPos = selectedPiece.gameObject.transform.localPosition;
                    
                    selectedPiece.TileID = targetTile.ID;

                    if (RowFilled(selectedPiece))
                        self.State = MillState.RemovePiece;
                    else
                        UpdateStatesAfterTurn();
                }
            }
        }
    }

    private bool Neighbours(Tile tile1, Tile tile2)
    {
        foreach (Row row in tile1.Rows)
        {
            // Same row
            if (row.Tiles.Contains(tile2))
            {
                // Neighbours in sorted row
                if (Math.Abs(row.Tiles.IndexOf(tile1) - row.Tiles.IndexOf(tile2)) == 1)
                    return true;
            }
        }

        
        return false;
    }

    private bool RowFilled(GamePieceSDC addedPiece)
    {
        Tile tile = null;


        foreach (Tile t in tiles)
        {
            if (addedPiece.TileID == t.ID)
            {
                tile = t;
            }
        }

        foreach (Row row in tile.Rows)
        {
                if (RowFilled(row, addedPiece))
                {
                    return true;
                }
        }

        return false;
    }

    private bool RowFilled(Row row, GamePieceSDC addedPiece)
    {

        foreach (Tile tile in row.Tiles)
        {
            if (GetOwnerID(tile) != addedPiece.PlayerID && addedPiece.TileID != tile.ID)
            {
                return false;
            }
        }

        return true;
    }

    
    private int GetOwnerID(Tile tile)
    {
        GamePieceSDC[] pieces = FindObjectsOfType(typeof(GamePieceSDC)) as GamePieceSDC[];

        foreach (GamePieceSDC piece in pieces)
        {
            if (piece.TileID == tile.ID)
            {
                return piece.PlayerID;
            }
        }

        return -1;
    }
    

    // This is Called when a player successfully ended his turn
    private void UpdateStatesAfterTurn()
    {
        MillPlayerSDC [] millPlayers = FindObjectsOfType(typeof(MillPlayerSDC)) as MillPlayerSDC [];

        if (millPlayers.Length >= 2)
        {
            if (millPlayers[0].State == MillState.Wait)
                UpdateStates(millPlayers[0], millPlayers[1]);
            else if (millPlayers[1].State == MillState.Wait)
                UpdateStates(millPlayers[1], millPlayers[0]);

            /*
            //TODO: Delete after Desktop Testing
            if (self.ID == 0)
                self = millPlayers[1];
            else
                self = millPlayers[0];
            */
        }
    }

    private void UpdateStates(MillPlayerSDC playerWait, MillPlayerSDC playerActive)
    {
        if (!TurnPossible(playerWait))
        {
            playerWait.State = MillState.Lost;
            playerActive.State = MillState.Won;
        }
        else if (playerActive.State == MillState.MovePiece || playerActive.State == MillState.PlacePiece || playerActive.State == MillState.RemovePiece)
        {
            if (playerWait.PiecesToPlace > 0)
                playerWait.State = MillState.PlacePiece;
            else
                playerWait.State = MillState.MovePiece;

            playerActive.State = MillState.Wait;
        }
    }

    private bool TurnPossible(MillPlayerSDC player)
    {
        bool result = true;

        if (player.PiecesToPlace == 0)
        {
            int piecesOnBoard = 0;
            result = false;

            GamePieceSDC[] pieces = FindObjectsOfType(typeof(GamePieceSDC)) as GamePieceSDC[];

            foreach (GamePieceSDC piece in pieces)
            {
                if (piece.PlayerID == player.ID)
                {
                    piecesOnBoard++;

                    Tile tile1 = null;
                    foreach (Tile tile in tiles)
                    {
                        if (piece.TileID == tile.ID)
                            tile1= tile;
                    }

                    foreach (Row row in tile1.Rows)
                    {
                        foreach (Tile tile2 in tiles)
                        {
                            if (GetOwnerID(tile2) == -1 && Math.Abs(row.Tiles.IndexOf(tile1) - row.Tiles.IndexOf(tile2)) == 1)
                                    result = true;
                        }

                    }
                }

            }

            if (piecesOnBoard < 3)
                return false;

        }

        return result; ;
    }

    public override void AddDataContainer(SyncDataContainer sdc)
    {
        /*
        if (sdc is MillPlayerSDC)
        {
            MillPlayerSDC player = sdc as MillPlayerSDC;
            millPlayers.Add(player);

            // Game can begin
            if (millPlayers.Count == 2 && millPlayers[0].Role == Role.Observer)
            {
                millPlayers[0].Role = Role.White;
                millPlayers[0].State = MillState.PlacePiece;
                millPlayers[1].Role = Role.Black;
                millPlayers[1].State = MillState.Wait;
            }
        }
        else if (sdc is GamePieceSDC)
        {
            GamePieceSDC piece = sdc as GamePieceSDC;
            pieces.Add(piece);
        }
        */
        base.AddDataContainer(sdc);
    }

    public override void CreateStartObjects()
    {
        config = this.gameObject.GetComponent<MillConfig>();
        GameObject board = base.CreateAsyncGameObjectRelToCenter(config.boardPrefab, new Vector3());
        
        stateText = board.transform.Find("State").Find("Text").gameObject.GetComponent<Text>();


        //MillPlayerSDC player = base.CreateSyncGameObject(config.millPlayerPrefab, new Vector3(), new Quaternion()) as MillPlayerSDC;
        selfRandomID = base.CreateSyncGameObject(config.millPlayerPrefab, new Vector3(), new Quaternion());

        

        //self = player;
        //self.gameObject.name = "Player" + self.GetInstanceID();


        // TODO: Delete after Desktop testing
        //base.CreateSyncGameObject(config.millPlayerPrefab, new Vector3(), new Quaternion());
    }
}
