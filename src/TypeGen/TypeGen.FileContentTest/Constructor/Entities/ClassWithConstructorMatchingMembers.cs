using TypeGen.Core.TypeAnnotations;
using Xunit;

namespace TypeGen.FileContentTest.Constructor.Entities;

[ExportTsClass]
public class ClassWithConstructorMatchingMembers
{
    public string MyProp { get; set; }
    public int myField;
    public SubClass SubClass { get; set; }

    [TsConstructor]
    public ClassWithConstructorMatchingMembers(string myProp, int myField, SubClass subClass)
    { }
}

[ExportTsClass]
public class ClassWithConstructorMatchingMembersMismatch
{
    public string MyProp { get; set; }
    public int myField;
    public SubClass SubClass { get; set; }

    [TsConstructor]
    public ClassWithConstructorMatchingMembersMismatch(string myProp1, int myField, SubClass subClass)
    { }
}

[ExportTsClass]
[TsConstructor]
public class ClassWithConstructorMatchingMembersAndEmpty
{
    public string MyProp { get; set; }
    public int myField;
    public SubClass SubClass { get; set; }
    
    public ClassWithConstructorMatchingMembersAndEmpty(string myProp, int myField, SubClass subClass)
    { }

    public ClassWithConstructorMatchingMembersAndEmpty()
    { }
}

[ExportTsClass]
[TsConstructor]
public record MyRecord(string Name);

public class SubClass
{
    public string Name { get; set; }

    public SubClass(string name)
    {
        Name = name;
    }
}