/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { GenericClass } from "./generic-class";
import { TestInterface } from "./test-interfaces/test-interface";
import { FClass } from "./f-class";

export class EClass extends GenericClass<number> {
    b: TestInterface;
    c: string;
    d: string;
    e: number;
    f: string;
    g: string;
    h: boolean;
    i: string;
    link: string = "someUrl/";
    js: FClass[] = [];
}
