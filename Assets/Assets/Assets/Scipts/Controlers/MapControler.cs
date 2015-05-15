using UnityEngine;
using System.Collections;
using Assets.Assets.Scipts.Entitys;
using Assets.Assets.Scipts.Managers;
using System.Collections.Generic;

public class MapControler : MonoBehaviour {
    /// <summary>
    /// Quản lý các phương thức liên quan đến vẽ map
    /// </summary>
    public List<Map> _ListMapElement = new List<Map>();//Chứa danh sách các phần tử ma trận trọng số load từ file text
    public GameObject[] _ListObstruction = null;
    /////////
    #region====
    public LineRenderer oblineRenderer;
    private float counter;
    private float dist;

    public Terrain terain;
    public float lineDrawSpeed = 6f;

    public  int lineHorizonCount = 14;// 14 đường kẻ
    public  int lineVerticalCount = 14;// 14 đường kẻ theo chiều dọc
    public float HeightMap = 10;//Bề cao theo trục y của map
    public float partHorizonLeng;//KHoảng cách giữa 2 line chiều ngang// khai báo static để gọi bên MoveObject
    public float partVerticalLeng;//Khoảng cách giữa 2 line chiều dọc
    Vector3 pointA = new Vector3(130, 0, 130); //origin.position;
    Vector3 pointB = new Vector3(390, 0, 130);//destination.position;
    public Vector3 pointA0 = new Vector3();//Để lưu lại tọa độ đầu tiên làm gốc bản đồ các đường line để tính vị trí của điểm 
    static float widthMap;//Chiều theo trục x của map
    static float longMap;//Chiều theo truc z của map
    #endregion
    // Use this for initialization
    void Start()
    {
        InitMapLine();
        //========
        LoadListMapElementAndObstruct();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter < dist)
        {
            counter += .1f / lineDrawSpeed;
            float x = Mathf.MoveTowards(0, dist, counter);
        }
    }
    /// <summary>
    /// Khởi tạo một ma trận các đường trên terrain
    /// </summary>
    private void InitMapLine()
    {
        widthMap = terain.GetComponent<Terrain>().terrainData.size.x;
        longMap = terain.GetComponent<Terrain>().terrainData.size.z;

        //14*2 đường có 2 đường chung vậy có (14*2-2-1) phần
        partHorizonLeng = widthMap / ((lineHorizonCount - 1) * 2);//KHoảng cách giữa 2 line chiều ngang
        partVerticalLeng = longMap / ((lineVerticalCount - 1) * 2);//Khoảng cách giữa 2 line chiều dọc

        for (int i = 0; i < lineVerticalCount; i++)//Vẽ dòng kẻ ngang
        {
            Instantiate(oblineRenderer);
            LineRenderer lineRenderer = oblineRenderer.GetComponent<LineRenderer>();
            lineRenderer.SetWidth(.2f, 0.2f);
            pointA = new Vector3(((float)(lineHorizonCount - 1) / 2) * partHorizonLeng, HeightMap, ((float)(lineVerticalCount - 1) / 2) * partVerticalLeng + (i * partVerticalLeng));
            pointB = new Vector3((((float)(lineHorizonCount - 1) / 2) + (lineHorizonCount - 1)) * partHorizonLeng, HeightMap, ((float)(lineVerticalCount - 1) / 2) * partVerticalLeng + (i * partVerticalLeng));
            if (pointA0.x == 0.0)
            {
                pointA0 = pointA;
            }
            lineRenderer.SetPosition(0, pointA);
            lineRenderer.SetPosition(1, pointB);
        }
        for (int i = 0; i < lineHorizonCount; i++)//Vẽ dòng kẻ dọc
        {
            Instantiate(oblineRenderer);
            LineRenderer lineRenderer = oblineRenderer.GetComponent<LineRenderer>();
            lineRenderer.SetWidth(.2f, 0.2f);
            pointA = new Vector3(((float)(lineHorizonCount - 1) / 2) * partHorizonLeng + (i * partHorizonLeng), HeightMap, ((float)(lineVerticalCount - 1) / 2) * partVerticalLeng);//
            pointB = new Vector3(((float)(lineHorizonCount - 1) / 2) * partHorizonLeng + (i * partHorizonLeng), HeightMap, (((float)(lineVerticalCount - 1) / 2) + (lineVerticalCount - 1)) * partVerticalLeng);
            lineRenderer.SetPosition(0, pointA);
            lineRenderer.SetPosition(1, pointB);
        }
    }
    /// <summary>
    /// Load các thông số cho _ListMapElement
    /// </summary>
    private void LoadListMapElementAndObstruct()
    {
        //_ListMapElement= list
        //Sử dụng ở đâu:Tự khởi tạo
        //Khi nào sử dụng:Bắt đầu vào ngay trong hàm start và chỉ gọi một lần
        //Phụ thuộc cái gì:Không có, chỉ tự load các thông số ban đầu từ file text
        int[,] _Arr=new int[0,2];//Trả về loại vật cản và index ô đầu tiên chứa vật cản 
        _ListMapElement = MapManager.InitListWeight("Assets/Datas/input.txt",out _Arr);
        //====Load Vat can
        for (int i = 0; i < _Arr.GetLength(0); i++)//Chỉ số hàng mảng 2 chiều
        {
            Map map=_ListMapElement[_Arr[i,1]];
            Vector3 pos=new Vector3(map.Position.x*partHorizonLeng+pointA0.x,HeightMap,map.Position.z*partVerticalLeng+pointA0.z);
            GameObject.Instantiate(_ListObstruction[_Arr[i, 0]], pos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Trả về vị trí chính xác để đặt tháp xuống bản đồ
    /// </summary>
    public Map GetMapByPossiton(Vector3 pos)
    {
        pos.x -= pointA0.x; pos.z -= pointA0.z;
        int x = (int)(pos.x / partHorizonLeng);
        int z = (int)(pos.z / partVerticalLeng);
        Vector3 vt = new Vector3(x * partHorizonLeng + pointA0.x, 0, z * partVerticalLeng + pointA0.z);
        Map map = new Map();
        for (int i = 0; i < _ListMapElement.Count; i++)
        {
            if ((int)_ListMapElement[i].Position.x==x && (int)_ListMapElement[i].Position.z==z)
            {
                map = _ListMapElement[i];
                break;
            }
        }
        return map;
    }
    //Lấy vị trí để qoái vật di chuyển
    public Vector3 GetPossition(float x, float y, float z)//X,Z là hàng và cột của ma trận đường đi
    {
        Vector3 pos = new Vector3();
        pos.x = x * partHorizonLeng + pointA0.x + partHorizonLeng / 2;
        pos.y = y;
        pos.z = (z * partVerticalLeng + pointA0.z + partVerticalLeng / 2);
        return pos;
    }
    //Trả về chỉ số ma trận trên map theo vitri
    public int GetIndexMap(Vector3 pos)
    {
        pos.x -= pointA0.x; pos.z -= pointA0.z;
        int x = (int)(pos.x / partHorizonLeng);
        int z = (int)(pos.z / partVerticalLeng);
        int NumRow = lineHorizonCount - 1;
        int index = NumRow * x + z;
        return index;
    }
}
