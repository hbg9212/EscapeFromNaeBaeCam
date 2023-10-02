using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{

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
        for (int i = 0; i < siblingNumber; ++i)
        {
            GenerateRoom(generator._node_array[begin + i]);
        }

        int parentNumber = (int)Mathf.Pow(2, generator._property._map_generation_patition_number) - 1;
        for (int i = 0; i < parentNumber; ++i)
        {
            GenerateCorridor(generator._node_array[i]);
        }

        GenerateWall();

    }

    private void GenerateRoom(MapGenerator.Node node)
    {

        Debug.Assert(node.IsLeaf);

        for(int y = 0; y < node.RoomRect.height; ++y)
        {
            for (int x = 0; x < node.RoomRect.width; ++x)
            {

                _tile_map.SetTile(new Vector3Int(node.RoomRect.x + x, node.RoomRect.y + y, 0), _tile);

            }
        }

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

        //���� �����ϴ� Ÿ���� �����ϴ� ���� ����ϴ�.
        bool[] isFloorMap = new bool[_map_generation_property._cell_number_x * _map_generation_property._cell_number_y];
        for (int y = 0; y < _map_generation_property._cell_number_y; ++y)
        {
            for (int x = 0; x < _map_generation_property._cell_number_x; ++x)
            {
                isFloorMap[y * _map_generation_property._cell_number_x + x] = _tile_map.GetTile(new Vector3Int(x, y, 0)) != _background_tile;
            }
        }


        //�ؿ� if���� ������ �ϱ� ���� for����[0, w], [0, h]���·� ������ ������ �ε����� ���� �� -1�� �� �ʿ���� �մϴ�.
        var w = _map_generation_property._cell_number_x - 1;
        var h = _map_generation_property._cell_number_y - 1;
        for (int y = 0; y <= h; ++y)
        {
            for (int x = 0; x <= w; ++x)
            {

                //�ֺ��� ���� �����ϴ� Ÿ���� �ִٸ� ���� ����ϴ�.
                if(!isFloorMap[y * _map_generation_property._cell_number_x + x])
                {

                    if (isFloorMap[Math.Max(y - 1, 0) * _map_generation_property._cell_number_x + Math.Max(x - 1, 0)]   ||
                        isFloorMap[Math.Max(y - 1, 0) * _map_generation_property._cell_number_x + (x + 0)]              ||
                        isFloorMap[Math.Max(y - 1, 0) * _map_generation_property._cell_number_x + Math.Min(x + 1, w)]   ||
                        isFloorMap[y + 0 * _map_generation_property._cell_number_x + Math.Max(x - 1, 0)]                           ||
                        isFloorMap[y + 0 * _map_generation_property._cell_number_x + Math.Min(x + 1, w)]                           ||
                        isFloorMap[Math.Min(y + 1, h) * _map_generation_property._cell_number_x + Math.Max(x - 1, 0)]   ||
                        isFloorMap[Math.Min(y + 1, h) * _map_generation_property._cell_number_x + (x + 0)]              ||
                        isFloorMap[Math.Min(y + 1, h) * _map_generation_property._cell_number_x + Math.Min(x + 1, w)])
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

}
