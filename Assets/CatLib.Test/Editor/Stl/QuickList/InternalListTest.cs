﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib.Stl;
using NUnit.Framework;

namespace CatLib.Test.Stl
{
    /// <summary>
    /// 内部列表测试
    /// </summary>
    [TestFixture]
    class InternalListTest
    {
        private class TestClass
        {
            public int Val;
        }

        /// <summary>
        /// 移除指定位置的元素
        /// </summary>
        [Test]
        public void RemoveAtTest()
        {
            var list = new InternalList<TestClass>(4);
            list.Init(new[]
            {
                new TestClass {Val = 0},
                new TestClass {Val = 1},
                new TestClass {Val = 8},
                new TestClass {Val = 9},
            });

            list.RemoveAt(1);
            Assert.AreEqual(0 , list[0].Val);
            Assert.AreEqual(8, list[1].Val);
            Assert.AreEqual(9, list[2].Val);
            Assert.AreEqual(null, list[3]);
        }

        /// <summary>
        /// 插入到指定位置测试
        /// </summary>
        [Test]
        public void InsertAtTest()
        {
            var list = new InternalList<int>();
            list.Init(new[] { 0, 1, 8, 9 });
            list.InsertAt(111, 1);
            list.InsertAt(222, 0);
            list.InsertAt(333, 5);
            list.InsertAt(444, 7);

            Assert.AreEqual(222, list[0]);
            Assert.AreEqual(0, list[1]);
            Assert.AreEqual(111, list[2]);
            Assert.AreEqual(1, list[3]);
            Assert.AreEqual(8, list[4]);
            Assert.AreEqual(333, list[5]);
            Assert.AreEqual(9, list[6]);
            Assert.AreEqual(444, list[7]);
        }

        /// <summary>
        /// 将从的数据追加到主的末尾
        /// </summary>
        [Test]
        public void MergeAfter()
        {
            var master = new InternalList<int>(10);
            var slave = new InternalList<int>(10);

            master.Init(new[] { 0, 1, 2, 3, 4 });
            slave.Init(new[] { 5, 6, 7, 8, 9 });

            master.Merge(slave, true);

            for (var i = 0; i < master.Count; i++)
            {
                Assert.AreEqual(i, master[i]);
            }
        }

        /// <summary>
        /// 将从的数据追加到主的头部
        /// </summary>
        [Test]
        public void MergeBefore()
        {
            var master = new InternalList<int>(10);
            var slave = new InternalList<int>(10);

            master.Init(new[] { 5, 6, 7, 8, 9 });
            slave.Init(new[] { 0, 1, 2, 3, 4 });

            master.Merge(slave, false);

            for (var i = 0; i < master.Count; i++)
            {
                Assert.AreEqual(i, master[i]);
            }
        }

        /// <summary>
        /// offset之后的内容将会从列表中被拆分(正常)
        /// </summary>
        [Test]
        public void SplitListAfter()
        {
            var master = new InternalList<int>(10);
            master.Init(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var elements = master.Split(3, true);

            for (var i = 0; i < elements.Length; i++)
            {
                Assert.AreEqual(i + 4, elements[i]);
            }

            Assert.AreEqual(0, master[0]);
            Assert.AreEqual(1, master[1]);
            Assert.AreEqual(2, master[2]);
            Assert.AreEqual(3, master[3]);
            Assert.AreEqual(0, master[4]);
            Assert.AreEqual(0, master[9]);
            Assert.AreEqual(4, master.Count);
        }

        /// <summary>
        /// offset之后的内容将会从列表中被拆分(左边边界)
        /// </summary>
        [Test]
        public void SplitListAfterLeftBound()
        {
            var master = new InternalList<int>(10);
            master.Init(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var elements = master.Split(0, true);

            for (var i = 0; i < elements.Length; i++)
            {
                Assert.AreEqual(i + 1, elements[i]);
            }

            Assert.AreEqual(0, master[0]);
            Assert.AreEqual(0, master[1]);
            Assert.AreEqual(0, master[9]);
            Assert.AreEqual(1, master.Count);
        }

        /// <summary>
        /// offset之后的内容将会从列表中被拆分(右边边界)
        /// </summary>
        [Test]
        public void SplitListAfterRightBound()
        {
            var master = new InternalList<int>(10);
            master.Init(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var elements = master.Split(9, true);

            Assert.AreEqual(0, elements.Length);
            Assert.AreEqual(10, master.Count);
            Assert.AreEqual(0, master[0]);
            Assert.AreEqual(9, master[9]);
        }

        /// <summary>
        /// offset之后的内容将会从列表中被拆分(正常)
        /// </summary>
        [Test]
        public void SplitListBefor()
        {
            var master = new InternalList<int>(10);
            master.Init(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var elements = master.Split(3, false);

            for (var i = 0; i < elements.Length; i++)
            {
                Assert.AreEqual(i, elements[i]);
            }

            Assert.AreEqual(3, master[0]);
            Assert.AreEqual(6, master[3]);
            Assert.AreEqual(9, master[6]);
            Assert.AreEqual(0, master[7]);
            Assert.AreEqual(0, master[9]);
            Assert.AreEqual(7, master.Count);
        }

        /// <summary>
        /// offset之前的内容将会从列表中被拆分(左边边界)
        /// </summary>
        [Test]
        public void SplitListBeforeLeftBound()
        {
            var master = new InternalList<int>(10);
            master.Init(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var elements = master.Split(0, false);

            Assert.AreEqual(0, elements.Length);
            Assert.AreEqual(10, master.Count);
            Assert.AreEqual(0, master[0]);
            Assert.AreEqual(9, master[9]);
        }

        /// <summary>
        /// offset之前的内容将会从列表中被拆分(右边边界)
        /// </summary>
        [Test]
        public void SplitListBeforeRightBound()
        {
            var master = new InternalList<int>(10);
            master.Init(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var elements = master.Split(9, false);

            for (var i = 0; i < elements.Length; i++)
            {
                Assert.AreEqual(i, elements[i]);
            }

            Assert.AreEqual(9, master[0]);
            Assert.AreEqual(0, master[1]);
            Assert.AreEqual(0, master[9]);
            Assert.AreEqual(1, master.Count);
        }

        /// <summary>
        /// 推入测试
        /// </summary>
        [Test]
        public void PushTest()
        {
            var master = new InternalList<int>(10);

            for (int i = 0; i < 10; i++)
            {
                master.Push(i);
            }
            for (var i = 0; i < master.Count; i++)
            {
                Assert.AreEqual(i, master[i]);
            }
        }

        /// <summary>
        /// 头部推入测试
        /// </summary>
        [Test]
        public void UnShiftTest()
        {
            var master = new InternalList<int>(10);

            for (int i = 0; i < 10; i++)
            {
                master.UnShift(9 - i);
            }
            for (var i = 0; i < master.Count; i++)
            {
                Assert.AreEqual(i, master[i]);
            }
        }

        /// <summary>
        /// 弹出测试
        /// </summary>
        [Test]
        public void PopTest()
        {
            var master = new InternalList<int>(10);
            for (int i = 0; i < 10; i++)
            {
                master.UnShift(i);
            }
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i, master.Pop());
            }
            Assert.AreEqual(0, master.Count);
        }

        /// <summary>
        /// 头部弹出测试
        /// </summary>
        [Test]
        public void ShiftTest()
        {
            var master = new InternalList<int>(10);
            for (int i = 0; i < 10; i++)
            {
                master.Push(i);
            }
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i, master.Shift());
            }
            Assert.AreEqual(0, master.Count);
        }

        /// <summary>
        /// 替换测试
        /// </summary>
        [Test]
        public void ReplaceAtTest()
        {
            var master = new InternalList<int>(10);
            for (int i = 0; i < 10; i++)
            {
                master.Push(i);
            }
            for (int i = 0; i < 10; i++)
            {
                master.ReplaceAt(10 - i, i);
            }
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(10 - i, master[i]);
            }
            Assert.AreEqual(10, master.Count);
        }
    }
}
