/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using UnityEngine;
using System.Runtime.CompilerServices;

namespace Funk
{
	public static partial class Trigonometry
	{


		/// <summary>
		/// Projects vector A onto vector B and returns the distance along vector B.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Project( Vector2 vectorA, Vector2 vectorB )
		{
			float bMag = vectorB.x * vectorB.x + vectorB.y * vectorB.y;
			if( bMag <= 0f ) return 0f;
			bMag = Mathf.Sqrt( bMag );

			return ( vectorA.x * vectorB.x + vectorA.y * vectorB.y ) / bMag;
		}


		/// <summary>
		/// Projects vector A onto vector B and returns the normalized distance along vector B.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ProjectToNormalized( Vector2 vectorA, Vector2 vectorB )
		{
			float bMag = vectorB.x * vectorB.x + vectorB.y * vectorB.y;
			if( bMag <= 0f ) return 0f;
			bMag = Mathf.Sqrt( bMag );

			return ( vectorA.x * vectorB.x + vectorA.y * vectorB.y ) / bMag / bMag;
		}


		/// <summary>
		/// Projects vector A onto vector B and returns the resulting vector along vector B.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ProjectToVector( Vector2 vectorA, Vector2 vectorB )
		{
			float bMag = vectorB.x * vectorB.x + vectorB.y * vectorB.y;
			if( bMag <= 0f ) return Vector2.zero;
			bMag = Mathf.Sqrt( bMag );

			float d = ( vectorA.x * vectorB.x + vectorA.y * vectorB.y ) / bMag / bMag;
			return new Vector2( vectorB.x * d, vectorB.y * d );
		}

	}
}