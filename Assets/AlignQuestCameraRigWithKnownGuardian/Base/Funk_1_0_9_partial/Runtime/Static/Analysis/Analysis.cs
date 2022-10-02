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
		public const float Tau = 6.28318530718f;

		static readonly float rootOf2 = Mathf.Sqrt( 2f );
		const float Epsilon = 0.0000001f; // 7 digits.
		const float EpsilonAngular2 = 0.00001f;


		/// <summary>
		/// Computes the signed angular difference between angleA and angleB, in radians, wrapped between -PI and PI.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float DeltaAngleSigned( float angleA, float angleB )
		{
			float delta = Mathf.Repeat( angleB - angleA, Tau );
			if( delta > Mathf.PI ) delta -= Tau;
			return delta;
		}


		/// <summary>
		/// Computes the signed angular difference between angleA and angleB, in radians, wrapped between -PI and PI.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float DeltaAngleSigned( Vector2 vectorA, Vector2 vectorB )
		{
			// sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
			float denominator = (float) Math.Sqrt( vectorA.sqrMagnitude * vectorB.sqrMagnitude );
			if( denominator < Epsilon ) return 0f;

			float dot = ( vectorA.x * vectorB.x + vectorA.y * vectorB.y ) / denominator;
			float signF = ( vectorA.x * -vectorB.y + vectorA.y * vectorB.x ) / denominator;
			if( dot < -1f ) dot = -1f;
			else if( dot > 1f ) dot = 1f;
			float a = (float) Math.Acos( dot );
			if( signF > 0f ) a = -a;
			return a;
		}


		/// <summary>
		/// Computes the unsigned angular difference between angleA and angleB, in radians, wrapped between 0.0 and PI.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float DeltaAngle( float angleA, float angleB )
		{
			float delta = Mathf.Repeat( angleB - angleA, Tau );
			if( delta > Mathf.PI ) delta -= Tau;
			if( delta < 0 ) delta = -delta;
			return delta;
		}


		/// <summary>
		/// Computes the unsigned angular difference between vectorA and vectorB, in radians, wrapped between 0.0 and PI.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float DeltaAngle( Vector2 vectorA, Vector2 vectorB )
		{
			// sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
			float denominator = (float) Math.Sqrt( vectorA.sqrMagnitude * vectorB.sqrMagnitude );
			if( denominator < Epsilon ) return 0f;

			float dot = ( vectorA.x * vectorB.x + vectorA.y * vectorB.y ) / denominator;
			if( dot < -1f ) dot = -1f;
			else if( dot > 1f ) dot = 1f;
			return (float) Math.Acos( dot );
		}


		/// <summary>
		/// Evaluates if two vectors are pointing in exactly the same direction. If any vector is zero, the result is false.
		/// </summary>
		public static bool AreVectorsSameDirection( Vector2 v0, Vector2 v1 )
		{
			// First check sign.
			if( v0.x > 0f != v1.x > 0f || v0.y > 0f != v1.y > 0f ) return false;

			// Then check for horizontal and vertical cases.
			if( v0.x > -Epsilon && v0.x < Epsilon ){
				if( v0.y > -Epsilon && v0.y < Epsilon || v1.y > -Epsilon && v1.y < Epsilon ) return false; // Zero case.
				return v1.x > -Epsilon && v1.x < Epsilon;
			}
			if( v0.y > -Epsilon && v0.y < Epsilon ){
				if( v0.x > -Epsilon && v0.x < Epsilon || v1.x > -Epsilon && v1.x < Epsilon ) return false; // Zero case.
				return v1.y > -Epsilon && v1.y < Epsilon;
			}

			// Then compare slope.
			float slopeDiff = ( v0.y / v0.x ) - ( v1.y / v1.x );
			return slopeDiff > -Epsilon && slopeDiff < Epsilon;
		}


		/// <summary>
		/// Evaluates if two vectors are pointing in exactly the opposite direction. If any vector is zero, the result is false.
		/// </summary>
		public static bool AreVectorsOppositeDirection( Vector2 v0, Vector2 v1 )
		{
			// First check sign.
			if(  ( v0.x > 0f == v1.x > 0f && v0.x != 0f ) || ( v0.y > 0f == v1.y > 0f  && v0.y != 0f ) ) return false;

			// Then check for horizontal and vertical cases.
			if( v0.x > -Epsilon && v0.x < Epsilon ){
				if( v0.y > -Epsilon && v0.y < Epsilon || v1.y > -Epsilon && v1.y < Epsilon ) return false; // Zero case.
				return v1.x > -Epsilon && v1.x < Epsilon;
			}
			if( v0.y > -Epsilon && v0.y < Epsilon ){
				if( v0.x > -Epsilon && v0.x < Epsilon || v1.x > -Epsilon && v1.x < Epsilon ) return false; // Zero case.
				return v1.y > -Epsilon && v1.y < Epsilon;
			}

			// Then compare slope.
			float slopeDiff = ( v0.y / v0.x ) - ( v1.y / v1.x );
			return slopeDiff > -Epsilon && slopeDiff < Epsilon;
		}



		/// <summary>
		/// Evaluates whether a point is located on a line going through points 'a' and 'b'.
		/// </summary>
		public static bool IsPointOnLine( Vector2 point, Vector2 a, Vector2 b )
		{
			// Using y-interception.
			// http://www.sunshine2k.de/coding/java/PointOnLine/PointOnLine.html

			// First check for vertical and horisontal line cases.
			float dx = b.x - a.x;
			if( dx < Epsilon && dx > -Epsilon ) return point.x > a.x - Epsilon && point.x < a.x + Epsilon; // Vertical line.
			float dy = b.y - a.y;
			if( dy < Epsilon && dy > -Epsilon ) return point.y > a.y - Epsilon && point.y < a.y + Epsilon; // Horizontal line.

			// Compare the y-axis intercept for line the the y-axis intercept of the point using the same slope.
			float slope = dy / dx;
			float yIntercept = a.y - a.x * slope;
			float yInterceptP = point.y - slope * point.x;
			return yInterceptP > yIntercept - Epsilon && yInterceptP < yIntercept + Epsilon;
		}


		/// <summary>
		/// Evaluates whether a point is located on a line segment going from point 'a' to point 'b'.
		/// </summary>
		public static bool IsPointOnSegment( float pointX, float pointY, float ax, float ay, float bx, float by )
		{
			// Using the perpendicular dot product.
			// http://www.sunshine2k.de/coding/java/PointOnLine/PointOnLine.html

			// First check bounding box.
			if(
				!( ( pointX > ax-Epsilon && pointX < bx+Epsilon ) || ( pointX > bx-Epsilon && pointX < ax+Epsilon ) ) ||	// Outside horizontal bounds.
				!( ( pointY > ay-Epsilon && pointY < by+Epsilon ) || ( pointY > by-Epsilon && pointY < ay+Epsilon ) )		// Outside vertical bounds.
			){
				return false;
			}

			// Then check if the area of the parallelogram formed by vectors a->b and a->point is zero.
			// The area of a parallelogram is computed using the perp dod product.
			float dx = bx - ax;
			float dy = by - ay;
			float area = dx * ( ay - pointY ) + dy * ( pointX - ax );


			// Floating point precision varies with the length of the vector, so we need a dynamic epsilon based on the vector.
			float epsilon = 0.0000001f * ( dx * dx + dy * dy );
			return area > - epsilon && area < epsilon;
		}

		/// <summary>
		/// Evaluates whether a point is located on a line segment going from point 'a' to point 'b'.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static bool IsPointOnSegment( Vector2 point, Vector2 a, Vector2 b )
		{
			return IsPointOnSegment( point.x, point.y, a.x, a.y, b.x, b.y );
		}


		/// <summary>
		/// Centroid of a bunch of points.
		/// </summary>
		public static Vector2 Centroid( IList<Vector2> points )
		{
			if( points == null || points.Count == 0 ) return Vector2.zero;

			float x = 0;
			float y = 0;
			int count = points.Count;
			for( int p = 0; p < count; p++ ) {
				Vector2 point = points[ p ];
				x += point.x;
				y += point.y;
			}
			x /= (float) count;
			y /= (float) count;
			return new Vector2( x, y );
		}


		/// <summary>
		/// Length of the diagonal in a square.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float SquareDiagonalLength( float squareSideLength )
		{
			return squareSideLength * rootOf2;
		}
	}
}