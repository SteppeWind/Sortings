using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings
{

    abstract class Item<T> : BaseItem//наследуем BaseItem, Т - это обобщенный тип, в него мы можем положить все что угодно
          where T : struct//но в данном случае только структур, здесь явно это указано 
    {

        public Item() : base() { }//эта штука позволяет вызывать сначала конструктор базовго класса, а уже затем этого

        public Item(T value)//конструктор класса с параметром
        {
            this.currentValue = value;//здесь тупо присваиваем входной параметр 
            Rnd = new Random();
        }


        protected T currentValue;//поле, в котором будет лежать значение свойства Value
        /// <summary>
        /// Значение ячейки
        /// </summary>
        public virtual T Value //ключевое слово virtual позволяет нам в классах наследниках переопределять члены класса, помеченные этим словом
        {
            get { return currentValue; }//метод get - возвращаем поле currentValue
            set
            {
                //метод set у свойств позволяет проконтролировать входные значения
                //входное значение указывается словом value (СИНИМ ЦВЕТОМ!!!!)
                if (IsValueExceed(value))//если входное значение больше допустимого (допустимое значение хранится в defaultMaxValue)
                {
                    this.currentValue = defaultMaxValue;//currentValue присваиваем максимально возможное значение
                }
                else//иначе
                {
                    IsEmpty = false;//помечаем, что Value уже не пустое 
                    this.currentValue = value;//currentValue присваиваем входное значение
                    ChangeProperty();//вызываем метод, который уведомляет о изменении этого свойства
                }
            }
        }


        /// <summary>
        /// Максимальное значение ячейки (дополнительно поддерживается запись и перезапись)
        /// </summary>
        protected abstract T defaultMaxValue { get; set; }//ключевое слово abstract обязывает нас при наследовании класса реализовать члены класса, помеченные этим словом
        
        
        /*Абстрактные члены класса можно ОБЪЯВЛЯТЬ ТОЛЬКО В АБСТРАКТНЫХ КЛАССАХ*/


        /// <summary>
        /// Абстрактный метод для возврата рандомного значения (по умолчанию максимум - это defaultMaxValue)
        /// </summary>
        /// <returns>Обобщенный тип T</returns>
        public abstract T GetRandomValue();


        /// <summary>
        /// Абстрактный метод для возврата рандомного значения 
        /// </summary>
        /// <param name="maxValue">Максимальное значение T</param>
        /// <returns>Возвращаемое значение - это T</returns>
        public abstract T GetRandomValue(T maxValue);


        public override void SetRandomValue()
        {
            currentValue = GetRandomValue();
        }

        public override void SetRandomValue(BaseItem maxValue)
        {
            //приводим maxValue к типу Item<T>, у которого есть свойство Value и вызываем метод GetRandomValue с указанным параметром
            currentValue = GetRandomValue((maxValue as Item<T>).Value);
        }

        /// <summary>
        /// Абстрактный метод для проверки, является ли входное значение больше максимально возможного.
        /// Вернет true, если значение больше максимального, иначе false
        /// </summary>
        /// <param name="value">Входной параметр</param>
        /// <returns></returns>
        public abstract bool IsValueExceed(T value);


        /// <summary>
        /// Абстрактный метод для проверки, является ли входное значение допустимым для обновления Value
        /// Вернет true, если входное значение недопустимо, иначе false
        /// </summary>
        /// <param name="value">Проверяемое значение (может быть любым объектом)</param>
        /// <returns></returns>
        public abstract bool IsInvalidValue(object value);

        //переопределяем метод ToString, в нем просто возвращаем Value
        public override string ToString()
        {
            return Value.ToString();
        }

    }

    //класс, в котором будет реализованы всевозможные действия с символом (это то, что мы описали в абстрактном классе Item<T>)
    class CharItem : Item<char>
    {
        public CharItem() : base() { }

        public CharItem(char value) : base(value) { }//вызываем базовый конструктор (конструктор Item<T>, где T в данном случае - тип char)

        protected override char defaultMaxValue { get; set; } = 'z';//устанавливаем максимальный символ z
        
        public override bool IsMore(BaseItem item)//переопределяем метод IsMore
        {
            //приводим символы к типу int и сравниваем уже их коды, если больше, то возвращаем true 
            if ((int)currentValue > (int)(item as CharItem).Value)
                return true;
            else//иначе false
                return false;
        }

        //переопределяем опрератор явного приведения типа
        public static explicit operator CharItem(char item)
        {
            //Пример, когда вызывается этот метод
            /*
             * CharItem c = (CharItem)'z';
             */
            return new CharItem(item);
        }

        //здесь исользуюя класс Random, генерируем число, которое конвертируем в символ, максимум по дефолту
        public override char GetRandomValue()
        {
            var c = (char)Rnd.Next(defaultMaxValue);
            return c;
        }

        //аналогично методу выше, только уже входной параметр указываем явно и также генерируем число, которое конвертируем в символ
        public override char GetRandomValue(char maxValue)
        {
            var c = (char)Rnd.Next(defaultMaxValue);
            return c;
        }

        //метод возвращает true, если value больше дефолтного максимального значения, иначе false
        public override bool IsValueExceed(char value)
        {
            return (int)value > (int)defaultMaxValue;
        }

        //проверка на валидность значения 
        public override bool IsInvalidValue(object value)
        {
            char resulValue;
            //TryParse вернет true, если значение валидное и в переменную resulValue запишет результат парсинга
            return !char.TryParse(value.ToString(), out resulValue);
        }

    }

    /*
     * Все, что было сказано о прошлом классе применимо и к этому, только вместо символа
     * используется целое число, преобразований к символьной форме делать не нужно.
     * Также есть макисмальное генеруемое значение по дефолту,
     * Класс Random для генерации чисел также используется
     */
    class IntItem : Item<int>
    {

        public IntItem() : base() { }

        public IntItem(int value) : base(value) { }

        protected override int defaultMaxValue { get; set; } = 100000;
        
        public override bool IsValueExceed(int value)
        {
            return value > defaultMaxValue;
        }

        public override bool IsMore(BaseItem item)
        {
            if (currentValue > (item as IntItem).Value)
                return true;
            else
                return false;
        }

        public static explicit operator IntItem(int item)
        {
            //Пример, когда вызывается этот метод
            /*
             * IntItem i = (IntItem)10;
             */
            return new IntItem(item);
        }

        public override int GetRandomValue()
        {
            return Rnd.Next(defaultMaxValue);
        }

        public override int GetRandomValue(int maxValue)
        {
            return Rnd.Next(maxValue);
        }


        public override bool IsInvalidValue(object value)
        {
            int result;
            //TryParse вернет true, если значение валидное и в переменную result запишет результат парсинга
            //только в прошлом примере было слово char, а здесь ключевое слово int
            //метод TryParse статический
            return !int.TryParse(value.ToString(), out result);
        }
    }


    /*
     * Все, что было сказано о прошлом классе применимо и к этому, только вместо целого числа
     * используется вещественное число.
     * Также есть макисмальное генеруемое значение по дефолту,
     * Класс Random для генерации чисел также используется
     * Я думаю, что тут все логично, в классах CharItem, IntItem и DoubleItem исползуется
     * такой приемм ООП, как наследование. Наследуем класс Item<T>, вместо T указываем соответствующий тип
     * (char, int, double) и уже производный класс работает с конкретными значениями
     */
    class DoubleItem : Item<double>
    {
        public DoubleItem() : base() { }

        public DoubleItem(double value) : base(value) { }

        protected override double defaultMaxValue { get; set; } = 1000000.123545;
        
        public override bool IsMore(BaseItem item)
        {
            if (currentValue > (item as DoubleItem).Value)
                return true;
            else
                return false;
        }

        public static explicit operator DoubleItem(double item)
        {
            return new DoubleItem(item);
        }

        public override double GetRandomValue()
        {
            //здесь нет такой возможности, как использовать промежуток 
            //рандомных значений, метод NextDouble возвращает
            //вещественное число от 0 до 1, поэтому результат
            //домножается на defaultMaxValue, в методе ниже аналогично,
            //только уже умножаем на передаваемое в метод значение 
            return Rnd.NextDouble() * defaultMaxValue;
        }

        public override double GetRandomValue(double maxValue)
        {
            return Rnd.NextDouble() * maxValue;
        }

        public override bool IsValueExceed(double value)
        {
            return value > defaultMaxValue;
        }

        public override bool IsInvalidValue(object value)
        {
            double result;
            return !double.TryParse(value.ToString(), out result);
        }

        public override string ToString()
        {
            //переопределяем метод ToString, округляем Value до 4х знаков после запятой
            return Math.Round(currentValue, 4).ToString();
        }
    }
}