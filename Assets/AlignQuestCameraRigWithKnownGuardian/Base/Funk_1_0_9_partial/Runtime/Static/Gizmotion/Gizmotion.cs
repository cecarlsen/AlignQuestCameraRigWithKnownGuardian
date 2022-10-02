/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
	public static partial class Gizmotion
	{
		/// <summary>
		/// Draw polyline path.
		/// </summary>
		public static void DrawPath( IList<Vector2> points, bool close = false )
		{
			if( points == null ) return;

			int count = points.Count;
			if( count < 1 ) return;

			Vector2 p0 = points[ 0 ];
			for( int i = 1; i < count; i++ ) {
				Vector2 p1 = points[ i ];
				Gizmos.DrawLine( p0, p1 );
				p0 = p1;
			}
			if( close ) {
				Gizmos.DrawLine( p0, points[ 0 ] );
			}
		}


		/// <summary>
		/// Draw polyline path at offset.
		/// </summary>
		public static void DrawPath( IList<Vector2> points, Vector2 position, bool close = false )
		{
			if( points == null ) return;

			int count = points.Count;
			if( count < 1 ) return;

			Vector2 p0 = points[ 0 ];
			p0.x += position.x;
			p0.y += position.y;
			for( int i = 1; i < count; i++ ) {
				Vector2 p1 = points[ i ];
				p1.x += position.x;
				p1.y += position.y;
				Gizmos.DrawLine( p0, p1 );
				p0 = p1;
			}
			if( close ) {
				Vector2 p1 = points[ 0 ];
				p1.x += position.x;
				p1.y += position.y;
				Gizmos.DrawLine( p0, p1 );
			}
		}


        /// <summary>
        /// Draw polyline path at offset. Alias for DrawPath.
        /// </summary>
        public static void DrawPolyline( IList<Vector2> points, Vector2 position )
        {
            DrawPath( points, position, close: false );
        }


        /// <summary>
        /// Draw polyline path. Alias for DrawPath.
        /// </summary>
        public static void DrawPolyline( IList<Vector2> points )
        {
            DrawPath( points, Vector2.zero, close: false );
        }


        /// <summary>
        /// Draw polyline path at offset. Alias for DrawPath.
        /// </summary>
        public static void DrawOutline( IList<Vector2> points, Vector2 position )
        {
            DrawPath( points, position, close: true );
        }


        /// <summary>
        /// Draw polyline path. Alias for DrawPath.
        /// </summary>
        public static void DrawOutline( IList<Vector2> points )
        {
            DrawPath( points, Vector2.zero, close: true );
        }


        /// <summary>
        /// Draw polygon path at offset.
        /// </summary>
        public static void DrawWirePolygon( IList<Vector2> points, Vector2 position )
		{
			DrawPath( points, position, close: true );
		}


		/// <summary>
		/// Draw polygon path.
		/// </summary>
		public static void DrawWirePolygon( IList<Vector2> points )
		{
			DrawPath( points, Vector2.zero, close: true );
		}



		/// <summary>
		/// Draw polyline path with gradient.
		/// </summary>
		public static void DrawPathGradient( IList<Vector2> points, Color colorBegin, Color colorEnd, bool close = false )
		{
			int count = points.Count;
			if( count < 1 ) return;

			Vector2 p0 = points[ 0 ];
			float step = close ? 1f / (float) count : 1f / ( count - 1f ); 
			for( int i = 1; i < count; i++ ) {
				Gizmos.color = Color.Lerp( colorBegin, colorEnd, i * step );
				Vector2 p1 = points[ i ];
				Gizmos.DrawLine( p0, p1 );
				p0 = p1;
			}
			if( close ) {
				Gizmos.color = colorEnd;
				Gizmos.DrawLine( p0, points[ 0 ] );
			}
		}


		/// <summary>
		/// Draw points.
		/// </summary>
		public static void DrawPoints( IList<Vector2> points, float radius )
		{
			foreach( Vector2 point in points ) Gizmos.DrawSphere( point, radius );
		}


		/// <summary>
		/// Draw points.
		/// </summary>
		public static void DrawPoints( HashSet<Vector2> points, float radius )
		{
			foreach( Vector2 point in points ) Gizmos.DrawSphere( point, radius );
		}


		/// <summary>
		/// Draw points with gradient.
		/// </summary>
		public static void DrawPointsGradient( IList<Vector2> points, float radius, Color colorBegin, Color colorEnd )
		{
			for( int p = 0; p < points.Count; p++ ) {
				float t = p / (float) points.Count;
				Gizmos.color = Color.Lerp( colorBegin, colorEnd, t );
				Gizmos.DrawSphere( points[ p ], radius );
			}
		}


		/// <summary>
		/// Draw a line segment. Same as Gizmos.DrawLine().
		/// </summary>
		public static void DrawSegment( Vector2 p1, Vector2 p2 )
		{
			Gizmos.DrawLine( p1, p2 );
		}


		/// <summary>
		/// Draw a continous line that goes through points 'p1' and 'p2'.
		/// </summary>
		public static void DrawLine( Vector2 p1, Vector2 p2 )
		{
			Vector2 offset = ( p2 - p1 ) * 999f;
			Gizmos.DrawLine( p1 - offset, p1 + offset );
		}

		/// <summary>
		/// Draw a non-verical line using the line equation y = ax + b.
		/// </summary>
		public static void DrawNonVerticalLine( float a, float b )
		{
			const float x = 9999f;
			Gizmos.DrawLine( new Vector2( x, a * x + b ), new Vector2( -x, a * -x + b ) );
		}


		/// <summary>
		/// Draw a non-verical line using the line equation y = ax + b, where l.x is slope and l.y is y-intersect.
		/// </summary>
		public static void DrawNonVerticalLine( Vector2 l )
		{
			const float x = 9999f;
			Gizmos.DrawLine( new Vector2( x, l.x * x + l.y ), new Vector2( -x, l.x * -x + l.y ) );
		}


		/// <summary>
		/// Draw a verical line at x offset.
		/// </summary>
		public static void DrawVerticalLine( float x )
		{
			const float y = 9999f;
			Gizmos.DrawLine( new Vector2( x, -y ), new Vector2( x, y ) );
		}


		/// <summary>
		/// Draw a horizontal line at y offset.
		/// </summary>
		public static void DrawHorizontalLine( float y )
		{
			const float x = 9999f;
			Gizmos.DrawLine( new Vector2( -x, y ), new Vector2( x, y ) );
		}


		/// <summary>
		/// Draw an arc (angle in radians).
		/// </summary>
		public static void DrawWireArc( Vector3 center, float radius, float angleBegin, float angleDeltaSigned, int circleResolution = 64 )
		{
			List<Vector2> points = null;
			int resolution = Mathf.Max( 3, (int) ( circleResolution * Mathf.Abs( angleDeltaSigned ) / Trigonometry.Tau ) );
			Subdivision.ArcToPolylinePoints( center, radius, angleBegin, angleDeltaSigned, resolution, ref points );
			DrawPath( points );
		}


		/// <summary>
		/// Draw a circle.
		/// </summary>
		public static void DrawWireCircle( Vector2 center, float radius, int resolution = 128 )
		{
			List<Vector2> points = null;
			Generation.CirclePoints( center, radius, resolution, ref points );
			DrawPath( points, close: true );
		}


		/// <summary>
		/// Draw a rectangle.
		/// </summary>
		public static void DrawWireRect( Rect rect )
		{
			Vector2 min = rect.min;
			Vector2 max = rect.max;
			Gizmos.DrawLine( new Vector3( min.x, min.y ), new Vector3( min.x, max.y ) );
			Gizmos.DrawLine( new Vector3( min.x, max.y ), new Vector3( max.x, max.y ) );
			Gizmos.DrawLine( new Vector3( max.x, max.y ), new Vector3( max.x, min.y ) );
			Gizmos.DrawLine( new Vector3( max.x, min.y ), new Vector3( min.x, min.y ) );
		}


        /// <summary>
        /// Draw a rectangle centered at (0,0).
        /// </summary>
        public static void DrawWireRect( Vector2 size )
        {
            Vector2 extents = size * 0.5f;
            Gizmos.DrawLine( new Vector3( -extents.x, -extents.y ), new Vector3( -extents.x, extents.y ) );
            Gizmos.DrawLine( new Vector3( -extents.x, extents.y ), new Vector3( extents.x, extents.y ) );
            Gizmos.DrawLine( new Vector3( extents.x, extents.y ), new Vector3( extents.x, -extents.y ) );
            Gizmos.DrawLine( new Vector3( extents.x, -extents.y ), new Vector3( -extents.x, -extents.y ) );
        }


        /// <summary>
        /// Draw a wire triangle.
        /// </summary>
        public static void DrawWireTriangle( Vector2 a, Vector2 b, Vector2 c )
		{
			Gizmos.DrawLine( a, b );
			Gizmos.DrawLine( b, c );
			Gizmos.DrawLine( c, a );
		}


		/// <summary>
		/// Draw a flat arrow.
		/// </summary>
		public static void DrawArrow2D( Vector2 a, Vector2 b, float headSize )
		{
			Vector2 v = a - b;
			v.Normalize();
			v.Set( ( v.x - v.y ) * headSize, ( v.y + v.x ) * headSize );
			Gizmos.DrawLine( b, b + v );
			v = Operation.RotatePerpendicularRight( v );
			Gizmos.DrawLine( b, b + v );
			Gizmos.DrawLine( a, b );
		}
	}
}