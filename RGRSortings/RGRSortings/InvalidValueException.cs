using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings
{
    class InvalidValueException : Exception
    {
        public override string Message { get; }
        private object CurrValue { get; set; }

        public InvalidValueException(object currValue)
        {
            CurrValue = currValue;
            Message = $"Значение {currValue} недопустимо";
        }
    }
}