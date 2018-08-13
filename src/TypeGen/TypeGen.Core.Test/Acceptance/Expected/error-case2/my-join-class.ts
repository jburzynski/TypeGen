/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { MyClass } from "../error-case2/my-class";
import { TestClass } from "../test-classes/test-class";
import { BaseClass2 } from "./base-class2";

export class MyJoinClass {
    class: MyClass;
    classId: string;
    otherClass: TestClass<string, BaseClass2<string>>;
    otherClassId: string;
}
