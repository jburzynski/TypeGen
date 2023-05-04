/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */
import { DefaultMemberComplexValues } from "./default-member-complex-values";

export class DefaultMemberValues {
    fieldString: string = "fieldString";
    fieldIntUnassigned: number;
    fieldIntAssignedDefaultValue: number;
    fieldFloatAssignedDefaultValue: number;
    fieldDateTimeUnassigned: Date;
    static staticFieldNumber: number = 2;
    propertyNumber: number = 3;
    static staticPropertyString: string = "StaticPropertyString";
    propertyComplexDefaultValue: DefaultMemberComplexValues;
    propertyComplexNotDefaultValue: DefaultMemberComplexValues = {"number":4,"numberNull":null,"string":"default","stringNull":null};
}
