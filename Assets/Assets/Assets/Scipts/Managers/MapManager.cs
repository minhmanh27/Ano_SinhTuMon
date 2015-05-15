using Assets.Assets.Scipts.Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Assets.Scipts.Managers
{
    /// <summary>
    /// Chứa các phương thức quản lý Map
    /// </summary>
    class MapManager
    {
        /// <summary>
        /// Khởi tạo ma trận map ban đầu là những đường line chưa trọng số, chưa vật cản 
        /// </summary>
        private static void InitMap(int lineVer, int lineHor)
        {
            //Khởi tạo map là các đường kẻ dọc và ngang dựa vào số dòng theo chiều dọc lineVer,dòng theo chiều ngang lineHor
            //Sử dụng ở đâu: Tự khởi tạo
        }
       
        //public int NumRow = 13;//Chi so dong cua ma tran duong di
        //public int NumCol = 13;//Chi số cột của ma trận đường đi

        public static int ReadDataFromFile(string path, ref int[,] _Array,out int[][] _ArrObstruc,out int _Row)
        {
            int _numSize = 0;
            int[][] _arrobstruction = new int[0][]; int _row=0; 
            StreamReader sr = new StreamReader(path);
            string input = sr.ReadToEnd();
            sr.Close();
            string[] arr = input.Split('\n');
            int indexLine = 0;
            foreach (var line in arr)
            {
                if (line.Trim().Length == 0) continue;
                //169#0:4:5:6:17:18:19:30:31:32:43:44:45:56:57:58:69:70:71:82:83:84:95:96:97#1:13:26:39:52:65:78:91:104//Vi tri dau la do dai mang ma tran # tiêp theo là các vị trí mảng đặt vật cản 
                //Chỉ số đầu của mảng vật cản là: loại vạt cản: các chỉ số sau là vị trí vật cản
                if (line==arr[0])
                {
                    string[] arrObj = line.Split('#');
                    _numSize = Convert.ToInt32(arrObj[0]);
                    _Array = new int[_numSize, _numSize];
                   
                    //====================================
                    //Trả về mảng vật cản
                    _arrobstruction=new int[arrObj.Length-1][];
                    _row = arrObj.Length - 1;
                    for (int i = 1; i < arrObj.Length; i++)
                    {
                        string[] arrObstruc = arrObj[i].Split(':');
                        _arrobstruction[i - 1] = new int[arrObstruc.Length];
                        for (int j = 0; j < arrObstruc.Length; j++)
                        {
                            _arrobstruction[i - 1][j] =int.Parse(arrObstruc[j]);
                        }
                    }
                    continue;
                }
                string[] arr2 = line.Trim().Split('\t');
                int indexCol = 0;
                foreach (var item in arr2)
                {
                    if (item.Trim().Length == 0) continue;
                    _Array[indexLine, indexCol] = Convert.ToInt32(item);
                    indexCol++;
                }
                indexLine++;
            }
            _ArrObstruc = _arrobstruction;
            _Row = _row;
            return _numSize;
        }
        /// <summary>
        /// Khỏi tạo list trọng số ban đầu được load lên từ file text gồm trọng số và vị trí các vật cản ban đầu
        /// </summary>
        public static List<Map> InitListWeight(string _path,out int[,] _ArrIndexObstruc)//vd:Assets/Datas/input.txt
        {
            //Trả về mảng nhiều dòng và có 2 cột : cột đầu loại vật cản: côt 2 là trả về vị trí là chỉ số của ô map trong danh sách từ 1->169
            string path = Application.dataPath + "\\"+_path;
            int[,] _Array = new int[0, 0];
            int[][] _ArrayObstruction;int _Row;
            int numSize = ReadDataFromFile(path, ref _Array,out _ArrayObstruction,out _Row);
            //=========
            int[,] arr = new int[_Row, 2];
            for (int i = 0; i < _Row; i++)
            {
                arr[i, 0] = _ArrayObstruction[i][0];
                arr[i, 1] = _ArrayObstruction[i][1];
            }
            _ArrIndexObstruc = arr;


            List<Map> list= GetListMap(_Array, numSize);
            //========Xóa vị trí có vật cản
            for (int i = 0; i < _Row; i++)
            {
                int col = _ArrayObstruction[i].Length;//Lấy Length từng dòng
                for (int j = 1; j < col; j++)//Vị trí đầu 0 là loại vật cản do đó bắt đầu duyệt từ  1
                {
                    RemoveMapElement(list, _ArrayObstruction[i][j],1);
                }
            }

            return list;
        }
        public static List<Map> GetListMap(int[,] _Array, int _numSize)
        {
            List<Map> lstMap = new List<Map>();
            for (int i = 0; i < Math.Sqrt(_numSize); i++)
            {
                for (int j = 0; j < Math.Sqrt(_numSize); j++)
                {
                    Map Map = new Map(new Vector3(i, 0, j), (i * (int)Math.Sqrt(_numSize)) + j);
                    lstMap.Add(Map);
                }
            }
            for (int i = 0; i < lstMap.Count; i++)
            {
                for (int j = i + 1; j < lstMap.Count; j++)
                {
                    if (_Array[i, j] != 0)
                    {
                        Map MapTmp = lstMap[j];
                        lstMap[i].MakeLink(ref MapTmp);
                        lstMap[j] = MapTmp;
                    }
                }
            }
            return lstMap;
        }
        /// <summary>
        /// Tìm đường đi của quái từ vị trí bắt đầu đến vị trí kết thúc
        /// </summary>
        public static Stack<Map> FindWay(Map start, Map Target)
        {
            if (Target == null) return null;
            List<Map> Open = new List<Map>();
            List<Map> Close = new List<Map>();

            //make open and close is empty
            Open.Clear();
            Close.Clear();
            //calculate h,g,f for start
            start.g = 0;//distance between current and start
            start.h = start.Distance(Target);
            start.f = start.h + start.g;

            //add start to open list
            Open.Add(start);
            while (Open.Count != 0)
            {
                //Find Map in open list have min f
                Map current = Open[0];
                foreach (Map i_Map in Open)
                {
                    if (i_Map.f < current.f)
                        current = i_Map;
                }
                //Remove current from open list
                Open.Remove(current);
                //Add curent to close list
                Close.Add(current);
                if (current == Target)//If current is target then return path
                {
                    //Return path
                    Open.Clear();
                    Close.Clear();
                    return ReconstructPath(start, Target);

                }
                else
                {
                    //with next Map in current.next
                    foreach (Map i_Map in current.Next)
                    {
                        if (Close.Contains(i_Map))
                            continue;//if Map in close will do nothing

                        double tmp_current_g = current.g + current.Distance(i_Map);
                        if (!Open.Contains(i_Map) || tmp_current_g < i_Map.g)
                        {
                            i_Map.came_from = current;
                            i_Map.g = tmp_current_g;
                            i_Map.h = i_Map.Distance(Target);
                            i_Map.f = i_Map.g + i_Map.h;
                            if (!Open.Contains(i_Map))
                                Open.Add(i_Map);
                        }
                    }
                }
            }
            return null;
        }

        private static Stack<Map> ReconstructPath(Map s, Map t)
        {
            Stack<Map> path = new Stack<Map>();
            path.Clear();
            Map tmp = t;
            while (tmp != null)
            {
                path.Push(tmp);
                Map c_f = tmp;
                tmp = tmp.came_from;
                c_f.came_from = null;

            }
            return path;
        }
        /// <summary>
        /// Xóa một vị trí liên kết trong danh sách  _ListMapElement và trả về danh sách trọng số mới
        /// </summary>
        public static List<Map> RemoveMapElement(List<Map> list, int index, int value)
        {
            //Ảnh hương: Hình thành đường đi mới của từng quái vật
            //Phụ thuộc: List _ListMapElement 
            //Dùng khi nào: Ngay trong class,Xóa thử nếu tìm được đường đi mới thì cho đặt tháp
            //Dùng ở dâu: Trong class TowerManager
            for (int i = 0; i < list.Count; i++)
            {
                if (i == index){
                    list[i].Next.Clear();
                    list[i].Value = value;
                }
                else
                {
                    Map Map = list[i].Next.Find(item => item.Index == index);
                    if (Map != null)
                        list[i].Next.Remove(Map);
                }
            }
            return list;
        }
        /// <summary>
        /// Thêm một vị trí liên kết trong danh sách  _ListMapElement và trả về danh sách trọng số mới
        /// </summary>
        public static List<Map> AddMapElement(List<Map> lstWeight, int size, int index)
        {
            //Ảnh hương: Hình thành đường đi mới của từng quái vật
            //Phụ thuộc: List _ListMapElement 
            //Dùng khi nào: Ngay trong class,Khi xóa một vị trí RemoveMapElement mà ko tìm thấy List đường đi mới 
            //tức trường hợp bị chặn hết đường đi thì thêm lại điểm đó hoặc trường hợp bán đi một tháp tại vị trí nào đó
            //Dùng ở dâu: Trong class TowerManager
            //===========================================================================
            //Add lại theo chỉ số của phần tử trong danh sách
            //index+1,index-1,index+size,index-size
            //4 vị trí xung quanh phải không có tháp và không có vật cản
            lstWeight[index].Value = 0;//Add lại thành không có vật cản
            RemoveMapElement(lstWeight, index, 0);//Xóa đi trước khi add lại
            if (index / size != 0)
            {
                Map MapTmp = lstWeight[index - size];
                if (MapTmp.Value== 0)//Có vạt cản hoặc có tháp
                {
                    lstWeight[index].MakeLink(ref MapTmp);
                    lstWeight[index - size] = MapTmp;
                }
            }
            if (index / size != size - 1)
            {
                Map MapTmp = lstWeight[index + size];
                if (MapTmp.Value== 0)//Có vạt cản hoặc có tháp
                {
                    lstWeight[index].MakeLink(ref MapTmp);
                    lstWeight[index + size] = MapTmp;
                }
            }
            if (index % size != 0)
            {
                Map MapTmp = lstWeight[index - 1];
                if (MapTmp.Value == 0)//Có vạt cản hoặc có tháp
                {
                    lstWeight[index].MakeLink(ref MapTmp);
                    lstWeight[index - 1] = MapTmp;
                }
            }
            if (index % size != size - 1)
            {
                Map MapTmp = lstWeight[index + 1];
                if (MapTmp.Value == 0)//Có vạt cản hoặc có tháp
                {
                    lstWeight[index].MakeLink(ref MapTmp);
                    lstWeight[index + 1] = MapTmp;
                }
            }
            return lstWeight;
        }
    }
}
