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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using CatLib.Stl;
using NUnit.Framework;

namespace CatLib.Test.Stl
{
    /// <summary>
    /// 快速列表测试
    /// </summary>
    [TestFixture]
    class QuickListTest
    {
        /// <summary>
        /// 推入数据到尾部测试
        /// </summary>
        [Test]
        public void PushShiftData()
        {
            var num = 500000;
            var lst = new QuickList<int>();
            var rand = new Random();
            var lst2 = new List<int>();
            for (var i = 0; i < num; i++)
            {
                var v = rand.Next();
                lst.Push(v);
                lst2.Add(v);
            }
            foreach (var v in lst2)
            {
                Assert.AreEqual(v, lst.Shift());
            }
            Assert.AreEqual(0, lst.Count);
            Assert.AreEqual(0, lst.Length);
        }

        /// <summary>
        /// 头部推入和尾部推出测试
        /// </summary>
        [Test]
        public void UnShiftPopTest()
        {
            var num = 500000;
            var lst = new QuickList<int>();
            var rand = new Random();
            var lst2 = new List<int>();
            for (var i = 0; i < num; i++)
            {
                var v = rand.Next();
                lst.Unshift(v);
                lst2.Add(v);
            }
            foreach (var v in lst2)
            {
                Assert.AreEqual(v, lst.Pop());
            }
            Assert.AreEqual(0, lst.Count);
            Assert.AreEqual(0, lst.Length);
        }

        /// <summary>
        /// 在扫描到的第一个元素之后插入
        /// </summary>
        [Test]
        public void InsertAfterTest()
        {
            var lst = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                lst.Push(i);
            }

            lst.InsertAfter(1, 999);
            lst.InsertAfter(3, 999);
            lst.InsertAfter(5, 999);
            lst.InsertAfter(7, 999);
            lst.InsertAfter(9, 999);

            Assert.AreEqual(999, lst[2]);
            Assert.AreEqual(999, lst[5]);
            Assert.AreEqual(999, lst[8]);
            Assert.AreEqual(999, lst[11]);
            Assert.AreEqual(15, lst.Count);
        }

        /// <summary>
        /// 在扫描到的第一个元素之后插入边界测试
        /// </summary>
        [Test]
        public void InsertAfterBound()
        {
            var lst = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                lst.Push(i);
            }

            lst.InsertAfter(4, 999);
            lst.InsertAfter(999, 888);
            lst.InsertAfter(888, 777);
            Assert.AreEqual(999, lst[5]);
            Assert.AreEqual(888, lst[6]);
            Assert.AreEqual(777, lst[7]);
        }

        /// <summary>
        /// 在扫描到的第一个元素之前插入边界测试
        /// </summary>
        [Test]
        public void InsertBeforeBound()
        {
            var lst = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                lst.Push(i);
            }

            lst.InsertBefore(4, 999);
            lst.InsertBefore(999, 888);
            lst.InsertBefore(888, 777);
            Assert.AreEqual(777, lst[4]);
            Assert.AreEqual(888, lst[5]);
            Assert.AreEqual(999, lst[6]);
        }

        /// <summary>
        /// 运行时增加
        /// </summary>
        [Test]
        public void ForeachAdd()
        {
            var master = new QuickList<int>(10);
            for (int i = 0; i < 10; i++)
            {
                master.Push(i);
            }

            int n = 0;
            foreach (var val in master)
            {
                if (n < 20)
                {
                    master.Push(10 + n);
                }
                Assert.AreEqual(n++, val);
            }
        }

        /// <summary>
        /// 通过下标搜索
        /// </summary>
        [Test]
        public void FindByIndex()
        {
            var master = new QuickList<int>(256);
            for (var i = 0; i < 5000; i++)
            {
                master.Push(i);
            }

            for (var i = 0; i < 5000; i++)
            {
                Assert.AreEqual(i, master[i]);
            }
        }

        /// <summary>
        /// 负数下标
        /// </summary>
        [Test]
        public void FindByIndexNegativeSubscript()
        {
            var master = new QuickList<int>(256);
            for (var i = 0; i < 5000; i++)
            {
                master.Push(i);
            }
            for (var i = 0; i < 5000; i++)
            {
                Assert.AreEqual(5000 - i - 1, master[-(i + 1)]);
            }
        }

        /// <summary>
        /// 通过下标搜索溢出测试
        /// </summary>
        [Test]
        public void FindByIndexOverflow()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                master.Push(i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var val = master[master.Count];
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var val = master[-(master.Count + 1)];
            });
        }

        /// <summary>
        /// 获取区间测试
        /// </summary>
        [Test]
        public void GetRangeTest()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            var elements = master.GetRange(10, 100);
            Assert.AreEqual(90, elements.Length);
            for (var i = 10; i < 100; i++)
            {
                Assert.AreEqual(i, elements[i - 10]);
            }
        }

        /// <summary>
        /// 无效的获取区间测试
        /// </summary>
        [Test]
        public void InvalidGetRange()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                master.GetRange(-1, 10);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                master.GetRange(50, 10);
            });
        }

        /// <summary>
        /// 顺序的移除元素测试
        /// </summary>
        [Test]
        public void SequenceRemove()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            master.Remove(5);
            master.Remove(6);

            Assert.AreEqual(7, master[5]);
            Assert.AreEqual(8, master[6]);
            Assert.AreEqual(254, master.Count);
        }

        /// <summary>
        /// 顺序随机的移除元素测试
        /// </summary>
        [Test]
        public void SequenceRemoveRandom()
        {
            var master = new QuickList<int>(8);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            var lst = new List<int>();
            for (var i = 255; i >= 0; i--)
            {
                if (!lst.Contains(i))
                {
                    lst.Add(i);
                    master.Remove(i);
                }
            }

            foreach (var v in master)
            {
                if (lst.Contains(v))
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// 逆序的移除元素测试
        /// </summary>
        [Test]
        public void ReverseRemove()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            master.Remove(5, -999);
            master.Remove(6, -999);

            Assert.AreEqual(7, master[5]);
            Assert.AreEqual(8, master[6]);
            Assert.AreEqual(254, master.Count);
        }

        /// <summary>
        /// 逆序的随机删除元素
        /// </summary>
        [Test]
        public void ReverseRemoveRandom()
        {
            var master = new QuickList<int>(8);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            var lst = new List<int>();
            for (var i = 255; i >= 0; i--)
            {
                if (!lst.Contains(i))
                {
                    lst.Add(i);
                    master.Remove(i, -999);
                }
            }

            foreach (var v in master)
            {
                if (lst.Contains(v))
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// 移除返回值测试
        /// </summary>
        [Test]
        public void RemoveReturnNumTest()
        {
            var master = new QuickList<int>(8);
            master.Push(111);
            master.Push(111);
            master.Push(111);
            master.Push(222);
            master.Push(333);
            master.Push(111);
            master.Push(111);
            master.Push(444);
            master.Push(333);

            var removeNum = master.Remove(111);
            Assert.AreEqual(5, removeNum);
        }

        /// <summary>
        /// 移除返回值限制测试
        /// </summary>
        [Test]
        public void RemoveReturnNumLimitTest()
        {
            var master = new QuickList<int>(8);
            master.Push(111);
            master.Push(111);
            master.Push(111);
            master.Push(222);
            master.Push(333);
            master.Push(111);
            master.Push(111);
            master.Push(444);
            master.Push(333);

            var removeNum = master.Remove(111, 3);
            Assert.AreEqual(3, removeNum);
            Assert.AreEqual(222, master[0]);
            Assert.AreEqual(111, master[2]);
        }

        /// <summary>
        /// 逆序的移除返回值限制测试
        /// </summary>
        [Test]
        public void ReverseRemoveReturnNumLimitTest()
        {
            var master = new QuickList<int>(8);
            master.Push(111);
            master.Push(111);
            master.Push(111);
            master.Push(222);
            master.Push(333);
            master.Push(111);
            master.Push(111);
            master.Push(444);
            master.Push(333);

            var removeNum = master.Remove(111, -3);
            Assert.AreEqual(3, removeNum);
            Assert.AreEqual(111, master[0]);
            Assert.AreEqual(111, master[1]);
            Assert.AreEqual(222, master[2]);
            Assert.AreEqual(333, master[3]);
            Assert.AreEqual(444, master[4]);
            Assert.AreEqual(6, master.Count);
        }

        /// <summary>
        /// 合并结点测试
        /// </summary>
        [Test]
        public void MergeNodeTest()
        {
            var master = new QuickList<int>(5);

            //node 0
            master.Push(111); // remove
            master.Push(222); // remove
            master.Push(333);
            master.Push(111); // remove
            master.Push(222); // remove

            //node 1
            master.Push(111); // remove
            master.Push(111); // remove
            master.Push(555);
            master.Push(111); // remove
            master.Push(222); // remove

            //node 2
            master.Push(666);
            master.Push(777);
            master.Push(888);

            Assert.AreEqual(3, master.Length);

            master.Remove(111);
            master.Remove(222);

            //trigger merge
            master.InsertAfter(333, 999);

            Assert.AreEqual(2 , master.Length);
        }
    }
}
