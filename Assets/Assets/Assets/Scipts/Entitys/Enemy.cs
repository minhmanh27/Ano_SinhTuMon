using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Assets.Scipts.Entitys
{
    /// <summary>
    /// Class này sẽ chứa các thông số quái vật load từ cơ sở dữ liệu
    /// </summary>
    class Enemy
    {
        //Chứa các thông số
        public int Loai { get; set; }//Loại quái
        public int Mau { get; set; }//Lượng máu quái là bao nhiêu
        //Và các thông số khác
    }
}
