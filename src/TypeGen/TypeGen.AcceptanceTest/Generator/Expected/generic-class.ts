/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { GenericBaseClass } from "./generic-base-class";
import { TestEnum } from "./test-enums/test-enum";
import { NestedEntity } from "./very/nested/directory/nested-entity";

export class GenericClass<T> extends GenericBaseClass<T> {
    genericProperty: T;
    enumProperty: TestEnum;
    nested: NestedEntity;
}
