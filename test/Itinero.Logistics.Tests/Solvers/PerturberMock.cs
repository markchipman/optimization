﻿// Itinero.Logistics - Route optimization for .NET
// Copyright (C) 2016 Abelshausen Ben
// 
// This file is part of Itinero.
// 
// Itinero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// Itinero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Itinero. If not, see <http://www.gnu.org/licenses/>.

using Itinero.Logistics.Solvers;

namespace Itinero.Logistics.Tests.Solvers
{
    /// <summary>
    /// A mockup of a perturber procedure for a very simple problem, reduce a number to zero.
    /// </summary>
    class PerturberMock : IPerturber<float, ProblemMock, ObjectiveMock, SolutionMock, float>
    {
        /// <summary>
        /// Returns the name of the operator.
        /// </summary>
        public string Name
        {
            get { return "MOCK_PERTURBER"; }
        }

        /// <summary>
        /// Returns true if the given object is supported.
        /// </summary>
        /// <returns></returns>
        public bool Supports(ObjectiveMock objective)
        {
            return true;
        }

        /// <summary>
        /// Returns true if there was an improvement, false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool Apply(ProblemMock problem, ObjectiveMock objective, SolutionMock solution, out float delta)
        {
            return this.Apply(problem, objective, solution, 1, out delta);
        }
        
        /// <summary>
        /// Returns true if there was an improvement, false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool Apply(ProblemMock problem, ObjectiveMock objective, SolutionMock solution, int level, out float delta)
        {
            var fitnessBefore = problem.Max - solution.Value;
            delta = Algorithms.RandomGeneratorExtensions.GetRandom().Generate(problem.Max / 100);
            delta = delta - (problem.Max / (80 - level)); // mock approx 20% chance of a better solution at level 1 and decrease with levels.
            solution.Value = solution.Value - delta;
            if (delta < 0) // yes, I know this code can be shorter.
            { // increase in fitness, mock a worse solution.
                return false;
            }
            else
            { // decrease in fitness, mock a better solution.
                return true;
            }
        }
    }
}