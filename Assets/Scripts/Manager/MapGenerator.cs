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

        public int X;
        public int Y;
        public int W;
        public int H;

        public int RoomX;
        public int RoomY;
        public int RoomW;
        public int RoomH;

        public bool DoorHorizon;
        public int  DoorBegin;
        public int  DoorEnd;

        public GameObject   Grid;
        public GameObject   Room;
        public GameObject   DoorGrid;

        public void UpdateGrid(int gridNumberX, int gridNumberY, Vector2 size, Vector2 pivot)
        {

            var gridLineRenderer = Grid.GetComponent<LineRenderer>();

            var left                        = (float)X / gridNumberX * size.x;
            var right                       = (float)(X + W) / gridNumberX * size.x;
            var top                         = (float)(Y + H) / gridNumberY * size.y;
            var bottom                      = (float)Y / gridNumberY * size.y;
            gridLineRenderer.enabled        = false;
            gridLineRenderer.startColor     = Color.blue;
            gridLineRenderer.endColor       = Color.blue;
            gridLineRenderer.positionCount  = 4;
            gridLineRenderer.SetPosition(0, new Vector2(left,   top) - pivot);
            gridLineRenderer.SetPosition(1, new Vector2(right,  top) - pivot);
            gridLineRenderer.SetPosition(2, new Vector2(right,  bottom) - pivot);
            gridLineRenderer.SetPosition(3, new Vector2(left,   bottom) - pivot);


            //Room
            var roomLineRenderer = Room.GetComponent<LineRenderer>();

            left                            = (float)RoomX / gridNumberX * size.x;
            right                           = (float)(RoomX + RoomW) / gridNumberX * size.x;
            top                             = (float)(RoomY + RoomH) / gridNumberY * size.y;
            bottom                          = (float)RoomY / gridNumberY * size.y;
            roomLineRenderer.startColor     = Color.red;
            roomLineRenderer.endColor       = Color.red;
            roomLineRenderer.positionCount  = 4;
            roomLineRenderer.SetPosition(0, new Vector2(left,   top) - pivot);
            roomLineRenderer.SetPosition(1, new Vector2(right,  top) - pivot);
            roomLineRenderer.SetPosition(2, new Vector2(right,  bottom) - pivot);
            roomLineRenderer.SetPosition(3, new Vector2(left,   bottom) - pivot);

            var doorGridLineRenderer = DoorGrid.GetComponent<LineRenderer>();
            doorGridLineRenderer.positionCount = 0;
            

        }

    }


    public void GenerateMap()
    {

        _node_array = new Node[(int)System.Math.Pow(2, _map_generation_patition_number + 1) - 1];
        for (int i = 0; i < _node_array.Length; ++i)
        {
            _node_array[i] = new Node();
            _node_array[i].Grid     = Instantiate(_grid_prefab);
            _node_array[i].Room     = Instantiate(_grid_prefab);
            _node_array[i].DoorGrid = Instantiate(_grid_prefab);
            _node_array[i].Grid.transform.SetParent(_grid_parent.transform);
            _node_array[i].Room.transform.SetParent(_grid_parent.transform);
            _node_array[i].DoorGrid.transform.SetParent(_grid_parent.transform);
        }
        _node_array[0].X = 0;
        _node_array[0].Y = 0;
        _node_array[0].W = _grid_number_x;
        _node_array[0].H = _grid_number_y;
        _node_array[0].UpdateGrid(_grid_number_x, _grid_number_y, _map_size, _map_pivot);

        bool isHorizonSplit = Random.Range(0, 2) % 2 == 0;
        var siblingNumber = 1;
        for (int i = 0; i < _map_generation_patition_number; ++i)
        {

            var begin = (int)System.Math.Pow(2, i) - 1;
            for (int j = 0; j < siblingNumber; ++j)
            {

                //포화 이진 트리이기 때문에 자식, 부모의 인덱스를 계산할 수 있습니다.
                var index = begin + j;
                var childLeft = index * 2 + 1;
                var childRight = childLeft + 1;

                //자신을 분할해 자식을 설정합니다.
                Split(_node_array[index], _node_array[childLeft], _node_array[childRight], isHorizonSplit);
                if (i == _map_generation_patition_number - 1)
                {
                    GenerateRoom(_node_array[childLeft]);
                    GenerateRoom(_node_array[childRight]);
                }
                
                _node_array[childLeft].UpdateGrid(_grid_number_x, _grid_number_y, _map_size, _map_pivot);
                _node_array[childRight].UpdateGrid(_grid_number_x, _grid_number_y, _map_size, _map_pivot);

            }

            isHorizonSplit = !isHorizonSplit;
            siblingNumber *= 2;

        }

    }

    public void Split(Node parent, Node left, Node right, bool isHorizonSplit)
    {

        if (isHorizonSplit)
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
