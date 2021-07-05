using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_02___Vorobev_31P
{

    class Program
    {
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("log.txt")));
            Debug.AutoFlush = true;
            crit Cp = new crit();
            Cp.solution();
        }
    }


    /// <summary>
    /// Класс решения задачи
    /// </summary>
    public class crit
    {
        public int max, maxind;
        string str = "";
        public List<List<Path>> result = new List<List<Path>>();//лист функций и путей
        public List<Path> Lway; //лист путей
        public List<Path> Ldata = FileRead();//лист исходных данных 

        /// <summary>
        /// Структура записи пути
        /// </summary>
        public struct Path
        {
            public int point1;
            public int point2;
            public int length;


            public override string ToString()
            {
                return point1.ToString() + " - " + point2.ToString() + " " + length.ToString();
            }
        }


        /// <summary>
        /// Метод вытягивания исходных данных в лист исходных данных
        /// </summary>
        public void PullData()
        {
            try
            {
                Lway = Ldata.FindAll(x => x.point1 == Ldata[first(Ldata)].point1);//запись точки начала в лист путей
                foreach (Path rb in Lway) //построение путей из начальных возможных перемещений
                {
                    LengthP1xP2(Ldata, rb);
                    result.Add(FindBranch(Ldata, str));
                    str = "";
                }
            }
            catch
            {
                Console.WriteLine("Данные неккоректны.\n Введите правильно");
            }
        }

        /// <summary>
        /// Чтение данных
        /// </summary>
        /// <returns></returns>
        public static List<Path> FileRead()
        {
            List<Path> ret = new List<Path>();
            using (StreamReader sr = new StreamReader("Ввод.csv"))
            {
                while (sr.EndOfStream != true)
                {
                    string[] str1 = sr.ReadLine().Split(';');
                    string[] str2 = str1[0].Split('-');
                    ret.Add(new Path { point1 = Convert.ToInt32(str2[0]), point2 = Convert.ToInt32(str2[1]), length = Convert.ToInt32(str1[1]) });
                }
            }
            return ret;
        }


        /// <summary>
        /// Метод поиска начало пути
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int first(List<Path> ls)
        {
            int min = ls[0].point1, minind = 0;
            foreach (Path rb in ls)
            {
                if (rb.point1 <= min)
                {
                    min = rb.point1;
                    minind = ls.IndexOf(rb);
                }
            }
            return minind;
        }

        /// <summary>
        /// Метод определения конца пути
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int last(List<Path> ls)
        {
            int min = ls[0].point2, maxind = 0;
            foreach (Path rb in ls)
            {
                if (rb.point2 >= min)
                {
                    min = rb.point1;
                    maxind = ls.IndexOf(rb);
                }
            }
            return maxind;
        }

        /// <summary>
        /// Подсчет времени перемещения из одной точки в другую
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="minel"></param>
        /// <returns></returns>
        int LengthP1xP2(List<Path> ls, Path minel)
        {
            int ret = 0;
            Path rb = ls.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);//Варианты передвижения
            str += rb.point1.ToString() + "-" + rb.point2.ToString();//Передвижение
            if (rb.point2 == ls[last(ls)].point2)//Конец пути или нет
            {
                str += ";";
                return rb.length;
            }
            else
            {
                for (int i = 0; i < ls.Count; i++)//Время перемещения
                {
                    if (ls[i].point1 == rb.point2)
                    {
                        str += ",";
                        ret = LengthP1xP2(ls, ls[i]) + rb.length;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Метод поиска ветвлений 
        /// Строки > массив; Массив идет в начало ветки
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<Path> FindBranch(List<Path> ls, string s)
        {
            List<List<Path>> ret = new List<List<Path>>();
            string[] str1 = s.Split(';');
            foreach (string st1 in str1)
            {
                if (st1 != "")
                {
                    ret.Add(new List<Path>());
                    string[] str2 = st1.Split(',');
                    foreach (string st2 in str2)
                    {
                        if (st2 != "")
                        {
                            string[] str3 = st2.Split('-');
                            ret[ret.Count - 1].Add(ls.Find(x => x.point1 == Convert.ToInt32(str3[0]) && x.point2 == Convert.ToInt32(str3[1])));
                        }
                    }
                }
            }
            for (int i = 0; i < ret.Count; i++)
            {
                if (i > 0)
                {
                    if (ret[i][0].point1 != ret[i][ret[i].Count - 1].point2)
                    {
                        ret[i].InsertRange(0, ret[i - 1].FindAll(x => ret[i - 1].IndexOf(x) <= ret[i - 1].FindIndex(y => y.point2 == ret[i][0].point1)));
                    }
                }
            }
            int max = ret[0][0].length, maxind = 0;
            for (int i = 0; i < ret.Count; i++)
            {
                if (LengthPath(ret[i]) >= max)
                {
                    max = LengthPath(ret[i]);
                    maxind = i;
                }
            }
            return ret[maxind];
        }


        /// <summary>
        /// Метод подсчет длины пути
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int LengthPath(List<Path> ls)
        {
            int ret = 0;
            foreach (Path rb in ls)
            {
                ret += rb.length;
            }
            return ret;
        }

        /// <summary>
        /// Метод основного решения
        /// </summary>
        public void solution()
        {
            PullData();
            maxind = 0;
            max = result[0][0].length;
            for (int i = 0; i < Lway.Count; i++) // подсчет стоимости путей
            {
                if (LengthPath(result[i]) >= max) // выбор самого большого
                {
                    max = LengthPath(result[i]);
                    maxind = i;
                }
            }
            OutputData();
        }

        /// <summary>
        /// Вывод данных
        /// </summary>
        public void OutputData()
        {

            foreach (Path rb in result[maxind])
            {
                string s = (rb.point1 + " - " + rb.point2);
                Debug.WriteLine(s);
            }
            Debug.WriteLine(max);


            // Запись в файл
            string date = DateTime.Now.ToString("dd-MMMM-yyyy-hh-mm");
            using (StreamWriter sw = new StreamWriter("Вывод " + date + ".csv", false, Encoding.Default, 10))
            {
                foreach (Path rb in result[maxind])
                {
                    string s = (rb.point1 + " - " + rb.point2);
                    sw.WriteLine(s);
                }
                sw.WriteLine(max);
            }
        }

    }
}
