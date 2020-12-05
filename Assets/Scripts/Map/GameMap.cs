using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameMap : MonoBehaviour
{
    public static GameMap GM;

    #region PublicVariables

    public GameObject GameFieldParent { get; private set; }
    public GameObject Corridors { get; private set; }
    public GameObject Walls { get; private set; }
    public GameObject Floor { get; private set; }
    public GameObject Props { get; private set; }
    public Dictionary<Vector2, Cell> gameField = new Dictionary<Vector2, Cell>();
    public Grid grid;
    public Vector2 MapSize { private get; set; }

    public int xMapSize
    {
        get
        {
            return Mathf.RoundToInt(MapSize.x);
        }
    }
    public int yMapSize
    {
        get
        {
            return Mathf.RoundToInt(MapSize.y);
        }
    }

    public int minRoomSize, maxRoomSize;
    public GameObject floorTile;
    public GameObject corridorTile;

    [Header("Enemies")]
    public GameObject[] enemies;
    [Header("Mechanisms")]
    public GameObject[] mechs;
    [Header("Props, traps, etc..")]
    public GameObject[] props;
    #endregion

    #region PrivateVariables

    Queue<Cell[]> destroyedCells = new Queue<Cell[]>();
    Queue<Coroutine> coroutines = new Queue<Coroutine>();

    [SerializeField] float timeToRestoreCell = 5f;

    [SerializeField]
    GameObject fieldCell;

    [Header("Blocks")]
    [SerializeField] GameObject blockBase;
    [SerializeField] BlockBase[] blocks;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        GM = this;
    }

    /// <summary>
    /// Check existing of cell at argument position
    /// </summary>
    /// <param name="cellPos"></param>
    /// <returns></returns>
    public bool CellisAvialble(Vector2 cellPos)
    {
        Cell cell;

        return gameField.TryGetValue(cellPos, out cell);
    }

    public bool CellisObstacle(Vector2 cellPos)
    {

        return gameField[cellPos].isObstacle;
    }

    void RemoveCell(Vector2 cellPos)
    {
        Destroy(gameField[cellPos].cellObject);
        gameField.Remove(cellPos);
    }

    public void DestroyMapCells(Cell[] cells)
    {
        foreach (var cell in cells)
        {
            cell.isObstacle = true;
            cell.cellObject.SetActive(false);
        }
        destroyedCells.Enqueue(cells);
        Coroutine coroutine = StartCoroutine(WaitAndRestore());
        coroutines.Enqueue(coroutine);
    }

    IEnumerator WaitAndRestore()
    {
        yield return new WaitForSeconds(timeToRestoreCell);
        RestoreMapCells();
    }

    void RestoreMapCells()
    {
        foreach (var cell in destroyedCells.Dequeue())
        {
            cell.isObstacle = false;
            cell.cellObject.SetActive(true);
        }
        StopCoroutine(coroutines.Dequeue());
    }

    private Vector2 randomPos;

    public Vector2 RandomizePosionOnMap()
    {
        StartCoroutine(RandomizePosition());
        return randomPos;
    }
    

    IEnumerator RandomizePosition()
    {
        bool positionNotFound = true;
        Vector2 position = -Vector2.one;

        while (positionNotFound)
        {
            position = new Vector2(Random.Range(0, xMapSize), Random.Range(0, yMapSize));

            if (GameMap.GM.CellisAvialble(position))
            {
                positionNotFound = false;
            }
            yield return null;
        }

        randomPos = position;
        yield break;
    }

    Vector2 StopPointFinded(Vector2 startPosition, Vector2 dir)
    {
        int length = 0;

        if (Mathf.Abs(dir.y) > 0)
            length = yMapSize;
        else if (Mathf.Abs(dir.x) > 0)
            length = xMapSize;

        Vector2 position = startPosition;
        Vector2 stopPoint;
        for (int i = 0; i <= length; i++)
        {
            position += dir;
            if (!CellisAvialble(position) || CellisObstacle(position))
            {
                stopPoint = position - dir;

                return stopPoint;
            }
        }
        Debug.Log($"Error, point not founded! SpawnPosition: {startPosition}, Length {length} (Return -777 | -777)");;
        return new Vector2(-777, -777);
    }

    public Vector2 GetLastPointInSelectedDirection(Vector2 startPosition, Vector2 dir)
    {
        if (dir == Vector2.up || dir == -Vector2.up || dir == -Vector2.left || dir == Vector2.left)
            return StopPointFinded(startPosition, dir);
        else
        {
            Debug.Log("Direction is wrong! Error! (Return -999 | -999)");
            return new Vector2(-999, -999);
        }
    }
    [SerializeField] GameObject coord;
    //Генерация игрового поля 
    public string GenerateGameField(Vector2 mapSize, string seed = " ")
    {
        GameMap.GM.MapSize = mapSize;

        GameFieldParent = new GameObject("GameField");
        Corridors = new GameObject("Corridors");
        Corridors.transform.SetParent(GameFieldParent.transform);
        Floor = new GameObject("Floor");
        Floor.transform.SetParent(GameFieldParent.transform);
        Walls = new GameObject("Walls");
        Walls.transform.SetParent(GameFieldParent.transform);
        Props = new GameObject("Props");
        Props.transform.SetParent(GameFieldParent.transform);

        bool useRandomSeed = seed.Equals(" ") ? true : false;
        GenerateMap(useRandomSeed, seed);
        for (int x = 0; x < xMapSize; x++)
        {
            for (int y = 0; y  <yMapSize; y++)
            {
                if(map[x, y] <= 0)
                {
                    Vector2 position = new Vector2(x, y);
                    GameObject instance = Instantiate(floorTile, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    GameMap.GM.gameField.Add(position, new Cell(instance, position));
                    instance.transform.SetParent(Floor.transform);
                }
            }
        }
        GenerateWalls();

        //bool[,] map = new bool[xMapSize, yMapSize];
        //GameObject gameCoords = new GameObject("GameCoords");
        //gameCoords.SetActive(false);
        //for (int x = 0; x < xMapSize; x++)
        //{
        //    for (int y = 0; y < yMapSize; y++)
        //    {
        //        Vector2 pos = new Vector2(x, y);
        //        map[x, y] = CellisAvialble(pos);
        //        Transform coordinate = Instantiate(coord).transform;
        //        coordinate.name = x + "/" + y;
        //        coordinate.position = new Vector3(x, y, -0.1f);
        //        coordinate.GetComponent<TextMeshPro>().text = x + "/" + y;
        //        coordinate.SetParent(gameCoords.transform);
        //    }
        //}

        //grid.CreateGrid(map, MapSize);

        return this.seed;
    }
    [SerializeField] GameObject wall;
    void GenerateWalls()
    {
        List<Vector2> walls = new List<Vector2>();
        foreach (var item in gameField)
        {
            Vector2 position = new Vector2(item.Key.x, item.Key.y);

            for (int i = Mathf.RoundToInt(position.x) - 1; i < Mathf.RoundToInt(position.x) + 2; i++)
            {
                for (int k = Mathf.RoundToInt(position.y) - 1; k < Mathf.RoundToInt(position.y) + 2; k++)
                {
                    Vector2 wpos = new Vector2(i, k);

                    if (!CellisAvialble(wpos))
                    {
                        Transform _wall = Instantiate(wall).transform;
                        _wall.position = new Vector3(wpos.x, wpos.y, 0f);
                        _wall.SetParent(Walls.transform);
                        walls.Add(wpos);
                    }
                }
            }
        }
    }

    #region MapGenerator

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public int[,] map;


    void GenerateMap(bool randomSeed, string _seed = "")
    {
        useRandomSeed = randomSeed;
        if(!useRandomSeed) seed = _seed;
        map = new int[xMapSize, yMapSize];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < xMapSize; x++)
        {
            for (int y = 0; y < yMapSize; y++)
            {
                if (x == 0 || x == xMapSize - 1 || y == 0 || y == yMapSize - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < xMapSize; x++)
        {
            for (int y = 0; y < yMapSize; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < xMapSize && neighbourY >= 0 && neighbourY < yMapSize)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    #endregion


    public void DestroyGameField()
    {
        gameField.Clear();
        Destroy(GameFieldParent);
    }
}


public class Cell
{
    #region PublicVariables

    public GameObject cellObject;

    public Vector2 myPosition;

    public bool isObstacle;

    #endregion
    #region PrivateRegion


    #endregion
    public Cell(GameObject cell, Vector2 myPos)
    {
        cellObject = cell;
        myPosition = myPos;
    }
}
