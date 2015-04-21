using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour {

public GameObject GridTile;
public int gridX = 10;
public int gridY = 10;
public float spacing = 1f;
public List<GridTile> Tiles = new List<GridTile>();

public static GridGenerator Instance;

//private int TileIndex = 0;
//private int TileIndex2 = 1;

void Awake()
{
    Instance = this;
}

void Start() {
    for (int y = 0; y < gridY; y++) {
        for (int x = 0; x < gridX; x++) {
            Vector3 pos = new Vector3(x, 0, y) * spacing;
            GameObject Tile = Instantiate(GridTile, pos, Quaternion.identity) as GameObject;
            Tile.transform.parent = GameObject.Find("Grid").transform;
            Tile.name = "tile(" + x + "," + y + ")";
            Tile.transform.tag = "Selectable";
        }
    }
} 
	// Update is called once per frame
	void Update () {
	
	}

    public GridTile NewTile()
    {
        int TileIndex = 0;
        int TileIndex2 = 1;
        GridTile Tile;
        if (Gamemanager.instance.Current().CurrentPlayer == 0)
        {
            Tile = Tiles[TileIndex];
            while(Tile.transform.tag == "UnSelectable")
            {
                TileIndex++;
                Tile = Tiles[TileIndex];
            }
            Tile.transform.tag = "UnSelectable";
        }
        else
        {
            Tile = Tiles[Tiles.Count -TileIndex2];
            while(Tile.transform.tag == "UnSelectable")
            {
                TileIndex2++;
                Tile = Tiles[Tiles.Count - TileIndex2];
            }
            Tile.transform.tag = "UnSelectable";
        }

        return Tile;
    }
}
