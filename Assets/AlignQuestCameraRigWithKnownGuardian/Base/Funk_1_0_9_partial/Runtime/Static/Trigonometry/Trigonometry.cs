/*
	Copyright © Carl Emil Carlsen 2021
	http://cec.dk
*/

using UnityEngine;
using System.Runtime.CompilerServices;

namespace Funk
{
	public static partial class Trigonometry
	{
		public static float PI = Mathf.PI;
		public const float Tau = 6.28318530718f;
		public const float HalfPI = 1.57079632679f;
		public const float QuaterPI = 0.78539816339f;
		public const float Epsilon = 0.00001f;
		public const float OneMinusEpsilon = 1f - Epsilon;


		/// <summary>
		/// Lerp angle from a to b always returning positive values.
		/// Similar to Mathf.LerpAngle(), but avoids flipping to negative when delta is greater than PI.
		/// Angles in radians.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpAnglePositive( float a, float b, float t )
		{
			while( a < Tau ) {
				a += Tau;
				b += Tau;
			}
			while( b < a ) b += Tau;
			float angle = a + ( b - a ) * t;
			if( angle > Tau ) angle %= Tau;
			return angle;
		}
	}
}