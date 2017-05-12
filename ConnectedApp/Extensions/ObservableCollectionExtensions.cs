using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConnectedApp.Extensions
{
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Merge the specified originalSource and newSource.
        /// </summary>
        /// <param name="originalSource">Original source.</param>
        /// <param name="newSource">New source.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <remarks>
        /// Current version is way to generic!
        /// Add and remove will only work if objects are the same ( which is higly unlikely )
        /// There is no actual merging of items that exist in both sources but are updated in the new source
        /// </remarks>
        public static void Merge<T>(this ObservableCollection<T> originalSource, IEnumerable<T> newSource)
        {
            var add = newSource.Except(originalSource);
            var remove = originalSource.Except(newSource);

            foreach (var i in remove)
                originalSource.Remove(i);

            foreach (var i in add)
                originalSource.Add(i);

            //If there are any changes make sure the order is the same as how we got the data in the new source
            if (add.Count() > 0 || remove.Count() > 0)
                for (int i = 0; i < newSource.Count(); i++)
                    originalSource.Move(originalSource.IndexOf(newSource.ElementAt(i)), i);
        }
    }
}