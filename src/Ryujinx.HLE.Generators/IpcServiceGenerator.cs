using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Ryujinx.HLE.Generators
{
    [Generator]
    public class IpcServiceGenerator : IIncrementalGenerator
    {
        private void Execute(SourceProductionContext context, Compilation compilation)
        {
            ServiceSyntaxReceiver syntaxReceiver = new ServiceSyntaxReceiver();
            syntaxReceiver.Visit(compilation);
            CodeGenerator generator = new CodeGenerator();

            generator.AppendLine("#nullable enable");
            generator.AppendLine("using System;");
            generator.EnterScope($"namespace Ryujinx.HLE.HOS.Services.Sm");
            generator.EnterScope($"partial class IUserInterface");

            generator.EnterScope($"public IpcService? GetServiceInstance(Type type, ServiceCtx context, object? parameter = null)");
            foreach (var className in syntaxReceiver.Types)
            {
                if (className.Modifiers.Any(SyntaxKind.AbstractKeyword) || className.Modifiers.Any(SyntaxKind.PrivateKeyword) || !className.AttributeLists.Any(x => x.Attributes.Any(y => y.ToString().StartsWith("Service"))))
                    continue;
                var name = GetFullName(className, compilation).Replace("global::", "");
                if (!name.StartsWith("Ryujinx.HLE.HOS.Services"))
                    continue;
                var constructors = className.ChildNodes().Where(x => x.IsKind(SyntaxKind.ConstructorDeclaration)).Select(y => y as ConstructorDeclarationSyntax);

                if (!constructors.Any(x => x.ParameterList.Parameters.Count >= 1))
                    continue;

                if (constructors.Where(x => x.ParameterList.Parameters.Count >= 1).FirstOrDefault().ParameterList.Parameters[0].Type.ToString() == "ServiceCtx")
                {
                    generator.EnterScope($"if (type == typeof({GetFullName(className, compilation)}))");
                    if (constructors.Any(x => x.ParameterList.Parameters.Count == 2))
                    {
                        var type = constructors.Where(x => x.ParameterList.Parameters.Count == 2).FirstOrDefault().ParameterList.Parameters[1].Type;
                        var model = compilation.GetSemanticModel(type.SyntaxTree);
                        var typeSymbol = model.GetSymbolInfo(type).Symbol as INamedTypeSymbol;
                        var fullName = typeSymbol.ToString();
                        generator.EnterScope("if (parameter != null)");
                        generator.AppendLine($"return new {GetFullName(className, compilation)}(context, ({fullName})parameter);");
                        generator.LeaveScope();
                    }

                    if (constructors.Any(x => x.ParameterList.Parameters.Count == 1))
                    {
                        generator.AppendLine($"return new {GetFullName(className, compilation)}(context);");
                    }

                    generator.LeaveScope();
                }
            }

            generator.AppendLine("return null;");
            generator.LeaveScope();

            generator.LeaveScope();
            generator.LeaveScope();
            context.AddSource($"IUserInterface.g.cs", generator.ToString());
        }

        private string GetFullName(ClassDeclarationSyntax syntaxNode, Compilation compilation)
        {
            var typeSymbol = compilation.GetSemanticModel(syntaxNode.SyntaxTree).GetDeclaredSymbol(syntaxNode);

            return typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(context.CompilationProvider, Execute);
        }
    }
}
