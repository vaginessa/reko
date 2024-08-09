#region License
/* 
 * Copyright (C) 2017-2024 Christian Hostelet.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using System;
using System.Runtime.Serialization;

namespace Reko.ImageLoaders.IntelHex
{

    /// <summary>
    /// Exception for signaling Intel Hexadecimal 32-bit object format errors.
    /// </summary>
    public class IntelHexException : Exception
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// Can be used when the error's root cause can't be determined.
        /// </summary>
        /// <param name="lineNo">(Optional) The line number in the IHEX32 datum.</param>
        public IntelHexException(int lineNo = 0)
            : base("Undefined Intel HEX32 error")
        {
            LineNumber = lineNo;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The message as a string.</param>
        /// <param name="lineNo">(Optional) The line number in the IHEX32 datum.</param>
        public IntelHexException(string message, int lineNo = 0)
            : base(message)
        {
            LineNumber = lineNo;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The message as a string.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="lineNo">(Optional) The line number in the IHEX32 datum.</param>
        public IntelHexException(string message, Exception innerException, int lineNo = 0)
            : base(message, innerException)
        {
            LineNumber = lineNo;
        }

        #endregion

        /// <summary>
        /// Gets the Intel IHEX32 record line number at which the exception occurred.
        /// </summary>
        /// <value>
        /// The line number.
        /// </value>
        public int LineNumber { get; } = 0;

    }

}
