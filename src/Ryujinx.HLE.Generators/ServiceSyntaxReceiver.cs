using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Ryujinx.HLE.Generators
{
    internal class ServiceSyntaxReceiver
    {
        public HashSet<ClassDeclarationSyntax> Types = new HashSet<ClassDeclarationSyntax>();

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
                if (classDeclaration.BaseList == null)
                {
                    return;
                }

                Types.Add(classDeclaration);
            }
        }
    }
}
