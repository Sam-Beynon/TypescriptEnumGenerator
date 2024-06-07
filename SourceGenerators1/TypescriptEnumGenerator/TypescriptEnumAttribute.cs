using System;

namespace TypescriptEnumGenerator;

[AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class TypescriptEnumAttribute : Attribute
{
}