# TypeGen

**Single-class-per-file C# to TypeScript generator**

[![Build status](https://ci.appveyor.com/api/projects/status/pwi1gh8o1byigo2x?svg=true)](https://ci.appveyor.com/project/JacekBurzynski/typegen)

***Website: http://jburzynski.net/TypeGen***

***Documentation: http://typegen.readthedocs.io***

## How to get

* [NuGet](https://www.nuget.org/packages/TypeGen)
* [NuGet - .NET CLI tool](https://www.nuget.org/packages/dotnet-typegen)

## How to build

Running the following commands will create NuGet packages in the root directory (in this example for version 3.0.0):

```powershell
.\update-version.ps1 3.0.0
.\publish.ps1
```

## How to use

1. Add the [TypeGen NuGet package](https://www.nuget.org/packages/TypeGen) to your project. The [dotnet-typegen](https://www.nuget.org/packages/dotnet-typegen) package can be used for a .NET tool. 

2. Select the types to export to TypeScript:

```c#
// using attributes

[ExportTsClass]
public class ProductDto
{
    public decimal Price { get; set; }
    public string[] Tags { get; set; }
}

// or using a specification file (a "generation spec") created anywhere in your project

public class MyGenerationSpec : GenerationSpec
{
    public MyGenerationSpec()
    {
        AddClass<ProductDto>();
    }
}
```

3. If you're using a generation spec, create a file named `tgconfig.json` directly in your project folder with the following content:

```json
{
  "generationSpecs": ["MyGenerationSpec"]
}
```

4. Build your project and type `TypeGen generate` or `TypeGen -p "MyProjectName" generate` (depending on the current working directory of the PM Console) into the Package Manager Console (you might need to restart Visual Studio), or `dotnet typegen generate` in the system console to use the .NET tool.

After completing the above, a single TypeScript file (named *product-dto.ts*) should be generated in your project's root directory with the following content:

```typescript
export class ProductDto {
    price: number;
    tags: string[];
}
```

## Features

The features include (full list of features is available in the [documentation](http://typegen.readthedocs.io)):

* generating TypeScript classes, interfaces and enums (single class per file)
* generating barrel (index) files
* support for collections
* support for generic types
* support for inheritance
* customizable translation of naming conventions