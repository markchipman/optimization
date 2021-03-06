﻿/*
 *  Licensed to SharpSoftware under one or more contributor
 *  license agreements. See the NOTICE file distributed with this work for 
 *  additional information regarding copyright ownership.
 * 
 *  SharpSoftware licenses this file to you under the Apache License, 
 *  Version 2.0 (the "License"); you may not use this file except in 
 *  compliance with the License. You may obtain a copy of the License at
 * 
 *       http://www.apache.org/licenses/LICENSE-2.0
 * 
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

using Itinero.Optimization.Tours;
using NUnit.Framework;
using System.Collections.Generic;

namespace Itinero.Optimization.Test.Tours
{
    /// <summary>
    /// Contains tests for the permutation enmerable.
    /// </summary>
    [TestFixture]
    public class PermutationEnumerableTests
    {
        /// <summary>
        /// Tests the permutation counts.
        /// </summary>
        [Test]
        public void TestPermutationCount()
        {
            var testSequence = new int[] { 1, 2 };
            var enumerable = new PermutationEnumerable<int>(testSequence);
            var set = new List<int[]>(enumerable);
            Assert.AreEqual(2, set.Count);

            testSequence = new int[] { 1, 2, 3 };
            enumerable = new PermutationEnumerable<int>(testSequence);
            set = new List<int[]>(enumerable);
            Assert.AreEqual(6, set.Count);

            testSequence = new int[] { 1, 2, 3, 4 };
            enumerable = new PermutationEnumerable<int>(testSequence);
            set = new List<int[]>(enumerable);
            Assert.AreEqual(24, set.Count);

            testSequence = new int[] { 1, 2, 3, 4, 5 };
            enumerable = new PermutationEnumerable<int>(testSequence);
            set = new List<int[]>(enumerable);
            Assert.AreEqual(120, set.Count);
        }

        /// <summary>
        /// Tests the permutation contents.
        /// </summary>
        [Test]
        public void TestPermutationContent()
        {
            var testSequence = new int[] { 1, 2 };
            var enumerable = new PermutationEnumerable<int>(testSequence);
            var set = new List<int[]>(enumerable);
            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 1 }));

            testSequence = new int[] { 1, 2, 3 };
            enumerable = new PermutationEnumerable<int>(testSequence);
            set = new List<int[]>(enumerable);
            Assert.AreEqual(6, set.Count);
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 2, 3 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 3, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 1, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 2, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 3, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 1, 3 }));

            // 4 items tests all the crucial elements of the algorithm. (full code coverage)
            testSequence = new int[] { 1, 2, 3, 4 };
            enumerable = new PermutationEnumerable<int>(testSequence);
            set = new List<int[]>(enumerable);
            Assert.AreEqual(24, set.Count);
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 2, 3, 4 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 3, 2, 4 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 1, 2, 4 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 2, 1, 4 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 3, 1, 4 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 1, 3, 4 }));

            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 2, 4, 3 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 3, 4, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 1, 4, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 2, 4, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 3, 4, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 1, 4, 3 }));

            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 4, 2, 3 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 4, 3, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 4, 1, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 4, 2, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 4, 3, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 4, 1, 3 }));

            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 4, 1, 2, 3 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 4, 1, 3, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 4, 3, 1, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 4, 3, 2, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 4, 2, 3, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 4, 2, 1, 3 }));
        }

        /// <summary>
        /// Tests resetting the permutation enumerable.
        /// </summary>
        [Test]
        public void TestReset()
        {
            var testSequence = new int[] { 1, 2, 3 };
            var enumerable = new PermutationEnumerable<int>(testSequence);
            var set = new List<int[]>();
            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                set.Add(enumerator.Current);
            }
            enumerator.Reset();
            set.Clear();
            while (enumerator.MoveNext())
            {
                set.Add(enumerator.Current);
            }
            Assert.AreEqual(6, set.Count);
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 2, 3 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 1, 3, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 1, 2 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 3, 2, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 3, 1 }));
            Assert.IsTrue(this.TestPermutationContent(set, new int[] { 2, 1, 3 }));
        }

        /// <summary>
        /// Tests if the given permutation exists in the list of permutations.
        /// </summary>
        /// <param name="permuations"></param>
        /// <param name="permutation"></param>
        /// <returns></returns>
        private bool TestPermutationContent(List<int[]> permuations, int[] permutation)
        {
            foreach (int[] current in permuations)
            {
                bool equal = true;
                for (int idx = 0; idx < current.Length; idx++)
                {
                    if (current[idx] != permutation[idx])
                    {
                        equal = false;
                        break;
                    }
                }
                if (equal)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
