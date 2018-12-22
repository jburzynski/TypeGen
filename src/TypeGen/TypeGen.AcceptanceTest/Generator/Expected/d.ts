/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { GenericClass } from "./generic-class";
import { TestInterface } from "./test-interfaces/test-interface";
import { Bar } from "./bar";
import { FClass } from "./f-class";
import { TestEnum } from "./test-enums/test-enum";

export class D extends GenericClass<number> {
    aId: string;
    a: TestInterface;
    bId: string;
    cId: number;
    c: TestInterface;
    barId: number;
    bar: Bar;
    e: string;
    j: Date;
    k: Date;
    fs: FClass[];
    l: Date;
    m: Date;
    n: TestEnum;
    oId: string;
}
