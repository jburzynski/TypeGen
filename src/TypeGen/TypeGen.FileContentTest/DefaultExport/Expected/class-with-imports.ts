/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import ClassWithDefaultExport from "./class-with-default-export";
import GenericClassWithDefaultExport from "./generic-class-with-default-export";
import { ClassWithoutDefaultExport } from "./class-without-default-export";
import InterfaceWithDefaultExport from "./interface-with-default-export";
import BaseWithDefaultExport from "./my-path/example/base";
import TypeWithDefaultExport from "./my-path/example";

export class ClassWithImports extends BaseWithDefaultExport {
    classWithDefaultExport: ClassWithDefaultExport;
    genericClassWithDefaultExport: GenericClassWithDefaultExport<number, string>;
    classWithoutDefaultExport: ClassWithoutDefaultExport;
    interfaceWithDefaultExport: InterfaceWithDefaultExport;
    customTypeWithDefaultExport: TypeWithDefaultExport;
}
