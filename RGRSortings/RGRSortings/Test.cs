using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RGRSortings
{
    //по заданию нужно протестировать несколько раз сортировки 
    //этот класс будет реализовывать один тест
    class Test 
    {
        public Shaker Shaker { get; private set; }//шейкер

        public Insertion Insertion { get; private set; }//вставки

        public int CountElements { get; private set; }//количество элементов в спсике

        public int TheoreticalTime { get { return CountElements * CountElements; } }//теоретическое время CountElements в квадрате (по заданию именно так)

        public InfoCalculating ShakerInfo { get; private set; }//информация о результате сортировки шейкером

        public InfoCalculating InsertionInfo { get; private set; }//информация о результате сортировки вставками

        public int NumberTest { get; private set; }//номер теста
        

        public Test(int countElements, int numberTest)//конструктор, в него передаем кол-во элементов списка и номер теста
        {
            NumberTest = numberTest;//сохраняем номер теста
            CountElements = countElements;//сохраняем кол-во элементов
            CreateTest();//создаем тест
         }


        private void CreateTest()
        {
            var container = new Container<IntItem>(CountElements);//создаем контейнер с указанным размером

            container.FillRandomValues();//заполняем рандомными значениями

            var container2 = new Container<IntItem>();//создаем 2ой контейнер

            foreach (var item in container.ListItems)//заполняем 2ой контейнер теми же значениями, что сгенерировал первый
            {
                container2.AddItem(item);
            }//из-за ссылочных типов приходится выполнять этот кусок кода, 
            //в методе StartTest происходит запуск сортировок, если бы передали в 2 сортировки один и тот же контейнер,
            //то при изменении первого изменится и второй контейнер, и вторая сортировка проверяла уже отсортированный контейнер

            Shaker = new Shaker(container);//создаем шейкер
            Insertion = new Insertion(container2);//создаем вставки
        }

        //запуск теста
        //async означает, что метод асинхронный, метод ничего не возращает,
        //но Task нужен, чтобы происходило ожидание выполнения этого метода
        public async Task StartTest()
        {
            ShakerInfo= await Shaker.StartSoring();
            InsertionInfo = await Insertion.StartSoring();
        }
    }
}