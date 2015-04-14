// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.Rendering
{
    public class HtmlString
    {
        private static readonly HtmlString _empty = new HtmlString(string.Empty);
        private StringCollectionTextWriter _writer;

        /// <summary>
        /// Initializes a new instance of <see cref="HtmlString"/> with the <paramref name="input"/> string.
        /// </summary>
        /// <param name="input">Contents of the <see cref="HtmlString"/></param>
        public HtmlString(string input)
        {
            Writer.Write(input);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HtmlString"/> that is backed by <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer"></param>
        public HtmlString([NotNull] StringCollectionTextWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// <see cref="StringCollectionTextWriter"/> containing the contents of <see cref="HtmlString"/>.
        /// </summary>
        public StringCollectionTextWriter Writer
        {
            get
            {
                if (_writer == null)
                {
                    _writer = new StringCollectionTextWriter();
                }

                return _writer;
            }
            set
            {
                _writer = value;
            }
        }

        /// <summary>
        /// Empty <see cref="HtmlString"/>.
        /// </summary>
        public static HtmlString Empty
        {
            get
            {
                return _empty;
            }
        }

        /// <summary>
        /// Writes the value in this instance of <see cref="HtmlString"/> to the target <paramref name="targetWriter"/>.
        /// </summary>
        /// <param name="targetWriter">The <see cref="TextWriter"/> to write contents to.</param>
        public void WriteTo(TextWriter targetWriter)
        {
            if (_writer != null)
            {
                _writer.CopyTo(targetWriter);
            }
            else
            {
                targetWriter.Write(Writer.ToString());
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Writer.ToString();
        }
    }
}
