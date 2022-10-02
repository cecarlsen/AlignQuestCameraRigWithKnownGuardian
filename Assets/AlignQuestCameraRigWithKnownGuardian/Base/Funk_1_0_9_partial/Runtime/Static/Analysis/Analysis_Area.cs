/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Funk
{
	public static partial class Analysis
	{
		/// <summary>
		/// Area of a triangle.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float TriangleArea( Vector2 pointA, Vector2 pointB, Vector2 pointC )
		{
			// The "Perp Dot Product" is a dot product where the second input vector is first rotated 90 degrees left (perpendicular).
			// This results in the area of the parallelogram that is formed by first and second vector.
			// Half the parallelogram area is therefore the area of a triangle.
			// http://www.sunshine2k.de/coding/java/PointOnLine/PointOnLine.html
			return ( ( pointB.x - pointA.x ) * ( pointA.y - pointC.y ) + ( pointB.y - pointA.y ) * ( pointC.x - pointA.x ) ) * 0.5f;
		}


        /// <summary>
        /// Area of a polygon. Polygon must not be self-intersecting or have overlapping points.
		/// Points order can both be clockwise or anti-clockwise.
        /// </summary>
        public static float PolygonArea( IList<Vector2> points )
		{
			if( points == null || points.Count < 3 ) return 0;

			// The "Shoelace" algorithm: https://www.omnicalculator.com/math/irregular-polygon-area
			float sum = 0;
			int count = points.Count;
			Vector2 point0 = points[ count - 1 ];
			foreach( Vector2 point1 in points )
			{
				sum += point0.x * point1.y - point1.x * point0.y;
				point0 = point1;
			}
			sum *= 0.5f;
			if( sum < 0f ) sum = -sum;
			return sum;
		}
	}
}