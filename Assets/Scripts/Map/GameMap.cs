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
    int deathLimit;
    [SerializeField]
    int birthLimit;

    [SerializeField]
    float chanceToStartAlive = 0.45f;

    [SerializeField]
    int numberOfSteps;

    [SerializeField]
    GameObject fieldCell;

    GameObject[,] boardPositionsFloor;

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
    public void GenerateGameField()
    {
        GameFieldParent = new GameObject("GameField");
        Corridors = new GameObject("Corridors");
        Corridors.transform.SetParent(GameFieldParent.transform);
        Floor = new GameObject("Floor");
        Floor.transform.SetParent(GameFieldParent.transform);
        Walls = new GameObject("Walls");
        Walls.transform.SetParent(GameFieldParent.transform);
        Props = new GameObject("Props");
        Props.transform.SetParent(GameFieldParent.transform);

        GenerateMap();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y  <height; y++)
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

        //SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, xMapSize, yMapSize));
        //CreateBSP(rootSubDungeon);
        //rootSubDungeon.CreateRoom();

        //boardPositionsFloor = new GameObject[xMapSize, yMapSize];
        //DrawRooms(rootSubDungeon);
        //DrawCorridors(rootSubDungeon);

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

        GenerateWalls();
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

    #region MyRegion

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public int[,] map;

    //void Start()
    //{
    //    GenerateMap();
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        GenerateMap();
    //    }
    //}

    void GenerateMap()
    {
        map = new int[width, height];
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

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
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
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
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


    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

    #endregion


    public class SubDungeon
    {
        public SubDungeon left, right;
        public Rect rect;
        public Rect room = new Rect(-1, -1, 0, 0); // i.e null
        public int debugId;
        public List<Rect> corridors = new List<Rect>();

        private static int debugCounter = 0;

        bool hasEnemies;
        bool hasMech;
        bool hasProps;
        bool hasTraps;

        public SubDungeon(Rect mrect)
        {
            rect = mrect;
            debugId = debugCounter;
            debugCounter++;
        }

        public bool IAmLeaf()
        {
            return left == null && right == null;
        }

        public bool Split(int minRoomSize, int maxRoomSize)
        {
            if (!IAmLeaf())
            {
                return false;
            }

            // choose a vertical or horizontal split depending on the proportions
            // i.e. if too wide split vertically, or too long horizontally, 
            // or if nearly square choose vertical or horizontal at random
            bool splitH;
            if (rect.width / rect.height >= 1.25)
            {
                splitH = false;
            }
            else if (rect.height / rect.width >= 1.25)
            {
                splitH = true;
            }
            else
            {
                splitH = Random.Range(0.0f, 1.0f) > 0.5;
            }

            if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
            {
                return false;
            }

            if (splitH)
            {
                // split so that the resulting sub-dungeons widths are not too small
                // (since we are splitting horizontally) 
                int split = Random.Range(minRoomSize, (int)(rect.width - minRoomSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, rect.width, split));
                right = new SubDungeon(
                    new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
            }
            else
            {
                int split = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, split, rect.height));
                right = new SubDungeon(
                    new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
            }

            return true;
        }

        public void CreateRoom()
        {
            if (left != null)
            {
                left.CreateRoom();
            }
            if (right != null)
            {
                right.CreateRoom();
            }
            if (left != null && right != null)
            {
                CreateCorridorBetween(left, right);
            }
            if (IAmLeaf())
            {
                int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
                int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);
                // room position will be absolute in the board, not relative to the sub-dungeon
                room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
            }
        }

        public void SettingRoom(Vector2 roomPosition, int width, int height)
        {
            hasProps = height > width;
            if (hasEnemies)
                SetEnemies(roomPosition, width, height);
            else
            {

            }
            if (hasMech)
                SetMech(roomPosition, width, height);
            if (hasProps)
                SetProps(roomPosition, width, height);
            if (hasTraps)
                SetTraps(roomPosition, width, height);
        }
        void SetMiningBlocks(Vector2 roomPosition, int width, int height)
        {

        }
        void SetEnemies(Vector2 roomPosition, int width, int height)
        {
            int squareRoom = width * height;
            int countOfProps = Random.Range(1, squareRoom / 4);
            for (int i = 0; i < countOfProps; i++)
            {
                int xPos = Random.Range((int)roomPosition.x, (int)roomPosition.x + width);
                int yPos = Random.Range((int)roomPosition.y, (int)roomPosition.y + height);
                Vector3 position = new Vector3(xPos, yPos, 0f);
                Transform prop = Instantiate(GameMap.GM.props[0]).transform;
                prop.transform.position = position;
            }
        }
        int propPerSquare = 1;
        void SetProps(Vector2 roomPosition, int width, int height)
        {
            int squareRoom = width * height;
            int countOfProps = Random.Range(1, squareRoom / 4);
            for (int i = 0; i < countOfProps; i++)
            {
                int xPos = Random.Range((int)roomPosition.x, (int)roomPosition.x + width);
                int yPos = Random.Range((int)roomPosition.y, (int)roomPosition.y + height);
                Vector3 position = new Vector3(xPos, yPos, 0f);
                Transform prop = Instantiate(GameMap.GM.props[0]).transform;
                prop.transform.position = position;
                prop.transform.SetParent(GameMap.GM.Props.transform);
            }
        }
        void SetMech(Vector2 roomPosition, int width, int height)
        {

        }
        void SetTraps(Vector2 roomPosition, int width, int height)
        {

        }

        public void CreateCorridorBetween(SubDungeon left, SubDungeon right)
        {
            Rect lroom = left.GetRoom();
            Rect rroom = right.GetRoom();

            // attach the corridor to a random point in each room
            Vector2 lpoint = new Vector2((int)Random.Range(lroom.x + 1, lroom.xMax - 1), (int)Random.Range(lroom.y + 1, lroom.yMax - 1));
            Vector2 rpoint = new Vector2((int)Random.Range(rroom.x + 1, rroom.xMax - 1), (int)Random.Range(rroom.y + 1, rroom.yMax - 1));

            // always be sure that left point is on the left to simplify the code
            if (lpoint.x > rpoint.x)
            {
                Vector2 temp = lpoint;
                lpoint = rpoint;
                rpoint = temp;
            }

            int w = (int)(lpoint.x - rpoint.x);
            int h = (int)(lpoint.y - rpoint.y);

            // if the points are not aligned horizontally
            if (w != 0)
            {
                // choose at random to go horizontal then vertical or the opposite
                if (Random.Range(0, 1) > 2)
                {
                    // add a corridor to the right
                    corridors.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 1));

                    // if left point is below right point go up
                    // otherwise go down
                    if (h < 0)
                    {
                        corridors.Add(new Rect(rpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        corridors.Add(new Rect(rpoint.x, lpoint.y, 1, -Mathf.Abs(h)));
                    }
                }
                else
                {
                    // go up or down
                    if (h < 0)
                    {
                        corridors.Add(new Rect(lpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        corridors.Add(new Rect(lpoint.x, rpoint.y, 1, Mathf.Abs(h)));
                    }

                    // then go right
                    corridors.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 1));
                }
            }
            else
            {
                // if the points are aligned horizontally
                // go up or down depending on the positions
                if (h < 0)
                {
                    corridors.Add(new Rect((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    corridors.Add(new Rect((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs(h)));
                }
            }
        }

        public Rect GetRoom()
        {
            if (IAmLeaf())
            {
                return room;
            }
            if (left != null)
            {
                Rect lroom = left.GetRoom();
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }
            if (right != null)
            {
                Rect rroom = right.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }

            // workaround non nullable structs
            return new Rect(-1, -1, 0, 0);
        }
    }

    public void CreateBSP(SubDungeon subDungeon)
    {
        if (subDungeon.IAmLeaf())
        {
            // if the sub-dungeon is too large split it
            if (subDungeon.rect.width > maxRoomSize
                || subDungeon.rect.height > maxRoomSize
                || Random.Range(0.0f, 1.0f) > 0.25)
            {

                if (subDungeon.Split(minRoomSize, maxRoomSize))
                {
                    CreateBSP(subDungeon.left);
                    CreateBSP(subDungeon.right);
                }
            }
        }
    }

    public void DrawRooms(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }
        if (subDungeon.IAmLeaf())
        {
            for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                {
                    Vector2 position = new Vector2(i, j);
                    GameObject instance = Instantiate(floorTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    if (!GameMap.GM.CellisAvialble(position))
                        GameMap.GM.gameField.Add(position, new Cell(instance, position));
                    instance.transform.SetParent(Floor.transform);
                    boardPositionsFloor[i, j] = instance;

                    SpawnBlock(position);
                }
            }
            Vector2 roomPos = (new Vector2((int)subDungeon.room.x, (int)subDungeon.room.y));
            int width = (int)subDungeon.room.xMax - (int)subDungeon.room.x;
            int height = (int)subDungeon.room.yMax - (int)subDungeon.room.y;
            subDungeon.SettingRoom(roomPos, width, height);
        }
        else
        {
            DrawRooms(subDungeon.left);
            DrawRooms(subDungeon.right);
        }
    }

    void SpawnBlock(Vector2 position)
    {
        GameObject _blockBase = Instantiate(blockBase);
        _blockBase.transform.position = new Vector3(position.x, position.y, 0f);
        Block bLock = _blockBase.GetComponent<Block>();
        bLock.blockBase = blocks[ChooseItemInListOfItems(blocks)];
    }

    int ChooseItemInListOfItems(BlockBase[] blocks)
    {

        float total = 0;

        foreach (var elem in blocks)
        {
            total += elem.spawnChance;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (randomPoint < blocks[i].spawnChance)
            {
                return (int)i;
            }
            else
            {
                randomPoint -= blocks[i].spawnChance;
            }
        }
        return blocks.Length - 1;
    }

    void DrawCorridors(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }

        DrawCorridors(subDungeon.left);
        DrawCorridors(subDungeon.right);

       
        foreach (Rect corridor in subDungeon.corridors)
        {
            for (int i = (int)corridor.x; i < corridor.xMax; i++)
            {
                for (int j = (int)corridor.y; j < corridor.yMax; j++)
                {
                    if (boardPositionsFloor[i, j] == null)
                    {
                        Vector2 position = new Vector2(i, j);
                        GameObject instance = Instantiate(corridorTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        if (!GameMap.GM.CellisAvialble(position))
                            GameMap.GM.gameField.Add(position, new Cell(instance, position));
                        instance.transform.SetParent(Corridors.transform);
                        boardPositionsFloor[i, j] = instance;

                        SpawnBlock(position);
                    }
                }
            }
        }
    }


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
