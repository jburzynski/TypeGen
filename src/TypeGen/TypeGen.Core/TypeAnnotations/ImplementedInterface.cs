namespace TypeGen.Core.TypeAnnotations;

/// <summary>
/// An interface implemented by a type.
/// </summary>
/// <param name="Name">The interface's name.</param>
/// <param name="ImportPath">The path of the file containing the interface (can be left null if no imports are required).</param>
/// <param name="OriginalTypeName">
/// <para>The original TypeScript interface name.</para>
/// <para>This property should be used when the specified <see cref="Name"/> differs from the original type name defined in the file under <see cref="ImportPath"/>.</para>
/// <para>This property should only be used in conjunction with <see cref="ImportPath"/>.</para>
/// </param>
/// <param name="IsDefaultExport">Whether default export is used for the referenced TypeScript interface - used only in combination with <see cref="ImportPath"/>.</param>
public record struct ImplementedInterface(string Name = null, string ImportPath = null, string OriginalTypeName = null, bool IsDefaultExport = false)
{
}