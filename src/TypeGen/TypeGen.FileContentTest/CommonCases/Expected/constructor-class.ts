/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

export class ConstructorClass {
    constructorArgWithDefault: number;
    constructorArgWithAttributeDefault: number;
    constructorArgWithNoDefault: number;
    nonConstructorArg: string = "test";

    constructor(constructorArgWithDefault: number = 4, constructorArgWithAttributeDefault: number = 5, constructorArgWithNoDefault: number) {
        this.constructorArgWithDefault = constructorArgWithDefault;
        this.constructorArgWithAttributeDefault = constructorArgWithAttributeDefault;
        this.constructorArgWithNoDefault = constructorArgWithNoDefault;
    }
}
