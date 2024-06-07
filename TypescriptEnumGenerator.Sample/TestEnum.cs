using System.ComponentModel.DataAnnotations;

namespace TypescriptEnumGenerator.Sample;

[TypescriptEnum]
public enum TestEnum
{
    Active,
    Closed,
    [Display(Name = "Something else")]
    SomethingElse = 13,
    [Display(Name = "Hello it's magic")]
    HelloItsMagic
}