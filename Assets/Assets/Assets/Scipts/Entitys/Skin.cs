using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Assets.Scipts.Entitys
{
    public class Skin
    {
        //Buff skin cho thap
        //Buff skin cho quái
        //Ap skin vào một vị trí trên map 
        public int SkinID { get; set; }//Loại skin
        public string Name { get; set; }//Ten loại skin
        public string Note { get; set; }//Ghi chú tác dụng của loại skin này
        public int Value { get; set; }//Giá trị dame
        public float Time { get; set; }//Thời gian tồn tại
        public int Price { get; set; }//Giá loại skin
        public float Circle { get; set; }//Phạm vi tác dụng
        public string LinkIMG { get; set; }//Link chứa hình ảnh hiển thị
        public string Help { get; set; }//Hướng dẫn sử dụng
    }
}
