﻿using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Compiler.Core.Syntax.AL.Statements
{
    public class LocalDeclarationSyntax : AlSourceStatementSyntax<LocalDeclarationStatementSyntax>
    {
        public SyntaxType Type { get; protected set; }

        public Dictionary<string, string> Variables { get; set; }

        public LocalDeclarationSyntax()
        {
            Variables = new Dictionary<string, string>();
        }

        public override bool TryParse(StatementSyntax statementSyntax,
            Func<StatementSyntax, SyntaxStatement> analyser, out SyntaxStatement syntaxStatement)
        {
            syntaxStatement = null;

            if (statementSyntax is LocalDeclarationStatementSyntax declaration)
            {
                Type = AlParser.ParseType(declaration.Declaration.Type);


                foreach (var variable in declaration.Declaration.Variables)
                {
                    var initializer = Type.GetInitializer(variable.Initializer);
                    var identifier = variable.Identifier.Text;

                    Variables.Add(identifier, initializer);
                }

                syntaxStatement = new LocalDeclarationSyntax()
                {
                    CSharpStatement = declaration,
                    Name = nameof(LocalDeclarationSyntax),
                    Type = Type,
                    Variables = Variables
                };

                return true;
            }

            return false;
        }

        internal void SetType(SyntaxType syntaxType) => Type = syntaxType;

        internal override void ParseCSharp()
        {
            Type.ParseCSharp();

            CSharpStatement = SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(Type.GetCSharpSyntax()));
        }
    }
}
