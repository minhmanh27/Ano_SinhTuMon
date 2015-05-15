using Assets.Assets.Scipts.Entitys;
using Assets.Assets.Scipts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameControler : MonoBehaviour
{
    MapControler _MapControler = null;
    GUIManager _GuiManager = null;
    //1. Chiếu tọa độ từ camera xuống bề mặt terain để lấy tọa độ trên terain
    // sau đó chuyển thành tọa độ phù hợp để đặt tháp
    RaycastHit hit;//Tọa độ chiếu trên bề mặt camera xuống terrain
    private GameObject gameObjectSelect = null;//đối tượng lưu trữ các đối tượng khi click chuột
    void Start()
    {
        _MapControler = GameObject.FindObjectOfType<MapControler>();
        _GuiManager = GameObject.FindObjectOfType<GUIManager>();
    }
    void Update()
    {
        if (_GuiManager._ShowPopup)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            SetTower_Click();
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetVisiblePopup();
            //DelTower_Click();
        }
    }
    /// <summary>
    /// Dùng để ẩn hiện popup nâng cấp hoặc xóa tháp
    /// </summary>
    private void SetVisiblePopup()
    {
        gameObjectSelect = GetGameObjByRaycat();//Trả về đối tượng được chọn
        if (gameObjectSelect.tag == "Tower")
        {
            _GuiManager.GameObjectSelect = gameObjectSelect;
            _GuiManager.ShowEditTowerPanel(true);
        }
    }

    #region Đặt tháp trên các ô Terain

    private Vector3 locatePosition()//Trả về Tọa độ trên bề mặt camera
    {
        Vector3 position = new Vector3();
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            // Debug.Log("Hit point: " + hit.point);
        }
        return position;
    }
    /// <summary>
    /// Lấy đối tượng được chọn
    /// </summary>
    /// <returns></returns>
    private GameObject GetGameObjByRaycat()//Trả về đối tượng được chọn từ bề mặt camera xuống
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObjectSelect = hit.collider.gameObject;
            // Debug.Log("Hit point: " + hit.point);
        }
        return gameObjectSelect;
    }
    /// <summary>
    /// Kiểm tra xem có cho phép đặt tháp hay không>
    /// </summary>
    private bool CheckSetTower(int index)
    {
        //Kiểm tra vị trí đặt tháp theo chỉ số ma trận
        //Quy trình đặt tháp
        //1. Kiểm tra tháp có trùng vị trí hiện tại hay vị trí mà nó đang đi đến hay ko thì mới cho đặt
        //2. Tìm từ vị trí tiếp theo đến đích xem có tồn tại đường đi hay không?
      
    //=====================================
        //1. Không có vật cản hoặc không có tháp
        //2. Không chặn hết đường đi từ đầu đến cuối
        if (_MapControler._ListMapElement[index].Value!=0)
        {
            return false;
        }
        MapManager.RemoveMapElement(_MapControler._ListMapElement, index,2);
        Stack<Map> stackTmp = MapManager.FindWay(_MapControler._ListMapElement[0], _MapControler._ListMapElement[_MapControler._ListMapElement.Count - 1]);
        if (stackTmp == null)
        {
           // Debug.Log("Không đặt được!!!");
            // Add lại điểm cũ
            MapManager.AddMapElement(_MapControler._ListMapElement, _MapControler.lineHorizonCount - 1, index);
            return false;
        }

        List<EnemyControler> enemies = FindObjectsOfType<EnemyControler>().ToList();
        foreach (var enemy in enemies)
        {
            //1. Không chặn hết đường đi
            //2. Không con quái đi đến vị trí đặt tháp
            //======================
            if (enemy._MapNext.Index == index || enemy._PosCurrent == index)
            {
               // Debug.Log("Không đặt được cu ơi!");
                // add lại điểm cũ
                MapManager.AddMapElement(_MapControler._ListMapElement, _MapControler.lineHorizonCount - 1, index);
                return false;
            }
            stackTmp = MapManager.FindWay(enemy._MapNext, enemy._MapFinish);
            if (stackTmp == null)
            {
               // Debug.Log("Không đặt được cu ơi!");
                // add lại điểm cũ
                MapManager.AddMapElement(_MapControler._ListMapElement, _MapControler.lineHorizonCount - 1, index);
                return false;
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            // Update lại trạng thái cho phép update đường đi mới cho enemy
            enemies[i]._IsEnableUpdateWay = true;
        }
        return true;
    }
    /// <summary>
    /// Đặt tháp vào 1 vị trí chuột được click
    /// </summary>
    public void SetTower(int towerID)
    {
        //Lay towerID từ popup chọn loại tháp
        //Láy được cái Prefab (model) của loại tháp tương ứng
        Map map = _MapControler.GetMapByPossiton(hit.point);//Lấy vị trí tháp trên bản đồ
        string pathTowerID = "Assets/Assets/Prefabs/Tower" + towerID + ".prefab";
        UnityEngine.Object tower = Resources.LoadAssetAtPath(pathTowerID, typeof(GameObject));
        Vector3 pos = new Vector3(map.Position.x * _MapControler.partHorizonLeng + _MapControler.pointA0.x + _MapControler.partHorizonLeng / 2, _MapControler.HeightMap + 0.1f, map.Position.z * _MapControler.partVerticalLeng + _MapControler.pointA0.z + _MapControler.partVerticalLeng / 2);
        GameObject.Instantiate(tower, pos, Quaternion.identity);
    }
   
    /// <summary>
    /// Đặt tháp vào vị trí click chuột
    /// </summary>
    private void SetTower_Click()
    {
        //Dùng ở bên GuiManager khi click chuột vào loại tháp
        GameObject obj = GetGameObjByRaycat();//Trả về đối tượng được chọn
        if (obj.tag=="Map")
        {
            locatePosition();
            Map map = _MapControler.GetMapByPossiton(hit.point);//Lấy vị trí tháp trên bản đồ
            if (CheckSetTower(map.Index))//Nếu cho phép đặt tháp
            {
                _GuiManager.ShowPanelTowerSelect(true); 
            }
        }
    }
    /// <summary>
    /// Tạo 1 list Map clone
    /// </summary>
    private List<Map> CopyFromOtherList(List<Map> list)
    {
        List<Map> li = new List<Map>();
        for (int i = 0; i < list.Count; i++)
        {
            Map Map = new Map(list[i].Position, list[i].Index);
            Map.came_from = list[i].came_from;
            for (int j = 0; j < list[i].Next.Count; j++)
            {
                Map a = new Map();
                a = list[i].Next[j];
                Map.Next.Add(a);
            }
            Map.f = list[i].f;
            Map.g = list[i].g;
            Map.h = list[i].h;
            //Map Map = new Map(list[i].Position, list[i].Next, list[i].came_from, list[i].Name, list[i].g, list[i].h, list[i].f);
            li.Add(Map);
        }
        return li;
    }

    #endregion

}

