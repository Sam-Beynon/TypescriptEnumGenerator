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
