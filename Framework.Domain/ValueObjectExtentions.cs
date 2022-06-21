using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public static class ValueObjectExtentions
    {
        public static void UpdateValueObject<T>(this IList<T> existingItems, List<T> newItems) where T : ValueObjectBase
        {
            var addedItems = newItems.Except(existingItems).ToList();
            var deletedItems = existingItems.Except(newItems).ToList();

            deletedItems.ForEach(a => existingItems.Remove(a));
            addedItems.ForEach(existingItems.Add);
        }
    }
}
