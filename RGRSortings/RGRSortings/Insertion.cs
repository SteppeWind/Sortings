using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings
{
    class Insertion : Sorting
    {
        public Insertion(BaseContainer container) : base(container) { }

        public override Task<InfoCalculating> StartSoring()
        {
            return Task.Run(() =>
            {
                int count = 0;//количество сравнений
                int countComparisons = 0;//количество перестановок
                InfoCalculating result = null;

                StopWatch = Stopwatch.StartNew();

                for (int i = 1; i < Container.Length; i++)
                {
                    for (int j = i; j > 0; j--)// пока j>0 и элемент j-1 > j, x-массив int
                    {
                        count++;
                        if (Container[j - 1].IsMore(Container[j]))
                        {
                            countComparisons++;
                            Container.Swap(j - 1, j);// меняем местами элементы j и j-1
                            OnCurrentStateItems();
                        }
                    }
                }

                result = new InfoCalculating()
                {
                    CountChecks = count,
                    CountComparisons = countComparisons,
                    TimeSorting = StopWatch.ElapsedMilliseconds,
                    CountElements = Container.Length
                };
                StopWatch.Reset();
                OnSortingEnded(result);

                return result;
            });
        }
    }
}