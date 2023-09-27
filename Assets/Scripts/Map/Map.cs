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

    }

    private void GenerateRoom(MapGenerator.Node node)
    {

        Debug.Assert(node.IsLeafNode);

        for(int y = 0; y < node.RoomH; ++y)
        {
            for (int x = 0; x < node.RoomW; ++x)
            {

                _tile_map.SetTile(new Vector3Int(node.RoomX + x, node.RoomY + y, 0), _tile);

            }
        }

    }

    private void GenerateCorridor(MapGenerator.Node node)
    {

        Debug.Assert(!node.IsLeafNode);

        for (int y = 0; y < node.CorridorH; ++y)
        {
            for (int x = 0; x < node.CorridorW; ++x)
            {

                var tileIndex   = new Vector3Int(node.CorridorX + x, node.CorridorY + y, 0);
                var tile        = _tile_map.GetTile(tileIndex);

                _tile_map.SetTile(tileIndex, _tile);

            }
        }

    }

    [SerializeField] private TileBase                               _tile;
    [SerializeField] private TileBase                               _background_tile;
    [SerializeField] private Tilemap                                _tile_map;
    [SerializeField] private MapGenerator.MAP_GANARATION_PROPERTY   _map_generation_property;

}
