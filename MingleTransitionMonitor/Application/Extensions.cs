using System;
using System.Collections.Generic;

namespace MingleTransitionMonitor.Application
{
    public static class Extensions
    {
        public static void ForEach<TEntity>(this IEnumerable<TEntity> list, Action<TEntity> itemAction)
        {
            if (list==null)
            {
                return;
            }

            foreach (var entity in list)
            {
                itemAction(entity);
            }
        }
    }
}