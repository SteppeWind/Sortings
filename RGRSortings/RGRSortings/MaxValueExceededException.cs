using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings
{
    class MaxValueExceededException : Exception
    {
        private object MaxValue { get; set; }
        private object CurrentValue { get; set; }
        public override string Message { get; }

        public MaxValueExceededException(object currValue, object maxValue)
        {
            MaxValue = maxValue;
            CurrentValue = currValue;
            Message = $"Ваше значение: {CurrentValue} превысило максимально возможное значение: {maxValue}";
        }
    }
}