using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;



namespace System.Collections.Generic
{


    public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable

    {
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        int Count { get; }
    }
}


