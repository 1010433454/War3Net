﻿// ------------------------------------------------------------------------------
// <copyright file="CommentSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class CommentSyntax : SyntaxNode
    {
        private readonly TokenNode _slashes;
        private readonly TokenNode _comment;
        private readonly TokenNode _newline;

        public CommentSyntax(TokenNode slashesNode, TokenNode commentNode, TokenNode newlineNode)
            : base(slashesNode, commentNode, newlineNode)
        {
            _slashes = slashesNode ?? throw new ArgumentNullException(nameof(slashesNode));
            _comment = commentNode ?? throw new ArgumentNullException(nameof(commentNode));
            _newline = newlineNode ?? throw new ArgumentNullException(nameof(newlineNode));
        }

        public TokenNode DoubleForwardSlashToken => _slashes;

        public TokenNode CommentToken => _comment;

        public TokenNode NewlineToken => _newline;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new CommentSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.DoubleForwardSlash));
                AddParser(TokenParser.Get(SyntaxTokenType.Comment));
                AddParser(TokenParser.Get(SyntaxTokenType.NewlineSymbol));

                return this;
            }
        }
    }
}