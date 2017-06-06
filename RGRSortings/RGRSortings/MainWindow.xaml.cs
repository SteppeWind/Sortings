using RGRSortings.Grapgics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RGRSortings
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseContainer Container = null;//создаваемый контейнер
        Sorting Sorting = null;//сортировка

        BaseItem TempItem = null;//элемент для добавления в контейнер
        

        public MainWindow()
        {
            InitializeComponent();
            //int length = 20;
            //DrawingFrame(length);
            //DrawingMesh(length);
            //DrawingLables(length);
            //GraphImage.Source = new DrawingImage(drawingGroup);
        }

        //обработчик событие клика по кнопке добавить (плюсом обозначен)
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            Container.AddItem(TempItem);//добавляем пустой элемент
        }

        //обработчик событие клика по кнопке удалить (минусом обозначен)
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            //пробегаемся по выделенным item'мам в списке ItemsListView
            while (ItemsListView.SelectedItems.Count != 0)//пока кол-во выделенных элементов неравно нулю
            {
                Container.RemoveItem((BaseItem)ItemsListView.SelectedItem);//удаляем текущий выделенный элемент 
            }
        }

        //обработчик события кнопки CreateCODButton (Создать КОД)
        private void CreateCODButton_Click(object sender, RoutedEventArgs e)
        {
            int length = 1;//переменная, в которой будет храниться длина КОД

            SortResultListView.Items.Clear();//чистим SortResultListView
            ResultInfoCalculatingPanel.DataContext = null;//обнуляем результы предыдущей сортировки, если такая была

            if (int.TryParse(LengthCOD.Text, out length) && length > 0)//проверяем данные на валидность
            {
                if ((bool)IntRB.IsChecked)//если выбрали целые числа
                {
                    Container = new Container<IntItem>(length);//создаем целочисленный контейнер
                    TempItem = new IntItem();//в TempItem кладем целочисленное значение 
                }

                if ((bool)DoubleRB.IsChecked)//если выбрали вещественные числа
                {
                    Container = new Container<DoubleItem>(length);//создаем вещественный контейнер
                    TempItem = new DoubleItem();//в TempItem кладем вещественное значение 
                }

                if ((bool)CharRB.IsChecked)//если выбрали символы
                {
                    Container = new Container<CharItem>(length);//создаем символьный контейнер
                    TempItem = new CharItem();//в TempItem кладем символьное значение 
                }

                if ((bool)RandomCB.IsChecked)//если стоит галочка на заполнить рандомом
                {
                    Container.FillRandomValues();//то заполняем рандомом
                }
                //здесь в качестве исполняемого потока указываем текущий, т.к.
                //в этом окне мы можем изменить данные контейнера
                Container.ThisDispatcher = Dispatcher;

                ItemsListView.ItemsSource = Container.ListItems;//в качестве источника указываем ListItems
                CODSettingsBox.IsEnabled = true;//делаем панель для выбора сортировки активной
                ControlButtonsPanel.IsEnabled = true;//делаем панель для редактирования КОД активной
            }
            else
            {
                MessageBox.Show("Вы ввели некорректное значение");
            }
        }

        //обработчик события клика по кнопке SortingButton (Посмотреть результат)
        private async void SortingButton_Click(object sender, RoutedEventArgs e)
        {
            SortResultListView.Items.Clear();//чистим SortResultListView
            ResultInfoCalculatingPanel.DataContext = null;//обнуляем результы предыдущей сортировки, если такая была

            if ((bool)ShakerRB.IsChecked)//если выбрали шейкер
            {
                Sorting = new Shaker(Container);//то в Sorting кладем экземпляр Shaker
            }
            else
            {
                Sorting = new Insertion(Container);//то в Sorting кладем экземпляр Insertion
            }
            Sorting.OnCurrentItemsStateEvent += Sorting_OnCurrentItemsStateEvent;//подписываемся на событие на изменение КОД
            Sorting.OnSortingEndedEvent += Sorting_OnSortingEndedEvent;////подписываемся на событие завершения сортировки
            await Sorting.StartSoring();//выполняем запуск сортировки           
        }

        //обработчик события окончания сортировки
        private void Sorting_OnSortingEndedEvent(InfoCalculating obj)
        {
            Dispatcher.InvokeAsync(() =>
            {
                //не советую использовать большие объемы списка для сортировки!!!

                ResultInfoCalculatingPanel.DataContext = obj;//выводим результат сортировки в ResultInfoCalculatingPanel
                GraphicContainer graphicContainer = new GraphicContainer();//создаем контэйнер для графиков
                Graphic graphic1 = new Graphic(Container.Length);//этот график в итоге будет представлять из себя прямую
                //еще один пример использования params, здесь мы указываем, 
                graphic1.AddRangeXCoordinates(0, obj.CountElements);//следовательно здесь мы переадаем 2 точки по оси Х
                graphic1.AddRangeYCoordinates(0, obj.TimeSorting);//а здесь по оси У, чтобы построить прямую, необходимо 2 точки минимум

                ParabolaGraphic parabola = new ParabolaGraphic(Container.Length);//создаем параболу
                parabola.Fill();//заполняем значениями
                graphicContainer.ListGraphics.Add(graphic1);//добавялем в список созданный график

                graphicContainer.ListGraphics.Add(parabola);//добавляем прямую
                GraphImage.Source = graphicContainer.DrawingGraphic();//источнику указываем результат отрисовки графика
            });          
        }

        private void Sorting_OnCurrentItemsStateEvent()
        {
            Dispatcher.InvokeAsync(() =>
            {
                SortResultListView.Items.Add(Sorting.Container.ToString());//здесь в SortResultListView добавляем текущее состояние контейнера 
            });
        }

        /*
         * В этом окне результат сортировки по времени будет заметно больше,
         * потому что здесь мы явно подписываемся на событие об изменении КОД.
         * На то, чтобы выполнить код в обработчике любого события требуется какое-то время,
         * из-за этого время сортировки больше, чем в результате сессии тестов, т.к.
         * там мы не подписваемся на это событие
         */

        //обработчик события SessionStartButton (Выполнить сессию)
        private void SessionStartButton_Click(object sender, RoutedEventArgs e)
        {
            new SessionWindow().ShowDialog();//создаем экземляр класса SessionWindow и сразу вызыавем метод ShowDialog (открывает окно)
        }
    }
}