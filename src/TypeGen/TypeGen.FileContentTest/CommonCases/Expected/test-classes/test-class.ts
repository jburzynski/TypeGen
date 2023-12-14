/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { BaseClass2 } from "./base-class2";
import { BaseClass } from "./base-class";
import { CircularRefClass1 } from "./circular-ref-class1";
import { Regex as RegExp } from "../acme/regex";
import { SomeOtherClass } from "./some-path/some-other-class";
import { TestClass } from "./some-path/test-class";
import { string? } from "./some-path/string";

export class TestClass<T, U extends BaseClass2<string>> extends BaseClass<number> {
    genericField: BaseClass<string>;
    helloWorld: string;
    regex: RegExp;
    regex2: RegExp;
    intAsAny: any;
    dictionaryProperty: { [key: string]: number; };
    circularRefClass1: CircularRefClass1;
    guidProperty: string;
    sampleProperty: string;
    selfProperty: TestClass<T, U>;
    arrayTsType: SomeOtherClass[][];
    genericTsType: TestClass<string, BaseClass2<string>>;
    undefinedableTsType: string?;
    newProp: number;
}
