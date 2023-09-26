using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class MapGenerator : MonoBehaviour
{

    public class Node
    {

        public int CenterX => X + W / 2;
        public int CenterY => Y + H / 2;

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

        public GameObject   Grid;
        public GameObject   Room;
        public GameObject   Corridor;

    }


    public void GenerateMap()
    {

        //맵 전체 크기의 루트 노드를 생성합니다.
        //포화 이진 트리 구조이기에 노드의 개수를 알 수 있으니 미리 생성해 놓습니다.
        _node_array = new Node[(int)Mathf.Pow(2, _map_generation_patition_number + 1) - 1];
        for (int i = 0; i < _node_array.Length; ++i)
        {
            _node_array[i] = new Node();
            _node_array[i].Grid     = Instantiate(_grid_prefab);
            _node_array[i].Room     = Instantiate(_grid_prefab);
            _node_array[i].Corridor = Instantiate(_grid_prefab);
            _node_array[i].Grid.transform.SetParent(_grid_parent.transform);
            _node_array[i].Room.transform.SetParent(_grid_parent.transform);
            _node_array[i].Corridor.transform.SetParent(_grid_parent.transform);
        }
        _node_array[0].X = 0;
        _node_array[0].Y = 0;
        _node_array[0].W = _grid_number_x;
        _node_array[0].H = _grid_number_y;
        UpdateLineRenderer(_node_array[0]);

        
        //영역을 분할합니다.
        var siblingNumber   = 1;
        int begin           = 0;
        bool isHorizonSplit = Random.Range(0, 2) % 2 == 0;
        for (int i = 0; i < _map_generation_patition_number; ++i)
        {

            begin = (int)Mathf.Pow(2, i) - 1;
            for (int j = 0; j < siblingNumber; ++j)
            {

                //포화 이진 트리이기 때문에 자식, 부모의 인덱스를 계산할 수 있습니다.
                var index = begin + j;
                var childLeft = index * 2 + 1;
                var childRight = childLeft + 1;

                //자신을 분할해 자식을 설정합니다.
                Split(_node_array[index], _node_array[childLeft], _node_array[childRight], isHorizonSplit);


            }
            
            isHorizonSplit = !isHorizonSplit;
            siblingNumber *= 2;

        }

        //분할된 영역에 방을 생성합니다.
        siblingNumber   = (int)Mathf.Pow(2, _map_generation_patition_number);
        begin           = (int)Mathf.Pow(2, _map_generation_patition_number) - 1;
        for (int i = 0; i < siblingNumber; ++i)
        {
            GenerateRoom(_node_array[begin + i]);
            UpdateLineRenderer(_node_array[begin + i]);
        }


        //방들을 연결합니다.

        //for (int i = _node_array.Length - 1; i >= 0; i -= 2)
        //{
        //
        //    GenerateCorrider(_node_array[i - 1], _node_array[i]);
        //
        //}

        siblingNumber   = (int)Mathf.Pow(2, _map_generation_patition_number);
        begin           = (int)Mathf.Pow(2, _map_generation_patition_number) - 1;
        for (int i = 0; i < siblingNumber; i += 2)
        {
            GenerateCorrider(_node_array[(begin + i - 1) / 2], _node_array[begin + i], _node_array[begin + i + 1]);
            UpdateLineRenderer(_node_array[(begin + i - 1) / 2]);
        }

    }


    public void Split(Node parent, Node left, Node right, bool isHorizonSplit)
    {

        parent.IsSplitHorizon = isHorizonSplit;
        if (parent.IsSplitHorizon)
        {

            int splitY = (int)(parent.H * Random.Range(_grid_split_random_range_y.x, _grid_split_random_range_y.y));

            left.X = parent.X;
            left.Y = parent.Y;
            left.W = parent.W;
            left.H = splitY;

            right.X = parent.X;
            right.Y = parent.Y + splitY;
            right.W = parent.W;
            right.H = parent.H - splitY;

        }
        else
        {

            var splitX = (int)(parent.W * Random.Range(_grid_split_random_range_x.x, _grid_split_random_range_x.y));

            left.X = parent.X;
            left.Y = parent.Y;
            left.W = splitX;
            left.H = parent.H;

            right.X = parent.X + splitX;
            right.Y = parent.Y;
            right.W = parent.W - splitX;
            right.H = parent.H;

        }

    }


    public void GenerateRoom(Node node)
    {

        node.RoomW = (int)(node.W * Random.Range(_room_random_range_x.x, _room_random_range_x.y));
        node.RoomH = (int)(node.H * Random.Range(_room_random_range_x.y, _room_random_range_x.y));
        node.RoomX = Random.Range(node.X + 1, node.X + node.W - node.RoomW - 1);
        node.RoomY = Random.Range(node.Y + 1, node.Y + node.H - node.RoomH - 1);

    }

    private void GenerateCorrider(Node parent, Node node0, Node node1)
    {

        int maxBegin;
        int minEnd;
        if (parent.IsSplitHorizon)
        {

            maxBegin    = Mathf.Max(node0.RoomX + _door_grid_size, node1.RoomX + _door_grid_size);
            minEnd      = Mathf.Min(node0.RoomX + node0.RoomW - _door_grid_size, node1.RoomX + node1.RoomW - _door_grid_size);

            parent.CorridorX        = Random.Range(maxBegin, minEnd + 1);
            parent.CorridorW        = _door_grid_size;
            parent.CorridorY        = Mathf.Min(node0.CenterY, node1.CenterY);
            parent.CorridorH        = Mathf.Abs(node1.CenterY - node0.CenterY);

        }
        else
        {

            maxBegin    = Mathf.Max(node0.RoomY + _door_grid_size, node1.RoomY + _door_grid_size);
            minEnd      = Mathf.Min(node0.RoomY + node0.RoomH - _door_grid_size, node1.RoomY + node1.RoomH - _door_grid_size);

            parent.CorridorX        = Mathf.Min(node0.CenterX, node1.CenterX);
            parent.CorridorW        = Mathf.Abs(node1.CenterX - node0.CenterX);
            parent.CorridorY        = Random.Range(maxBegin, minEnd + 1);
            parent.CorridorH        = _door_grid_size;

        }

    }

    public void UpdateLineRenderer(Node node)
    {

        var gridLineRenderer = node.Grid.GetComponent<LineRenderer>();

        var left                        = (float)node.X / _grid_number_x * _map_size.x;
        var right                       = (float)(node.X + node.W) / _grid_number_x * _map_size.x;
        var top                         = (float)(node.Y + node.H) / _grid_number_y * _map_size.y;
        var bottom                      = (float)node.Y / _grid_number_y * _map_size.y;
        gridLineRenderer.enabled        = false;
        gridLineRenderer.startColor     = Color.blue;
        gridLineRenderer.endColor       = Color.blue;
        gridLineRenderer.positionCount  = 4;
        gridLineRenderer.SetPosition(0, new Vector2(left,   top) - _map_pivot);
        gridLineRenderer.SetPosition(1, new Vector2(right,  top) - _map_pivot);
        gridLineRenderer.SetPosition(2, new Vector2(right,  bottom) - _map_pivot);
        gridLineRenderer.SetPosition(3, new Vector2(left,   bottom) - _map_pivot);


        //Room
        var roomLineRenderer = node.Room.GetComponent<LineRenderer>();

        left                            = (float)node.RoomX / _grid_number_x * _map_size.x;
        right                           = (float)(node.RoomX + node.RoomW) / _grid_number_x * _map_size.x;
        top                             = (float)(node.RoomY + node.RoomH) / _grid_number_y * _map_size.y;
        bottom                          = (float)node.RoomY / _grid_number_y * _map_size.y;
        roomLineRenderer.startColor     = Color.red;
        roomLineRenderer.endColor       = Color.red;
        roomLineRenderer.positionCount  = 4;
        roomLineRenderer.SetPosition(0, new Vector2(left,   top) - _map_pivot);
        roomLineRenderer.SetPosition(1, new Vector2(right,  top) - _map_pivot);
        roomLineRenderer.SetPosition(2, new Vector2(right,  bottom) - _map_pivot);
        roomLineRenderer.SetPosition(3, new Vector2(left,   bottom) - _map_pivot);

        //Corrior
        var corridorLineRenderer = node.Corridor.GetComponent<LineRenderer>();

        left                                = (float)node.CorridorX / _grid_number_x * _map_size.x;
        right                               = (float)(node.CorridorX + node.CorridorW) / _grid_number_x * _map_size.x;
        top                                 = (float)(node.CorridorY + node.CorridorH) / _grid_number_y * _map_size.y;
        bottom                              = (float)node.CorridorY / _grid_number_y * _map_size.y;
        corridorLineRenderer.startColor     = Color.red;
        corridorLineRenderer.endColor       = Color.red;
        corridorLineRenderer.positionCount  = 4;
        corridorLineRenderer.SetPosition(0, new Vector2(left,   top) - _map_pivot);
        corridorLineRenderer.SetPosition(1, new Vector2(right,  top) - _map_pivot);
        corridorLineRenderer.SetPosition(2, new Vector2(right,  bottom) - _map_pivot);
        corridorLineRenderer.SetPosition(3, new Vector2(left,   bottom) - _map_pivot);
        

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

    [SerializeField] private Vector2    _map_size   = new Vector2();
    [SerializeField] private Vector2    _map_pivot  = new Vector2();
    [SerializeField] private int        _map_generation_patition_number;

    [SerializeField] private int        _grid_number_x;
    [SerializeField] private int        _grid_number_y;
    [SerializeField] private Vector2    _grid_split_random_range_x;
    [SerializeField] private Vector2    _grid_split_random_range_y;

    [SerializeField] private Vector2    _room_random_range_x;
    [SerializeField] private Vector2    _room_random_range_y;

    [SerializeField] private int        _door_grid_size;

    private Node[] _node_array;

}
