/*
	Copyright © Carl Emil Carlsen 2022
	http://cec.dk
*/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace  Funk 
{
	public static partial class Subdivision
	{
		/// <summary>
		/// Subdivide circle.
		/// </summary>
		public static void CircleToPolygonPoints( Vector2 center, float radius, int resolution, ref List<Vector2> outputPoints, bool clearOutputList = true )
		{
			if( resolution == 0 ) return;

			if( outputPoints == null ) {
				outputPoints = new List<Vector2>( resolution );
			} else {
				if( clearOutputList ) outputPoints.Clear();
				if( outputPoints.Capacity < outputPoints.Count + resolution ) outputPoints.Capacity = outputPoints.Count + resolution;
			}

			float angleStep = Trigonometry.Tau / (float) resolution;
			for( int p = 0; p < resolution; p++ ) {
				float a = p * angleStep;
				outputPoints.Add( new Vector2( center.x + Mathf.Cos( a ) * radius, center.y + Mathf.Sin( a ) * radius ) );
			}
		}


		/// <summary>
		/// Subdivide arc into points. Angles in radians.
		/// </summary>
		public static void ArcToPolylinePoints( Vector2 center, float radius, float angleBegin, float angleDeltaSigned, int pointCount, ref List<Vector2> outputPoints, bool clearOutputList = true, bool includeFirst = true, bool includeLast = true )
		{
			if( pointCount == 0 ) return;

			if( outputPoints == null ) {
				outputPoints = new List<Vector2>( pointCount );
			} else {
				if( clearOutputList ) outputPoints.Clear();
				if( outputPoints.Capacity < outputPoints.Count + pointCount ) outputPoints.Capacity = outputPoints.Count + pointCount;
			}

			float angleStep = angleDeltaSigned / ( pointCount -1f );
			int pBegin = includeFirst ? 0 : 1;
			int pEnd = includeLast ? pointCount : pointCount - 1;
			for( int p = pBegin; p < pEnd; p++ ) {
				float a = angleBegin + p * angleStep;
				outputPoints.Add( new Vector2( center.x + Mathf.Cos( a ) * radius, center.y + Mathf.Sin( a ) * radius ) );
			}
		}



		/// <summary>
		/// Subdivide an outline with a fixed number of subdivisions per segment.
		/// </summary>
		public static void OutlinePerSegment( IList<Vector2> outline, int subdivisionsPerSegment, ref List<Vector2> outputOutline )
		{
			int count = outline.Count;
			if( count < 2 ) throw new System.Exception( "An outline needs at least two points." );

			int subdividedCount = outline.Count * subdivisionsPerSegment;
			if( outputOutline.Capacity < subdividedCount ) outputOutline.Capacity = subdividedCount;
			outputOutline.Clear();

			Vector2 p0 = outline[ count - 1 ];
			int segmentPointCount = subdivisionsPerSegment + 1;
			float step = 1f / (float) segmentPointCount;
            for( int p = 0; p < count; p++ )
			{
				Vector2 p1 = outline[ p ];
				for( int sp = 0; sp < segmentPointCount; sp++ ) outputOutline.Add( Vector2.Lerp( p0, p1, sp * step ) );
				p0 = p1;
			}
		}
	}
}