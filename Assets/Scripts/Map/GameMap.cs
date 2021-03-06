using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Rendering;
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
	
	public MapSize mapSize;
	[SerializeField] MapStyle mapStyles;
	GameMapStyle myMapStyle;
	[SerializeField] Volume postProccesing;
	[SerializeField] AstarPath astar;
	[Header("Perlin noise")]
    [SerializeField] float scale = 1f;
    [SerializeField] float persistance = 4f;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] int octaves = 3;
    void Awake()
    {
        GM = this;
		
    }
	void Start() {
		propHolder = Resources.Load<GameObject>(Path.Combine("Props/", "PropHolder"));
		itemHolder = Resources.Load<GameObject>(Path.Combine("Items/", "ItemHolder"));
		obelisk = Resources.Load<Obelisk>(Path.Combine("Props/", "Obelisk"));
		wormhole = Resources.Load<Wormhole>(Path.Combine("Props/", "Wormhole"));
		
	}
	public void SetAstarGrid(){
		astar.data.gridGraph.SetDimensions(mapSize.xMapSize, mapSize.yMapSize, 1);
		astar.data.gridGraph.center = new Vector3(mapSize.xMapSize/2 + 0.5f, mapSize.yMapSize/2 + 0.5f, 1f);
		AstarPath.active.Scan();
	}
    Prop wormhole;
    Prop obelisk;
    GameObject propHolder;
	GameObject itemHolder;
	void CreateMapHolders(){
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
	}
    //Генерация игрового поля 
	List<Vector2> groundMask;
    public string GenerateGameField(bool isCheckpointLevel, string seed = " ")
    {
		myMapStyle = Resources.Load<GameMapStyle>(Path.Combine("MapStyles", mapStyles.GetCurrentMapStyle(relocation.CurrentDungeonLevel)));
        postProccesing.profile = myMapStyle.levelPostProccess;
        CreateMapHolders();
		Vector2 playerSpawn = Vector2.zero;
        bool useRandomSeed = seed.Equals(" ") ? true : false;
		if(!isCheckpointLevel){
			GenerateMap(useRandomSeed, seed);
			float[,] noise = Noise.GenerateNoiseMap(mapSize.xMapSize, mapSize.yMapSize, 1011, scale, octaves, persistance, lacunarity, Vector2.zero);
	 		int width = noise.GetLength(0);
	 		int height = noise.GetLength(1);
			groundMask = new List<Vector2>();
			for (int x = 0; x < mapSize.xMapSize; x++)
			{
				for (int y = 0; y  <mapSize.yMapSize; y++)
				{
					if(mapMask[x, y] == 0) //if is ground
					{
						SpawnGround(x, y, noise[x,y]);
						groundMask.Add(new Vector2(x, y));
					}else{ //if walls
						bool canSpawnWall = false;
						for (int k = x-1; k <= x+1; k++)
						{
							for (int j = y-1; j <= y+1; j++)
							{
								try{
									if(mapMask[k, j] == 0){
										canSpawnWall = true;
										break;
									}
								}catch(Exception ex){
									continue;
								}
							}
							if(canSpawnWall) break;
						}
							if(canSpawnWall)
								SpawnWalls(x, y);
					}
				}
			}
			playerSpawn = GeneratePlayerPos();
			SpawnLevelMobs(playerSpawn);
			SpawnBuildings();
			SpawnProps();
			SpawnBlocks();
			SpawnPlayerDeathPoint();
		}else{
			SpawnCheckPointRoom();
			playerSpawn = Vector2.zero;
		}
		GameSession.SpawnPlayer(new Vector3(playerSpawn.x, playerSpawn.y, 0f));
        return this.seed;
    }
	Vector2 GeneratePlayerPos(){
		int randomRoom = Random.Range(0, mapRooms.Count);
		Room room = mapRooms[randomRoom];
		int randomTile = Random.Range(0, room.tiles.Count);
		Coord tile = room.tiles[randomTile];
		return new Vector2(tile.tileX, tile.tileY);
	}
	[SerializeField] int safePlayerSpawnDist = 5;
	void SpawnLevelMobs(Vector2 playerPos){
		int playerX = Mathf.RoundToInt(playerPos.x);
		int playerY = Mathf.RoundToInt(playerPos.y);
		for (int i = 0; i < myMapStyle.mobs.Length; i++)
		{
			GameObject mobPrefab = myMapStyle.mobs[i];
			int count = Random.Range(1, 10); //count of enemy
			for (int k = 0; k < count; k++)
			{
				//Get enemy spawn chance
				Enemy myEnemy = myMapStyle.mobs[i].GetComponent<Enemy>();
				float chance = Random.Range(0f, 100f) / 100f;
				if(chance > myEnemy.spawnChance)
					continue;
				//Select random room for enemy
				int randomRoom = Random.Range(0, mapRooms.Count);
				Room room = mapRooms[randomRoom];
				//Future enemy position var
				Coord enemyPos;
				//if this room contains player position
				if(room.tiles.Contains(new Coord(playerX, playerY))){
					List<Coord> tiles = new List<Coord>(room.tiles);
					for (int h = playerX-safePlayerSpawnDist; h <= playerX+safePlayerSpawnDist; h++)
					{
						for (int j = playerY-safePlayerSpawnDist; j <= playerY+safePlayerSpawnDist; j++)
						{
							Coord curTile = new Coord(h, j);
							if(tiles.Contains(curTile))
								tiles.Remove(curTile);//Remove closest to player tiles
						}
					}
					int randTile = Random.Range(0, tiles.Count);
					Coord enemyPosTile = tiles[randTile];
					enemyPos = new Coord(enemyPosTile.tileX, enemyPosTile.tileY);
				}else{
					int randTile = Random.Range(0, room.tiles.Count);
					enemyPos = room.tiles[randTile];
				}
				//Spawn enemy
				GameObject mob = Instantiate(mobPrefab);
				mob.transform.position = new Vector3(enemyPos.tileX, enemyPos.tileY, 1f);
			}
		}
	}
	void SpawnBuildings(){

	}
    void SpawnProps(){
		
    }
	void SpawnPlayerDeathPoint(){
		DeathPointData deathPoint = SaveSystem.instance.saves.playerSaves.deathPointData;
		if(!deathPoint.createDeathPoint)
			return;

		int currentLevel = relocation.CurrentDungeonLevel;
		if(currentLevel != deathPoint.levelOfDeath)
			return;

		Item playerLoot = Resources.Load<Item>(Path.Combine("Items/", "DeathPlayerLoot"));
		DeathPlayerLoot pl = (DeathPlayerLoot)playerLoot;
		foreach (var slot in deathPoint.loot)
		{
			Item slotItem = Resources.Load<Item>(Path.Combine("Items/", slot.itemName));
			if(slotItem == null)
				continue;
			pl.loot.Add(Instantiate(slotItem));
		}
		Vector3 lootPosition = new Vector3(deathPoint.position.x, deathPoint.position.y, 1f);
		GameObject _itemHolder = Instantiate(itemHolder, lootPosition, Quaternion.identity);
		ItemHolder holder = _itemHolder.GetComponent<ItemHolder>();
		holder.SetItem(playerLoot, false);
	}
    public void SpawnWormhole(int x, int y){
		Vector3 position = new Vector3(x, y, 1f);
        PropHolder pHolder = Instantiate(propHolder, position, Quaternion.identity, GameFieldParent.transform).GetComponent<PropHolder>();
        pHolder.SetMyProp(wormhole);
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
	void SpawnGround(int x, int y, float groundLevel){
		//Select ground sprite based on perlin noise
		int size = myMapStyle.GetGroundSize();
		float divisionPrice = 1f/size;
		int index = Mathf.RoundToInt(groundLevel/divisionPrice);
		index = Mathf.Clamp(index, 0, size-1);
		//Use selected index
        Vector2 position = new Vector2(x, y);
        GameObject instance = Instantiate(myMapStyle.groundPrefab, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        SpriteRenderer ground = instance.GetComponent<SpriteRenderer>();
		ground.sprite = myMapStyle.GetGroundSprite(index);
        instance.transform.SetParent(Floor.transform);
    }
	void SpawnGround(int x, int y){
		Vector2 position = new Vector2(x, y);
        GameObject instance = Instantiate(myMapStyle.groundPrefab, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        SpriteRenderer ground = instance.GetComponent<SpriteRenderer>();
		ground.sprite = myMapStyle.GetGroundSprite(0);
        instance.transform.SetParent(Floor.transform);
	}
    void SpawnWalls(int x, int y){
		Transform _wall = Instantiate(myMapStyle.wallPrefab).transform;
		SpriteRenderer wall = _wall.GetComponent<SpriteRenderer>();
		wall.sprite = myMapStyle.GetWallSprite(0);
		_wall.position = new Vector3(x, y, 0f);
		_wall.SetParent(Walls.transform);
    }
	#region CheckPointSpawning
	[SerializeField] int radius = 1;
	void SpawnCheckPointRoom(){
		int rSquared = radius * radius;
		int x = Vector2Int.zero.x;
		int y = Vector2Int.zero.y;

		List<Vector2Int> ground = new List<Vector2Int>();
		for (int u = x - radius; u < x + radius + 1; u++)
			for (int v = y - radius; v < y + radius + 1; v++)
				if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
				{
					ground.Add(new Vector2Int(u, v));
					SpawnGround(u, v);
					if(u == x && v == y+1)
						SpawnObelisk(u, v);
					if(u == x-radius+1 && v == y)
						SpawnWormhole(u, v);
					if(u == x && v == y+radius-3)
						SpawnMerchantShop(u, v);
				}

		radius += 1;
		rSquared = radius * radius;
		for (int u = x - radius; u < x + radius + 1; u++)
			for (int v = y - radius; v < y + radius + 1; v++){
				if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared){
					Vector2Int pos = new Vector2Int(u,v);
					if (!ground.Contains(pos))
					{
						Transform _wall = Instantiate(myMapStyle.wallPrefab).transform;
						SpriteRenderer wall = _wall.GetComponent<SpriteRenderer>();
						wall.sprite = myMapStyle.GetWallSprite(0);
						_wall.position = new Vector3(pos.x, pos.y, 0f);
						_wall.SetParent(Walls.transform);
					}
				}
			}
	}
	void SpawnObelisk(int x, int y){
		Vector3 pos = new Vector3(x, y, 1f);
		GameObject prefab = Instantiate(propHolder, pos, Quaternion.identity);
		prefab.transform.position = pos;
		PropHolder holder = prefab.GetComponent<PropHolder>();
		holder.SetMyProp(obelisk);
	}
	void SpawnMerchantShop(int x, int y){
		Vector3 position = new Vector3(x, y, 1f);
		GameObject prefab = Resources.Load<GameObject>(Path.Combine("Props/", "merchant's shop"));
        GameObject shop = Instantiate(prefab, position, Quaternion.identity, GameFieldParent.transform);
	}
	#endregion
    
	#region MapGenerator
	int width;
	int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	public int[,] mapMask;
	List<Room> mapRooms;

	void GenerateMap(bool randomSeed, string _seed = "") {
        width = mapSize.xMapSize;
		height = mapSize.yMapSize;
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
		mapRooms = new List<Room>(survivingRooms);
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
	//Connections between rooms
	List<Coord> mapCorridorCoords = new List<Coord>();
	void DrawCircle(Coord c, int r) {
		for (int x = -r; x <= r; x++) {
			for (int y = -r; y <= r; y++) {
				if (x*x + y*y <= r*r) {
					int drawX = c.tileX + x;
					int drawY = c.tileY + y;
					if (IsInMapRange(drawX, drawY)) {
						mapMask[drawX,drawY] = 0;
						mapCorridorCoords.Add(new Coord(drawX, drawY));
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
        Destroy(GameFieldParent);
    }
}

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
