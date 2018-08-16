using System;
using System.Collections;

namespace TypeGen.Core.Validation
{
    internal class Requires
    {
        private const string EmptyStringMessage = "string '{0}' must not be empty or start with a null character";
        private const string EmptyArrayMessage = "'{0}' must contain at least one element";
        
        /// <summary>
        /// Throws exception if value of the object is null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNull(object obj, string argumentName)
        {
            if (obj == null) throw new ArgumentNullException(argumentName);
        }
        
        /// <summary>
        /// Throws an exception if the specified parameter's value is null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void NotNullOrEmpty(string value, string argumentName)
        {
            // source: https://github.com/AArnott/Validation/blob/master/src/Validation/Requires.cs
            
            if (value == null) throw new ArgumentNullException(argumentName);

            if (value.Length == 0 || value[0] == '\0')
            {
                throw new ArgumentException(string.Format(EmptyStringMessage, argumentName), argumentName);
            }
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null,
        /// has no elements or has an element with a null value
        /// </summary>
        /// <param name="values"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void NotNullOrEmpty(IEnumerable values, string argumentName)
        {
            // source: https://github.com/AArnott/Validation/blob/master/src/Validation/Requires.cs
            
            switch (values)
            {
                case null:
                    throw new ArgumentNullException(argumentName);
                case ICollection collection when collection.Count > 0:
                    return;
                case ICollection collection:
                    throw new ArgumentException(string.Format(EmptyArrayMessage, argumentName), argumentName);
            }

            IEnumerator enumerator = values.GetEnumerator();

            using (enumerator as IDisposable)
            {
                if (enumerator.MoveNext()) return;
            }

            throw new ArgumentException(string.Format(EmptyArrayMessage, argumentName), argumentName);
        }
    }
}