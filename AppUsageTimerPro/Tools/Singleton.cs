using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUsageTimerPro.Tools
{
    public class Singleton<T> where T : new()
    {
        private static T? _instance;

        protected Singleton() { }

        public static T Instance => _instance ??= new T();
    }
}
