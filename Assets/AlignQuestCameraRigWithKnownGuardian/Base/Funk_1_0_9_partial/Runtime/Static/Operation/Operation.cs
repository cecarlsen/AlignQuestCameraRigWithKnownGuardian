/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/

using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Funk
{
	public static partial class Operation
	{
		/// <summary>
		/// Swap two elements.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static void Swap<T>( ref T a, ref T b )
		{
			T temp = a;
			a = b;
			b = temp;
		}


		/// <summary>
		/// Rotate a vector 90 degrees left.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static Vector2 RotatePerpendicularLeft( Vector2 v ) => new Vector2( -v.y, v.x );


		/// <summary>
		/// Rotate a vector 90 degrees right.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static Vector2 RotatePerpendicularRight( Vector2 v ) => new Vector2( v.y, -v.x );


		/// <summary>
		/// Rotate a vector 90 degrees left.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static void RotatePerpendicularLeft( ref float dx, ref float dy )
		{
			float temp = dx;
			dx = -dy;
			dy = temp;
		}


		/// <summary>
		/// Rotate a vector 90 degrees right.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static void RotatePerpendicularRight( ref float dx, ref float dy )
		{
			float temp = dx;
			dx = dy;
			dy = -temp;
		}


		/// <summary>
		/// Wrap angle between 0 and Tau.
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float WrapAngle( float angle )
		{
			if( angle < 0 ) return Trigonometry.Tau - ( ( (int) ( angle / Trigonometry.Tau ) * Trigonometry.Tau ) - angle );
			return angle % Trigonometry.Tau; // Perhaps this is faster: angle - ( ( int) angle / Tau ) * Tau;
		}
	}
}