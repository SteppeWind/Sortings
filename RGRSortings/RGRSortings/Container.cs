using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGRSortings
{
    class Container<T> : BaseContainer
        where T : BaseItem, new() //new() означает, что может создавать экземпляры T
    {
        public Container(): base() { }

        public Container(int length) : base(length) { }
    
        public override void FillEmptysValue(int length)
        {
            if (length >= 0)
            {
                //в цикле добавляем пустые элементы в список
                for (int i = 0; i < length; i++)
                {
                    AddItem(new T());
                }
            }
        }
    }
}