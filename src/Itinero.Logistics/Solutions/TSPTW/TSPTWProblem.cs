﻿// Itinero.Logistics - Route optimization for .NET
// Copyright (C) 2015 Abelshausen Ben
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

using Itinero.Logistics.Solutions.Algorithms;
using Itinero.Logistics.Weights;
using System.Collections.Generic;

namespace Itinero.Logistics.Solutions.TSPTW
{
    /// <summary>
    /// A TSP with time windows.
    /// </summary>
    public class TSPTWProblem<T> : Itinero.Logistics.Solutions.TSP.TSPProblem<T>, ITSPTW<T>
        where T : struct
    {
        /// <summary>
        /// An empty constructor used just to clone stuff.
        /// </summary>
        private TSPTWProblem(WeightHandler<T> weightHandler)
            : base(weightHandler)
        {

        }

        /// <summary>
        /// Creates a new TSP 'open' TSP with only a start customer.
        /// </summary>
        public TSPTWProblem(WeightHandler<T> weightHandler, int first, T[][] weights, TimeWindow[] windows)
            : base(weightHandler, first, weights)
        {
            this.Windows = windows;
        }

        /// <summary>
        /// Creates a new TSP, 'closed' when first equals last.
        /// </summary>
        public TSPTWProblem(WeightHandler<T> weightHandler, int first, int last, T[][] weights, TimeWindow[] windows)
            : base(weightHandler, first, last, weights)
        {
            this.Windows = windows;
        }

        /// <summary>
        /// Gets the windows.
        /// </summary>
        public TimeWindow[] Windows
        {
            get;
            private set;
        }

        /// <summary>
        /// Converts this problem to it's closed equivalent.
        /// </summary>
        /// <returns></returns>
        public override TSP.ITSP<T> ToClosed()
        {
            if (this.Last == null)
            { // 'open' problem, just set weights to first to 0.
                // REMARK: weights already set in constructor.
                return new TSPTWProblem<T>(this.WeightHandler, this.First, this.First, this.Weights, this.Windows);
            }
            else if (this.First != this.Last)
            { // 'open' problem but with fixed weights.
                var weights = new T[this.Weights.Length - 1][];
                for (var x = 0; x < this.Weights.Length; x++)
                {
                    if (x == this.Last)
                    { // skip last edge.
                        continue;
                    }
                    var xNew = x;
                    if (x > this.Last)
                    { // decrease new index.
                        xNew = xNew - 1;
                    }

                    weights[xNew] = new T[this.Weights[x].Length - 1];

                    for (var y = 0; y < this.Weights[x].Length; y++)
                    {
                        if (y == this.Last)
                        { // skip last edge.
                            continue;
                        }
                        var yNew = y;
                        if (y > this.Last)
                        { // decrease new index.
                            yNew = yNew - 1;
                        }

                        if (yNew == xNew)
                        { // make not sense to keep values other than '0' and to make things easier to understand just use '0'.
                            weights[xNew][yNew] = this.WeightHandler.Zero;
                        }
                        else if (y == this.First)
                        { // replace -> first with -> last.
                            weights[xNew][yNew] = this.Weights[x][this.Last.Value];
                        }
                        else
                        { // nothing special about this connection, yay!
                            weights[xNew][yNew] = this.Weights[x][y];
                        }
                    }
                }
                return new TSPTWProblem<T>(this.WeightHandler, this.First, this.First, weights, this.Windows);
            }
            return this; // problem already closed with first==last.
        }

        /// <summary>
        /// Creates a deep-copy of this problem.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var weights = new T[this.Weights.Length][];
            for (var i = 0; i < this.Weights.Length; i++)
            {
                weights[i] = this.Weights[i].Clone() as T[];
            }
            var clone = new TSPTWProblem<T>(this.WeightHandler);
            clone.First = this.First;
            clone.Last = this.Last;
            clone.Weights = this.Weights;
            clone.Windows = this.Windows;
            return clone;
        }
    }
}