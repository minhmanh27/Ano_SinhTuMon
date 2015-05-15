using UnityEngine;
using System.Collections;
using Assets.Assets.Scipts.Entitys;
using System.Collections.Generic;
using Assets.Assets.Scipts.Managers;
using System;

public class EnemyControler : MonoBehaviour {

    /// <summary>
    /// Quản lý các phương thức liên quan đến sự di chuyển của quái vật
    /// </summary>
	// Use this for initialization

    private MapControler _MapControler = null;
    private ButletControler[] _ButletControler = null;
    public Map _MapNext;//Vị trí tạm thời mà Enemy chuẩn bị đến
    public float _Speed = 5f;//Toc do di chuyen cua quai vat
    public int _PosCurrent=0;//Vị trí hiện tại của quái vật
    public Map _MapFinish;//Vị trí đích mà qoái vật cần di chuyển đến
    public bool _IsEnableUpdateWay = false;// Trạng thái có cho phép update lại đường đi mới (khi đặt tháp xuống hoặc hủy bỏ tháp đi)
    public float _Blood = 100;//Blood: Máu của quái vật

    bool IsFinish = false;//Trạng thái quái vật đã về đích chưa?
    Stack<Map> _StackWay = null;//Stack lưu trữ đường đi của quái vật

    private bool checkNextMap = true;
    void Start()
    {
        _MapControler = GameObject.FindObjectOfType<MapControler>();
        if (_MapControler==null)
        {
            return;
        }
        Map _MapCurrent = _MapControler._ListMapElement[0];//Lấy phần tử đầu tiên của list trọng số đường đi
        _MapFinish = _MapControler._ListMapElement[_MapControler._ListMapElement.Count-1];//Lấy phần tử cuôi cùng của list trọng số đường đi
        //Di chuyển quái vật theo vị trí trong Stack
        _StackWay = MapManager.FindWay(_MapCurrent, _MapFinish);
    }
    void Update()
    {
        if (CheckEnemyDead())
        {
            SetPositionDead();
            GameObject.Destroy(gameObject);
            return;
        }
        if (checkNextMap)
        {
            if (_IsEnableUpdateWay)
            {
                _PosCurrent = _MapControler.GetIndexMap(this.transform.position);//Trả về vị trí index hiện tại trong matran chi so
                _StackWay = MapManager.FindWay(_MapControler._ListMapElement[_PosCurrent], _MapFinish);
                if (_StackWay == null) return;
                _IsEnableUpdateWay = false;
            }
            _MapNext = _StackWay.Pop();

            checkNextMap = false;
        }
        //Di chuyển đến vị trí Map
        Vector3 posDes = _MapControler.GetPossition(_MapNext.Position.x, _MapControler.HeightMap, _MapNext.Position.z);
        Move(posDes, _Speed);
        if (Math.Round(posDes.x, 1) == Math.Round(transform.position.x, 1) && Math.Round(posDes.z, 1) == Math.Round(transform.position.z, 1))
        {
            checkNextMap = true;
            if (_StackWay.Count == 0)
                IsFinish = true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private bool CheckEnemyDead()
    {
        if (_StackWay == null || IsFinish)
        {
            return true;
        }
        else
        {
            if (_Blood<=0)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Set vị trí chết cho các viên đạn đang bắn con quái này
    /// </summary>
    private void SetPositionDead()
    {
        _ButletControler = GameObject.FindObjectsOfType<ButletControler>();
        for (int i = 0; i < _ButletControler.Length; i++)
        {
            if (_ButletControler[i].Enemy == gameObject)
            {
                _ButletControler[i].PositionEnemyDead = gameObject.transform.position;
            }
        }
    }
    /// <summary>
    /// Giúp quái di chuyển giữa 2 ô ma trận 
    /// </summary>
    public void Move(Vector3 posDes, float Speed)
    {
        //Di chuyển từ vị trí hiện tại đến vị trí đích posDes với vận tốc Speed
        //Được gọi ở đâu
        //Phụ thuộc vào cái gì:Danh sách đường đi riêng của nó,Khi đặt tháp,Đường đi của quái khác
        //-==================
        transform.position = Vector3.MoveTowards(transform.position, posDes, Time.deltaTime * Speed);
    }
}
