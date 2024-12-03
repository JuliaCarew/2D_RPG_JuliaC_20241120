using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class LoadMap : MonoBehaviour
{
    [Header("Transform & GameObjects")]
    public Transform mapCenter;

    [Header("Tilemap & Tiles")]
    public Tilemap myTilemap;
    public TileBase _wall;
    public TileBase _door;
    public TileBase _chest;
    public TileBase _enemy;
    public TileBase _none;
    public TileBase _win;

    [Header("Tile String Characters")]
    public string wall = "#";
    public string door = "O";
    public string chest = "*";
    public string enemy = "@";
    public string none = " ";
    public string win = "%";

    [Header("Map Dimensions")]
    public int mapWidth;
    public int mapHeight;

     [Header("Combat System Reference")]
    public Combat combatSystem;
    public static List<Vector3Int> enemyPositions = new List<Vector3Int>();

    void Start()
    {
        //Debug.Log("Loading premade map...");
        LoadPremadeMap();
        if (combatSystem != null)
        {
            combatSystem.enemies = new List<Vector3Int>(enemyPositions); // Pass enemy positions to Combat
        }
    }

    public void LoadPremadeMap()
    {
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
        enemyPositions.Clear();
        // centering the map
        // converts the mapCenter position to integer tilemap coordinates
        Vector3Int mapOrigin = new Vector3Int(
            Mathf.RoundToInt(mapCenter.position.x) - mapWidth / 2,
            Mathf.RoundToInt((mapCenter.position.y) - mapHeight / 2) + 7, // + y 0.5
            0
        );

        // placing tiles
        for (int y = myLines.Length - 1; y>= 0; y--) // from mylines.length until it reaches 0 (to reverse it), still not flipped, maybe myLines ?
        {
            string myLine = myLines[y]; // so each line gets read in proper order one-by-one
            //Debug.Log($"Reading Line: {myLine} at {-y}");

            for (int x = 0; x < myLine.Length; x++)
            {   // on x axis, so accross the line to idv. char, read & assign each one
                char myChar = myLine[x];
                //Debug.Log($"Reading Char: {myChar} at {x}");
                Vector3Int position = new Vector3Int(x, -y, 0) + mapOrigin;
                    
                switch (myChar)
                {
                    case '#':
                        myTilemap.SetTile(position, _wall);
                        break;
                    case 'O':
                        myTilemap.SetTile(position, _door);
                        break;
                    case '*':
                        myTilemap.SetTile(position, _chest);
                        break;
                    case '@':
                        myTilemap.SetTile(position, _enemy);
                        enemyPositions.Add(position); // Add to enemy positions
                        break;
                    case ' ':
                        myTilemap.SetTile(position, null);
                        break;
                    case '%':
                        myTilemap.SetTile(position, _win);
                        break;
                }
            }
        }
    }
}
// make the chests do something
// add player health bar
// fix some maps having blocked win tiles