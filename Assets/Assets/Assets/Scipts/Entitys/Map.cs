using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Assets.Scipts.Entitys
{
    /// <summary>
    /// Phần tử ma trận trọng số 
    /// </summary>
    public class Map
    {
        //Chứa các thông số từng phần tử của map trọng số
        public Vector3 Position;//Vị trí trong ma trận
        public List<Map> Next = new List<Map>();//Chứa list các phần tử xung quanh mà nó có thể đi đến được
        public Map came_from = null;//dinh di toi
        public int Index = 0;////Chỉ số của của ô ma trận này, chỉ số trong file text
        public int Value = 0;//Gía trị 0:ko có gì 1: Vật cản 2: Có đặt tháp
        public double g = 0;//gia thanh duong di dinh ban dau den n
        public double h = 0;//gia thanh uoc luong den dich
        public double f = 0;//gia thanh uoc luong
        public void MakeLink(ref Map Map)
        {
            this.Next.Add(Map);
            Map.Next.Add(this);
        }
        /// <summary>
        /// Khoảng cách từ 1 điểm đến 1 điểm khác
        /// </summary>
        public double Distance(Map Map)
        {
            double a = (Map.Position.x - this.Position.x) * (Map.Position.x - this.Position.x) + (Map.Position.z - this.Position.z) * (Map.Position.z - this.Position.z);
            return Math.Sqrt(a);
        }

        public Map() { }
        public Map(Vector3 pos, List<Map> next, Map came_from, int index, double g, double h, double f)
        {
            this.Position = pos;
            this.Next = next;
            this.came_from = came_from;
            this.Index = index;
            this.g = g;
            this.h = h;
            this.f = f;
        }
        public Map(Vector3 pos, int index)
        {
            this.Position = pos;
            Next.Clear();
            this.came_from = null;
            this.Index = index;
        }
    }
}
