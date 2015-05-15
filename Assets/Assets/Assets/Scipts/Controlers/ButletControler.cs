using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    /// <summary>
    /// Chứa các phương thức điều khiển viên đạn
    /// </summary>
class ButletControler : MonoBehaviour
{
    public GameObject Enemy=null;//Con quái mà viên đạn này sẽ bắn
    public float MovementSpeed=20f, rotSpeed=20f;
    public  Vector3 PositionEnemyDead;//Vị trí quái chết truyền từ EnemyControler sang khi quái hết máu
    public float dameDoc = 0;//Dame độc
    public float dameVL = 0;//Dame vật lý
    private float dameAll = 0;//Lưu lại dame tổng cộng để trừ máu của địch
    // Use this for initialization
    void Start()
    {
        dameAll = dameDoc + dameVL; 
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            HitEnemy(Enemy);
        }
        catch //Ko tìm thấy đối tượng địch nào
        {
            GameObject.Destroy(gameObject);
        }
    }
    /// <summary>
    /// Bắn con quái 
    /// </summary>
    private void HitEnemy(GameObject enemy)
    {
        //Di chuyển đến vị trí của con quái
        Vector3 targetPosition;//Lấy vị trí con quái
        if (enemy == null)
        {
            targetPosition = PositionEnemyDead;
        }
        else
            targetPosition = enemy.transform.position;
        if (transform.position==targetPosition)
        {
            GameObject.Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, MovementSpeed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.tag == "Enemy")
        {
            //Trừ máu
            //Hủy đạn
            GameObject.Destroy(gameObject);
            enemy.gameObject.GetComponent<EnemyControler>()._Blood -= dameAll;
        }
    }
}
