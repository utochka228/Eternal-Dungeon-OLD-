using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct Style{
	public Vector2Int range;
	public string styleName;
	
}
[System.Serializable]
public struct MapStyle{
	[SerializeField] Style[] styles;
	public string GetCurrentMapStyle(int level){
		if(level > styles[styles.Length-1].range.y){
			for (int i = 0; i < styles.Length; i++)
			{
				styles[i].range =  new Vector2Int(styles[i].range.x + styles[styles.Length-1].range.y, styles[i].range.y + styles[styles.Length-1].range.y);
			}
		}
		for (int i = 0; i < styles.Length; i++)
		{
			int a = styles[i].range.x;
			int b = styles[i].range.y;

			if(level >= a && level <= b){
				Debug.Log(styles[i].styleName);
				return styles[i].styleName;
			}
		}
		return "ERROR";
	}
}
[System.Serializable]
public struct MapSize{

    public Vector2 Size;
    public int xMapSize
    {
        get{return Mathf.RoundToInt(Size.x);}
    }
    public int yMapSize
    {
        get{return Mathf.RoundToInt(Size.y);}
    }
}
public class GameMap : MonoBehaviour
{
    public static GameMap GM;

	public Relocation relocation;
    public GameObject GameFieldParent { get; private set; }
    public GameObject Corridors { get; private set; }
    public GameObject Walls { get; private set; }
    public GameObject Floor { get; private set; }
    public GameObject Props { get; private set; }
    public GameObject Blocks { get; private set; }
    public Dictionary<Vector2, Cell> gameField = new Dictionary<Vector2, Cell>();
    public Grid grid;
	
	public MapSize mapSize;
	[SerializeField] MapStyle mapStyles;

	GameMapStyle myMapStyle;

    void Awake()
    {
        GM = this;
    }

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
            position = new Vector2(Random.Range(0, mapSize.xMapSize), Random.Range(0, mapSize.yMapSize));

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
            length = mapSize.yMapSize;
        else if (Mathf.Abs(dir.x) > 0)
            length = mapSize.xMapSize;

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
    [SerializeField] Prop tunnelProp;
    [SerializeField] Prop checkpointProp;
    [SerializeField] GameObject propHolder;

    [SerializeField] bool GenerateGameCoords;
    //Генерация игрового поля 
    public string GenerateGameField(string seed = " ")
    {
		myMapStyle = Resources.Load<GameMapStyle>(Path.Combine("MapStyles", mapStyles.GetCurrentMapStyle(relocation.CurrentDungeonLevel)));
        #region Create_Field_Parents
        GameFieldParent = new GameObject("GameField");
        Corridors = new GameObject("Corridors");
        Corridors.transform.SetParent(GameFieldParent.transform);
        Floor = new GameObject("Floor");
        Floor.transform.SetParent(GameFieldParent.transform);
        Walls = new GameObject("Walls");
        Walls.transform.SetParent(GameFieldParent.transform);
        Props = new GameObject("Props");
        Props.transform.SetParent(GameFieldParent.transform);
        Blocks = new GameObject("Blocks");
        Blocks.transform.SetParent(GameFieldParent.transform);
        #endregion
        bool[,] map = new bool[0, 0];
        GameObject gameCoords = null;
        if(GenerateGameCoords){
            map = new bool[mapSize.xMapSize, mapSize.yMapSize];
            gameCoords = new GameObject("GameCoords");
            gameCoords.SetActive(false);
        }

        bool useRandomSeed = seed.Equals(" ") ? true : false;
        GenerateMap(useRandomSeed, seed);
		
		List<Vector2> groundMask = new List<Vector2>();
        for (int x = 0; x < mapSize.xMapSize; x++)
        {
            for (int y = 0; y  <mapSize.yMapSize; y++)
            {
                if(mapMask[x, y] == 0)
                {
                    SpawnGround(x, y);
					groundMask.Add(new Vector2(x, y));

                    if(GenerateGameCoords)
                        map[x, y] = GenerateAdditionalCoords(x, y, gameCoords.transform);
                }
            }
        }
        GenerateWalls();
        SpawnBlocks();
		
		Debug.Log("groundMask.Count" + groundMask.Count);
		int random = Random.Range(0, groundMask.Count);
		Debug.Log("random" + random);
		Vector2 playerSpawn = groundMask[random];
		GameSession.instance.SpawnPlayer(new Vector3(playerSpawn.x, playerSpawn.y, 0f));


        //grid.CreateGrid(map, MapSize);

        return this.seed;
    }

    void SpawnProps(){
		
    }

    public void SpawnExit(Vector3 position){

        PropHolder pHolder = Instantiate(propHolder, position, Quaternion.identity, GameFieldParent.transform).GetComponent<PropHolder>();
        pHolder.SetMyProp(tunnelProp);
		Debug.Log("EXIT");
    }

    [SerializeField] float maxPercentageRoomBlocks = 0.6f;
    [SerializeField] float minPercentageRoomBlocks = 0.1f;
    void SpawnBlocks(){
        List<Coord> region = GetRegions(0)[0];
        float percentage = Random.Range(minPercentageRoomBlocks, maxPercentageRoomBlocks);
        int blockCount = Mathf.RoundToInt(region.Count * percentage);
        Utils.Shuffle(region);

        int exitIndex = Random.Range(0, blockCount);

        //Get random tile and procces it
        for(int i = 0; i < blockCount; i++)
        {
            Coord tile = region[i];
            bool canSpawn = Random.Range(0, 2) == 1? true: false;
            if(canSpawn){
                GameObject block = Instantiate(myMapStyle.blockHolder, new Vector3(tile.tileX, tile.tileY, 0f), Quaternion.identity, Blocks.transform);
                Block _block = block.GetComponent<Block>();
                if(i == exitIndex){
                    _block.spawnExit = true;
                    Debug.Log($"Block coord:{tile.tileX} | {tile.tileY} has exit");
                }
                int index = Random.Range(0, myMapStyle.blocks.Length);
                _block.SetBlock(myMapStyle.blocks[index]);
            }
        }
        
        
    }

    bool GenerateAdditionalCoords(int x, int y, Transform gameCoords){
        Vector2 pos = new Vector2(x, y);
        Transform coordinate = Instantiate(coord).transform;
        coordinate.name = x + "/" + y;
        coordinate.position = new Vector3(x, y, -0.1f);
        coordinate.GetComponent<TextMeshPro>().text = x + "/" + y;
        coordinate.SetParent(gameCoords);
        return CellisAvialble(pos);
    }

    void SpawnGround(int x, int y){
        Vector2 position = new Vector2(x, y);
        GameObject instance = Instantiate(myMapStyle.groundPrefab, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        SpriteRenderer ground = instance.GetComponent<SpriteRenderer>();
		ground.sprite = myMapStyle.GetGroundSprite(0);
		GameMap.GM.gameField.Add(position, new Cell(instance, position));
        instance.transform.SetParent(Floor.transform);
    }

    public void SpawnCheckPointProp(){
        PropHolder _pHolder = Instantiate(propHolder, Vector3.one, Quaternion.identity, GameFieldParent.transform).GetComponent<PropHolder>();
        _pHolder.SetMyProp(Instantiate(checkpointProp));
    }
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
                        Transform _wall = Instantiate(myMapStyle.wallPrefab).transform;
						SpriteRenderer wall = _wall.GetComponent<SpriteRenderer>();
						wall.sprite = myMapStyle.GetWallSprite(0);
                        _wall.position = new Vector3(wpos.x, wpos.y, 0f);
                        _wall.SetParent(Walls.transform);
                        walls.Add(wpos);
                    }
                }
            }
        }
    }

    #region MapGenerator
public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	public int[,] mapMask;


	void GenerateMap(bool randomSeed, string _seed = "") {
        useRandomSeed = randomSeed;
        if(!useRandomSeed) seed = _seed;
		mapMask = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < 5; i ++) {
			SmoothMap();
		}

		ProcessMap ();

		int borderSize = 1;
		int[,] borderedMap = new int[width + borderSize * 2,height + borderSize * 2];

		for (int x = 0; x < borderedMap.GetLength(0); x ++) {
			for (int y = 0; y < borderedMap.GetLength(1); y ++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
					borderedMap[x,y] = mapMask[x-borderSize,y-borderSize];
				}
				else {
					borderedMap[x,y] =1;
				}
			}
		}

	}

	void ProcessMap() {
		List<List<Coord>> wallRegions = GetRegions (1);
		int wallThresholdSize = 50;

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					mapMask[tile.tileX,tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);
		int roomThresholdSize = 50;
		List<Room> survivingRooms = new List<Room> ();
		
		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					mapMask[tile.tileX,tile.tileY] = 1;
				}
			}
			else {
				survivingRooms.Add(new Room(roomRegion, mapMask));
			}
		}
		survivingRooms.Sort ();
		survivingRooms [0].isMainRoom = true;
		survivingRooms [0].isAccessibleFromMainRoom = true;

		ConnectClosestRooms (survivingRooms);
	}

	void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false) {

		List<Room> roomListA = new List<Room> ();
		List<Room> roomListB = new List<Room> ();

		if (forceAccessibilityFromMainRoom) {
			foreach (Room room in allRooms) {
				if (room.isAccessibleFromMainRoom) {
					roomListB.Add (room);
				} else {
					roomListA.Add (room);
				}
			}
		} else {
			roomListA = allRooms;
			roomListB = allRooms;
		}

		int bestDistance = 0;
		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		Room bestRoomA = new Room ();
		Room bestRoomB = new Room ();
		bool possibleConnectionFound = false;

		foreach (Room roomA in roomListA) {
			if (!forceAccessibilityFromMainRoom) {
				possibleConnectionFound = false;
				if (roomA.connectedRooms.Count > 0) {
					continue;
				}
			}

			foreach (Room roomB in roomListB) {
				if (roomA == roomB || roomA.IsConnected(roomB)) {
					continue;
				}
			
				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA ++) {
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB ++) {
						Coord tileA = roomA.edgeTiles[tileIndexA];
						Coord tileB = roomB.edgeTiles[tileIndexB];
						int distanceBetweenRooms = (int)(Mathf.Pow (tileA.tileX-tileB.tileX,2) + Mathf.Pow (tileA.tileY-tileB.tileY,2));

						if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
							bestDistance = distanceBetweenRooms;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}
			if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}

		if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
			CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms(allRooms, true);
		}

		if (!forceAccessibilityFromMainRoom) {
			ConnectClosestRooms(allRooms, true);
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
		Room.ConnectRooms (roomA, roomB);
		Debug.DrawLine (CoordToWorldPoint (tileA), CoordToWorldPoint (tileB), Color.green, 100);

		List<Coord> line = GetLine (tileA, tileB);
		foreach (Coord c in line) {
			DrawCircle(c,5);
		}
	}

	void DrawCircle(Coord c, int r) {
		for (int x = -r; x <= r; x++) {
			for (int y = -r; y <= r; y++) {
				if (x*x + y*y <= r*r) {
					int drawX = c.tileX + x;
					int drawY = c.tileY + y;
					if (IsInMapRange(drawX, drawY)) {
						mapMask[drawX,drawY] = 0;
					}
				}
			}
		}
	}

	List<Coord> GetLine(Coord from, Coord to) {
		List<Coord> line = new List<Coord> ();

		int x = from.tileX;
		int y = from.tileY;

		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		bool inverted = false;
		int step = Math.Sign (dx);
		int gradientStep = Math.Sign (dy);

		int longest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs(dy);
			shortest = Mathf.Abs(dx);

			step = Math.Sign (dy);
			gradientStep = Math.Sign (dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i =0; i < longest; i ++) {
			line.Add(new Coord(x,y));

			if (inverted) {
				y += step;
			}
			else {
				x += step;
			}

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest) {
				if (inverted) {
					x += gradientStep;
				}
				else {
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}

		return line;
	}

	Vector3 CoordToWorldPoint(Coord tile) {
		return new Vector3 (-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
	}

	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (mapFlags[x,y] == 0 && mapMask[x,y] == tileType) {
					List<Coord> newRegion = GetRegionTiles(x,y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}

		return regions;
	}

	List<Coord> GetRegionTiles(int startX, int startY) {
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width,height];
		int tileType = mapMask [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if (IsInMapRange(x,y) && (y == tile.tileY || x == tile.tileX)) {
						if (mapFlags[x,y] == 0 && mapMask[x,y] == tileType) {
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}

		return tiles;
	}

	bool IsInMapRange(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}


	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					mapMask[x,y] = 1;
				}
				else {
					mapMask[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0;
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					mapMask[x,y] = 1;
				else if (neighbourWallTiles < 4)
					mapMask[x,y] = 0;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (IsInMapRange(neighbourX,neighbourY)) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += mapMask[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}

	struct Coord {
		public int tileX;
		public int tileY;

		public Coord(int x, int y) {
			tileX = x;
			tileY = y;
		}
	}


	class Room : IComparable<Room> {
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;
		public bool isAccessibleFromMainRoom;
		public bool isMainRoom;

		public Room() {
		}

		public Room(List<Coord> roomTiles, int[,] map) {
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();
			foreach (Coord tile in tiles) {
				for (int x = tile.tileX-1; x <= tile.tileX+1; x++) {
					for (int y = tile.tileY-1; y <= tile.tileY+1; y++) {
						if (x == tile.tileX || y == tile.tileY) {
							if (map[x,y] == 1) {
								edgeTiles.Add(tile);
							}
						}
					}
				}
			}
		}

		public void SetAccessibleFromMainRoom() {
			if (!isAccessibleFromMainRoom) {
				isAccessibleFromMainRoom = true;
				foreach (Room connectedRoom in connectedRooms) {
					connectedRoom.SetAccessibleFromMainRoom();
				}
			}
		}

		public static void ConnectRooms(Room roomA, Room roomB) {
			if (roomA.isAccessibleFromMainRoom) {
				roomB.SetAccessibleFromMainRoom ();
			} else if (roomB.isAccessibleFromMainRoom) {
				roomA.SetAccessibleFromMainRoom();
			}
			roomA.connectedRooms.Add (roomB);
			roomB.connectedRooms.Add (roomA);
		}

		public bool IsConnected(Room otherRoom) {
			return connectedRooms.Contains(otherRoom);
		}

		public int CompareTo(Room otherRoom) {
			return otherRoom.roomSize.CompareTo (roomSize);
		}
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
