using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Assets.Scipts.Entitys;
using Assets.Assets.Scipts.Managers;

public class GUIManager : MonoBehaviour {

    public bool _ShowPopup = false;//Đặt trạng thái đang show popup (panel) để ko click đc các sự kiện của lớp khác khi đang hiển thị panel
    public GameObject GameObjectSelect;//Truyền đối tượng được chọn tại thời điểm hiển thị panel ở lớp khác
    GameControler _GameControler = null;

    public GameObject TowerEditPanel;//Đối tượng panel nâng cấp và xóa tháp
    public Button[] btnTowerEditArr;//Danh sách button trong panel nâng cấp và xóa tháp

    public GameObject PanelTowerSelect;//Đối tượng panel nâng cấp và xóa tháp
    public Button[] btnTowerSelectArr;//Danh sách button trong panel nâng cấp và xóa tháp
	// Use this for initialization
	void Start () {
        _GameControler = FindObjectOfType<GameControler>();
        ShowEditTowerPanel(false);
        ShowPanelTowerSelect(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        #region=========Su kien cua ShowEditTowerPanel
        btnTowerEditArr[0].onClick.AddListener(() =>//Update thap
        {
            btnTowerEditArr[0].onClick.RemoveAllListeners();
            ShowEditTowerPanel(false);
        });

        btnTowerEditArr[1].onClick.AddListener(() =>//Xóa tháp
        {
            TowerControler towerControler = GameObjectSelect.GetComponent<TowerControler>();
            towerControler.DelTower();
            btnTowerEditArr[1].onClick.RemoveAllListeners();
            ShowEditTowerPanel(false);
        });

        btnTowerEditArr[2].onClick.AddListener(() =>//Close panel
        {
            ShowEditTowerPanel(false);
            btnTowerEditArr[2].onClick.RemoveAllListeners();
        });
        #endregion==========
        #region======Sukien chon thap========
        //Thành viên sẽ phải luu đc ds loại tháp hiện tại đang có trong csdl
        //Load danh sách thap hien tại đang có vào đây cái panel này
        //Lưu lại đc vị trí đặt tháp khi click chuột

        //btnTowerSelectArr[0].onClick.AddListener(() => {
        //    _GameControler.SetTower(1);
        //    btnTowerSelectArr[0].onClick.RemoveAllListeners();
        //    ShowPanelTowerSelect(false);
        //});

        //btnTowerSelectArr[1].onClick.AddListener(() =>
        //{
        //    btnTowerSelectArr[1].onClick.RemoveAllListeners();
        //    _GameControler.SetTower(2);
        //    ShowPanelTowerSelect(false);
        //});
        //btnTowerSelectArr[2].onClick.AddListener(() =>
        //{
        //    _GameControler.SetTower(3);
        //    ShowPanelTowerSelect(false);
        //    btnTowerSelectArr[2].onClick.RemoveAllListeners();
        //});
        btnTowerSelectArr[3].onClick.AddListener(() =>
        {
            _GameControler.SetTower(4);
            ShowPanelTowerSelect(false);
            btnTowerSelectArr[3].onClick.RemoveAllListeners();
        });
        btnTowerSelectArr[4].onClick.AddListener(() =>
        {
            _GameControler.SetTower(5);
            ShowPanelTowerSelect(false);
        });
        btnTowerSelectArr[5].onClick.AddListener(() =>
        {
            _GameControler.SetTower(6);
            ShowPanelTowerSelect(false);
        });

        btnTowerSelectArr[6].onClick.AddListener(() =>
        {
            ShowPanelTowerSelect(false);
        });
        #endregion
    }

    public void InitTower1()
    {
        _GameControler.SetTower(1);
        ShowPanelTowerSelect(false);
    }
    public void InitTower2()
    {
        _GameControler.SetTower(2);
        ShowPanelTowerSelect(false);
    }
    public void InitTower3()
    {
        _GameControler.SetTower(3);
        ShowPanelTowerSelect(false);
    }
    /// <summary>
    /// Hieenr thi và ẩn panel nâng cấp và xóa tháp
    /// </summary>
    public void ShowEditTowerPanel(bool _isShow)
    {
        TowerEditPanel.SetActive(_isShow);
        _ShowPopup = _isShow;
    }
    /// <summary>
    /// Hieenr thi và ẩn panel chọn tháp
    /// </summary>
    public void ShowPanelTowerSelect(bool _isShow)
    {
        PanelTowerSelect.SetActive(_isShow);
        _ShowPopup = _isShow;
    }
    /// <summary>
    /// Thực hiện lấy list skin từ ManagerSkin và sau đó điều hướng gán giá trị của Skin tương ứng vào Controler Skin của nó 
    /// </summary>
    public void SetListSkin()
    {
        List<Skin> listSkin = SkinManager.LoadListSkin();
        //gán SkinControler cho từng skin 
        //Gán giá trị tương ứng của skin vào biến skin trong SkinControler
    }
    
}
