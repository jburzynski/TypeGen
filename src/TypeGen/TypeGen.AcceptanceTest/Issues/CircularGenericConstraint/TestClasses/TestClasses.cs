using System;
using System.Collections.Generic;
using System.Text;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.AcceptanceTest.Issues
{
    [ExportTsClass]
    public class RecursiveConstraintClass<TSelf>
                where TSelf : RecursiveConstraintClass<TSelf>
    {
    }

    [ExportTsInterface]
    public interface IRecursiveConstraintInterface<TSelf>
        where TSelf : IRecursiveConstraintInterface<TSelf>
    {

    }

    [ExportTsInterface]
    public interface IRecursiveConstraintInterfaceWithClassConstraint<TSelf>
        where TSelf : class, IRecursiveConstraintInterfaceWithClassConstraint<TSelf>, new()
    {

    }

    [ExportTsInterface]
    public interface IRecursiveConstraintInterfaceWithStructConstraint<TSelf>
        where TSelf : struct, IRecursiveConstraintInterfaceWithStructConstraint<TSelf>
    {

    }

    [ExportTsInterface]
    public interface ICicrularConstraintInterface<TSelf, TOther>
        where TSelf : ICicrularConstraintInterface<TSelf, TOther>
        where TOther : ICicrularConstraintInterface<TOther, TSelf>
    {

    }


    public interface IA
    {

    }

    public interface IB
    {

    }

    [ExportTsInterface]
    public interface IMultipleConstraintInterface<T>
        where T : IA, IB
    {

    }
}
