using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RGRSortings
{
    class Shaker : Sorting
    {
        public Shaker(BaseContainer container) : base(container) { }

        //Реализуем сортировку шейкером (оборчивая результат в Task означает то, что мы будем будем ожидать выполнения метода (асинхронные операции))
        public override Task<InfoCalculating> StartSoring()
        {
            //запускаем задачу на выполнение (Run принимает делегат Action - делегает без параметров)
            //C# позволяет упростить вызов таких делегатов лямбда-выражениями 
            //() - такие скобочки в данном случае будут означать метод без параметров
            //=> - это лямбда-выражение, следом за ними идут скобочки - это тело метода
            //можно было создать отдельный метод, в котором была бы реализована логика ниже
            return Task.Run(() =>
            {
                int left = 0;
                int right = Container.Length;
                int count = 0;//количество сравнений
                int countComparisons = 0;//количество перестановок
                InfoCalculating result = null;

                StopWatch = Stopwatch.StartNew();

                while (true)
                {
                    bool check = left >= right;//смотрим, достигли ли мы серидины

                    if (check) break;//если достигли, то выходим из цикла
                    var res = BubbleOnewWay(left, right);//идем слева направо 
                    right--;//после прохода правую границу уменьшаем на 1

                    count += res.CountChecks;//прибавляем кол-во проверок 

                    //если количество перестановок равно нулю, значит что мы уже имеем отсортированный массив, следовательно выходим из цикла
                    if (res.CountComparisons == 0)
                        break;

                    countComparisons += res.CountComparisons;//прибавляем кол-во перестановок


                    if (check) break;//если достигли, то выходим из цикла
                    res = BubbleOnewWay(right, left);//идем справа налево
                    left++;//после прохода увеличиваем левую границу на 1

                    count += res.CountChecks;//прибавляем кол-во проверок 

                    //если количество перестановок равно нулю, значит что мы уже имеем отсортированный массив, следовательно выходим из цикла
                    if (res.CountComparisons == 0)
                        break;

                    countComparisons += res.CountComparisons;//прибавляем кол-во перестановок
                }

                //создаем переменную типа InfoCalculating, в которой сохраняем результаты сортировки
                result = new InfoCalculating()
                {
                    CountChecks = count,
                    CountComparisons = countComparisons,
                    TimeSorting = StopWatch.ElapsedMilliseconds,
                    CountElements = Container.Length
                };
                StopWatch.Reset();//сбрасываем таймер
                OnSortingEnded(result);//уведомляем о том, что сортировка завершена
                return result;
            });
        }

        //сортровка пузырьком в одну сторону (begin - начало, end - конец)
        private InfoCalculating BubbleOnewWay(int begin, int end)
        {
            int count = 0;//количество сравнений
            int countComparisons = 0;//количество перестановок
            if (begin < end)//если begin меньше end, то значит, что движемся слева направо по списку
            {
                for (int i = begin; i < end; i++)
                {
                    int currIndex = i;//текущий индекс
                    int nextIndex = i + 1;//следующий индекс
                    if (nextIndex != end)//если следующий индекс не равен концу
                    {
                        count++;//увеличиваем count на 1
                        //в этом условии используются те самые индексаторы
                        //можно было написать
                        //Container.ListItems[currIndex].IsMore(Container.ListItems[nextIndex])
                        //но эта запись намного меньше и удобнее 
                        if (Container[currIndex].IsMore(Container[nextIndex]))//если текущий элемент больше следующего
                        {
                            Container.Swap(currIndex, nextIndex);//меняем эти элементы местами
                            countComparisons++;//увеличиваем количество перестановок на 1
                            OnCurrentStateItems();//уведомляем о текущем состоянии списка
                        }
                    }
                }
            }
            else if (begin > end)//если begin больше end, то идем справа налево
            {
                for (int i = begin; i > end; i--)//цикл от begin до end, i в каждой итерации уменьшаем на 1
                {
                    if (i != begin)//если не дошли до начала 
                    {
                        count++;
                        int currIndex = i;//текущий индекс
                        int predIndex = i - 1;//предыдущий индекс
                        if (Container[predIndex].IsMore(Container[currIndex]))//если предыдущий элемент больше текущего
                        {
                            Container.Swap(predIndex, currIndex);
                            countComparisons++;
                            OnCurrentStateItems();
                        }
                    }
                }
            }
            return new InfoCalculating()//возвращаем результат одного прохода
            {
                CountChecks = count,
                CountComparisons = countComparisons
            };
        }

    }
}