using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RGRSortings.Grapgics
{
    class GraphicContainer
    {
        public DrawingGroup MainDrawingGroup { get; set; }

        public List<Graphic> ListGraphics { get; set; }

        /// <summary>
        /// Шаг сетки графика по горизонтали
        /// </summary>
        public int HorisontalMeshStep { get; } = 2;

        /// <summary>
        /// Шаг сетки графика по вертикали
        /// </summary>
        public int VerticalMeshStep { get; } = 20;

        public GraphicContainer()
        {
            MainDrawingGroup = new DrawingGroup();
            ListGraphics = new List<Graphic>();
        }

        private DrawingImage Drawing(List<Graphic> graphics)//метод, в который передаем список графиков для отрисовки
        {
            //DrawingImage - тип, используемый для указания источника контролу Image, его мы и будем вовзвращать
            //его конструктор принимает тип переменной Drawing, а используемый здесь тип DrawingGroup как раз и наследуется от него
            //следовательно мы можем передать этот объект в конструктор класса DrawingImage

            double maxYCoordinate = graphics[0].FindMaxYCoordinate;//здесь будет храниться максимальное значение по оси У
            double maxXCoordinate = graphics[0].FindMaxXCoordinate;//здесь будет храниться максимальное значение по оси X

            foreach (var item in graphics)//ищем максимальную У координату среди всех графиков
            {
                //аналогично методу FindMax в классе Graphic
                var currentMaxYCoordinate = item.FindMaxYCoordinate;
                if (currentMaxYCoordinate > maxYCoordinate)
                    maxYCoordinate = currentMaxYCoordinate;
            }

            foreach (var item in graphics)//ищем максимальную Х координату среди всех графиков
            {
                //аналогично методу FindMax в классе Graphic
                var currentMaxXCoordinate = item.FindMaxXCoordinate;
                if (currentMaxXCoordinate > maxYCoordinate)
                    maxXCoordinate = currentMaxXCoordinate;
            }
            
            DrawingMesh(maxXCoordinate, maxYCoordinate);//вызываем прорисовку сетки
            DrawingLables(maxXCoordinate, maxYCoordinate);//вызываем прорисовку надписей
            foreach (var item in graphics)//в цикле добавляем сами графики на общий рисунок
            {
                DrawingAnyGraphic(item.CoordinatesXList, item.CoordinatesYList, maxYCoordinate);
            }

            return new DrawingImage(MainDrawingGroup);
        }

        //метод DrawingGraphic имеет 2 перегрузки, первая с принимаемым параметром
        public DrawingImage DrawingGraphic(List<Graphic> graphics)
        {
            //здесь мы передаем конкретный список графиков для их отрисовки
            return Drawing(graphics);
        }

        //вторая без принимаемого параметра
        public DrawingImage DrawingGraphic()
        {
            //а здесь используем список, определнный в классе, то есть ListGraphics
            return Drawing(ListGraphics);
        }

        //сетка
        private void DrawingMesh(double maxX, double maxY, int minX = 0, int minY = 0)
        {
            DrawingGroup drGr = new DrawingGroup();

            //вертикальные полосы
            GeometryGroup geometryGroupX = new GeometryGroup();

            double X = 0;
            while (X <= maxX)//пока Х меньше maxX
            {
                LineGeometry line = new LineGeometry(
                new Point((maxY / maxX) * X, maxY),//начальная координата
                new Point((maxY / maxX) * X, minY));//конечная координата
                geometryGroupX.Children.Add(line);//добавили линию
                X += HorisontalMeshStep;//увеличиваем Х на VerticalMeshStep
            }

            GeometryDrawing geometryDrawingX = new GeometryDrawing();
            geometryDrawingX.Geometry = geometryGroupX;
            geometryDrawingX.Pen = new Pen(Brushes.Teal, 0.15);

            MainDrawingGroup.Children.Add(geometryDrawingX);

            //горизонтальные полосы
            GeometryGroup geometryGroupY = new GeometryGroup();
            double Y = maxY;
            while (Y >= 0)//идем с конца пока У больше либо равен 0
            {
                LineGeometry line = new LineGeometry(
                new Point(minX, Y),
                new Point(maxY, Y));
                geometryGroupY.Children.Add(line);//добавили линию
                Y -= VerticalMeshStep;//уменьшаем У на HorisontalMeshStep
            }
            GeometryDrawing geometryDrawingY = new GeometryDrawing();
            geometryDrawingY.Geometry = geometryGroupY;
            geometryDrawingY.Pen = new Pen(Brushes.Teal, 0.15);

            MainDrawingGroup.Children.Add(geometryDrawingY);//добавили в общий список новую геометрию
        }


        //надписи (числа слева и снизу осей коориднат)
        private void DrawingLables(double maxX, double maxY)
        {
            int j = 0;
            double sizeShrift = maxX;
            if (maxX == 0)
                sizeShrift = 8;

            GeometryGroup geometryGroupX = new GeometryGroup(); // вертикальные подписи
            for (double i = maxY; i >= 0; i -= 20)
            {
                FormattedText formattedText = new FormattedText(
                $"{j}",
                CultureInfo.InvariantCulture,
                FlowDirection.RightToLeft,//чтобы наши числа распологались справа налево
                new Typeface("Verdana"),//шрифт Verdana
                0.3 * sizeShrift,//размер числа
                Brushes.Black);//цвет черный

                formattedText.SetFontWeight(FontWeights.Bold);//выделение жирным 

                Geometry geometry = formattedText.BuildGeometry(new Point(-0.3, i - (maxX * 0.05)));//задаем отступ от оси координат
                geometryGroupX.Children.Add(geometry);
                j += 20;//увеличиваем на 20 (т.е. в итоге будет 0 20 40 и т.д.)
            }

            GeometryDrawing geometryDrawingX = new GeometryDrawing();
            geometryDrawingX.Geometry = geometryGroupX;//сохраняем созданную геометрию 

            geometryDrawingX.Brush = Brushes.Black;//фон черный
            geometryDrawingX.Pen = new Pen(Brushes.Black, 0.01);//контур тоже черный, толщина 0.01

            MainDrawingGroup.Children.Add(geometryDrawingX);//добавили рисунок в общий список

            GeometryGroup geometryGroupY = new GeometryGroup(); // горисонтальная разметка
            for (double i = 2; i <= maxX; i += 2)
            {
                FormattedText formattedText = new FormattedText(
                String.Format("{0}", i),
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                0.3 * sizeShrift,
                Brushes.Black);

                formattedText.SetFontWeight(FontWeights.Bold);

                //здесь уже нужно делать отступ снизу, поэтому координату У у объекта Point увеличиваем на 0,2 (maxY + 0.2)
                Geometry geometry = formattedText.BuildGeometry(new Point((maxY / maxX) * i - (maxX * 0.05), maxY + 0.2));
                geometryGroupX.Children.Add(geometry);
            }
            GeometryDrawing geometryDrawingY = new GeometryDrawing();
            geometryDrawingY.Geometry = geometryGroupY;

            geometryDrawingY.Brush = Brushes.Black;
            geometryDrawingY.Pen = new Pen(Brushes.Black, 0.01);
            MainDrawingGroup.Children.Add(geometryDrawingY);
        }

        private void DrawingAnyGraphic(List<double> dataX, List<double> dataY, double maxY)
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            for (int i = 0; i < dataX.Count - 1; i++)
            {
                //линия, соединяющая (Xi, Yi) с (X i+1, Y i+1)
                var x1 = (maxY / dataX[dataX.Count - 1]) * dataX[i];//высчитываем координату Х, в списке dataX последний элемент будет гарантированно максимальным
                var y1 = maxY - dataY[i];//берем координату maxY и вычитаем текущую по списку dataY (нужно вычитать, потому что график строится сверху вниз)
                var x2 = (maxY / dataX[dataX.Count - 1]) * dataX[i + 1];
                var y2 = maxY - dataY[i + 1];
                LineGeometry line = new LineGeometry(//строим линию из посчитанных значений
                new Point(x1, y1),
                new Point(x2, y2));
                geometryGroup.Children.Add(line);//добавили линию
            }
            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Geometry = geometryGroup;
            geometryDrawing.Pen = new Pen(Brushes.Red, dataX[dataX.Count - 1] * 0.1);//указываем толщину пера в этом месте (dataX[dataX.Count - 1] * 0.1)
            MainDrawingGroup.Children.Add(geometryDrawing);
        }
    }
}