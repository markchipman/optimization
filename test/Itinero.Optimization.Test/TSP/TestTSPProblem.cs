﻿using Itinero.Optimization.Algorithms.NearestNeighbour;
using Itinero.Optimization.Algorithms.Solvers.GA;
using Itinero.Optimization.Tours;
using Itinero.Optimization.TSP;
using Itinero.Optimization.TSP.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itinero.Optimization.Test.TSP
{
    public class TestTSPProblem : ITSProblem
    {
        /// <summary>
        /// An empty constructor used just to clone stuff.
        /// </summary>
        private TestTSPProblem()
        {

        }

        /// <summary>
        /// Creates a new TSP 'open' TSP with only a start customer.
        /// </summary>
        public TestTSPProblem(int first, float[][] weights)
        {
            this.First = first;
            this.Last = null;
            this.Weights = weights;
            this.Count = weights.Length;

            for (var x = 0; x < this.Weights.Length; x++)
            {
                this.Weights[x][first] = 0;
            }
        }

        /// <summary>
        /// Creates a new TSP, 'closed' when first equals last.
        /// </summary>
        public TestTSPProblem(int first, int last, float[][] weights)
        {
            this.First = first;
            this.Last = last;
            this.Weights = weights;
            this.Count = weights.Length;

            this.Weights[first][last] = 0;
        }

        /// <summary>
        /// Gets the weight between the two given visits.
        /// </summary>
        public float Weight(int from, int to)
        {
            return this.Weights[from][to];
        }

        /// <summary>
        /// Gets the weights.
        /// </summary>
        public float[][] Weights { get; set; }

        /// <summary>
        /// Gets the first customer.
        /// </summary>
        public int First { get; set; }

        /// <summary>
        /// Gets the last customer if the problem is closed.
        /// </summary>
        public int? Last { get; set; }

        /// <summary>
        /// Gets the number of visits.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Converts this problem to it's closed equivalent.
        /// </summary>
        /// <returns></returns>
        public ITSProblem ToClosed()
        {
            if (this.Last == null)
            { // 'open' problem, just set weights to first to 0.
                // REMARK: weights already set in constructor.
                return new TSProblem(this.First, this.First, this.Weights);
            }
            else if (this.First != this.Last)
            { // 'open' problem but with fixed last.
                var weights = new float[this.Weights.Length - 1][];
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

                    weights[xNew] = new float[this.Weights[x].Length - 1];

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
                            weights[xNew][yNew] = 0;
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
                return new TSProblem(this.First, this.First, weights);
            }
            return this; // problem already closed with first==last.
        }

        /// <summary>
        /// Holds the nearest neighbours.
        /// </summary>
        private Dictionary<int, NearestNeighbours[]> _forwardNearestNeighbours;

        /// <summary>
        /// Generate the nearest neighbour list.
        /// </summary>
        /// <returns></returns>
        public NearestNeighbours GetNNearestNeighboursForward(int n, int customer)
        {
            if (_forwardNearestNeighbours == null)
            { // not there yet, create.
                _forwardNearestNeighbours = new Dictionary<int, NearestNeighbours[]>();
            }
            NearestNeighbours[] nearestNeighbours = null;
            if (!_forwardNearestNeighbours.TryGetValue(n, out nearestNeighbours))
            { // not found for n, create.
                nearestNeighbours = new NearestNeighbours[this.Weights.Length];
                _forwardNearestNeighbours.Add(n, nearestNeighbours);
            }
            var result = nearestNeighbours[customer];
            if (result == null)
            { // not found, calculate.
                result = NearestNeighbours.Forward(this.Weights, n, customer);
                nearestNeighbours[customer] = result;
            }
            return result;
        }

        /// <summary>
        /// Holds the nearest neighbours.
        /// </summary>
        private Dictionary<int, NearestNeighbours[]> _backwardNearestNeighbours;

        /// <summary>
        /// Generate the nearest neighbour list.
        /// </summary>
        /// <returns></returns>
        public NearestNeighbours GetNNearestNeighboursBackward(int n, int customer)
        {
            if (_backwardNearestNeighbours == null)
            { // not there yet, create.
                _backwardNearestNeighbours = new Dictionary<int, NearestNeighbours[]>();
            }
            NearestNeighbours[] nearestNeighbours = null;
            if (!_backwardNearestNeighbours.TryGetValue(n, out nearestNeighbours))
            { // not found for n, create.
                nearestNeighbours = new NearestNeighbours[this.Weights.Length];
                _backwardNearestNeighbours.Add(n, nearestNeighbours);
            }
            var result = nearestNeighbours[customer];
            if (result == null)
            { // not found, calculate.
                result = NearestNeighbours.Backward(this.Weights, n, customer);
                nearestNeighbours[customer] = result;
            }
            return result;
        }

        /// <summary>
        /// Holds the nearest neighbours.
        /// </summary>
        private Dictionary<float, SortedNearestNeighbours[]> _forwardSortedNearestNeighbours;

        /// <summary>
        /// Generate the nearest neighbour list.
        /// </summary>
        /// <returns></returns>
        public SortedNearestNeighbours GetNearestNeighboursForward(float weight, int customer)
        {
            if (_forwardSortedNearestNeighbours == null)
            { // not there yet, create.
                _forwardSortedNearestNeighbours = new Dictionary<float, SortedNearestNeighbours[]>();
            }
            SortedNearestNeighbours[] nearestNeighbours = null;
            if (!_forwardSortedNearestNeighbours.TryGetValue(weight, out nearestNeighbours))
            { // not found for n, create.
                nearestNeighbours = new SortedNearestNeighbours[this.Weights.Length];
                _forwardSortedNearestNeighbours.Add(weight, nearestNeighbours);
            }
            var result = nearestNeighbours[customer];
            if (result == null)
            { // not found, calculate.
                result = SortedNearestNeighbours.Forward(this.Weights, weight, customer);
                nearestNeighbours[customer] = result;
            }
            return result;
        }

        /// <summary>
        /// Holds the nearest neighbours.
        /// </summary>
        private Dictionary<float, SortedNearestNeighbours[]> _backwardSortedNearestNeighbours;

        /// <summary>
        /// Generate the nearest neighbour list.
        /// </summary>
        /// <returns></returns>
        public SortedNearestNeighbours GetNearestNeighboursBackward(float weight, int customer)
        {
            if (_backwardSortedNearestNeighbours == null)
            { // not there yet, create.
                _backwardSortedNearestNeighbours = new Dictionary<float, SortedNearestNeighbours[]>();
            }
            SortedNearestNeighbours[] nearestNeighbours = null;
            if (!_backwardSortedNearestNeighbours.TryGetValue(weight, out nearestNeighbours))
            { // not found for n, create.
                nearestNeighbours = new SortedNearestNeighbours[this.Weights.Length];
                _backwardSortedNearestNeighbours.Add(weight, nearestNeighbours);
            }
            var result = nearestNeighbours[customer];
            if (result == null)
            { // not found, calculate.
                result = SortedNearestNeighbours.Backward(this.Weights, weight, customer);
                nearestNeighbours[customer] = result;
            }
            return result;
        }

        /// <summary>
        /// Solves this TSP using a default solver.
        /// </summary>
        /// <returns></returns>
        public Tour Solve()
        {
            return this.Solve(new EAXSolver(GASettings.Default));
        }

        /// <summary>
        /// Solvers this problem using the given solver.
        /// </summary>
        public Tour Solve(Itinero.Optimization.Algorithms.Solvers.ISolver<float, ITSProblem, TSPObjective, Tour, float> solver)
        {
            return solver.Solve(this, new TSPObjective());
        }
    }
}
