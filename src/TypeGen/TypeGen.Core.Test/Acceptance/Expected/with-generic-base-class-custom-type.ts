/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { BaseClass } from "./base-class";
import { TestClass } from "./test-classes/test-class";
import { NestedEntity } from "./very/nested/directory/nested-entity";
import { BaseClass2 } from "./base-class2";

export class WithGenericBaseClassCustomType extends BaseClass<TestClass<NestedEntity, BaseClass2<string>>> {

}
