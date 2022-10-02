/*
	Copyright © Carl Emil Carlsen 2022
	http://cec.dk
*/

using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
	public static partial class Analysis
	{
		/// <summary>
		/// Find convex hull using the Quickhull algorithm.
		/// </summary>
		public static void ConvexHull( IList<Vector2> points, ref List<Vector2> convexHull, int iterations = 99999 )
		{
			QuickhullAlgorithm.Execute( points, ref convexHull, iterations );
		}


		static class QuickhullAlgorithm
		{
			// Based on explanation by Dirk Gregorius from Valve at GDC 2014.
			// https://www.gdcvault.com/play/1020606/Physics-for-Game-Programmers
			// http://media.steampowered.com/apps/valve/2014/DirkGregorius_ImplementingQuickHull.pdf
			

			static float epsilon = 0f;


			public static void Execute( IList<Vector2> points, ref List<Vector2> convexHull, int iterations )
			{
				if( convexHull == null ) convexHull = new List<Vector2>();
				else convexHull.Clear();

				if( points.Count < 3 ) return;

				HashSet<Vector2> conflictingPoints = new HashSet<Vector2>( points.Count );
				foreach( var point in points ) conflictingPoints.Add( point );

				// Find bounding points.
				Vector2 xMin = points[ 0 ];
				Vector2 xMax = xMin;
				Vector2 yMin = xMin;
				Vector2 yMax = xMin;
				for( int p = 1; p < points.Count; p++ ) {
					Vector2 point = points[ p ];
					if( point.x < xMin.x ) xMin = point;
					else if( point.x > xMax.x ) xMax = point;
					if( point.y > yMax.y ) yMax = point;
					else if( point.y < yMin.y ) yMin = point;
				}

				// Update epsilon. See about 16:00 minutes into the video above.
				epsilon = 2f * ( Mathf.Max( Mathf.Abs( xMax.x ), Mathf.Abs( xMin.x ) ) + Mathf.Max( Mathf.Abs( yMin.y ), Mathf.Abs( yMax.y ) ) ) * 0.000001f; // 0.000001f found by experimentation.

				// Choose the pair furhest apart (the horizon points).
				Vector2 a, b;
				if( xMax.x - xMin.x > yMax.y - yMin.y ) {
					a = xMin;
					b = xMax;
				} else {
					a = yMin;
					b = yMax;
				}

				// Transfer from conflicting to hull.
				conflictingPoints.Remove( a );
				conflictingPoints.Remove( b );
				convexHull.Add( a );
				convexHull.Add( b );

				// Partition conflicing points on each side of a->b.
				HashSet<Vector2> conflictingPointsLeft, conflictingPointsRight;
				PartitionPoints( a, b, conflictingPoints, out conflictingPointsLeft, out conflictingPointsRight );

				// Start recursing.
				int indexB = 1;
				int iter = 0;
				int addedPoints = 0;
				if( conflictingPointsRight.Count > 0 ) addedPoints += Recurse( a, b, indexB, convexHull, conflictingPointsRight, ref iter, iterations );
				if( conflictingPointsLeft.Count > 0 ) {
					indexB += 1 + addedPoints;
					Recurse( b, a, indexB, convexHull, conflictingPointsLeft, ref iter, iterations );
				}

				//Gizmos.color = Color.magenta;
				//Gizmotion.DrawLine( a, b );
			}


			static void PartitionPoints( Vector2 a, Vector2 b, HashSet<Vector2> points, out HashSet<Vector2> pointsLeft, out HashSet<Vector2> pointsRight )
			{
				pointsLeft = new HashSet<Vector2>( points.Count );
				pointsRight = new HashSet<Vector2>( points.Count );
				Vector2 abRight = Operation.RotatePerpendicularRight( b - a );
				foreach( var p in points ) {
					Vector2 ap = p - a;
					float dot = abRight.x * ap.x + abRight.y * ap.y;
					// "Fat plane". For now, if point is inside the plane we just exclude it.
					if( dot > epsilon ) // Use dot to find side.
						pointsRight.Add( p );
					else if( dot < -epsilon )
						pointsLeft.Add( p );
				}
			}


			static int Recurse( Vector2 a, Vector2 b, int insertIndex, List<Vector2> convexHull, HashSet<Vector2> conflictingPoints, ref int iter, int iterMax )
			{
				iter++;
				if( iter > iterMax ) return 0; // Safety third.
				//Debug.Log( "iter: " + iter + ", depth: " + depth + ", insertIndex: " + insertIndex + ", convexHull: " + convexHull.Count + "\n" );

				// Find point furthest away from line between a and b presuming all incoming conflicking points are on the right side line of a->b.
				float dMax = epsilon;
				Vector2 c = Vector2.zero;
				bool foundAnyPoint = false;
				foreach( var point in conflictingPoints )
				{
					float sd = PointToLineDistanceSigned( point, a, b ); // TODO: could we do with a squared distance and avoid Sqrt()?
					//if( sd < 0f ) Debug.LogWarning( "Not right: " + sd );
					if( sd > dMax ) {
						dMax = sd;
						c = point;
						foundAnyPoint = true;
					}
				}
				if( !foundAnyPoint ) return 0;

				// Remove point from conflicting and add it to the hull.
				conflictingPoints.Remove( c );
				convexHull.Insert( insertIndex, c );

				// Remove conflicking points that lie inside the triangle formed by a-c-b.
				conflictingPoints.RemoveWhere( ( p ) => IsInsideTriangle( a, b, c, p ) );

				// Find point that divides the new triangle by projecting ac onto ab.
				Vector2 cProj = a + Trigonometry.ProjectToVector( c - a, b - a );

				//Gizmos.color = Color.cyan;
				//Gizmos.DrawSphere( c, 0.05f );
				//Gizmos.DrawLine( cProj, c );
				//Gizmos.color = Color.gray;
				//Gizmos.DrawLine( a, c );
				//Gizmos.DrawLine( b, c );

				// Partition conflicing points on each side of a->b.
				HashSet<Vector2> conflictingPointsLeft, conflictingPointsRight;
				PartitionPoints( cProj, c, conflictingPoints, out conflictingPointsLeft, out conflictingPointsRight );

				// Dive recursively into right side.
				int addedPoints = 1;
				if( conflictingPointsRight.Count > 0 ) {
					//Debug.Log( "RIGHT\n" );
					addedPoints += Recurse( a, c, insertIndex, convexHull, conflictingPointsRight, ref iter, iterMax );
				}

				// Increase index only when exit processing of a right side.
				insertIndex += addedPoints;

				// Dive recursively into the left side.
				if( conflictingPointsLeft.Count > 0 ) {
					//Debug.Log( "LEFT\n" );
					addedPoints += Recurse( c, b, insertIndex, convexHull, conflictingPointsLeft, ref iter, iterMax );
				}

				return addedPoints;
			}
		}

	}
}