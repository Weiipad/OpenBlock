using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Utils
{
    public class SimplePool<T> where T: class
    {
        private T[] recyledData;
        private int count;
        private Func<T> createNew;
        public SimplePool(int maxSize, Func<T> createNew)
        {
            recyledData = new T[maxSize];
            this.createNew = createNew;
        }

        public T Obtain()
        {
            if (count != 0) return recyledData[count--];
            return createNew();
        }

        public void Free(T obj)
        {
            if (count <= recyledData.Length)
            {
                recyledData[count++] = obj;
            }
        }
    }
}
