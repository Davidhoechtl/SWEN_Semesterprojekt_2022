using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Infrastructure
{
    public static class ListExtensions
    {
        public static void RemoveRange<T>(this List<T> enumerable, IEnumerable<T> toRemove)
        {
            foreach (T item in toRemove)
            {
                enumerable.Remove(item);
            }
        }
    }
}
