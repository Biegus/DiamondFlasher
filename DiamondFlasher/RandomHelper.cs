using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
namespace DiamondFlasher
{
    public class RandomHelper
    {
        public static T GetRandomElement<T>(Random random, IList<T> collection)
        {
            if (random is null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            return collection[random.Next(0, collection.Count)];
        }
    }
}
