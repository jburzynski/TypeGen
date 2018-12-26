# TypeGen

Single-class-per-file C# to TypeScript generator

[![Build status](https://ci.appveyor.com/api/projects/status/pwi1gh8o1byigo2x?svg=true)](https://ci.appveyor.com/project/JacekBurzynski/typegen)

***Please visit project's website: http://jburzynski.net/TypeGen***
***Complete documentation: http://typegen.readthedocs.io***

## How to get

* [NuGet](https://www.nuget.org/packages/TypeGen)
* [NuGet - .NET CLI tool](https://www.nuget.org/packages/TypeGen.DotNetCli)

## Quick start

Add [TypeGen NuGet package](https://www.nuget.org/packages/TypeGen) to your project

Mark your C# classes/enums as exportable to TypeScript:

```c#
// with attributes

[ExportTsClass]
public class ProductDto
{
    public decimal Price { get; set; }
    public string[] Tags { get; set; }
}

// or with a generation spec
public class
```

After building your project, type `TypeGen generate` into the Package Manager Console, or `dotnet typegen generate` in the system console if you're using TypeGen .NET CLI tool.

This will generate a single TypeScript file (named *product-dto.ts*) in your project directory. The file will look like this:

```typescript
export class ProductDto {
    price: number;
    tags: string[];
}
```

## Features

Some of TypeGen's features include:

* generating TypeScript classes, interfaces and enums - single class per file
* support for collection (or nested collection) property types
* generic classes/types generation
* support for inheritance
* customizable convertion between C#/TypeScript names (naming conventions)

For complete list of features with examples, please refer to the project's documentation: http://typegen.readthedocs.io