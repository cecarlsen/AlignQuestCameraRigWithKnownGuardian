/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Funk
{
	public static partial class Analysis
	{
		/// <summary>
		/// Closest point on a continuous line from a given point using (orthogonal) 
		///	projection of a vector onto another vector.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static Vector2 ClosestPointOnLine( Vector2 point, Vector2 a, Vector2 b )
		{
			// TODO: optimize. Project vector.

			Vector2 pointVector = point - a;
			Vector2 lineVector = b - a;

			lineVector.Normalize();
			lineVector *= Vector2.Dot( lineVector, pointVector );

			return a + lineVector;
		}


		/// <summary>
		/// Closest point on a line segment from a given point using (orthogonal) 
		///	projection of a vector onto another vector.
		/// </summary>
		public static Vector2 ClosestPointOnSegment( Vector2 point, Vector2 a, Vector2 b )
		{
			// TODO: optimise!
			Vector2 pointVector = point - a;
			Vector2 lineVector = b - a;

			float lineMagnitude = lineVector.magnitude;
			lineVector.Normalize();

			float dot = Vector2.Dot( lineVector, pointVector );

			// Check to see if point is beyond the extents of the line segment
			if( dot < 0 ) return a;
			if( dot > lineMagnitude ) return b;

			lineVector *= dot;
			return a + lineVector;
		}


        /// <summary>
        /// Closest point on a path from a given point.
        /// </summary>
        public static Vector2 ClosestPointOnPath( Vector2 point, IList<Vector2> path, bool isPathClosed = false )
        {
			Vector2 p0 = isPathClosed ? path[ path.Count-1 ] : path[ 0 ];
			int beginP = isPathClosed ? 0 : 1;
			float minSqDist = float.MaxValue;
			Vector2 closestPoint = Vector2.zero;
            for( int p = 0; p < path.Count; p++ )
			{
				Vector2 p1 = path[ p ];
				float sqDist = PointToSegmentSquareDistance( point, p0, p1 );
				if( sqDist < minSqDist ){
					minSqDist = sqDist;
					closestPoint = ClosestPointOnSegment( point, p0, p1 ); ; // OPTIMIZE: this could be done only once in the very end instead.
                }
                p0 = p1;
			}
			return closestPoint;
        }


        /// <summary>
        /// Closest point on a polyline from a given point.
        /// </summary>
        public static Vector2 ClosestPointOnPolyline( Vector2 point, IList<Vector2> polyline )
        {
			return ClosestPointOnPath( point, polyline, isPathClosed: false );
        }


        /// <summary>
        /// Closest point on a polyline from a given point.
        /// </summary>
        public static Vector2 ClosestPointOnOutline( Vector2 point, IList<Vector2> outline )
        {
            return ClosestPointOnPath( point, outline, isPathClosed: true );
        }


        /// <summary>
        /// Closest point on a circle from a given point.
        /// </summary>
        public static Vector2 ClosesPointOnCircle( Vector2 point, Vector2 center, float radius )
		{
			float dx = point.x - center.x;
			float dy = point.y - center.y;
			float dist = dx * dx + dy * dy;
			if( dist == 0f ) return center;

			dist = Mathf.Sqrt( dist );
			dx = radius * dx / dist;
			dy = radius * dy / dist;
			return new Vector2( center.x + dx, center.y + dy );
		}


		/// <summary>
		/// Closest point on a continuous line from an array of points.
		/// </summary>
		public static int ClosestPointToLine( IList<Vector2> points, Vector2 a, Vector2 b )
		{
			float aby = b.y - a.y;
			float bax = a.x - b.x;
			float d = b.x * a.y - a.x * b.y;

			int mi = 0;
			Vector2 point = points[ 0 ];
			float min = aby * point.x + bax * point.y + d;
			if( min < 0 ) min = -min;

			for( int i = 1; i < points.Count; i++ )
			{
				point = points[ i ];
				float dist = aby * point.x + bax * point.y + d;
				if( dist < 0 ) dist = -dist; // Abs.
				if( dist < min ) {
					mi = i;
					min = dist;
				}
			}
			return mi;
		}
	}
}