using Assets.Assets.Scipts.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    /// <summary>
    /// Chứa các phương thức điều khiển skin
    /// </summary>
class SkinControler : MonoBehaviour
{
    public Skin _SKin = null;//Lưu trữ các thông số của loại skin này và được load từ cơ sở dữ liệu
    /// <summary>
    /// Sử dụng tác động vào con quái nào đó, trong một phạm vi nhất đinh
    /// </summary>
    private void HitEnemy(Vector3 pos)
    {
        //Ảnh hưởng: tăng,thay đổi thông số của quân địch trong phạm vi nào đó 
        //Dùng ở đâu: khi kéo skin vào quân địch
        //Phụ thuộc: Thời gian hồi phục skin,giá
    }
    /// <summary>
    /// Hỗ trợ tháp tăng thông số của tháp
    /// </summary>
    private void SupportTower(GameObject tower)
    {
        //Ảnh hưởng: tăng,thay đổi thông số của tháp 
        //Dùng ở đâu: khi kéo skin vào tháp
        //Phụ thuộc: Thời gian hồi phục skin,giá
    }
}

