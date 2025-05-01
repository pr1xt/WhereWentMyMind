using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapControler : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject staringRoom;
    [SerializeField] private GameObject endingRoom;
    [SerializeField] private List<GameObject> hospitalRooms;
    [SerializeField] private List<GameObject> hospitalCorridors;
    [SerializeField] private GameObject normalWall;
    [SerializeField] private GameObject wallWithDoor;

    [SerializeField] private GameObject mapObject;
    [SerializeField] private GameObject floorObject;
    [SerializeField] private GameObject roomIconPrefab;
    [SerializeField] private GameObject floorIconPrefab;
    [SerializeField] private GameObject corridorIconPrefab;
    public GameObject EnemyMapElement;
    public GameObject GunPickUpMapElement;
    public GameObject CoinPickUpMapElement;
    public GameObject HeartPickUpMapElement;
    GameObject prefabToSpawn = null;
    public float MapSpawnHeight = 20f;

    private GameObject GenerateMap(Vector2Int roomPosition, HashSet<Vector2Int> rooms)
    {
        GameObject roomMap = Instantiate(roomIconPrefab, floorObject.transform);
        roomMap.transform.localPosition = new Vector3(60 * roomPosition.x, 60 * roomPosition.y, 0);
        Instantiate(floorIconPrefab, roomMap.transform);

        List<int> rotations = new() { 270, 90, 0, 180 };
        List<Vector2Int> surroundingPositions = GetSurroundingPositions(roomPosition);
        for(int i = 0; i < surroundingPositions.Count; i++)
        {
            Vector2Int surroundingPosition = surroundingPositions[i];
            if(rooms.Contains(surroundingPosition))
            {
                GameObject corridor = Instantiate(corridorIconPrefab, roomMap.transform);
                corridor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotations[i]));
            } 
        }
        if(roomPosition != Vector2.zero){
            roomMap.SetActive(false);
        }
        return roomMap;
    }

    private List<Vector2Int> GetSurroundingPositions(Vector2Int roomPosition)
    {
        List<Vector2Int> surroundingPositions = new()
        {
            new Vector2Int(roomPosition.x + 1, roomPosition.y),
            new Vector2Int(roomPosition.x - 1, roomPosition.y),
            new Vector2Int(roomPosition.x, roomPosition.y + 1),
            new Vector2Int(roomPosition.x, roomPosition.y - 1)
        };
        return surroundingPositions;
    }

    private int CountTouchingRooms(Vector2Int roomPosition, HashSet<Vector2Int> rooms)
    {
        int count = 0;
        foreach(Vector2Int surroundingPosition in GetSurroundingPositions(roomPosition))
        {
            if(rooms.Contains(surroundingPosition)) count++;
        }
        return count;
    }

    private List<GameObject> FilterRoomsPrefabs(List<GameObject> roomPrefabs, Vector2Int roomPosition, HashSet<Vector2Int> rooms)
    {
        List<GameObject> filteredPrefabs = new();
        foreach(GameObject roomPrefab in roomPrefabs)
        {
            FlorOptions florOptions = roomPrefab.GetComponent<FlorOptions>();
            if(florOptions == null) {
                Debug.LogError("FlorOptions component not found on prefab: " + roomPrefab.name);
                continue;
            }
            bool canBePlaced = true;
            if(rooms.Contains(new Vector2Int(roomPosition.x + 1, roomPosition.y) ) && !florOptions.canExitNorth) canBePlaced = false;
            if(rooms.Contains(new Vector2Int(roomPosition.x - 1, roomPosition.y) ) && !florOptions.canExitSouth) canBePlaced = false;
            if(rooms.Contains(new Vector2Int(roomPosition.x, roomPosition.y + 1) ) && !florOptions.canExitWest) canBePlaced = false;
            if(rooms.Contains(new Vector2Int(roomPosition.x, roomPosition.y - 1) ) && !florOptions.canExitEast) canBePlaced = false;
            if(canBePlaced) filteredPrefabs.Add(roomPrefab);

        }
        return filteredPrefabs;

    }

    private List<Vector2Int> GetPosibleRooms(HashSet<Vector2Int> rooms)
    {
        HashSet<Vector2Int> posibleRooms = new();

        foreach(Vector2Int room in rooms)
        {
            foreach(Vector2Int surroundingPosition in GetSurroundingPositions(room))
            {
                if(!rooms.Contains(surroundingPosition) && CountTouchingRooms(surroundingPosition, rooms) == 1) posibleRooms.Add(surroundingPosition);
            }
        }
        return posibleRooms.ToList();
    }

    private List<Vector2Int> GetPosibleEndingRooms(HashSet<Vector2Int> rooms)
    {
        List<Vector2Int> posibleRooms = GetPosibleRooms(rooms);
        List<Vector2Int> posibleEndingRooms = new();
        foreach(Vector2Int room in posibleRooms)
        {
            if(Vector2Int.Distance(room, Vector2Int.zero) > 2) posibleEndingRooms.Add(room);
        }
        return posibleEndingRooms;
    }

    private void GenerateWalls(GameObject room, Vector2Int roomPosition, HashSet<Vector2Int> rooms)
    {
        List<int> rotations = new() { 180, 0, 90, 270 };
        List<Vector2Int> surroundingPositions = GetSurroundingPositions(roomPosition);
        for(int i = 0; i < surroundingPositions.Count; i++)
        {
            Vector2Int surroundingPosition = surroundingPositions[i];
            GameObject wall;
            if(rooms.Contains(surroundingPosition))
            {
                wall = Instantiate(wallWithDoor, room.transform);
                room.GetComponent<RoomControler>().doors.Add(wall.GetComponent<DoorControler>());
            } 
            else wall = Instantiate(normalWall, room.transform);
            wall.transform.rotation = Quaternion.Euler(new Vector3(0, rotations[i], 0));
        }
    }

    private void GenerateCorridors(GameObject room, Vector2Int roomPosition, HashSet<Vector2Int> rooms){
        List<int> rotations = new() { 180, 0, 90, 270 };
        List<Vector3> positions = new()
        {
            new Vector3(15, 0, 0),
            new Vector3(-15, 0, 0),
            new Vector3(0, 0, 15),
            new Vector3(0, 0, -15)
        };
        List<Vector2Int> surroundingPositions = GetSurroundingPositions(roomPosition);
        for(int i = 0; i < surroundingPositions.Count; i++)
        {
            if(!rooms.Contains(surroundingPositions[i])) continue;
            Vector2Int surroundingPosition = surroundingPositions[i];
            GameObject corridor = Instantiate(hospitalCorridors[Random.Range(0, hospitalCorridors.Count)], room.transform);
            corridor.transform.SetPositionAndRotation(corridor.transform.position + positions[i], Quaternion.Euler(new Vector3(0, rotations[i], 0)));
        }
    }

    private void GenerateFloor(int numberOfRooms)
    {
        HashSet<Vector2Int> rooms = new()
        {
            new Vector2Int(0, 0)
        };

        for(int i = 0; i < numberOfRooms - 1; i++)
        {
            List<Vector2Int> posibleRooms = GetPosibleRooms(rooms);
            Vector2Int room = posibleRooms[Random.Range(0, posibleRooms.Count)];
            rooms.Add(room);
        }

        List<Vector2Int> posibleEndingRooms = GetPosibleEndingRooms(rooms);
        Vector2Int endingRoomPosition = posibleEndingRooms[Random.Range(0, posibleEndingRooms.Count)];
        rooms.Add(endingRoomPosition);

        foreach(Vector2Int roomPosition in rooms)
        {
            GameObject room = Instantiate(roomPrefab, transform);
            room.transform.position = new Vector3(60 * roomPosition.x, 0, 60 * roomPosition.y);

            GameObject mapObject = GenerateMap(roomPosition, rooms);
            room.GetComponent<RoomControler>().mapIconObject = mapObject;

            GenerateWalls(room, roomPosition, new HashSet<Vector2Int>(rooms) { endingRoomPosition });
            GenerateCorridors(room, roomPosition, new HashSet<Vector2Int>(rooms) { endingRoomPosition });
            if(roomPosition == new Vector2Int(0, 0))
            {
                Instantiate(staringRoom, room.transform);
                continue;
            }

            if(roomPosition == endingRoomPosition)
            {
                Instantiate(endingRoom, room.transform);
                continue;
            } 

            List<GameObject> filteredPrefabs = FilterRoomsPrefabs(hospitalRooms, roomPosition, rooms);
            Instantiate(filteredPrefabs[Random.Range(0, filteredPrefabs.Count)], room.transform);
        }
    }

    public void UpdateIconsOnMap()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] guns = GameObject.FindGameObjectsWithTag("WeaponPickUp");

        foreach (GameObject enemy in enemies)
        {   
            Vector3 worldPos = new Vector3(
                enemy.transform.position.x,
                MapSpawnHeight,
                enemy.transform.position.z
            );

            Quaternion rotation = Quaternion.identity;

            if (enemy.name.StartsWith("EnemyPref"))
            {
                rotation = Quaternion.Euler(0, 180, 0);
            }

            GameObject attached = Instantiate(EnemyMapElement, worldPos, rotation);
            attached.transform.SetParent(enemy.transform, worldPositionStays: true);
        }foreach (GameObject gun in guns)
        {   
            Vector3 worldPos = new Vector3(
                gun.transform.position.x,
                MapSpawnHeight,
                gun.transform.position.z
            );

            Quaternion rotation = Quaternion.identity;

            GameObject attached = Instantiate(GunPickUpMapElement, worldPos, rotation);
            attached.transform.SetParent(gun.transform, worldPositionStays: true);
        }foreach (GameObject coin in coins)
        {   
            Vector3 worldPos = new Vector3(
                coin.transform.position.x,
                MapSpawnHeight,
                coin.transform.position.z
            );

            Quaternion rotation = Quaternion.identity;


            // Check the name of the object
            if (coin.name.Contains("Coin"))
            {
                prefabToSpawn = CoinPickUpMapElement;
            }
            else if (coin.name.Contains("Heart"))
            {
                prefabToSpawn = HeartPickUpMapElement;
            }
            
            // If we found a matching prefab, instantiate it
            if (prefabToSpawn != null)
            {
                GameObject attached = Instantiate(prefabToSpawn, worldPos, rotation);
                attached.transform.SetParent(coin.transform, worldPositionStays: true);
            }
        }
    }

    void Start()
    {
        GenerateFloor(6);
        UpdateIconsOnMap();
    }

    void Update() {
        if(Input.GetKey(KeyCode.Tab))
        {
            mapObject.SetActive(true);
        }
        else
        {
            mapObject.SetActive(false);
        }
    }
}
