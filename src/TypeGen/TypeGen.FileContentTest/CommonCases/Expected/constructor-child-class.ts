/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */
import { ConstructorClass } from "./constructor-class";

export class ConstructorChildClass extends ConstructorClass {
    childConstructorArg: string;

    constructor(constructorArgWithDefault: number = 4, constructorArgWithAttributeDefault: number = 5, constructorArgWithNoDefault: number, childConstructorArg: string = "testchild") {
        super(constructorArgWithDefault, constructorArgWithAttributeDefault, constructorArgWithNoDefault);
        this.childConstructorArg = childConstructorArg;
    }
}