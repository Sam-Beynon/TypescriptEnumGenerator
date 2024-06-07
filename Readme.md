# Typescript Enum Generator

TypescriptEnumGenerator is a package that utilizes the dotnet ISourceGenerator to provide enum types and helper methods for enums. It helps to reduce the overhead of modifying enums throughout your front and back end by generating Typescript types and classes. This assists in maintaining uniformity between C# and TypeScript.

## Features

- Helps maintain uniformity between C# and TypeScript enum definitions.
- Provides a clean mechanism for utilizing enums for front end dropdowns, while providing compatibility for most component libraries.

## Install

```
dotnet add package TypescriptEnumGenerator --version 0.1.0
```

```
Install-Package TypescriptEnumGenerator -Version 0.1.0
```

## Example
Given the following input code 

```csharp
[TypescriptEnum] //Decorate your enum with this little badger 
public enum TestEnum
{
    Active,
    Closed,
    SomethingElse,
    HelloItsMagic
}
```

And the system will generate the following typescript file

```ts
import DropDownOption from './DropDownOption'
export enum TestEnum {
Active = 0,
Closed = 1,
SomethingElse = 2,
HelloItsMagic = 3,
}

export function getTestEnumFromInt(value: number): TestEnum {
    switch (value) {

        case 0: return TestEnum.Active;
        case 1: return TestEnum.Closed;
        case 2: return TestEnum.SomethingElse;
        case 3: return TestEnum.HelloItsMagic;

        default: throw new Error(`Unknown TestEnum value: ${value}`);
    }
}


export function getTestEnumFromString(value: string): TestEnum {
    switch (value.toLowerCase()) {

        case "active": return TestEnum.Active;
        case "closed": return TestEnum.Closed;
        case "somethingelse": return TestEnum.SomethingElse;
        case "helloitsmagic": return TestEnum.HelloItsMagic;

        default: throw new Error(`Unknown TestEnum value: ${value}`);
    }
}

export function getTestEnumDropDownOptions(): DropDownOption[] { 
    const dropDownOptions = new Array<DropDownOption>();
    dropDownOptions.push(new DropDownOption(0, 'Active'));
    dropDownOptions.push(new DropDownOption(1, 'Closed'));
    dropDownOptions.push(new DropDownOption(2, 'SomethingElse'));
    dropDownOptions.push(new DropDownOption(3, 'HelloItsMagic'));
    return dropDownOptions;
}

```

The generator will also generate a single `DropDownOption.ts` file that is utilized for the `GetEnumDropDownOptions` method, this is designed to be used with `Select` and `DropDown` components provided by most component libraries and the vanilla HTML specification.

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