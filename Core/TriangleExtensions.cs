using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    public static class TriangleExtensions
    {
        /// <summary>
        /// Add the numbers from the bottom of the tree always taking the biggest child node and work upwards.
        /// The largest possible path from all routes will be calculated.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static int CollapseTree(this int[][] triangle)
        {
            // start at the n-1th row.
            for (int row = triangle.Length - 2; row != -1; row--)
            {
                for (int item = 0; item < triangle[row].Length; item++)
                {
                    int left = triangle[row + 1][item];
                    int right = triangle[row + 1][item + 1];

                    triangle[row][item] += Math.Max(left, right);
                }
            }

            return triangle[0][0];
        }
    }
}
