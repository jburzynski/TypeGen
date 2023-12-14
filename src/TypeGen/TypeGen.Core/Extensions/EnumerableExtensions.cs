﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeGen.Core.TypeAnnotations;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Extensions
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Checks if element is in a given set of elements
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool In<T>(this T element, params T[] elements)
        {
            Requires.NotNull(element, nameof(element));
            Requires.NotNull(elements, nameof(elements));
            
            return elements.Contains(element);
        }

        /// <summary>
        /// Filters away null values from an IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> enumerable)
        {
            Requires.NotNull(enumerable, nameof(enumerable));
            return enumerable.Where(v => v != null);
        }

        /// <summary>
        /// Checks if an array has the specified index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool HasIndex<T>(this T[] array, int index)
        {
            Requires.NotNull(array, nameof(array));
            return index >= 0 && array.Length >= index + 1;
        }

        /// <summary>
        /// Checks if an enumerable is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return true;
            return !enumerable.Any();
        }

        /// <summary>
        /// Checks if an enumerable is not null and not empty
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsNotNullAndNotEmpty<T>(this IEnumerable<T> enumerable)
            => !enumerable.IsNullOrEmpty();

        /// <summary>
        /// Checks if the <see cref="enumerable" /> is empty.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> enumerable) => !enumerable.Any();
        
        /// <summary>
        /// Checks if the <see cref="enumerable"/> has no elements satisfying the <see cref="predicate"/>.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) => !enumerable.Any(predicate);

        public static bool Contains<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            => enumerable.Where(predicate).Any();
    }
}
