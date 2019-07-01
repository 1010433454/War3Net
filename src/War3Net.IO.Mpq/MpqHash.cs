﻿// ------------------------------------------------------------------------------
// <copyright file="MpqHash.cs" company="Foole (fooleau@gmail.com)">
// Copyright (c) 2006 Foole (fooleau@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq
{
    public struct MpqHash
    {
        /// <summary>
        ///
        /// </summary>
        public const uint Size = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHash"/> struct.
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <param name="locale"></param>
        /// <param name="blockIndex"></param>
        /// <param name="mask"></param>
        public MpqHash(uint name1, uint name2, MpqLocale locale, uint blockIndex, uint mask)
            : this(name1, name2, locale, blockIndex)
        {
            Mask = mask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHash"/> struct.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="mask"></param>
        public MpqHash(BinaryReader br, uint mask)
            : this(br.ReadUInt32(), br.ReadUInt32(), (MpqLocale)br.ReadUInt32(), br.ReadUInt32())
        {
            Mask = mask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqHash"/> struct.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mask"></param>
        /// <param name="locale"></param>
        /// <param name="blockIndex"></param>
        public MpqHash(string fileName, uint mask, MpqLocale locale, uint blockIndex)
            : this(StormBuffer.HashString(fileName, 0x100), StormBuffer.HashString(fileName, 0x200), locale, blockIndex, mask)
        {
        }

        private MpqHash(uint name1, uint name2, MpqLocale locale, uint blockIndex)
            : this()
        {
            Name1 = name1;
            Name2 = name2;
            Locale = locale;
            BlockIndex = blockIndex;
        }

        /// <summary>
        ///
        /// </summary>
        public static MpqHash DELETED => new MpqHash(0xFFFFFFFF, 0xFFFFFFFF, (MpqLocale)0xFFFFFFFF, 0xFFFFFFFE);

        /// <summary>
        ///
        /// </summary>
        public static MpqHash NULL => new MpqHash(0xFFFFFFFF, 0xFFFFFFFF, (MpqLocale)0xFFFFFFFF, 0xFFFFFFFF); // todo: rename EMPTY?

        /// <summary>
        ///
        /// </summary>
        public uint Name1 { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public uint Name2 { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public MpqLocale Locale { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public uint BlockIndex { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public uint Mask { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="MpqHash"/> corresponds to an <see cref="MpqEntry"/>.
        /// </summary>
        public bool IsEmpty => BlockIndex == 0xFFFFFFFF;

        /// <summary>
        /// Gets a value indicating whether the <see cref="MpqHash"/> has had its corresponding <see cref="MpqEntry"/> deleted.
        /// </summary>
        public bool IsDeleted => BlockIndex == 0xFFFFFFFE;

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static uint GetIndex(string path, uint mask)
        {
            return StormBuffer.HashString(path, 0) & mask;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsEmpty ? "EMPTY" : IsDeleted ? "DELETED" : $"Entry #{BlockIndex}";
        }
    }
}