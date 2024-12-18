using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Room))]
public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator instance;

    public enum GeneratorType
    {
        singularPath,
        multiPath,
        multiPathBalanced
    }

    [SerializeField]
    public int numSpecialRooms = 0;
    public bool hasBossRoom = false;
    public bool hasLootRoom = false;

    [SerializeField]
    public PlayerInfo playerInfo;

    [SerializeField]
    public int floorNum;

    public static bool useSeed = false;
    public static readonly int seed = 0;

    [SerializeField]
    int amountToGenerate = 32;
    public string SceneToLoad;

    [Range(0,2)]
    public int density;
    private GeneratorType type;

    public Room roomPrefab1x1;
    public Room hexRoomPrefab1x1;
    Room selected1x1Prefab;

    public Room roomPrefab2x2;

    [Tooltip("Add 2x2 room to the dungeon - isn't generated on start room.\nReduces total amount of rooms by 3!")]
    public bool add2x2;
    public bool hex;

    public bool ultraSpeed;
    public bool pathfindingExample = true;

    [SerializeField]
    public static float prefabsDistance = 1;

    public Vector2[] offsets = new Vector2[]
    {
        Vector2.right * prefabsDistance,
        Vector2.left * prefabsDistance,
        Vector2.up * prefabsDistance,
        Vector2.down * prefabsDistance,
        //hex_right_up,
        new Vector2(0.653f,0.377f).normalized * prefabsDistance,
        //hex_right_down,
        new Vector2(0.653f,-0.377f).normalized * prefabsDistance,
        //hex_left_down,
        new Vector2(-0.653f,-0.377f).normalized * prefabsDistance,
        //hex_left_up
        new Vector2(-0.653f,0.377f).normalized * prefabsDistance
    };

    [SerializeField]
    public List<Room> RoomVal1 = new List<Room>();
    public List<Room> RoomVal2 = new List<Room>();
    public List<Room> RoomVal3 = new List<Room>();
    public List<Room> RoomVal4 = new List<Room>();
    public List<Room> RoomVal5 = new List<Room>();
    public List<Room> RoomVal6 = new List<Room>();
    public List<Room> RoomVal7 = new List<Room>();
    public List<Room> RoomVal8 = new List<Room>();
    public List<Room> RoomVal9 = new List<Room>();
    public List<Room> RoomVal10 = new List<Room>();
    public List<Room> RoomVal11 = new List<Room>();
    public List<Room> RoomVal12 = new List<Room>();
    public List<Room> RoomVal13 = new List<Room>();
    public List<Room> RoomVal14 = new List<Room>();
    public List<Room> RoomVal15 = new List<Room>();
    public List<Room> RoomValLoot = new List<Room>();
    public List<Room> RoomValBoss = new List<Room>();


    public List<Room> rooms;
    public List<Room> generatedRooms;
    private List<List<int>> roomChunks = new List<List<int>>();
    private int chunkWidth = 8;

    private Transform roomsContainer;

    private bool creatingPool = true;
    [HideInInspector]
    public bool generatingRooms;
    [HideInInspector]
    public bool generatingStructure = true;

    public Room generatorRoom;
    private Vector2 generatorPosition;

    public Room playerRoom;
    public Room playerRoomChange;

    private void Awake()
    {
        instance = this;
        if (useSeed)
            Random.InitState(seed);

        type = (GeneratorType)density;

        rooms = new List<Room>();

        generatorRoom = GetComponent<Room>();
        generatorRoom.jumpsFromStart = 0;
        generatorPosition = transform.position;
        generatingRooms = true;

        roomsContainer = new GameObject("Rooms").transform;
        selected1x1Prefab = hex ? hexRoomPrefab1x1 : roomPrefab1x1;

        floorNum = playerInfo.floorNum;
        amountToGenerate = (int)(Mathf.Ceil(3 * (floorNum + 1) + (int)Random.Range(5, 6)));
    }

    IEnumerator Start()
    {
        //pooling
        Debug.Log("Creating a pool...");
            StartCoroutine(CreatePool(selected1x1Prefab));
            while (creatingPool)
                yield return new WaitForSeconds(0.05f);
            Debug.Log("Pool created");

            //placing
            if (type == GeneratorType.singularPath)
                StartCoroutine(PlaceRooms());
            else
                StartCoroutine(PlaceRoomsMultiPath());
            while (generatingRooms)
                yield return new WaitForSeconds(0.05f);
            Debug.Log("Rooms placed");

            //features
            GenerateDoors();

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i] != null)
                {
                    rooms[i].AssignRoomValue();
                    rooms[i].AssignSpecial();
                    rooms[i].RepopEnemiesLeft();

                    if (rooms[i].specialRoom == true)
                    {
                        numSpecialRooms++;
                        if (hasBossRoom == false)
                        {
                            rooms[i].setBossRoom();
                            hasBossRoom = true;
                        }
                        else if (hasLootRoom == false)
                        {
                            rooms[i].setLootRoom();
                            hasLootRoom = true;
                        }
                    }
                }

            }
            if (add2x2 && !hex)
            {
                Add2x2Room();
                amountToGenerate -= 3;
            }

            if (pathfindingExample)
            {
                List<Room> roomsSortedByDist = PathManager.SortForPathFinding(generatorRoom, amountToGenerate + 1);
                PathManager.AssignJumps(roomsSortedByDist);
                FurthestRoomActions();
            }

            generatingStructure = false;
            roomChunks.Clear();

        // Will only continue if the dungeon generated has 2 or more special room types
        if (numSpecialRooms < 2)
        {
            SceneManager.LoadScene(SceneToLoad);
        }

        replaceRooms();
        GenerateDoors();
        GenerateShards();
    }


    void Update()
    {
        if (playerRoom.enemiesLeft.Count > 0 && playerRoom.enemiesLeft[0].enemyRef == null)
        {
            playerRoom.enemiesLeft.Remove(playerRoom.enemiesLeft[0]);
        }
        if (playerRoom.enemiesLeft.Count == 0)
        {
            playerRoom.UnlockRoom();
            playerRoom.RoomBeaten();
        }
        else
        {
            playerRoom.unlocked = false;
            playerRoom.beaten = false;
        }
        /*
        if (Input.GetKeyDown("u"))
        {
            playerRoom.UnlockRoom();
        }
        if (Input.GetKeyDown("l"))
        {
            print("Number of shards left: " + ShardsLeft());
        }
        */
        if (playerRoomChange != playerRoom)
        {
            print("Changing room player is in");
            //print("beaten" + playerRoomChange.beaten);
            //print("unlocked" + playerRoomChange.unlocked);
            playerRoom.DespawnEnemies();
            if (playerRoomChange.beaten == false)
            {
                playerRoomChange.RepopEnemiesLeft();
                playerRoomChange.RespawnEnemies();
            }
            playerRoom = playerRoomChange;
        }
    }

    public IEnumerator CreatePool(Room prefab)
    {
        Vector3 position = transform.position;
        for (int i = 0; i < amountToGenerate; i++)
        {
            if (i % 500 == 0) yield return null; //reduce lag

            Room newRoom = Instantiate(prefab, position, Quaternion.identity, roomsContainer);
            newRoom.gameObject.name = "Room " + i;
            newRoom.SetRandomBodyColor();
            rooms.Add(newRoom);
        }
        yield return null;
        creatingPool = false;
    }

    //singular path, multi direction sitting
    private IEnumerator PlaceRooms()
    {
        generatingRooms = true;
        Room.Directions dir;
        Vector2 offset;
        Vector2 last = transform.position;
        int roomsCorrectlySet = 0;

        for (int i = 0; i < amountToGenerate; i++)
        {
            if (ultraSpeed && i % 25 == 0)
                yield return null;

            dir = RandomDirection();

            offset = offsets[(int)dir];
            Vector2 newRoomPos = last + offset;
            List<int> selectedChunk;
            Room newRoom;
            PlaceRoom(roomsCorrectlySet, newRoomPos, out selectedChunk, out newRoom);

            bool collision;
            //yield return null;
            if (ultraSpeed)
                collision = newRoom.IsCollidingForPooled(selectedChunk, rooms, generatorPosition);
            else
            {
                yield return new WaitForFixedUpdate();      //best performance
                //yield return new WaitForSeconds(0.1f);    //animated look
                collision = newRoom.collision;
                newRoom.collision = false;
            }

            last = newRoomPos;
            if (collision)
            {
                i--;
                continue;
            }
            selectedChunk.Add(roomsCorrectlySet);
            roomsCorrectlySet++;
        }
        yield return null;
        generatingRooms = false;
    }

    //multi path, multi direction sitting
    private IEnumerator PlaceRoomsMultiPath()
    {
        Vector2[] lastPosition = new Vector2[PathManager.GENERATOR_PATHS_AMOUNT];
        int roomsCorrectlySet = 0;
        List<int> selectedChunk;

        Room SelectRoom(int pathIndex)
        {
            Room.Directions dir = RandomDirection();

            Vector2 offset = offsets[(int)dir];
            Vector2 newRoomPos = lastPosition[pathIndex] + offset;
            lastPosition[pathIndex] = newRoomPos;

            Room newRoom;
            PlaceRoom(roomsCorrectlySet,  newRoomPos, out selectedChunk, out newRoom);

            return newRoom;
        }

        generatingRooms = true;
        if (!roomsContainer)
            roomsContainer = new GameObject("Rooms").transform;

        //set random path sizes
        int[] pathRooms = PathManager.GeneratorPathRoomsAmount(amountToGenerate, type == GeneratorType.multiPathBalanced);
        int pathCalls = Mathf.Max(pathRooms);

        //generate paths
        for (int i = 1; i <= pathCalls; i++)
        {
            //partition ultra speed generation
            if (ultraSpeed && i % 25 == 0)
                yield return null;

            //generate in cycles: one room per path
            //1st iteration creates 1st room for each path (generator's neighbours)
            for (int pathIndex = 0; pathIndex < PathManager.GENERATOR_PATHS_AMOUNT; pathIndex++)
            {
                //skip if no rooms for this path
                if (i > pathRooms[pathIndex])
                    continue;

                Room newRoom = SelectRoom(pathIndex);
                bool collision;
                //yield return null;
                if (ultraSpeed)
                    collision = newRoom.IsCollidingForPooled(selectedChunk, rooms, generatorPosition);
                else
                {
                    yield return new WaitForFixedUpdate();      //best performance
                    //yield return new WaitForSeconds(0.1f);    //animated look
                    collision = newRoom.collision;
                    newRoom.collision = false;
                }

                if (collision)
                {
                    pathIndex--;
                    continue;
                }
                selectedChunk.Add(roomsCorrectlySet);
                roomsCorrectlySet++;
            }
        }
        yield return null;
        generatingRooms = false;
    }

    private void PlaceRoom(int roomsCorrectlySet, Vector2 newRoomPos, out List<int> selectedChunk, out Room newRoom)
    {
        selectedChunk = SelectChunk(newRoomPos);

        newRoom = rooms[roomsCorrectlySet];
        newRoom.transform.position = newRoomPos;
        newRoom.gameObject.SetActive(true);
    }

    private void GenerateDoors()
    {
        foreach(Room r in rooms)
        {
            r.resetDoors();
        }
        generatorRoom.AssignAllNeighbours(offsets);

        for (int i = 0; i < amountToGenerate; i++)
        {
            rooms[i].AssignAllNeighbours(offsets);
        }

    }

    private void GenerateShards()
    {
        int hasShard = 0;
        foreach(Room r in rooms)
        {
            if (r.bossRoom || r.lootRoom) {continue;};
            hasShard = Random.Range(0, 2);
            if (hasShard == 0)
            {
                r.setShard();
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().totalShards += 1;
            }
        }

        PlayerController playerC = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        GameObject.Find("ShardCountText").GetComponent<UnityEngine.UI.Text>().text = playerC.numShards + " / " + playerC.totalShards;
    }

    public int ShardsLeft()
    {
        int numShards = 0;
        foreach (Room r in rooms)
        {
            if (r.hasShard == true)
            {
                numShards++;
            }
        }
        return numShards;
    }

    private void Add2x2Room()
    {
        Room[] toRemove = PathManager.FindPlaceFor2x2(rooms, generatorRoom);
        Room start = toRemove[0];

        if (start != null)
        {
            Room newRoom = Instantiate(roomPrefab2x2, start.transform.position, Quaternion.identity, roomsContainer)
                .GetComponent<Room>();

            //make space for a new room
            foreach (Room r in toRemove)
            {
                if (r != null)
                {
                    rooms.Remove(r);
                    r.gameObject.SetActive(false);
                    Destroy(r.gameObject);
                }
            }

            newRoom.SetRandomBodyColor();
            newRoom.gameObject.SetActive(true);
            rooms.Add(newRoom);
            newRoom.AssignAllNeighbours(offsets);

            //fix doors at neighbours
            foreach (Room.Doors d in newRoom.roomDoors)
                if (d.leadsTo != null)
                    d.leadsTo.AssignAllNeighbours(offsets);
        }
    }

    private void FurthestRoomActions()
    {
        Room furthest = PathManager.FindFurthestRoom(rooms);
        if (furthest != null)
        {
            furthest.MarkAsBossRoom();
            PathManager.SetPathToRoom(furthest,generatorRoom);
        }
        else
            Debug.LogError("FindFurthestRoom() returned null - cannot set path.");
    }

    private Room.Directions RandomDirection()
    {
        if (hex)
            return (Room.Directions)Random.Range(2, 8);
        else
            return (Room.Directions)Random.Range(0, 4);
    }

    private List<int> SelectChunk(Vector2 newRoomPos)
    {
        List<int> selectedChunk;
        int xDist = (int)Mathf.Abs(newRoomPos.x - generatorPosition.x);
        int chunkIndex = xDist / chunkWidth;
        while (chunkIndex >= roomChunks.Count)
            roomChunks.Add(new List<int>());
        selectedChunk = roomChunks[chunkIndex];
        return selectedChunk;
    }

    private void replaceRooms()
    {
        int roomVal;
        int numRooms = rooms.Count;
        Room[] toRemove = new Room[numRooms];
        Room start = rooms[0];
        Room newRoom = null;
        Room currRoom = start;
        //generatedRooms = new List<Room>();
        for(int i = 0; i < numRooms; i++)
        {
            toRemove[i] = rooms[i];
        }

        if (start != null)
        {
            for (int i = 0; i < numRooms; i++)
            {
                currRoom = rooms[i];
                roomVal = currRoom.roomValue;
                switch (roomVal)
                {
                    // Up
                    case 1:
                        if (currRoom.bossRoom == true)
                        {
                            newRoom = Instantiate(RoomValBoss[Random.Range(0, RoomValBoss.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else if (currRoom.lootRoom == true)
                        {
                            newRoom = Instantiate(RoomValLoot[Random.Range(0, RoomValLoot.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else
                        {
                            newRoom = Instantiate(RoomVal1[Random.Range(0, RoomVal1.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        break;

                    // Down
                    case 2:
                        if (currRoom.bossRoom == true)
                        {
                            newRoom = Instantiate(RoomValBoss[Random.Range(0, RoomValBoss.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else if (currRoom.lootRoom == true)
                        {
                            newRoom = Instantiate(RoomValLoot[Random.Range(0, RoomValLoot.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else
                        {
                            newRoom = Instantiate(RoomVal2[Random.Range(0, RoomVal2.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        break;

                    // Up, Down
                    case 3:
                        newRoom = Instantiate(RoomVal3[Random.Range(0, RoomVal3.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Left
                    case 4:
                        if (currRoom.bossRoom == true)
                        {
                            newRoom = Instantiate(RoomValBoss[Random.Range(0, RoomValBoss.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else if (currRoom.lootRoom == true)
                        {
                            newRoom = Instantiate(RoomValLoot[Random.Range(0, RoomValLoot.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else
                        {
                            newRoom = Instantiate(RoomVal4[Random.Range(0, RoomVal4.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        break;

                    // Left, Up
                    case 5:
                        newRoom = Instantiate(RoomVal5[Random.Range(0, RoomVal5.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Left, Down
                    case 6:
                        newRoom = Instantiate(RoomVal6[Random.Range(0, RoomVal6.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Left, Up, Down
                    case 7:
                        newRoom = Instantiate(RoomVal7[Random.Range(0, RoomVal7.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Right
                    case 8:
                        if (currRoom.bossRoom == true)
                        {
                            newRoom = Instantiate(RoomValBoss[Random.Range(0, RoomValBoss.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else if (currRoom.lootRoom == true)
                        {
                            newRoom = Instantiate(RoomValLoot[Random.Range(0, RoomValLoot.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        else
                        {
                            newRoom = Instantiate(RoomVal8[Random.Range(0, RoomVal8.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        }
                        break;

                    // Right, Up
                    case 9:
                        newRoom = Instantiate(RoomVal9[Random.Range(0, RoomVal9.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Right, Down
                    case 10:
                        newRoom = Instantiate(RoomVal10[Random.Range(0, RoomVal10.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Right, Up, Down
                    case 11:
                        newRoom = Instantiate(RoomVal11[Random.Range(0, RoomVal11.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Right, Left
                    case 12:
                        newRoom = Instantiate(RoomVal12[Random.Range(0, RoomVal12.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Left, Right, Up
                    case 13:
                        newRoom = Instantiate(RoomVal13[Random.Range(0, RoomVal13.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Left, Down, Right
                    case 14:
                        newRoom = Instantiate(RoomVal14[Random.Range(0, RoomVal14.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Up, Down, Left, Right
                    case 15:
                        newRoom = Instantiate(RoomVal15[Random.Range(0, RoomVal15.Count)], currRoom.transform.position, Quaternion.identity, roomsContainer).GetComponent<Room>();
                        break;

                    // Error
                    default:

                        break;
                }
                newRoom.SetRandomBodyColor();
                newRoom.gameObject.SetActive(true);
                rooms.Add(newRoom);
                //newRoom.AssignAllNeighbours(offsets);


            }
            for(int i = numRooms - 1; i >= 0; i--)
            {
                if (rooms[i] != null)
                {
                    rooms[i].gameObject.SetActive(false);
                    Destroy(rooms[i].gameObject);
                    rooms.Remove(rooms[i]);
                }
            }
            //fix doors at neighbours
            /*
            for (int i = 0; i < numRooms; i++)
            {
                  foreach (Room.Doors d in newRoom.roomDoors)
                         if (d.leadsTo != null)
                                 d.leadsTo.AssignAllNeighbours(offsets);
            }
            */
        }
    }



    //based on physical distance
    //private Room FindFurthestRoom()
    //{
    //    int index = -1;
    //    float biggestDist = 0;
    //    for (int i = 0; i < rooms.Count; i++)
    //    {
    //        float dist = (transform.position - rooms[i].transform.position).sqrMagnitude;
    //        if (dist > biggestDist)
    //        {
    //            index = i;
    //            biggestDist = dist;
    //        }
    //    }
    //    if (index != -1)
    //        return rooms[index];
    //    else
    //        return null;
    //}

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        playerRoomChange = room;
    }
}
