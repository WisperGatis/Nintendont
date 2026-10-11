using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Ryujinx.Horizon.Kernel.Generators
{
    class SyscallSyntaxReceiver
    {
        public List<MethodDeclarationSyntax> SvcImplementations { get; }

        public SyscallSyntaxReceiver()
        {
            SvcImplementations = new List<MethodDeclarationSyntax>();
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
            if (!(syntaxNode is ClassDeclarationSyntax classDeclaration) || classDeclaration.AttributeLists.Count == 0)
            {
                return;
            }

            if (!classDeclaration.AttributeLists.Any(attributeList =>
                    attributeList.Attributes.Any(x => x.Name.GetText().ToString() == "SvcImpl")))
            {
                return;
            }

            foreach (var memberDeclaration in classDeclaration.Members)
            {
                if (memberDeclaration is MethodDeclarationSyntax methodDeclaration)
                {
                    VisitMethod(methodDeclaration);
                }
            }
        }

        private void VisitMethod(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration.AttributeLists.Count == 0)
            {
                return;
            }

            if (methodDeclaration.AttributeLists.Any(attributeList =>
                    attributeList.Attributes.Any(x => x.Name.GetText().ToString() == "Svc")))
            {
                SvcImplementations.Add(methodDeclaration);
            }
        }
    }
}
