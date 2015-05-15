using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Assets.Scipts.Entitys
{
    class Butlet
    {
        //Dame cơ bản
        /// <summary>
        /// Skin buff vào tháp >> gán skin đó vào cái tháp 
        /// duyệt các thông số của tháp và xử lý
        /// +Tăng dame cho viên đạn 
        /// 
        /// Tác động đến quái khi đạn bắn vào quái (gán danh sách thuộc tính vào thằng quái)
        /// Quản lý các trạng thái trong đâu và như thế nào cho từng con quái
        /// Mỗi con quái khi bị bắn sẽ mang các trạng thái khác nhau (Controler)
        /// Sử dụng list các trạng thái và sau dó duyệt list để so sánh
        /// Các hàm xử lý cho từng trạng thái đặt ở đâu: EnemyManager (TaoHieuUng(gameObject,amThanh,HieuUng,giamToc) và HuyHieuUng)
        /// +Hiệu ứng, thời gian
        /// +Tê liệt, thời gian
        /// +Đóng băng, thời gian
        /// +Làm chậm ,thời gian
        /// 
        /// +Sát thương với thằng xung quanh //Hàm xử lý luôn vì nó chỉ sảy ra tại thời điểm va chạm
        /// 
        /// 
        /// 
        /// ==============================
        /// Skin buff vào quái
        /// Xử lý trên quái khi chạm tay vào quái
        /// 
        /// </summary>
        public float Crit { get; set; }
        
    }
}
