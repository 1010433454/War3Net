﻿// ------------------------------------------------------------------------------
// <copyright file="DeclarationListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<MemberDeclarationSyntax> Transpile(this Syntax.DeclarationListSyntax declarationListNode)
        {
            _ = declarationListNode ?? throw new ArgumentNullException(nameof(declarationListNode));

            foreach (var declaration in declarationListNode)
            {
                foreach (var memberDeclaration in declaration.Transpile())
                {
                    yield return memberDeclaration;
                }
            }
        }
    }
}