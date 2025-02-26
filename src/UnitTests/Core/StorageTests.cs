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

using NUnit.Framework;
using Reko.Core;
using Reko.Core.Types;

namespace Reko.UnitTests.Core
{
    [TestFixture]
	public class StorageTests
	{
        private Storage eax;
        private Storage ax;
        private Storage al;
        private Storage ah;
        private Storage fpu0;
        private Storage fpu1;
        private Storage tmpWord32;

        private RegisterStorage freg;
        private FlagGroupStorage szc;
        private FlagGroupStorage sz;
        private FlagGroupStorage c;
        private FlagGroupStorage z;
        private FlagGroupStorage s;

        public StorageTests()
        {
            this.eax = RegisterStorage.Reg32("eax", 0);
            this.ax = RegisterStorage.Reg16("ax", 0);
            this.al = RegisterStorage.Reg8("al", 0);
            this.ah = RegisterStorage.Reg8("ah", 0, 8);

            this.freg = RegisterStorage.Reg32("eflags", 70);
            this.szc = new FlagGroupStorage(freg, 0x7, "szc", PrimitiveType.Byte);
            this.sz = new FlagGroupStorage(freg, 0x6, "sz", PrimitiveType.Byte);
            this.c = new FlagGroupStorage(freg, 0x1, "c", PrimitiveType.Bool);
            this.z = new FlagGroupStorage(freg, 0x2, "z", PrimitiveType.Bool);
            this.s = new FlagGroupStorage(freg, 0x4, "s", PrimitiveType.Bool);

            this.fpu0 = new FpuStackStorage(0, PrimitiveType.Real64);
            this.fpu1 = new FpuStackStorage(1, PrimitiveType.Real64);

            this.tmpWord32 = new TemporaryStorage("tmp", 0, PrimitiveType.Word32);
        }

        [Test]
        public void Stg_RegistersOverlap()
        {
            Assert.IsTrue(eax.OverlapsWith(eax));
            Assert.IsTrue(eax.OverlapsWith(ax));
            Assert.IsTrue(eax.OverlapsWith(al));
            Assert.IsTrue(eax.OverlapsWith(ah));
            Assert.IsFalse(al.OverlapsWith(ah));
        }

        [Test]
        public void Stg_RegistersCover()
        {
            Assert.IsTrue(eax.Covers(eax));
            Assert.IsTrue(eax.Covers(ax));
            Assert.IsFalse(ax.Covers(eax));
            Assert.IsTrue(ax.Covers(al));
            Assert.IsFalse(ah.Covers(al));
        }

        [Test]
        public void Stg_RegisterOffsets()
        {
            Assert.AreEqual(0, eax.OffsetOf(eax));
            Assert.AreEqual(0, eax.OffsetOf(ax));
            Assert.AreEqual(0, eax.OffsetOf(al));
            Assert.AreEqual(8, eax.OffsetOf(ah));

            Assert.AreEqual(-1, ax.OffsetOf(eax));
            Assert.AreEqual(0, ax.OffsetOf(ax));
            Assert.AreEqual(0, ax.OffsetOf(al));
            Assert.AreEqual(8, ax.OffsetOf(ah));

            Assert.AreEqual(-1, al.OffsetOf(eax));
            Assert.AreEqual(-1, al.OffsetOf(ax));
            Assert.AreEqual(0, al.OffsetOf(al));
            Assert.AreEqual(-1, al.OffsetOf(ah));

            Assert.AreEqual(0, ah.OffsetOf(ah));
        }

        [Test]
        public void Stg_FlagGroupsOverlap()
        {
            Assert.IsTrue(szc.OverlapsWith(szc));
            Assert.IsTrue(szc.OverlapsWith(sz));
            Assert.IsTrue(sz.OverlapsWith(szc));
            Assert.IsTrue(c.OverlapsWith(szc));
            Assert.IsFalse(c.OverlapsWith(sz));
            Assert.IsFalse(sz.OverlapsWith(c));
        }

        [Test]
        public void Stg_FlagGroupsCover()
        {
            Assert.IsTrue(szc.Covers(szc));
            Assert.IsTrue(szc.Covers(sz));
            Assert.IsFalse(sz.Covers(szc));
            Assert.IsFalse(c.Covers(szc));
            Assert.IsFalse(c.Covers(sz));
            Assert.IsFalse(sz.Covers(c));
        }

        [Test]
        public void Stg_StackStorageCover()
        {
            // Mimic a big endian copy of the low 8 bits of a stack word.
            var argLarge = new StackStorage(4, PrimitiveType.Word32);
            var argSmall = new StackStorage(7, PrimitiveType.Byte);
            Assert.IsTrue(argLarge.Covers(argSmall), $"{argLarge} should cover {argSmall}.");
            Assert.False(argSmall.Covers(argLarge), $"{argSmall} shouldn't cover {argLarge}.");
        }

        [Test]
        public void Stg_FpuStackStorageCover()
        {
            Assert.True(fpu0.Covers(fpu0), $"{fpu0} should cover {fpu0}.");
            Assert.False(fpu0.Covers(fpu1), $"{fpu0} shouldn't cover {fpu1}.");
            Assert.False(fpu1.Covers(fpu0), $"{fpu1} shouldn't cover {fpu0}.");
        }

        [Test]
        public void Stg_TemporaryStorageWord32BitSize()
        {
            Assert.AreEqual(tmpWord32.BitSize, 32);
        }

        [Test]
        public void Stg_FpuStackStorageOffsetOf()
        {
            Assert.AreEqual(0, fpu0.OffsetOf(fpu0));
            Assert.AreEqual(-1, fpu0.OffsetOf(fpu1));
            Assert.AreEqual(-1, fpu1.OffsetOf(fpu0));
        }
    }
}
