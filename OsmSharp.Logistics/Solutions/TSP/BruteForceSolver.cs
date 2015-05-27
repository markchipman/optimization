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

using OsmSharp.Logistics.Routes;
using OsmSharp.Logistics.Solvers;
using System.Collections.Generic;

namespace OsmSharp.Logistics.Solutions.TSP
{
    /// <summary>
    /// Implements a brute force solver by checking all possible combinations.
    /// </summary>
    public class BruteForceSolver : SolverBase<ITSP, IRoute>
    {
        /// <summary>
        /// Returns a new for this solver.
        /// </summary>
        public override string Name
        {
            get
            {
                return "BF";
            }
        }

        /// <summary>
        /// Solves the given problem.
        /// </summary>
        /// <returns></returns>
        public override IRoute Solve(ITSP problem, out double fitness)
        {
            // initialize.
            var solution = new List<int>();
            for (int customer = 0; customer < problem.Weights.Length; customer++)
            { // add each customer again.
                if (customer != problem.First)
                {
                    solution.Add(customer);
                }
            }

            // keep on looping until all the permutations 
            // have been considered.
            var enumerator = new PermutationEnumerable<int>(
                solution.ToArray());
            IRoute bestSolution = null;
            var bestFitness = double.MaxValue;
            foreach (var permutation in enumerator)
            {
                // build route from permutation.
                var withFirst = new List<int>(permutation);
                withFirst.Insert(0, problem.First);
                var localRoute = Route.CreateFrom(withFirst, problem.IsClosed);

                // calculate fitness.
                var localFitness = 0.0;
                foreach(var pair in localRoute.Pairs())
                {
                    localFitness = localFitness +
                        problem.Weights[pair.From][pair.To];
                }
                if (localFitness < bestFitness)
                { // the best weight has improved.
                    bestFitness = localFitness;
                    bestSolution = localRoute;
                }
            }
            fitness = bestFitness;
            return bestSolution;
        }
    }
}