using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings.Grapgics
{
    class ParabolaGraphic : Graphic
    {
        public ParabolaGraphic(int length) : base(length)
        {
        }

        public void Fill(double eps = 1)
        {
            for (int i = 0; i < Length; i++)
            {
                var x = i * eps;//считаем координату Х
                CoordinatesXList.Add(x);//добавляем полученную координату
                CoordinatesYList.Add(F(x));//считаем значение функции в этой точке и добавляем его
            }
        }

        private double F(double x)//функция параболы
        {
            return x * x;
        }
    }
}