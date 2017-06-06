using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RGRSortings
{
    //Контейнер для BaseItem
    abstract class BaseContainer
    {
        public Dispatcher ThisDispatcher { get; set; }//нужен для изменения значений в списке, дальше по коду будет объяснение более детально

        //ObservableCollection позволяет обновлять привязанные к контролам значения автоматически
        //например при удалении из ListView какого-нибудь значения просто удаляем элемент из этого списка и в ListView он удалится также
        public ObservableCollection<BaseItem> ListItems { get; protected set; }//сам список BaseItem'ов
        

        public BaseContainer()
        {
            ListItems = new ObservableCollection<BaseItem>();//создаем список
        }

        public BaseContainer(int length)
        {
            if (length >= 0)
            {
                ListItems = new ObservableCollection<BaseItem>();
                FillEmptysValue(length);
            }
        }

        //здесь будет хранится длина списка
        public int Length { get { return ListItems.Count; } }

        //заполняет пустыми значениями список определенной длины
        public abstract void FillEmptysValue(int length);

        //очищает список
        public void ClearItems()
        {
            ListItems.Clear();
        }

        //заполняем рандомными значениями список
        public virtual void FillRandomValues()
        {
            foreach (var item in ListItems)
            {
                item.SetRandomValue();
            }
        }

        /*
         * Индексаторы позволяют индексировать объекты и использовать их как массивы.
         * Фактически индексаторы позволяют нам создавать специальные хранилища объектов или коллекции
         * https://metanit.com/sharp/tutorial/4.10.php
         */
        public BaseItem this[int index]
        {
            get
            {
                return ListItems[index];
            }
            set
            {
                if (ThisDispatcher == null)
                {
                    ThisDispatcher = Dispatcher.CurrentDispatcher;//указыаем Dispatcher для текущего потока, если ThisDispatcher == null
                }
                ThisDispatcher.Invoke(() =>
                {
                    //в потоке с Dispatcher выполняем изменения указанного элемента списка
                    ListItems[index] = value;
                });             
            }
        }

        //заполняем рандомными значениями список с ограничением 
        public virtual void FillRandomValues(BaseItem maxValue)
        {
            foreach (var item in ListItems)
            {
                item.SetRandomValue(maxValue);
            }
        }
        
        //добавляет элемент в список
        public virtual void AddItem(BaseItem item)
        {
            ListItems.Add(item);
        }

        //возвращает true, если элемент удален, иначе false
        public virtual bool RemoveItem(BaseItem item)
        {
            return ListItems.Remove(item);
        }

        //Меняет местами 2 элемента списка, в параметрах указаны индексы элементов 
        public void Swap(int firstIndex, int secondIndex)
        {
            //классическая замена элементов
            BaseItem temp = this[firstIndex];//запоминаем значение элемента списка с индексом firstIndex
            this[firstIndex] = this[secondIndex];//элемента списка с индексом firstIndex заменяем на элемент списка с индексом secondIndex
            this[secondIndex] = temp;//элемента списка с индексом secondIndex заменяем на temp
        }

        //переопределяем метод ToString
        public override string ToString()
        {
            string result = string.Empty;
            //В цикле выводи все элементы в одну строку с отступом \t (5 пробелов)
            foreach (var item in ListItems)
            {
                result += $"{item}\t";
            }

            return result;
        }
    }
}