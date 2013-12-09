namespace Common.MugenExtensions
{
    using System;
    using System.Collections.Generic;

    public class SingletonsCollection : List<Tuple<IEnumerable<Type>, Type>>
    {
        public void Add<TInterface, TImplementation>(bool bindAlsoToImplementation = false)
        {
            var typesToBind = new List<Type> { typeof(TInterface) };
            if (bindAlsoToImplementation)
            {
                typesToBind.Add(typeof(TImplementation));
            }

            Add(new Tuple<IEnumerable<Type>, Type>(typesToBind, typeof(TImplementation)));
        }
    }
}