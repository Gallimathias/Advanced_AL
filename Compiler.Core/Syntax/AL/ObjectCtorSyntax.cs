﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Compiler.Core.Syntax.AL
{
    public class ObjectCtorSyntax : ALSourceMemberSyntax<ConstructorDeclarationSyntax>
    {
        public ObjectCtorSyntax()
        {

        }
        private ObjectCtorSyntax(ObjectCtorSyntax objectCtorSyntax)
        {
            CSharpMember = objectCtorSyntax.CSharpMember;
        }

        public override bool TryParse(MemberDeclarationSyntax memberDeclaration, Func<MemberDeclarationSyntax, MemberSyntax> analyser, out MemberSyntax memberSyntax)
        {
            memberSyntax = null;

            if(memberDeclaration is ConstructorDeclarationSyntax ctorDeclaration)
            {
                CSharpMember = ctorDeclaration;
                memberSyntax = new ObjectCtorSyntax(this);
                return true;
            }

            return false;
        }
    }
}
