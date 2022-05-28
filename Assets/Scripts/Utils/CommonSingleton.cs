using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock
{
    public class CommonSingleton<T> where T: CommonSingleton<T>, new()
    {
        private static T _instance = null;
        public static T Instance => GetInstance();
        public static T GetInstance()
        {
            if (_instance == null) _instance = new T();
            return _instance;
        }
    }
}
