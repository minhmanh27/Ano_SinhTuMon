using Assets.Assets.Scipts.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Assets.Scipts.Managers
{
    /// <summary>
    /// Chứa các phương thức quản lý skin
    /// </summary>
    class SkinManager
    {
        /// <summary>
        /// Trả về list skin để load lên danh sách skin của nhân vật trên giao diện 2D
        /// </summary>
        /// <returns></returns>
        public static List<Skin> LoadListSkin()
        {
            //Load từ cơ sở dữ liệu
            List<Skin> list = new List<Skin>();
            return list;
        }
        /// <summary>
        /// Dùng nâng cấp loại Skin nào đó
        /// </summary>
        public static void UpgradeSkin(int skinID, float exp, int level, int anoCoin)
        {
            //Nâng cấp loại skin
            //Phụ thuộc: Loại,điểm exp, level nhân vật
            //Ảnh hưởng: Thông số skin,hiệu ứng,
            //Sử dụng ở đâu: Ngoài màn chơi trong vùng quản lý skin
            //Khi nào: Ấn vào nut nâng cấp
        }
        /// <summary>
        /// Mở khóa một skin
        /// </summary>
        public static void UnlockSkin(int skinID, int anoCoin, int level)
        {
            //Ảnh hưởng: Thêm một loại skin vào danh sách skin của tài khoản
            //Sử dụng ở đâu: Ngoài màn chơi trong vùng quản lý skin
            //Khi nào: Ấn vào nut unlock
        }
    }
}
