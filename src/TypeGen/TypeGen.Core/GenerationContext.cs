using System;
using System.Collections.Generic;
using System.Text;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.Validation;

namespace TypeGen.Core
{
    /// <summary>
    /// File generation context (used per Generator.Generate() invocation)
    /// </summary>
    internal class GenerationContext
    {
        /// <summary>
        /// Type of the last file generation
        /// </summary>
        public GenerationType LastGenerationType { get; set; }
        
        /// <summary>
        /// Types that have already been generated for a group of types in the current call to Generator.Generate()
        /// </summary>
        public IList<Type> GroupGeneratedTypes { get; private set; }

        /// <summary>
        /// Types that have already been generated for a type in the current call to Generator.Generate()
        /// </summary>
        public IList<Type> TypeGeneratedTypes { get; private set; }

        /// <summary>
        /// Generation spec used in current file generation
        /// </summary>
        public GenerationSpec GenerationSpec { get; set; }

        public GenerationContext()
        {
            LastGenerationType = GenerationType.Attribute;
        }

        /// <summary>
        /// Adds the type to the generation context
        /// </summary>
        /// <param name="type"></param>
        public void Add(Type type)
        {
            Requires.NotNull(type, nameof(type));
            
            GroupGeneratedTypes?.Add(type);
            TypeGeneratedTypes?.Add(type);
        }

        /// <summary>
        /// Checks if the generation context is for type group generation
        /// </summary>
        /// <returns></returns>
        public bool IsGroupContext()
        {
            return GroupGeneratedTypes != null;
        }

        /// <summary>
        /// Checks if a type has already been generated for a type group in the current context
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasBeenGeneratedForGroup(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return GroupGeneratedTypes?.Contains(type) ?? false;
        }

        /// <summary>
        /// Checks if a type dependency has already been generated for a currently generated type.
        /// This method also returns true if the argument is the currently generated type itself.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasBeenGeneratedForType(Type type)
        {
            Requires.NotNull(type, nameof(type));
            return TypeGeneratedTypes?.Contains(type) ?? false;
        }

        /// <summary>
        /// Initializes the group generated types collection
        /// </summary>
        public void InitializeGroupGeneratedTypes()
        {
            GroupGeneratedTypes = new List<Type>();
        }

        /// <summary>
        /// Clears the group generated types collection
        /// </summary>
        public void ClearGroupGeneratedTypes()
        {
            GroupGeneratedTypes = null;
        }

        /// <summary>
        /// Initializes the type generated types collection
        /// </summary>
        public void InitializeTypeGeneratedTypes()
        {
            TypeGeneratedTypes = new List<Type>();
        }

        /// <summary>
        /// Clears the type generated types collection
        /// </summary>
        public void ClearTypeGeneratedTypes()
        {
            TypeGeneratedTypes = null;
        }
    }
}
