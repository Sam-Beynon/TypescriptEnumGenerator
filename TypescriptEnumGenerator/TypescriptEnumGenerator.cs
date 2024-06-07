using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace TypescriptEnumGenerator;

[Generator]
public class TypescriptEnumSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var tsDir = EnsureDirectoryExists(context);

        //Generate the initial type that we use for the KeyValue pair to make it nice to use these with dropdown types provided with most UI libraries
        GenerateAndCreateDropDownOptionsTypescriptFile(tsDir);

        // Retrieve all enum symbols decorated with [TypescriptEnum]
        foreach (var enumSymbol in context.Compilation.GetSymbolsWithName(n => true, SymbolFilter.Type)
            .OfType<INamedTypeSymbol>()
            .Where(symbol => symbol.TypeKind == TypeKind.Enum && symbol.GetAttributes().Any(ad => ad.AttributeClass?.Name == nameof(TypescriptEnumAttribute))))
        {
            GenerateAndCreateEnumTypescriptFile(enumSymbol, tsDir);
        }
    }

    private static void GenerateAndCreateDropDownOptionsTypescriptFile(string tsDir)
    {
        var dropDownOptionContent = GenerateDropDownOptionClass();
#pragma warning disable RS1035
        File.WriteAllText( Path.Combine(tsDir, "DropDownOption.ts"), dropDownOptionContent);
#pragma warning restore RS1035
    }

    private static void GenerateAndCreateEnumTypescriptFile(INamedTypeSymbol enumSymbol, string tsDir)
    {
        var code = GenerateTypeScript(enumSymbol, enumSymbol.Name);

        var sourceText = SourceText.From(code, Encoding.UTF8);
        var fileName = $"{enumSymbol.Name}.ts";
            
#pragma warning disable RS1035
        File.WriteAllText( Path.Combine(tsDir, fileName), sourceText.ToString());            
#pragma warning restore RS1035
    }

    private static string EnsureDirectoryExists(GeneratorExecutionContext context)
    {
        var basePath = context.Compilation.SyntaxTrees.First(x => x.HasCompilationUnitRoot).FilePath;

        var tsDir = Path.Combine(Path.GetDirectoryName(basePath)!, "tsEnums");

#pragma warning disable RS1035
        if (!Directory.Exists(tsDir))
            Directory.CreateDirectory(tsDir);
#pragma warning restore RS1035
        return tsDir;
    }

    private static string GenerateDropDownOptionClass()
    {
        var sb = new StringBuilder();

        sb.AppendLine(@"export default class DropdownOption {
    id: number;
    label: string;

    constructor(id: number, label: string) {
        this.id = id;
        this.label = label;
    }
}");
        var dropDownOptionContent = sb.ToString();
        return dropDownOptionContent;
    }

    private static string GenerateTypeScript(INamedTypeSymbol enumSymbol, string typeName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("import DropDownOption from './DropDownOption'");
        GenerateTypescriptEnum(enumSymbol, typeName, stringBuilder);
        GenerateIntToEnumMethod(enumSymbol, typeName, stringBuilder);
        GenerateStringToEnumMethod(enumSymbol, typeName, stringBuilder);
        GenerateGetDropDownOptionsMethod(enumSymbol, typeName, stringBuilder);

        return stringBuilder.ToString();
    }

    private static void GenerateGetDropDownOptionsMethod(INamedTypeSymbol enumSymbol, string typeName,
        StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($"export function get{typeName}DropDownOptions(): DropDownOption[] {{ ");
        stringBuilder.AppendLine(@"    const dropDownOptions = new Array<DropDownOption>();");
        foreach (var member in enumSymbol.GetMembers())
        {
            var displayNameValue = GetDisplayNameArgumentString(member);
            if (member.Kind == SymbolKind.Field && member is IFieldSymbol fieldSymbol && fieldSymbol.HasConstantValue)
            {
                stringBuilder.AppendLine($@"    dropDownOptions.push(new DropDownOption({fieldSymbol.ConstantValue}, ""{displayNameValue ?? fieldSymbol.Name}""));");
            }
        }

        stringBuilder.AppendLine("    return dropDownOptions;");
        stringBuilder.AppendLine("}");
    }

    private static void GenerateStringToEnumMethod(INamedTypeSymbol enumSymbol, string typeName,
        StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($@"
export function get{typeName}FromString(value: string): {typeName} {{
    switch (value.toLowerCase()) {{
");

        foreach (var member in enumSymbol.GetMembers())
        {
            var displayNameValue = GetDisplayNameArgumentString(member);
            if (member.Kind == SymbolKind.Field && member is IFieldSymbol fieldSymbol && fieldSymbol.HasConstantValue)
            {
                stringBuilder.AppendLine($"        case \"{fieldSymbol.Name.ToLower()}\": return {typeName}.{fieldSymbol.Name};");
                if (displayNameValue != null)
                {
                    stringBuilder.AppendLine($"        case \"{displayNameValue?.ToLower()}\": return {typeName}.{fieldSymbol.Name};");
                }
            }
        }

        stringBuilder.AppendLine($@"
        default: throw new Error(`Unknown {typeName} value: ${{value}}`);
    }}
}}
");
    }

    private static string? GetDisplayNameArgumentString(ISymbol member)
    {
        var displayAttribute = 
            member
                .GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.Name == "DisplayAttribute");
        var displayNameArgument = 
            displayAttribute?
                .NamedArguments
                .FirstOrDefault(a => a.Key == "Name");
        return displayNameArgument?.Value.Value?.ToString();
    }

    private static void GenerateIntToEnumMethod(INamedTypeSymbol enumSymbol, string typeName, StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($@"
export function get{typeName}FromInt(value: number): {typeName} {{
    switch (value) {{
");
        foreach (var member in enumSymbol.GetMembers())
        {
            if (member.Kind == SymbolKind.Field && member is IFieldSymbol fieldSymbol && fieldSymbol.HasConstantValue)
            {
                stringBuilder.AppendLine($"        case {fieldSymbol.ConstantValue}: return {typeName}.{fieldSymbol.Name};");
            }
        }
        stringBuilder.AppendLine($@"
        default: throw new Error(`Unknown {typeName} value: ${{value}}`);
    }}
}}
");
    }

    private static void GenerateTypescriptEnum(INamedTypeSymbol enumSymbol, string typeName, StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($"export enum {typeName} {{");

        foreach (var member in enumSymbol.GetMembers())
        {
            if (member.Kind == SymbolKind.Field && member is IFieldSymbol fieldSymbol && fieldSymbol.HasConstantValue)
            {
                stringBuilder.AppendLine($"{fieldSymbol.Name} = {fieldSymbol.ConstantValue},");
            }
        }

        stringBuilder.AppendLine("}");
    }
}
