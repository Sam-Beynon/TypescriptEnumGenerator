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

export function getTestEnumDropDownOptions(): DropDownOption[] { 
    const dropDownOptions = new Array<DropDownOption>();
    dropDownOptions.push(new DropDownOption(0, "Active"));
    dropDownOptions.push(new DropDownOption(1, "Closed"));
    dropDownOptions.push(new DropDownOption(13, "Something else"));
    dropDownOptions.push(new DropDownOption(14, "Hello it's magic"));
    return dropDownOptions;
}
