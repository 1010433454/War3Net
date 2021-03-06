﻿// ------------------------------------------------------------------------------
// <copyright file="LocalVariableListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static LocalVariableListSyntax LocalVariableList(params LocalVariableDeclarationSyntax[] locals)
        {
            return new LocalVariableListSyntax(locals);
        }
    }
}