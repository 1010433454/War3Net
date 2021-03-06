﻿// ------------------------------------------------------------------------------
// <copyright file="ListFile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.IO.Mpq
{
    /// <summary>
    /// The <see cref="ListFile"/> lists all files that are contained in the <see cref="MpqArchive"/>.
    /// </summary>
    public sealed class ListFile : IDisposable
    {
        /// <summary>
        /// The key (filename) used to open the <see cref="ListFile"/>.
        /// </summary>
        public const string Key = "(listfile)";

        private readonly Stream _baseStream;
        private bool _readOnly;
        private bool _isStreamOwner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListFile"/> class.
        /// </summary>
        /// <param name="files">The collection of file paths to be included in the <see cref="ListFile"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="files"/> argument is null.</exception>
        public ListFile(IEnumerable<string> files)
        {
            _baseStream = new MemoryStream();
            _readOnly = false;
            _isStreamOwner = true;

            using (var writer = GetWriter())
            {
                foreach (var fileName in files ?? throw new ArgumentNullException(nameof(files)))
                {
                    writer.WriteLine(fileName);
                }
            }
        }

        /// <summary>
        /// Gets the underlying stream of this <see cref="ListFile"/>.
        /// </summary>
        public Stream BaseStream => _baseStream;

        /// <summary>
        /// Appends a single file path to the <see cref="ListFile"/>.
        /// </summary>
        /// <param name="fileName">The file path to append.</param>
        public void WriteFile(string fileName)
        {
            if (_readOnly)
            {
                throw new InvalidOperationException($"Cannot write to the ListFile, because it's read-only.");
            }

            using (var writer = GetWriter())
            {
                writer.WriteLine(fileName);
            }
        }

        /// <summary>
        /// Make the <see cref="ListFile"/> read-only.
        /// </summary>
        /// <param name="transferOwnership">If true, the <see cref="ListFile"/> will no longer be the stream owner, so the <see cref="BaseStream"/> won't be disposed.</param>
        public void Finish(bool transferOwnership = false)
        {
            if (_readOnly)
            {
                throw new InvalidOperationException($"Called the Finish method twice.");
            }

            _baseStream.Position = 0;
            _readOnly = true;

            if (transferOwnership)
            {
                _isStreamOwner = false;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ListFile"/>.
        /// </summary>
        public void Dispose()
        {
            if (_isStreamOwner)
            {
                _baseStream.Dispose();
            }
        }

        private StreamWriter GetWriter()
        {
            return new StreamWriter(_baseStream, new UTF8Encoding(false, true), 1024, true);
        }
    }
}