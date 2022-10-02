/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Funk
{
	public static partial class Analysis
	{

		/// <summary>
		/// Evaluates whether apoint is inside a triangle.
		/// </summary>
		public static bool IsInsideTriangle( Vector2 p0, Vector2 p1 , Vector2 p2, Vector2 testPoint )
		{
			// https://stackoverflow.com/a/2049593/2265840
			// https://forum.unity.com/threads/point-in-triangle-code-c.42878/#post-4664981
			float a = 0.5f * (-p1.y * p2.x + p0.y * (-p1.x + p2.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y);
			float sign = a < 0f ? -1f : 1f;
			float s = (p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * testPoint.x + (p0.x - p2.x) * testPoint.y) * sign;
			float t = (p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * testPoint.x + (p1.x - p0.x) * testPoint.y) * sign;
 
			return s > 0 && t > 0 && (s + t) < 2 * a * sign;
		}


		/// <summary>
		/// Evaluates whether a point lies inside a polygon.
		/// </summary>
		public static bool IsInsidePolygon( IList<Vector2> polygonPoints, Vector2 testPoint )
		{
			int count = polygonPoints.Count;
			if( count < 3 ) return false;

			// The function counts the number of sides of the polygon that intersect a horizontal line at the point 
			// (the first if-condition) and are to the left of it (the second if-condition). If the number of 
			// such sides is odd, then the point is inside the polygon.
			// https://stackoverflow.com/a/14998816/2265840
			bool result = false;
			Vector2 p0 = polygonPoints[ count - 1 ];
			foreach( var p1 in polygonPoints ) {
				if(
					( p1.y < testPoint.y && p0.y >= testPoint.y || p0.y < testPoint.y && p1.y >= testPoint.y ) &&   // Y intersects line?
					( testPoint.x > p1.x + ( testPoint.y - p1.y ) * ( ( p0.x - p1.x ) / ( p0.y - p1.y ) ) ) // Is on left side of line?
				) {
					result = !result;
				}
				p0 = p1;
			}
			return result;
		}
	}
}