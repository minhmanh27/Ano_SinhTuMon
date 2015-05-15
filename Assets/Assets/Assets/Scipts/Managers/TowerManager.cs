using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Assets.Scipts.Entitys;
using UnityEngine;

namespace Assets.Assets.Scipts.Managers
{
    /// <summary>
    /// Chứa các phương thức quản lý Tower
    /// </summary>
    public class TowerManager
    {
        /// <summary>
        /// Dùng để kiểm tra xem một điểm nào đo có đặt được tháp hay ko
        /// </summary>
        public static bool CheckInitTower(int index, List<Map> _ListMapElement)
        {
            //Đặt được khi không chặn hết các đường đi, và ko ô đó ko có vật cản
            //Khi chúng ta đặt tháp thì chúng ta sẽ tìm danh sách đường đi lại cho tất cả các con quái đang có mặt trên bản đồ, 
            //sau đó duyệt ds các đường đi này xem nếu có một đường đi trả về null thì ko cho phép đặt tháp (return false)
          //  MapManager.RemoveMapElement(_ListMapElement, 2);
            //Nếu không tìm thấy đường đi thì add lại vị trí
         //   MapManager.AddMapElement(_ListMapElement, 2);
            return true;
        }
        /// <summary>
        /// Đặt một tháp xuống vị trí pos cần thiết trên bản đồ
        /// </summary>
        public static void InitTower(Vector3 pos, int id)
        {
            //Đặt một tháp xuống vị trí pos cần thiết trên bản đồ, loại tháp (id) được đặt
            //Sử dụng ở đâu:Bên lớp bản đồ MapControl khi click vào vị trí nào đó trên bản đồ
            //Phụ thuộc vào cái gì:Có chặn hết đường đi quái vật hay ko, fai là vị trí ko có vật cản,
            //Ảnh hưởng đến cái gì: Đường đi của quái,
        }
        /// <summary>
        /// Nâng cấp tháp 
        /// </summary>
        public static void UpgradeTower(int towerID)
        {
            //Khi click vào một cái tháp nào đó trên bản đồ hiện lên cho chúng ta lựa chọn nâng cấp hoặc xóa tháp
            //Phụ thuộc: Cần đủ tiền nâng cấp
            //Ảnh hưởng: tất cả các tháp cùng loại sẽ được nâng lên,hay chỉ một vị trí được nâng lên?
            //Thay đổi thông số
            //Sử dụng ở đâu: Khi ấn vào nút nâng cấp tháp 
        }
      
    }
}
