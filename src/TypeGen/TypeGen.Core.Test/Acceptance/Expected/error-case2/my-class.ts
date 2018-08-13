/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { BaseClass } from "./base-class";
import { MyJoinClass } from "./my-join-class";

export class MyClass extends BaseClass<Guid> {
    myMember: MyClass;
    myMemberId: string;
    myJoins: MyJoinClass[];
}
