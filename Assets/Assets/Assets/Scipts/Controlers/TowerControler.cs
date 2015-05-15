using Assets.Assets.Scipts.Entitys;
using Assets.Assets.Scipts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TowerControler : MonoBehaviour {
    /// <summary>
    /// Chứa các phương thức điều khiển hoạt động của từng tháp
    /// </summary>
    public GameObject Butlet = null;//Loại đạn mà tháp này sẽ khởi tạo (bắn ra)
    public float Distance = 20;//Vùng phát hiện của tháp
    public float DotationDamping;//Tốc độ quay tháp
    private GameObject enemy = null;//Con quái vật mà tháp đang xác định bắn
 
    public float TimeSpeedBullet = 1.0f;//Thời gian chờ giữa 2 lần bắn quái
    private float timeAllowBullet = 0f;
    public bool _PopupVisible = false;//Kiểm tra trạng thái bật tắt popup của tháp
    MapControler _MapControler = null;
	void Start () {
        _MapControler = GameObject.FindObjectOfType<MapControler>();
        _PopupVisible = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (enemy == null)
        {
            enemy = GetEnemyNearest(Distance);
        }
        else
        {
            HitEnemyNearest();
        }
	}
  
    /// <summary>
    /// Tìm vị trí con quái gần nhất  có Tag=Enemy
    /// </summary>
    private GameObject GetEnemyNearest(float distance)
    {
        //Tìm kiếm danh sách các con quái hiện có và so sánh
        //Trả về con quái gần nhất trong bán kính distance
        float min = 1000;
        GameObject _obj = null;
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemy != null)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                if (Vector3.Distance(transform.position, enemy[i].transform.position) < min && Vector3.Distance(transform.position, enemy[i].transform.position) < distance)
                {
                    min = Vector3.Distance(transform.position, enemy[i].transform.position);
                    _obj = enemy[i];
                }
            }
        }
        return _obj;
    }
    /// <summary>
    /// Quay camera của tháp theo hướng viên đạn
    /// </summary>
    void lookAtPlayer(Transform enemy)
    {
        Quaternion rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DotationDamping);
    }
    private bool CheckOutOfDistance(GameObject enemy, float distance)
    {
        //Kiếm tra xem con quái có ra khỏi vùng phát hiện của tháp hay không
        if (Vector3.Distance(transform.position,enemy.transform.position)<distance)
        {
            return false; 
        }
        return true;
    }
    /// <summary>
    /// Bắn con quái gần nhất mà tháp xác định được
    /// </summary>
    private void HitEnemyNearest()
    {
        //bắn con quái gần nhất
        if (CheckOutOfDistance(enemy, Distance))
        {
            //Ra khỏi ngoài thì xác định lại con quái gần nhất
            enemy = GetEnemyNearest(Distance);
        }
        else
        {
            //Bắn con quái:Khởi tạo đạn và truyền doi tượng Enemy hiện tại cho viên đạn để nó thực hiên bắn
            lookAtPlayer(enemy.transform);
            if (timeAllowBullet <= TimeSpeedBullet)
            {
                timeAllowBullet += Time.deltaTime;
                return;
            }
            timeAllowBullet = 0;
            var objButlet = (GameObject)GameObject.Instantiate(Butlet, new Vector3(gameObject.transform.position.x, _MapControler.HeightMap, gameObject.transform.position.z), Quaternion.identity);
            var comButlet = objButlet.GetComponent<ButletControler>();
            comButlet.Enemy = enemy;
        }
    }
    /// <summary>
    /// Xoa thap
    /// </summary>
    public void DelTower()
    {
        Map map = _MapControler.GetMapByPossiton(gameObject.transform.position);//Lấy vị trí tháp trên bản đồ
        //===
        MapManager.AddMapElement(_MapControler._ListMapElement, _MapControler.lineHorizonCount - 1, map.Index);
        GameObject.Destroy(gameObject);
        //=======
        List<EnemyControler> enemies = FindObjectsOfType<EnemyControler>().ToList();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i]._IsEnableUpdateWay = true;//Cho quái tìm lại đường đi
        }
    }
}
