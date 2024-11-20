using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LoadMap : MonoBehaviour
{
    // tilemap & tile sprite variables
    [Header("Tilemap & Tile Variables")]
    public Tilemap myTilemap;
    public TileBase _wall;
    public TileBase _door;
    public TileBase _chest;
    public TileBase _enemy;
    public TileBase _none;
    public TileBase _checker;

    // Tile variables
    [Header("Tile String Characters")]
    public string wall = "#";
    public string door = "O";
    public string chest = "*";
    public string enemy = "@";
    public string none = " ";

    // map grid (20X20)
    public int[,] myMap = new int[22, 12]; // !!!

    // mapping each spawnable object to an integer (0-5) as a method - Edit: VERY USEFUL
    private Dictionary<string, int> GetTileMapping()
    {
        return new Dictionary<string, int>
        {
             { wall, 1 },
             { door, 2 },
             { chest, 3 },
             { enemy, 4 },
             { none, 0 }
        };
    }

    // need to iterate through list of all maps at random
    // need to set the tiles in the center of the map
    int PickRandomMap()
    {
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Loading premade map");
        //myTilemap.ClearAllTiles();
        LoadPremadeMap();
    }

    string LoadPremadeMap()
    {
        Debug.Log("reading text file");
        string pathToFile = ($"{Application.dataPath}/2DMapStrings/TestMap.txt"); // in the Unity Assets folder, then path
        string[] myLines = System.IO.File.ReadAllLines(pathToFile); // create string from all idv. lines read

        for (int y = myLines.Length - 1; y>= 0; y--) // from mylines.length until it reaches 0 (to reverse it), still not flipped, maybe myLines ?
        {
            string myLine = myLines[y]; // so each line gets read in proper order one-by-one
            Debug.Log($"Reading Line: {myLine} at {y}");

            for (int x = 0; x < myLine.Length; x++)
            {   // on x axis, so accross the line to idv. char, read & assign each one
                char myChar = myLine[x];
                Debug.Log($"Reading Char: {myChar} at {x}");

                if (x < myMap.GetLength(0)
                 && y < myMap.GetLength(1))
                {
                    myMap[x, y] = myChar;
                    // since ConvertMapToTilemap() references GenerateMapString() for it's data, I just convert directly
                    if (myChar == '#')
                    {
                        Debug.Log($"Reading Wall char at: {x} , {y}");
                        myTilemap.SetTile(new Vector3Int(x, y, 0), _wall);
                    }
                    if (myChar == 'O')
                    {
                        Debug.Log($"Reading Door char at: {x} , {y}");
                        myTilemap.SetTile(new Vector3Int(x, y, 0), _door);
                    }
                    if (myChar == '*')
                    {
                        Debug.Log($"Reading Chest char at: {x} , {y}");
                        myTilemap.SetTile(new Vector3Int(x, y, 0), _chest);
                    }
                    if (myChar == '@')
                    {
                        Debug.Log($"Reading Enemy char at: {x} , {y}");
                        myTilemap.SetTile(new Vector3Int(x, y, 0), _enemy);
                    }
                    if (myChar == ' ')
                    {
                        Debug.Log($"Reading None char at: {x} , {y}");
                        myTilemap.SetTile(new Vector3Int(x, y, 0), _none);
                    }
                }
            }
        }
        return ($"{myLines}");
    }
}
// map starts writing tiles at the bottom left but in the text file that is the top left char
// so it ends up being flipped horizontally
// need to start placing at top left until top right
// then increment Y
// replace none chars with null instead of the _none tile
// set the first tile to myMap's height
// not working, needs to READ the text file in reverse, not when writing