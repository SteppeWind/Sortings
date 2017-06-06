using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings
{
    class InfoCalculating
    {
        /// <summary>
        /// Количество проверок
        /// </summary>
        public int CountChecks { get; set; }

        /// <summary>
        /// Количество перестановок
        /// </summary>
        public int CountComparisons { get; set; }

        /// <summary>
        /// Время сортировки в миллисекундах
        /// </summary>
        public long TimeSorting { get; set; }

        /// <summary>
        /// Количество отсортированных элементов 
        /// </summary>
        public int CountElements { get; set; }
    }
}