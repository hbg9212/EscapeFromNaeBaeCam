using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class MapGenerator
{

    [System.Serializable]
    public struct MAP_GANARATION_PROPERTY
    {

        public GameObject _grid_prefab;
        public GameObject _grid_parent;

        public Vector2    _map_size;
        public Vector2    _map_pivot;
        public int        _map_generation_patition_number;

        public int        _cell_number_x;
        public int        _cell_number_y;
        public Vector2    _cell_split_random_range_x;
        public Vector2    _cell_split_random_range_y;

        public Vector2    _room_random_range_x;
        public Vector2    _room_random_range_y;

        public int        _door_grid_size;

    }

    public class Node
    {

        public int CenterX => X + W / 2;
        public int CenterY => Y + H / 2;

        public bool IsLeafNode => ChildLeft == null;

        public int X;
        public int Y;
        public int W;
        public int H;

        public int RoomX;
        public int RoomY;
        public int RoomW;
        public int RoomH;

        public int CorridorX;
        public int CorridorY;
        public int CorridorW;
        public int CorridorH;

        public bool IsSplitHorizon;

        public Node ChildLeft   = null;
        public Node ChildRight  = null;

        //디버깅용 입니다.
        public GameObject   Grid;
        public GameObject   Room;
        public GameObject   Corridor;


        public RectInt CalculateRect()
        {

            RectInt rect = new RectInt();

            if (IsLeafNode)
            {
                rect.min   = new Vector2Int(RoomX, RoomY);
                rect.max   = new Vector2Int(RoomX + RoomW, RoomY + RoomH);
                return rect;
            }

            var leftRect    = ChildLeft.CalculateRect();
            var rightRect   = ChildRight.CalculateRect();
            rect.min        = new Vector2Int(System.Math.Min(leftRect.min.x, rightRect.min.x), System.Math.Min(leftRect.min.y, rightRect.min.y));
            rect.max        = new Vector2Int(System.Math.Max(leftRect.max.x, rightRect.max.x), System.Math.Max(leftRect.max.y, rightRect.max.y));

            return rect;

        }


    }

    public void GenerateMap()
    {

        CreateTree();

        
        //영역을 분할합니다.
        var siblingNumber   = 1;
        int begin           = 0;
        bool isHorizonSplit = Random.Range(0, 2) % 2 == 0;
        for (int i = 0; i < _property._map_generation_patition_number; ++i)
        {

            begin = (int)Mathf.Pow(2, i) - 1;
            for (int j = 0; j < siblingNumber; ++j)
            {

                //포화 이진 트리이기 때문에 자식, 부모의 인덱스를 계산할 수 있습니다.
                var index = begin + j;
                var childLeft = index * 2 + 1;
                var childRight = childLeft + 1;

                //자신을 분할해 자식을 설정합니다.
                Split(_node_array[index], isHorizonSplit);


            }
            
            isHorizonSplit = !isHorizonSplit;
            siblingNumber *= 2;

        }

        //분할된 영역에 방을 생성합니다.
        siblingNumber   = (int)Mathf.Pow(2, _property._map_generation_patition_number);
        begin           = (int)Mathf.Pow(2, _property._map_generation_patition_number) - 1;
        for (int i = 0; i < siblingNumber; ++i)
        {
            GenerateRoom(_node_array[begin + i]);
            UpdateLineRenderer(_node_array[begin + i]);
        }


        //방들을 연결합니다.

        for (int i = (int)Mathf.Pow(2, _property._map_generation_patition_number) - 1 - 1; i >= 0; --i)
        {
            GenerateCorridor(_node_array[i]);
            UpdateLineRenderer(_node_array[i]);
        }

    }


    private void CreateTree()
    {

        //포화 이진 트리 구조이기에 노드의 개수를 알 수 있으니 미리 생성해 놓습니다.
        _node_array = new Node[(int)Mathf.Pow(2, _property._map_generation_patition_number + 1) - 1];
        for (int i = 0; i < _node_array.Length; ++i)
        {

            _node_array[i] = new Node();

            //디버깅 용도
            _node_array[i].Grid     = GameObject.Instantiate(_property._grid_prefab);
            _node_array[i].Room     = GameObject.Instantiate(_property._grid_prefab);
            _node_array[i].Corridor = GameObject.Instantiate(_property._grid_prefab);
            _node_array[i].Grid.transform.SetParent(_property._grid_parent.transform);
            _node_array[i].Room.transform.SetParent(_property._grid_parent.transform);
            _node_array[i].Corridor.transform.SetParent(_property._grid_parent.transform);

        }

        int   parentNodeCount = (int)Mathf.Pow(2, _property._map_generation_patition_number) - 1;
        for (int i = 0; i < parentNodeCount; ++i)
        {
            _node_array[i].ChildLeft  = _node_array[i * 2 + 1];
            _node_array[i].ChildRight = _node_array[i * 2 + 2];
        }

        //루트 노드를 맵 전체 크기로 설정합니다.
        _node_array[0].X = 0;
        _node_array[0].Y = 0;
        _node_array[0].W = _property._cell_number_x;
        _node_array[0].H = _property._cell_number_y;

    }

    private void Split(Node node, bool isHorizonSplit)
    {

        var left    = node.ChildLeft;
        var right   = node.ChildRight;
        node.IsSplitHorizon = isHorizonSplit;
        if (node.IsSplitHorizon)
        {

            int splitY = (int)(node.H * Random.Range(_property._cell_split_random_range_y.x, _property._cell_split_random_range_y.y));

            left.X = node.X;
            left.Y = node.Y;
            left.W = node.W;
            left.H = splitY;

            right.X = node.X;
            right.Y = node.Y + splitY;
            right.W = node.W;
            right.H = node.H - splitY;

        }
        else
        {

            var splitX = (int)(node.W * Random.Range(_property._cell_split_random_range_x.x, _property._cell_split_random_range_x.y));

            left.X = node.X;
            left.Y = node.Y;
            left.W = splitX;
            left.H = node.H;

            right.X = node.X + splitX;
            right.Y = node.Y;
            right.W = node.W - splitX;
            right.H = node.H;

        }

    }


    private void GenerateRoom(Node node)
    {

        node.RoomW = (int)(node.W * Random.Range(_property._room_random_range_x.x, _property._room_random_range_x.y));
        node.RoomH = (int)(node.H * Random.Range(_property._room_random_range_x.y, _property._room_random_range_x.y));
        node.RoomX = Random.Range(node.X + 1, node.X + node.W - node.RoomW - 1);
        node.RoomY = Random.Range(node.Y + 1, node.Y + node.H - node.RoomH - 1);

    }

    private void GenerateCorridor(Node node)
    {

        Node left               = node.ChildLeft;
        Node right              = node.ChildRight;
        RectInt leftRect        = left.CalculateRect();
        RectInt rightRect       = right.CalculateRect();
        int maxBegin;
        int minEnd;
        if (node.IsSplitHorizon)
        {

            maxBegin    = Mathf.Max(leftRect.x + _property._door_grid_size, rightRect.x + _property._door_grid_size);
            minEnd      = Mathf.Min(leftRect.x + leftRect.width - _property._door_grid_size, rightRect.x + rightRect.width - _property._door_grid_size);

            node.CorridorX        = Random.Range(maxBegin, minEnd + 1);
            node.CorridorW        = _property._door_grid_size;
            node.CorridorY        = leftRect.y;
            node.CorridorH        = (int)Mathf.Abs(rightRect.y + rightRect.height - leftRect.y);

        }
        else
        {

            maxBegin    = Mathf.Max(leftRect.y + _property._door_grid_size, rightRect.y + _property._door_grid_size);
            minEnd      = Mathf.Min(leftRect.y + leftRect.height - _property._door_grid_size, rightRect.y + rightRect.height - _property._door_grid_size);

            node.CorridorX        = leftRect.x;
            node.CorridorW        = (int)Mathf.Abs(rightRect.x + rightRect.width - leftRect.x);
            node.CorridorY        = Random.Range(maxBegin, minEnd + 1);
            node.CorridorH        = _property._door_grid_size;

        }

    }

    public void UpdateLineRenderer(Node node)
    {

        var gridLineRenderer = node.Grid.GetComponent<LineRenderer>();
        
        var left                        = (float)node.X / _property._cell_number_x * _property._map_size.x;
        var right                       = (float)(node.X + node.W) / _property._cell_number_x * _property._map_size.x;
        var top                         = (float)(node.Y + node.H) / _property._cell_number_y * _property._map_size.y;
        var bottom                      = (float)node.Y / _property._cell_number_y * _property._map_size.y;
        gridLineRenderer.enabled        = false;
        gridLineRenderer.startColor     = Color.blue;
        gridLineRenderer.endColor       = Color.blue;
        gridLineRenderer.positionCount  = 4;
        gridLineRenderer.SetPosition(0, new Vector2(left,   top) - _property._map_pivot);
        gridLineRenderer.SetPosition(1, new Vector2(right,  top) - _property._map_pivot);
        gridLineRenderer.SetPosition(2, new Vector2(right,  bottom) - _property._map_pivot);
        gridLineRenderer.SetPosition(3, new Vector2(left,   bottom) - _property._map_pivot);
        
        
        //Room
        var roomLineRenderer = node.Room.GetComponent<LineRenderer>();
        
        left                            = (float)node.RoomX / _property._cell_number_x * _property._map_size.x;
        right                           = (float)(node.RoomX + node.RoomW) / _property._cell_number_x * _property._map_size.x;
        top                             = (float)(node.RoomY + node.RoomH) / _property._cell_number_y * _property._map_size.y;
        bottom                          = (float)node.RoomY / _property._cell_number_y * _property._map_size.y;
        roomLineRenderer.startColor     = Color.red;
        roomLineRenderer.endColor       = Color.red;
        roomLineRenderer.positionCount  = 4;
        roomLineRenderer.SetPosition(0, new Vector2(left,   top) - _property._map_pivot);
        roomLineRenderer.SetPosition(1, new Vector2(right,  top) - _property._map_pivot);
        roomLineRenderer.SetPosition(2, new Vector2(right,  bottom) - _property._map_pivot);
        roomLineRenderer.SetPosition(3, new Vector2(left,   bottom) - _property._map_pivot);
        
        //Corrior
        var corridorLineRenderer = node.Corridor.GetComponent<LineRenderer>();
        
        left                                = (float)node.CorridorX / _property._cell_number_x * _property._map_size.x;
        right                               = (float)(node.CorridorX + node.CorridorW) / _property._cell_number_x * _property._map_size.x;
        top                                 = (float)(node.CorridorY + node.CorridorH) / _property._cell_number_y * _property._map_size.y;
        bottom                              = (float)node.CorridorY / _property._cell_number_y * _property._map_size.y;
        corridorLineRenderer.startColor     = Color.red;
        corridorLineRenderer.endColor       = Color.red;
        corridorLineRenderer.positionCount  = 4;
        corridorLineRenderer.SetPosition(0, new Vector2(left,   top) - _property._map_pivot);
        corridorLineRenderer.SetPosition(1, new Vector2(right,  top) - _property._map_pivot);
        corridorLineRenderer.SetPosition(2, new Vector2(right,  bottom) - _property._map_pivot);
        corridorLineRenderer.SetPosition(3, new Vector2(left,   bottom) - _property._map_pivot);
        

    }

    public MAP_GANARATION_PROPERTY _property;

    public Node[] _node_array;

}
