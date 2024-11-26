using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class LoadMap : MonoBehaviour
{
    // tilemap & tile sprite variables
    [Header("Tilemap & Tiles")]
    public Tilemap myTilemap;
    public TileBase _wall;
    public TileBase _door;
    public TileBase _chest;
    public TileBase _enemy;
    public TileBase _none;
    public TileBase _winTile;


    // Tile variables
    [Header("Tile String Characters")]
    public string wall = "#";
    public string door = "O";
    public string chest = "*";
    public string enemy = "@";
    public string none = " ";

    [Header("Map Center Reference")]
    public Transform mapCenter;

    [Header("Map Dimensions")]
    public int mapWidth;
    public int mapHeight;
    void Start()
    {
        Debug.Log("Loading premade map...");
        LoadPremadeMap();
    }

    public void LoadPremadeMap()
    {
        // get path to files
        //Debug.Log("reading text file");
        string folderPath = $"{Application.dataPath}/2DMapStrings"; // in the Unity Assets folder, then path to folder, pick rand file from these
        string[] mapFiles = Directory.GetFiles(folderPath, "*.txt"); // Get all text files

        // get random text file
        int randomIndex = Random.Range(0, mapFiles.Length);
        string selectedFile = mapFiles[randomIndex];
        Debug.Log($"Selected map {selectedFile}");

        // read the text
        string[] myLines = File.ReadAllLines(selectedFile); // create string from all idv. lines read
        mapHeight = myLines.Length;
        mapWidth = myLines[0].Length;

        myTilemap.ClearAllTiles();
        // centering the map
        // converts the mapCenter position to integer tilemap coordinates
        Vector3Int mapOrigin = new Vector3Int(
            Mathf.RoundToInt(mapCenter.position.x) - mapWidth / 2,
            Mathf.RoundToInt(mapCenter.position.y) - mapHeight / 2,
            0
        );

        // placing tiles
        for (int y = myLines.Length - 1; y>= 0; y--) // from mylines.length until it reaches 0 (to reverse it), still not flipped, maybe myLines ?
        {
            string myLine = myLines[y]; // so each line gets read in proper order one-by-one
            Debug.Log($"Reading Line: {myLine} at {-y}");

            for (int x = 0; x < myLine.Length; x++)
            {   // on x axis, so accross the line to idv. char, read & assign each one
                char myChar = myLine[x];
                Debug.Log($"Reading Char: {myChar} at {x}");
                Vector3Int position = new Vector3Int(x, y, 0) + mapOrigin;
                    
                if (myChar == '#')
                {
                    //Debug.Log($"Placing Wall char at: {x} , {-y}");
                    myTilemap.SetTile(position, _wall); // -y to follow how the lines are read in reverse
                }
                if (myChar == 'O')
                {
                    //Debug.Log($"Placing Door char at: {x} , {-y}");
                    myTilemap.SetTile(position, _door);
                }
                if (myChar == '*')
                {
                    //Debug.Log($"Placing Chest char at: {x} , {-y}");
                    myTilemap.SetTile(position, _chest);
                }
               if (myChar == '@')
                {
                    //Debug.Log($"Placing Enem-y char at: {x} , {-y}");
                    myTilemap.SetTile(position, _enemy);
                }
                if (myChar == ' ')
                {
                    //Debug.Log($"Placing None char at: {x} , {-y}");
                    myTilemap.SetTile(position, null);
                }
            }
        }
    }
}