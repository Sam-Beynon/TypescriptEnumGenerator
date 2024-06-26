# Typescript Enum Generator

TypescriptEnumGenerator is a package that utilizes the dotnet ISourceGenerator to provide enum types and helper methods for enums. It helps to reduce the overhead of modifying enums throughout your front and back end by generating Typescript types and classes. This assists in maintaining uniformity between C# and TypeScript.

## Features

- Helps maintain uniformity between C# and TypeScript enum definitions.
- Provides a clean mechanism for utilizing enums for front end dropdowns, while providing compatibility for most component libraries.
- Integrates into the `[Display]` attribute and respects usage of `[Display(Name="")`

## Install

```
dotnet add package TypescriptEnumGenerator --version 0.2.1
```

```
Install-Package TypescriptEnumGenerator -Version 0.2.1
```

## Example
Given the following input code

```csharp
[TypescriptEnum] //Decorate your enum with this little badger 
public enum TestEnum
{
    Active,
    Closed,
    [Display(Name = "Something else")]
    SomethingElse = 13,
    [Display(Name = "Hello it's magic")]
    HelloItsMagic
}
```

And the system will generate the following typescript file

```ts
import DropDownOption from './DropDownOption'
export enum TestEnum {
    Active = 0,
    Closed = 1,
    SomethingElse = 13,
    HelloItsMagic = 14,
}

export function getTestEnumFromInt(value: number): TestEnum {
    switch (value) {

        case 0: return TestEnum.Active;
        case 1: return TestEnum.Closed;
        case 13: return TestEnum.SomethingElse;
        case 14: return TestEnum.HelloItsMagic;

        default: throw new Error(`Unknown TestEnum value: ${value}`);
    }
}


export function getTestEnumFromString(value: string): TestEnum {
    switch (value.toLowerCase()) {

        case "active": return TestEnum.Active;
        case "closed": return TestEnum.Closed;
        case "somethingelse": return TestEnum.SomethingElse;
        case "something else": return TestEnum.SomethingElse;
        case "helloitsmagic": return TestEnum.HelloItsMagic;
        case "hello it's magic": return TestEnum.HelloItsMagic;

        default: throw new Error(`Unknown TestEnum value: ${value}`);
    }
}

export const TestEnumDropDownOptions: DropDownOption[] = [
    new DropDownOption(0, "Active"),
    new DropDownOption(1, "Closed"),
    new DropDownOption(13, "Something else"),
    new DropDownOption(14, "Hello it's magic"),
];

```

The generator will also generate a single `DropDownOption.ts` file that is utilized for the `DropDownOption` class, this is designed to be used with `Select` and `DropDown` components provided by most component libraries and the vanilla HTML specification.

```ts
export default class DropdownOption {
    id: number;
    label: string;

    constructor(id: number, label: string) {
        this.id = id;
        this.label = label;
    }
}

```