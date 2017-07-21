﻿using Compiler.Core.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Core.Parser
{
    public abstract class SyntaxTree
    {
        public SyntaxSource SyntaxSource { get; protected set; }
        public SyntaxStream SyntaxStream { get; protected set; }
        public MemberSyntax RootMember { get; protected set; }

        public SyntaxTree(MemberSyntax memberSyntax)
        {
            RootMember = memberSyntax;
        }
        public SyntaxTree(List<MemberSyntax> memberSyntaxList)
        {

        }

        public abstract SyntaxTree Parse(SyntaxTree syntaxTree);
        public virtual TSyntaxTree Parse<TSyntaxTree>(SyntaxTree syntaxTree) where TSyntaxTree : SyntaxTree 
            => (TSyntaxTree)Parse(syntaxTree);

        internal static SyntaxTree GetTree(List<MemberSyntax> result, SyntaxSource syntaxSource)
        {
            var type = Assembly.GetExecutingAssembly().GetTypes().Where(
                t => t.GetCustomAttributes<SyntaxMemberAttribute>()?.FirstOrDefault(
                        a => a.SyntaxSource == syntaxSource && a.SyntaxMember == SyntaxMember.tree) != null &&
                        !t.IsAbstract).FirstOrDefault();

            return (SyntaxTree)Activator.CreateInstance(type, result);
        }
    }
}
