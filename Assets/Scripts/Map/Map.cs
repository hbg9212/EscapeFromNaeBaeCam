using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{

    public GameObject Player { get => _player; }
    public GameObject BossPrefab { get => _boss_prefab; }

    private void Start()
    {

        MapGenerator generator = new MapGenerator();
        generator._property = _map_generation_property;
        generator.GenerateMap();

        for(int y = 0; y < generator._property._cell_number_y; ++y)
        {
            for(int x = 0; x < generator._property._cell_number_x; ++x)
            {
                _tile_map.SetTile(new Vector3Int(x, y, 0), _background_tile);
            }
        }

        int siblingNumber   = (int)Mathf.Pow(2, generator._property._map_generation_patition_number);
        int begin           = siblingNumber - 1;
        CreateRoomClass(siblingNumber);
        for (int i = 0; i < siblingNumber; ++i)
        {
            GenerateRoom(generator._node_array[begin + i], i);
        }

        int parentNumber = (int)Mathf.Pow(2, generator._property._map_generation_patition_number) - 1;
        for (int i = 0; i < parentNumber; ++i)
        {
            GenerateCorridor(generator._node_array[i]);
        }

        GenerateWall();

        foreach (var room in _roomList)
        {
            room.Start();
        }

    }

    private void CreateRoomClass(int roomNumber)
    {

        _roomList.Add(new PlayerStartRoom());
        _roomList.Add(new BossRoom());
        for (int i = 2; i < roomNumber; ++i)
        {
            _roomList.Add(new EmptyRoom());
        }
        _roomList = _roomList.OrderBy(x => Random.Range(0, 100)).ToList();

    }

    private void GenerateRoom(MapGenerator.Node node, int index)
    {

        Debug.Assert(node.IsLeaf);

        for(int y = 0; y < node.RoomRect.height; ++y)
        {
            for (int x = 0; x < node.RoomRect.width; ++x)
            {

                _tile_map.SetTile(new Vector3Int(node.RoomRect.x + x, node.RoomRect.y + y, 0), _tile);

            }
        }

        _roomList[index].map    = this;
        _roomList[index].index  = index;
        _roomList[index].rect   = node.RoomRect;

    }

    private void GenerateCorridor(MapGenerator.Node node)
    {

        Debug.Assert(!node.IsLeaf);

        for (int y = 0; y < node.CorridorRect.height; ++y)
        {
            for (int x = 0; x < node.CorridorRect.width; ++x)
            {

                var tileIndex   = new Vector3Int(node.CorridorRect.x + x, node.CorridorRect.y + y, 0);
                var tile        = _tile_map.GetTile(tileIndex);

                _tile_map.SetTile(tileIndex, _tile);

            }
        }

    }

    private void GenerateWall()
    {

        //벽을 생성하는 타일을 구분하는 맵을 만듭니다.
        bool[] isFloorMap = new bool[_map_generation_property._cell_number_x * _map_generation_property._cell_number_y];
        for (int y = 0; y < _map_generation_property._cell_number_y; ++y)
        {
            for (int x = 0; x < _map_generation_property._cell_number_x; ++x)
            {
                isFloorMap[y * _map_generation_property._cell_number_x + x] = _tile_map.GetTile(new Vector3Int(x, y, 0)) != _background_tile;
            }
        }


        //밑에 if문을 간단히 하기 위해 for문을[0, w], [0, h]형태로 구성해 마지막 인덱스를 구할 때 -1을 할 필요없게 합니다.
        var w = _map_generation_property._cell_number_x - 1;
        var h = _map_generation_property._cell_number_y - 1;
        for (int y = 0; y <= h; ++y)
        {
            for (int x = 0; x <= w; ++x)
            {

                //주변에 벽을 생성하는 타일이 있다면 벽을 만듭니다.
                //맵 밖을 넘어가는 인덱스일 경우 맵 안으로 값을 자릅니다.
                //OR연산자로 구성되기에 같은 타일을 두번 계산해도 문제는 발생하지 않습니다.
                if(!isFloorMap[y * _map_generation_property._cell_number_x + x])
                {

                    if (isFloorMap[System.Math.Max(y - 1, 0) * _map_generation_property._cell_number_x + System.Math.Max(x - 1, 0)]   ||
                        isFloorMap[System.Math.Max(y - 1, 0) * _map_generation_property._cell_number_x + (x + 0)]              ||
                        isFloorMap[System.Math.Max(y - 1, 0) * _map_generation_property._cell_number_x + System.Math.Min(x + 1, w)]   ||
                        isFloorMap[y + 0 * _map_generation_property._cell_number_x + System.Math.Max(x - 1, 0)]                ||
                        isFloorMap[y + 0 * _map_generation_property._cell_number_x + System.Math.Min(x + 1, w)]                ||
                        isFloorMap[System.Math.Min(y + 1, h) * _map_generation_property._cell_number_x + System.Math.Max(x - 1, 0)]   ||
                        isFloorMap[System.Math.Min(y + 1, h) * _map_generation_property._cell_number_x + (x + 0)]              ||
                        isFloorMap[System.Math.Min(y + 1, h) * _map_generation_property._cell_number_x + System.Math.Min(x + 1, w)])
                    {
                        _wall_tile_map.SetTile(new Vector3Int(x, y, 0), _wall_tile);
                    }
                }

            }
        }

    }

    [SerializeField] private TileBase                               _tile;
    [SerializeField] private TileBase                               _background_tile;
    [SerializeField] private TileBase                               _wall_tile;
    [SerializeField] private Tilemap                                _tile_map;
    [SerializeField] private Tilemap                                _wall_tile_map;
    [SerializeField] private MapGenerator.MAP_GANARATION_PROPERTY   _map_generation_property;

    [SerializeField] private GameObject                             _player;
    [SerializeField] private GameObject                             _boss_prefab;

    private List<Room>  _roomList = new List<Room>();

}
