﻿// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class FunctionDeclarationSyntax : SyntaxNode
    {
        private readonly TokenNode _id;
        private readonly TokenNode _takes;
        private readonly ParameterListReferenceSyntax _params;
        private readonly TokenNode _returns;
        private readonly TypeIdentifierSyntax _returnType;

        public FunctionDeclarationSyntax(TokenNode idNode, TokenNode takesNodes, ParameterListReferenceSyntax parameterListNode, TokenNode returnsNode, TypeIdentifierSyntax returnTypeNode)
            : base(idNode, takesNodes, parameterListNode, returnsNode, returnTypeNode)
        {
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _takes = takesNodes ?? throw new ArgumentNullException(nameof(takesNodes));
            _params = parameterListNode ?? throw new ArgumentNullException(nameof(parameterListNode));
            _returns = returnsNode ?? throw new ArgumentNullException(nameof(returnsNode));
            _returnType = returnTypeNode ?? throw new ArgumentNullException(nameof(returnTypeNode));
        }

        public TokenNode IdentifierNode => _id;

        public TokenNode TakesKeywordNode => _takes;

        public ParameterListReferenceSyntax ParameterListReferenceNode => _params;

        public TokenNode ReturnsKeywordNode => _returns;

        public TypeIdentifierSyntax ReturnTypeNode => _returnType;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new FunctionDeclarationSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as ParameterListReferenceSyntax, nodes[3] as TokenNode, nodes[4] as TypeIdentifierSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(TokenParser.Get(SyntaxTokenType.TakesKeyword));
                AddParser(ParameterListReferenceSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.ReturnsKeyword));
                AddParser(TypeIdentifierSyntax.Parser.Get);

                return this;
            }
        }
    }
}