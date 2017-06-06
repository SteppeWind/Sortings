using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RGRSortings
{
    class BaseItem
        : INotifyPropertyChanged//используется для уведомления клиентов, обычно привязки клиентов, что изменилось значение свойства.
                                //https://msdn.microsoft.com/ru-ru/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx
    {

        public BaseItem()//конструктор класса без параметров
        {
            Rnd = new Random();//создаем экземпляр класса Random
        }

        //protected - модификатор доступа, означает, что член класса доступен в классах наследниках 
        protected static Random Rnd { get; set; }

        /// <summary>
        /// Указывает на то, пустое ли значение.
        /// Вернет true, если value пуст, иначе false
        /// </summary>
        public bool IsEmpty { get; protected set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;//событие, которое возникает при изменении значения свойства

        protected void ChangeProperty([CallerMemberName] string name = "")//метод, который вызывает событие PropertyChanged
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //вопрос означает, что если PropertyChanged==null, то ничего происходить не будет, иначе вызываем метод Invoke
            //Invoke вызывает событие
        }


        public virtual void SetRandomValue()//виртуальный метод, нужен для переопределения в классах наследниках
        {
            //здесь будет реализована логика в классах наследниках
            //будет устанавливаться рандомное значение Value (свойство класса Item<T>)
        }
        

        public virtual void SetRandomValue(BaseItem maxValue)//виртуальный метод, нужен для переопределения в классах наследниках
        {
            //здесь будет реализована логика в классах наследниках
            //будет устанавливаться рандомное значение Value (свойство класса Item<T>)
            //с ограничением maxValue - до какого максимального значения гененрировать Value
        }

        public virtual bool IsMore(BaseItem item)//метод сравнения значений, также будет переопределен в классах наследниках 
        {
            return true;//по умолчанию true
        }
    }
}