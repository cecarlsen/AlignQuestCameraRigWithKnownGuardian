/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
	public static partial class Generation
	{
		/// <summary>
		/// Fills a list with circle points ordered anti-clockwise.
		/// </summary>
		public static void CirclePoints( Vector2 center, float radius, int pointCount, ref List<Vector2> points, bool clearOutputList = true )
		{
			Subdivision.CircleToPolygonPoints( center, radius, pointCount, ref points, clearOutputList );
		}


		/// <summary>
		/// Returns circle points ordered anti-clockwise.
		/// </summary>
		public static List<Vector2> CirclePoints( Vector2 center, float radius, int pointCount )
		{
			List<Vector2> points = null;
			Subdivision.CircleToPolygonPoints( center, radius, pointCount, ref points );
			return points;
		}


		/// <summary>
		/// Fills a list with arc points ordered anti-clockwise.
		/// </summary>
		public static void ArcPoints( Vector2 center, float radius, float angleBegin, float deltaAngleSigned, int pointCount, ref List<Vector2> points, bool clearOutputList = true )
		{
			Subdivision.ArcToPolylinePoints( center, radius, angleBegin, deltaAngleSigned, pointCount, ref points, clearOutputList );
		}


		/// <summary>
		/// Returns arc points ordered anti-clockwise.
		/// </summary>
		public static List<Vector2> ArcPoints( Vector2 center, float radius, float angleBegin, float deltaAngleSigned, int pointCount )
		{
			List<Vector2> points = null;
			Subdivision.ArcToPolylinePoints( center, radius, angleBegin, deltaAngleSigned, pointCount, ref points );
			return points;
		}


		/// <summary>
		/// Add perlinnoise offset to a list of points.
		/// </summary>
		public static void AddPerlinNoise( IList<Vector2> points, float amplitude, float frequency, Vector2 sampleOffset )
		{
			for( int p = 0; p < points.Count; p++ ) {
				Vector2 point = points[ p ];
				float x = point.x + sampleOffset.x;
				float y = point.y + sampleOffset.y;
				points[ p ] = new Vector2(
					point.x + ( Mathf.PerlinNoise( x + point.x * frequency, y + 17.164f ) * 2f - 1f ) * amplitude,
					point.y + ( Mathf.PerlinNoise( x + 17.1736193f, y + point.y * frequency ) * 2f - 1f ) * amplitude
				);
			}
		}


		/// <summary>
		/// Add random offset to a list of points.
		/// </summary>
		public static void AddRandomOffset( IList<Vector2> points, int randomSeed, float offsetMax )
		{
			Random.State prevState = Random.state;
			Random.InitState( randomSeed );
			for( int p = 0; p < points.Count; p++ ) points[ p ] += Random.insideUnitCircle * offsetMax;
			Random.state = prevState;
		}
	}
}