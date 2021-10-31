/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { Foo } from "./foo";
import { TestInterface } from "./test-interfaces/test-interface";
import { C } from "./c";
import { D } from "./d";

export class Bar {
    id: number;
    fooId: number;
    foo: Foo;
    aId: number;
    a: TestInterface;
    b: number;
    myCs: C[] = [];
    myDs: D[] = [];
}
