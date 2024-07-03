using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;
    Vector2Int size = new Vector2Int(3, 3);

    Tile[,] tileArray;

    public List<int> numbers = new List<int>();
    public List<Tile> tiles = new List<Tile>();

    Tile emptyTile;

    private void Awake()
    {
        tileArray = new Tile[size.x, size.y];
    }


    private void Start()
    {
        int maxNumber = size.x * size.y;
        for (int i = 0; i < maxNumber; i++)
        {
            numbers.Add(i);
        }
        
        Spawn();
        RPCRandomNumber();
        NumbersToTiles();
    }

   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (emptyTile.index.y == size.y - 1)
                return;

            Tile tile = tileArray[emptyTile.index.x, emptyTile.index.y + 1];
            SwichNumber(tile, emptyTile);
            emptyTile = tile;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (emptyTile.index.y == 0)
                return;

            Tile tile = tileArray[emptyTile.index.x, emptyTile.index.y - 1];
            SwichNumber(tile, emptyTile);
            emptyTile = tile;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (emptyTile.index.x == size.x - 1)
                return;

            Tile tile = tileArray[emptyTile.index.x + 1, emptyTile.index.y];
            SwichNumber(tile, emptyTile);
            emptyTile = tile;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (emptyTile.index.x == 0)
                return;

            Tile tile = tileArray[emptyTile.index.x -1 , emptyTile.index.y];
            SwichNumber(tile, emptyTile);
            emptyTile = tile;
        }
    }
    void Spawn()
    {
        for(int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                GameObject tileObj = Instantiate(tilePrefab, transform);
                Tile tile = tileObj.GetComponent<Tile>();
                tiles.Add(tile.GetComponent<Tile>());

                tile.index = new Vector2Int(j, size.y - i - 1);
                tileArray[j, size.y -i -1] = tile;
            }
        }
    }

    void SwichNumber(Tile t1, Tile t2)
    {
        int tileNumber = t2.number;
        t2.Number(t1.number);
        t1.Number(tileNumber);
    }

    void NumbersToTiles()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].Number(numbers[i]);
            if (tiles[i].number == 0)
            {
                emptyTile = tiles[i];
            }
        }
    }

    public void RPCRandomNumber()
    {
        for (int i = numbers.Count -1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int changeNumber = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = changeNumber;
        }
    }
}
