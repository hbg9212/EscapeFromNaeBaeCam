using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class MapGenerator : MonoBehaviour
{

    public class Node
    {

        public int x;
        public int y;
        public int w;
        public int h;

        public GameObject   Grid;

        public void UpdateGrid(int gridNumberX, int gridNumberY, Vector2 size, Vector2 pivot)
        {

            var gridLineRenderer = Grid.GetComponent<LineRenderer>();

            var left    = (float)x / gridNumberX * size.x;
            var right   = (float)(x + w) / gridNumberX * size.x;
            var top     = (float)(y + h) / gridNumberY * size.y;
            var bottom  = (float)y / gridNumberY * size.y;
            gridLineRenderer.positionCount = 4;
            gridLineRenderer.SetPosition(0, new Vector2(left,   top) - pivot);
            gridLineRenderer.SetPosition(1, new Vector2(right,  top) - pivot);
            gridLineRenderer.SetPosition(2, new Vector2(right,  bottom) - pivot);
            gridLineRenderer.SetPosition(3, new Vector2(left,   bottom) - pivot);

        }

        public void Split(ref Node left, ref Node right, Vector2 gridSplitRandomRangeX, Vector2 gridSplitRandomRangeY, bool isHorizonSplit)
        {

            //isHorizonSplit = Random.Range(0, 2) % 2 == 0;
            if (isHorizonSplit)
            {

                int splitY = (int)(h * Random.Range(gridSplitRandomRangeY.x, gridSplitRandomRangeY.y));

                left.x = x;
                left.y = y;
                left.w = w;
                left.h = splitY;

                right.x = x;
                right.y = y + splitY;
                right.w = w;
                right.h = h - splitY;

            }
            else
            {

                var splitX = (int)(w * Random.Range(gridSplitRandomRangeX.x, gridSplitRandomRangeX.y));

                left.x = x;
                left.y = y;
                left.w = splitX;
                left.h = h;

                right.x = x + splitX;
                right.y = y;
                right.w = w - splitX;
                right.h = h;

            }

        }

    }


    public void GenerateMap()
    {

        _node_array = new Node[(int)System.Math.Pow(2, _map_generation_patition_number + 1) - 1];
        for(int i = 0; i < _node_array.Length; ++i)
        {
            _node_array[i]          = new Node();
            _node_array[i].Grid     = Instantiate(_grid_prefab);
            _node_array[i].Grid.transform.SetParent(_grid_parent.transform);
        }
        _node_array[0].x = 0;
        _node_array[0].y = 0;
        _node_array[0].w = _map_generation_grid_number_x;
        _node_array[0].h = _map_generation_grid_number_y;
        _node_array[0].UpdateGrid(_map_generation_grid_number_x, _map_generation_grid_number_y, _map_size, _map_pivot);

        bool isHorizonSplit = Random.Range(0, 2) % 2 == 0;
        var siblingNumber = 1;
        for(int i = 0; i < _map_generation_patition_number; ++i)
        {

            var begin = (int)System.Math.Pow(2, i) - 1;
            for(int j = 0; j < siblingNumber; ++j)
            {

                var index       = begin + j;
                var childLeft   = index * 2 + 1;
                var childRight  = childLeft + 1;

                _node_array[index].Split(ref _node_array[childLeft], ref _node_array[childRight], _map_generation_grid_split_random_range_x, _map_generation_grid_split_random_range_y, isHorizonSplit);
                _node_array[childLeft].UpdateGrid(_map_generation_grid_number_x, _map_generation_grid_number_y, _map_size, _map_pivot);
                _node_array[childRight].UpdateGrid(_map_generation_grid_number_x, _map_generation_grid_number_y, _map_size, _map_pivot);


            }

            isHorizonSplit = !isHorizonSplit;
            siblingNumber *= 2;

        }

    }


    private void Start()
    {

        _map_size.x = Camera.main.orthographicSize * Camera.main.aspect * 2.0f;
        _map_size.y = Camera.main.orthographicSize * 2.0f;

        _map_pivot = _map_size * 0.5f;

        GenerateMap();

    }


    [SerializeField] private GameObject _grid_prefab;
    [SerializeField] private GameObject _grid_parent;

    [SerializeField] private Vector2 _map_size   = new Vector2();
    [SerializeField] private Vector2 _map_pivot  = new Vector2();

    [SerializeField] private int        _map_generation_grid_number_x;
    [SerializeField] private int        _map_generation_grid_number_y;
    [SerializeField] private Vector2    _map_generation_grid_split_random_range_x;
    [SerializeField] private Vector2    _map_generation_grid_split_random_range_y;
    [SerializeField] private int        _map_generation_patition_number;

    private Node[] _node_array;

}
