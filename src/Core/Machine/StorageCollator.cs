#region License
/* 
 * Copyright (C) 1999-2024 John Källén.
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

using Reko.Core.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Reko.Core.Machine
{
    /// <summary>
    /// Used to compare register storages based on collation order. 
    /// Registers will be sorted in the order specified by the 
    /// arguments to the constructor.
    /// </summary>
    public class StorageCollator : IComparer<Identifier>
    {
        private readonly IReadOnlyDictionary<StorageDomain, int> collationOrder;

        public StorageCollator(RegisterStorage[] collationOrder)
        {
            this.collationOrder = collationOrder.Select((x, i) => (x, i))
                .ToDictionary(p => p.x.Domain, p => p.i);
        }

        public int Compare(Identifier? x, Identifier? y)
        {
            if (x is null)
                return -1;
            if (y is null)
                return 1;
            int cx = collationOrder.TryGetValue(x.Storage.Domain, out int c) ? c : int.MaxValue;
            int cy = collationOrder.TryGetValue(y.Storage.Domain, out c) ? c : int.MaxValue;
            return cx - cy;
        }
    }
}
