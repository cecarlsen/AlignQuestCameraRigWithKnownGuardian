/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Funk
{
	public static partial class Analysis
	{
		/// <summary>
		/// Signed distance from a point to a continoues line.
		///	The distance is positive on the right side of the line (following point A to B),
		///	and negative on the other side.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float PointToLineDistanceSigned( Vector2 point, Vector2 a, Vector2 b )
		{
			float abx = b.x - a.x;
			float aby = b.y - a.y;
			float den = abx * abx + aby * aby;
			if( den > -Epsilon && den < Epsilon ) return 0f;

			float dist = -aby * point.x + abx * point.y + a.x * b.y - b.x * a.y;
			return (float) ( - dist / Math.Sqrt( den ) );
		}

		
		/// <summary>
		/// Distance from a point to a continous line.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float PointToLineDistance( Vector2 point, Vector2 a, Vector2 b )
		{
			float d = PointToLineDistanceSigned( point, a, b );
			return d > 0 ? d : -d;
		}

		
        /// <summary>
        /// Distance from a point to a line segment.
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float PointToSegmentDistance( Vector2 point, Vector2 a, Vector2 b )
        {
            return (float) Math.Sqrt( PointToSegmentSquareDistance( point, a, b ) );
        }


		/// <summary>
        /// Distance from a point to a line segment.
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float PointToSegmentSquareDistance( Vector2 point, Vector2 a, Vector2 b )
        {
            float A = point.x - a.x;
            float B = point.y - a.y;
            float C = b.x - a.x;
            float D = b.y - a.y;

            float dot = A * C + B * D;
            float len_sq = C * C + D * D;
            float t = -1f;
            if( len_sq != 0 ) t = dot / len_sq; // Zero length line.

            float xx, yy;
            if( t < 0 ){
                xx = a.x;
                yy = a.y;
            } else if( t > 1 ) {
                xx = b.x;
                yy = b.y;
            } else {
                xx = a.x + t * C;
                yy = a.y + t * D;
            }

            float dx = point.x - xx;
            float dy = point.y - yy;
            return dx * dx + dy * dy;
        }


        /// <summary>
		/// Distance from a point to a polyline (a series of connected line segments).
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float PointToPolylineDistance( Vector2 point, IList<Vector2> polyline )
        {
            return (float) Math.Sqrt( PointToPolylineSquareDistance( point, polyline ) );
        }


        /// <summary>
		/// Square distance from a point to a polyline (a series of connected line segments).
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float PointToPolylineSquareDistance( Vector2 point, IList<Vector2> polyline )
        {
            if( polyline.Count < 2 ) throw new Exception( "A polyline needs at least two points." );

            float minSqDist = float.MaxValue;
            Vector2 a = polyline[ 0 ];
            for( int p = 1; p < polyline.Count; p++ )
            {
                Vector2 b = polyline[ p ];
                float sqDist = PointToSegmentSquareDistance( point, a, b );
                if( sqDist < minSqDist ) minSqDist = sqDist;
                a = b;
            }
            return minSqDist;
        }


        /// <summary>
		/// Distance from a point to an outline.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float PointToOutlineDistance( Vector2 point, IList<Vector2> outline )
        {
            return (float) Math.Sqrt( PointToOutlineSquareDistance( point, outline ) );
        }


        /// <summary>
        /// Square distance from a point to an outline
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float PointToOutlineSquareDistance( Vector2 point, IList<Vector2> outline )
        {
            if( outline.Count < 2 ) throw new Exception( "An outline needs at least two points." );

            float minSqDist = float.MaxValue;
            Vector2 a = outline[ outline.Count - 1 ];
            for( int p = 0; p < outline.Count; p++ )
            {
                Vector2 b = outline[ p ];
                float sqDist = PointToSegmentSquareDistance( point, a, b );
                if( sqDist < minSqDist ) minSqDist = sqDist;
                a = b;
            }
            return minSqDist;
        }


        /// <summary>
        /// Distance from a point to circle. Positive outside, negative inside.
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float PointToCircleDistanceSigned( Vector2 point, Vector2 center, float radius )
		{
			return Vector2.Distance( point, center ) - radius;
		}
	}
}