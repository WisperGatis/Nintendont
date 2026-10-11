using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Ryujinx.Horizon.Generators.Hipc
{
    class HipcSyntaxReceiver
    {
        public List<CommandInterface> CommandInterfaces { get; }

        public HipcSyntaxReceiver()
        {
            CommandInterfaces = new List<CommandInterface>();
        }

        public void Visit(Compilation compilation)
        {
            foreach (SyntaxTree tree in compilation.SyntaxTrees)
            {
                foreach (SyntaxNode node in tree.GetRoot().DescendantNodesAndSelf())
                {
                    OnVisitSyntaxNode(node);
                }
            }
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclaration)
            {
                if (!classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword) || classDeclaration.BaseList == null)
                {
                    return;
                }

                CommandInterface commandInterface = new CommandInterface(classDeclaration);

                foreach (var memberDeclaration in classDeclaration.Members)
                {
                    if (memberDeclaration is MethodDeclarationSyntax methodDeclaration)
                    {
                        VisitMethod(commandInterface, methodDeclaration);
                    }
                }

                CommandInterfaces.Add(commandInterface);
            }
        }

        private void VisitMethod(CommandInterface commandInterface, MethodDeclarationSyntax methodDeclaration)
        {
            string attributeName = HipcGenerator.CommandAttributeName.Replace("Attribute", string.Empty);

            if (methodDeclaration.AttributeLists.Count != 0)
            {
                foreach (var attributeList in methodDeclaration.AttributeLists)
                {
                    if (attributeList.Attributes.Any(x => x.Name.ToString().Contains(attributeName)))
                    {
                        commandInterface.CommandImplementations.Add(methodDeclaration);
                        break;
                    }
                }
            }
        }
    }
}
