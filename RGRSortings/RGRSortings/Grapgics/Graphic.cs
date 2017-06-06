using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings.Grapgics
{
    class Graphic
    {
        /// <summary>
        /// Список точек по оси Х
        /// </summary>
        public List<double> CoordinatesXList { get; set; }

        /// <summary>
        /// Список точек по оси У
        /// </summary>
        public List<double> CoordinatesYList { get; set; }

        /// <summary>
        /// Количество координат (X, Y)
        /// </summary>
        public int Length { get; private set; }
        
        public Graphic(int length)
        {
            if (length >= 0)
            {
                Length = length;
                Initialize();
            }
        }
        
        private void Initialize()
        {
            CoordinatesXList = new List<double>();
            CoordinatesYList = new List<double>();
        }

        //Возвращает максимальное значение в координатах У
        public double FindMaxYCoordinate
        {
            get
            {
                return FindMax(CoordinatesYList);
            }
        }

        //Возвращает максимальное значение в координатах Х
        public double FindMaxXCoordinate
        {
            get
            {
                return FindMax(CoordinatesXList);
            }
        }

        //добавляет сразу несколько координат в коллекцию CoordinatesXList
        public void AddRangeXCoordinates(params double[] coordinates)
        {
            //params означает что мы можем вызвать этот метод, к примеру, следующим образом AddRangeXCoordinates(0, 1, 2, 3);
            //без этого ключевого слова нам пришлось бы в вызове этого метода создавать отдельный массив
            CoordinatesXList.AddRange(coordinates);
        }

        //добавляет сразу несколько координат в коллекцию CoordinatesYList
        public void AddRangeYCoordinates(params double[] coordinates)
        {
            CoordinatesYList.AddRange(coordinates);
        }

        //ищет максимальный элемент списка
        private double FindMax(List<double> coordinates)
        {
            double result = 0;
            if (coordinates.Count != 0)//если список не пуст
                result = coordinates[0]; //result присваиваем первый элемент списка

            for (int i = 0; i < coordinates.Count; i++)//открываем цикл по элементам
            {
                if (coordinates[i] > result)//если текущий элемент списка больше result
                    result = coordinates[i];//то обновляем значение result и в в текущей итерации эта переменная хранит максимальное значение списка
            }

            return result;
        }
    }
}