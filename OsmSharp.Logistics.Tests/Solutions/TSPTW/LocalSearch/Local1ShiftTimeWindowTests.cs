﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2015 Abelshausen Ben
// 
// This file is part of OsmSharp.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with OsmSharp. If not, see <http://www.gnu.org/licenses/>.

using NUnit.Framework;
using OsmSharp.Logistics.Routes;
using OsmSharp.Logistics.Solutions.TSPTW.LocalSearch;
using System.Linq;

namespace OsmSharp.Logistics.Tests.Solutions.TSPTW.LocalSearch
{
    /// <summary>
    /// Contains tests for the Local1Shift local search operator.
    /// </summary>
    [TestFixture]
    public class Local1ShiftTimeWindowTests
    {
        /// <summary>
        /// Tests an infeasible route where a violated customer can be moved backwards.
        /// </summary>
        [Test]
        public void Test1OneShiftViolatedBackward()
        {
            // create the problem and make sure 0->1->2->3->4 is the solution.
            var problem = TSPTWHelper.CreateTSP(0, 0, 5, 2);
            problem.Windows[2] = new Logistics.Solutions.TimeWindow()
            {
                Min = 1,
                Max = 3
            };

            // create a route with one shift.
            var route = new Route(new int[] { 0, 1, 2, 3, 4 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            var localSearch = new Local1TimeWindowShift();
            var delta = 0.0;
            Assert.IsTrue(localSearch.MoveViolatedBackward(problem, route, out delta)); // shifts 2 after 0.

            // test result.
            Assert.AreEqual(1, delta);
            Assert.AreEqual(new int[] { 0, 2, 1, 3, 4 }, route.ToArray());

            // create a route with one shift.
            route = new Route(new int[] { 0, 4, 1, 3, 2 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            Assert.IsTrue(localSearch.MoveViolatedBackward(problem, route, out delta)); // shifts 2 after 0

            // test result.
            Assert.AreEqual(5, delta);
            Assert.AreEqual(new int[] { 0, 2, 4, 1, 3 }, route.ToArray());

            // create a feasible route.
            route = new Route(new int[] { 0, 2, 4, 1, 3 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            Assert.IsFalse(localSearch.MoveViolatedBackward(problem, route, out delta));
        }

        /// <summary>
        /// Tests an infeasible route where a non-violated customer can be moved forward.
        /// </summary>
        [Test]
        public void Test1OneShiftNonViolatedForward()
        {
            // create the problem and make sure 0->1->2->3->4 is the solution.
            var problem = TSPTWHelper.CreateTSP(0, 0, 5, 2);
            problem.Windows[2] = new Logistics.Solutions.TimeWindow()
            {
                Min = 1,
                Max = 3
            };

            // create a route with one shift.
            var route = new Route(new int[] { 0, 1, 2, 3, 4 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            var localSearch = new Local1TimeWindowShift();
            var delta = 0.0;
            Assert.IsTrue(localSearch.MoveNonViolatedForward(problem, route, out delta)); // shifts 1 after 2.

            // test result.
            Assert.AreEqual(1, delta);
            Assert.AreEqual(new int[] { 0, 2, 1, 3, 4 }, route.ToArray());

            // create a feasible route.
            route = new Route(new int[] { 0, 2, 4, 1, 3 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            Assert.IsFalse(localSearch.MoveNonViolatedForward(problem, route, out delta));
        }

        /// <summary>
        /// Tests an infeasible route where a non-violated customer can be moved backward.
        /// </summary>
        [Test]
        public void Test1OneShiftNonViolatedBackward()
        {
            // create the problem and make sure 0->1->2->3->4 is the solution.
            var problem = TSPTWHelper.CreateTSP(0, 0, 5, 10);
            problem.Weights[0][3] = 1;
            problem.Weights[3][1] = 1;
            problem.Windows[1] = new Logistics.Solutions.TimeWindow()
            {
                Min = 1,
                Max = 3
            };

            // create a route with one shift.
            var route = new Route(new int[] { 0, 1, 2, 3, 4 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            var localSearch = new Local1TimeWindowShift();
            var delta = 0.0;
            Assert.IsTrue(localSearch.MoveNonViolatedBackward(problem, route, out delta)); // shifts 3 after 0.

            // test result.
            Assert.AreEqual(7, delta);
            Assert.AreEqual(new int[] { 0, 3, 1, 2, 4 }, route.ToArray());

            // create a feasible route.
            route = new Route(new int[] { 0, 2, 4, 1, 3 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            Assert.IsFalse(localSearch.MoveNonViolatedBackward(problem, route, out delta));
        }

        /// <summary>
        /// Tests an infeasible route where a violated customer can be moved forward.
        /// </summary>
        [Test]
        public void Test1OneShiftViolatedForward()
        {
            // create the problem and make sure 0->1->2->3->4 is the solution.
            var problem = TSPTWHelper.CreateTSP(0, 0, 5, 10);
            problem.Weights[0][3] = 1;
            problem.Weights[3][1] = 1;
            problem.Windows[1] = new Logistics.Solutions.TimeWindow()
            {
                Min = 1,
                Max = 3
            };
            problem.Windows[3] = new Logistics.Solutions.TimeWindow()
            {
                Min = 0,
                Max = 2
            };

            // create a route with one shift.
            var route = new Route(new int[] { 0, 1, 3, 2, 4 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            var localSearch = new Local1TimeWindowShift();
            var delta = 0.0;
            Assert.IsTrue(localSearch.MoveViolatedForward(problem, route, out delta)); // shifts 1 after 3.

            // test result.
            Assert.AreEqual(25, delta);
            Assert.AreEqual(new int[] { 0, 3, 1, 2, 4 }, route.ToArray());

            // create a feasible route.
            route = new Route(new int[] { 0, 3, 1, 2, 4 }, 0);

            // apply the 1-shift local search, it should find the customer to replocate.
            Assert.IsFalse(localSearch.MoveViolatedForward(problem, route, out delta));
        }
    }
}