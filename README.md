# TypeGen

C# to TypeScript file generator with support for one-class-per-file generation

[![Travis build status](https://travis-ci.org/jburzynski/TypeGen.svg?branch=master)](https://travis-ci.org/jburzynski/TypeGen)

***For a complete list of features with examples, please see the project's documentation: http://typegen.readthedocs.io***

***If you spotted a bug, please raise it in the [issue tracker](https://github.com/jburzynski/TypeGen/issues) or contact me directly at jburzyn@gmail.com, thanks!***

## Quick start

Get TypeGen via NuGet by either:
* typing `Install-Package TypeGen` into the Package Manager Console
* searching *TypeGen* in the NuGet gallery / NuGet package manager

Mark your C# classes/enums as exportable to TypeScript:

```c#
using TypeGen.Core.TypeAnnotations;

[ExportTsClass]
public class ProductDto
{
    public decimal Price { get; set; }
    public string[] Tags { get; set; }
}
```

After building your project, type into the Package Manager Console:

```
TypeGen ProjectFolder
```

...where *ProjectFolder* is your project folder, relative to your solution directory.

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