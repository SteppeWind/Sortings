using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings
{
    //абстрактный класс сортировки
    abstract class Sorting
    {
        //контейнер для элементов
        public BaseContainer Container { get; set; }

        //Stopwatch для замера времени
        public Stopwatch StopWatch { get; protected set; }

        //событие, которое уведомляет о текущем состоянии контейнера
        public event Action OnCurrentItemsStateEvent;
        //событие, которое уведомляет о завершении сортировки
        public event Action<InfoCalculating> OnSortingEndedEvent;

        //единственный конструктор, в который передаем контейнер 
        public Sorting(BaseContainer container)
        {
            Container = container;//сохраняем container
            StopWatch = new Stopwatch();
        }

        //здесь будет реализована сортировка в классах наследниках
        public abstract Task<InfoCalculating> StartSoring();

        //метод, который вызывает событие OnCurrentItemsStateEvent
        protected void OnCurrentStateItems()
        {
            OnCurrentItemsStateEvent?.Invoke();
        }

        //метод, который вызывает событие OnSortingEndedEvent, в параметрах указываем результат сортировки 
        protected void OnSortingEnded(InfoCalculating info)
        {
            OnSortingEndedEvent?.Invoke(info);
        }
    }
}