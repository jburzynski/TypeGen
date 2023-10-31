using System;

namespace TypeGen.Core.TypeAnnotations;

/// <summary>
/// Identifies a TypeScript class constructor.
/// Constructor implementation will be generated if empty or if the arguments match members of the class.
/// ie: ctor(string name) => public string Name { get; set; }
/// generated as :
/// constructor(name: string)
/// {
///     this.Name: name;
/// }
/// </summary>
[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Class)]
public class TsConstructorAttribute: Attribute
{
    
}