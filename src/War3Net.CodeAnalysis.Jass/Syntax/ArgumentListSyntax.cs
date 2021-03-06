﻿// ------------------------------------------------------------------------------
// <copyright file="ArgumentListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ArgumentListSyntax : SyntaxNode, IEnumerable<NewExpressionSyntax>
    {
        private readonly NewExpressionSyntax _head;
        private readonly ArgumentListTailSyntax _tail;

        public ArgumentListSyntax(NewExpressionSyntax headNode, ArgumentListTailSyntax tailNode)
            : base(headNode, tailNode)
        {
            _head = headNode ?? throw new ArgumentNullException(nameof(headNode));
            _tail = tailNode ?? throw new ArgumentNullException(nameof(tailNode));
        }

        public NewExpressionSyntax FirstArgument => _head;

        public ArgumentListTailSyntax RemainingArguments => _tail;

        public IEnumerator<NewExpressionSyntax> GetEnumerator()
        {
            yield return _head;

            foreach (var argument in _tail)
            {
                yield return argument;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return _head;

            foreach (var argument in _tail)
            {
                yield return argument;
            }
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new ArgumentListSyntax(nodes[0] as NewExpressionSyntax, nodes[1] as ArgumentListTailSyntax);
            }

            private Parser Init()
            {
                AddParser(NewExpressionSyntax.Parser.Get);
                AddParser(ArgumentListTailSyntax.Parser.Get);

                return this;
            }
        }
    }
}