/*
	Copyright © Carl Emil Carlsen 2017-2022
	http://cec.dk
*/

using System;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Funk
{
	[System.Serializable]
	public struct Matrix3x3 : IEquatable<Matrix3x3>
	{
		// m (row,col)
		public float
			m00, m01, m02, // Row 0
			m10, m11, m12, // Row 1
			m20, m21, m22; // Row 2

		const float Epsilon	= 0.000001f;


		/// <summary>
		/// Get the determinant of the matrix.
		/// </summary>
		public float determinant => // https://www.mathsisfun.com/algebra/matrix-determinant.html
			  m00 * ( m11 * m22 - m21 * m12 )
			- m10 * ( m01 * m22 - m21 * m02 )
			+ m20 * ( m01 * m12 - m11 * m02 );


		/// <summary>
		/// Get the inverse of the matrix.
		/// Returns true if an inverse matrix exists for this matrix.
		/// </summary>
		public Matrix3x3 inverse {
			get {
				Matrix3x3 i = this; // Copy.
				if( !i.Invert() ) i = identity;
				return i;
			}
		}


		/// <summary>
		/// Get a transposed version of this matrix (swapped columns and rows).
		/// </summary>
		public Matrix3x3 transpose {
			get {
				Matrix3x3 m = this; // Copy
				m.m10 = m01;
				m.m01 = m01;
				m.m21 = m12;
				m.m12 = m21;
				m.m20 = m02;
				m.m20 = m02;
				return m;
			}
		}

		/// <summary>
		/// Get the identity matrix.
		/// </summary>
		public static Matrix3x3 identity => new Matrix3x3(){ m00 = 1f, m11 = 1f, m22 = 1f };


		public Matrix3x3
		(
			float m00, float m01, float m02,
			float m10, float m11, float m12,
			float m20, float m21, float m22
		) {
			this.m00 = m00;
			this.m01 = m01;
			this.m02 = m02;
			this.m10 = m10;
			this.m11 = m11;
			this.m12 = m12;
			this.m20 = m20;
			this.m21 = m21;
			this.m22 = m22;
		}

		public Matrix3x3( float[,] m )
		{
			if( m.GetLength( 0 ) != 3 || m.GetLength( 1 ) != 3 ) throw new Exception( "Jagged array dimensions must be 3x3." );

			this.m00 = m[0,0];
			this.m01 = m[0,1];
			this.m02 = m[0,2];
			this.m10 = m[1,0];
			this.m11 = m[1,1];
			this.m12 = m[1,2];
			this.m20 = m[2,0];
			this.m21 = m[2,1];
			this.m22 = m[2,2];
		}


		public Matrix3x3( Matrix4x4 m )
		{
			m00 = m.m00;
			m01 = m.m01;
			m02 = m.m02;
			m10 = m.m10;
			m11 = m.m11;
			m12 = m.m12;
			m20 = m.m20;
			m21 = m.m21;
			m22 = m.m22;
		}

		/*
		/// <summary>
		/// Create a new Matrix3x3 from a MatrixNxN matrix.
		/// If dimensions don't match an exception will be thrown.
		/// </summary>
		public Matrix3x3( MatrixNxN m )
		{
			if( m.cols != 3 || m.rows != 3 ) throw new Exception( "Dimensions must match." );

			m00 = m.m[0,0];
			m01 = m.m[0,1];
			m02 = m.m[0,2];
			m10 = m.m[1,0];
			m11 = m.m[1,1];
			m12 = m.m[1,2];
			m20 = m.m[2,0];
			m21 = m.m[2,1];
			m22 = m.m[2,2];
		}


		/// <summary>
		/// Overwrite this matrix with another one.
		/// </summary>
		public void Set( Matrix3x3 m )
		{
			m00 = m.m00;
			m01 = m.m01;
			m02 = m.m02;
			m10 = m.m10;
			m11 = m.m11;
			m12 = m.m12;
			m20 = m.m20;
			m21 = m.m21;
			m22 = m.m22;
		}
		*/


		/// <summary>
		/// Transform vector by this matrix.
		/// </summary>
		public Vector3 MultiplyVector( Vector3 v )
		{
			Vector3 res;
			res.x = m00 * v.x + m01 * v.y + m02 * v.z;
			res.y = m10 * v.x + m11 * v.y + m12 * v.z;
			res.z = m20 * v.x + m21 * v.y + m22 * v.z;
			return res;
		}


		/// <summary>
		/// Transpose this matrix (swap columns and rows).
		/// </summary>
		public void Transpose()
		{
			Matrix3x3 m = this; // Copy
			m.m10 = m01;
			m.m01 = m01;
			m.m21 = m12;
			m.m12 = m21;
			m.m20 = m02;
			m.m20 = m02;
			this = m;
		}


		/// <summary>
		/// Invert this matrix. Returns true if succeeded.
		/// </summary>
		public bool Invert()
		{
			// What is inversion:
			// https://www.mathsisfun.com/algebra/matrix-inverse.html

			// Solving using Minors, Cofactors and Adjugate.
			// https://www.mathsisfun.com/algebra/matrix-inverse-minors-cofactors-adjugate.html

			float d11 = m11 * m22 - m12 * m21;
			float d12 = m10 * m22 - m12 * m20;
			float d13 = m10 * m21 - m11 * m20;

			float d = 
				  m00 * d11 
				- m01 * d12
				+ m02 * d13;
			if( d > -Mathf.Epsilon && d < Mathf.Epsilon ) return false;
			d = 1f / d;

			float d21 = m01 * m22 + m02 * -m21;
			float d22 = m00 * m22 + m02 * -m20;
			float d23 = m00 * m21 + m01 * -m20;

			float d31 = m01 * m12 - m02 * m11;
			float d32 = m00 * m12 - m02 * m10;
			float d33 = m00 * m11 - m01 * m10;

			m00 =  d11 * d; m01 = -d21 * d; m02 =  d31 * d;
			m10 = -d12 * d; m11 =  d22 * d; m12 = -d32 * d;
			m20 =  d13 * d; m21 = -d23 * d; m22 =  d33 * d;

			return true;
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public bool Equals( Matrix3x3 m )
		{
			return 
				m.m00 > this.m00 - Epsilon && m.m00 < this.m00 + Epsilon &&
				m.m01 > this.m01 - Epsilon && m.m01 < this.m01 + Epsilon &&
				m.m02 > this.m02 - Epsilon && m.m02 < this.m02 + Epsilon &&
				m.m10 > this.m10 - Epsilon && m.m10 < this.m10 + Epsilon &&
				m.m11 > this.m11 - Epsilon && m.m11 < this.m11 + Epsilon &&
				m.m12 > this.m12 - Epsilon && m.m12 < this.m12 + Epsilon &&
				m.m20 > this.m20 - Epsilon && m.m20 < this.m20 + Epsilon &&
				m.m21 > this.m21 - Epsilon && m.m21 < this.m21 + Epsilon &&
				m.m22 > this.m22 - Epsilon && m.m22 < this.m22 + Epsilon;
		}


		/// <summary>
		/// Return matrix as flattend array.
		/// </summary>
		public float[] ToArray()
		{
			return new float[]{
				m00, m01, m02,
				m10, m11, m12,
				m20, m21, m22,
			};
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public override bool Equals( object o )
		{
			if( o is Matrix3x3 ) return Equals( (Matrix3x3) o );
			return false;
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public override int GetHashCode()
		{
			return
				m00.GetHashCode() ^ m01.GetHashCode() ^ m02.GetHashCode() ^
				m10.GetHashCode() ^ m11.GetHashCode() ^ m12.GetHashCode() ^
				m20.GetHashCode() ^ m21.GetHashCode() ^ m22.GetHashCode();
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static Matrix3x3 operator*( Matrix3x3 l, Matrix3x3 r )
		{
			return Multiply( l, r );
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static Vector3 operator*( Matrix3x3 m, Vector3 v )
		{
			return m.MultiplyVector( v );
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static Vector2 operator *( Matrix3x3 m, Vector2 v )
		{
			return m.MultiplyVector( new Vector3( v.x, v.y, 1f ) );
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static bool operator==( Matrix3x3 a, Matrix3x3 b )
		{
			return Equals( a, b );
		}


		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static bool operator!=( Matrix3x3 a, Matrix3x3 b )
		{
			return !Equals( a, b );
		}


		public override string ToString()
		{
			return 
			m00 + ", " + m01 + ", " + m02 + ",\n" + 
			m10 + ", " + m11 + ", " + m12 + ",\n" + 
			m20 + ", " + m21 + ", " + m22;
		}


		/// <summary>
		/// Multiply two matrics.
		/// </summary>
		public static Matrix3x3 Multiply( Matrix3x3 l, Matrix3x3 r )
		{
			Matrix3x3 m;

			m.m00 = l.m00 * r.m00 + l.m01 * r.m10 + l.m02 * r.m20;
			m.m01 = l.m00 * r.m01 + l.m01 * r.m11 + l.m02 * r.m21;
			m.m02 = l.m00 * r.m02 + l.m01 * r.m12 + l.m02 * r.m22;

			m.m10 = l.m10 * r.m00 + l.m11 * r.m10 + l.m12 * r.m20;
			m.m11 = l.m10 * r.m01 + l.m11 * r.m11 + l.m12 * r.m21;
			m.m12 = l.m10 * r.m02 + l.m11 * r.m12 + l.m12 * r.m22;

			m.m20 = l.m20 * r.m00 + l.m21 * r.m10 + l.m22 * r.m20;
			m.m21 = l.m20 * r.m01 + l.m21 * r.m11 + l.m22 * r.m21;
			m.m22 = l.m20 * r.m02 + l.m21 * r.m12 + l.m22 * r.m22;

			return m;
		}


		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		public static Matrix3x3 Translate( Vector2 translation )
		{
			return new Matrix3x3(
				1f, 0f, translation.x,
				0, 1f, translation.y,
				0f, 0f, 1f
			);
		}


		/// <summary>
		/// Creates a rotation matrix. Angles in radians.
		/// </summary>
		public static Matrix3x3 Rotate( float angleZ )
		{
			float s = (float) Math.Sin( angleZ );
			float c = (float) Math.Cos( angleZ );
			return new Matrix3x3(
				c, -s, 0f,
				s, c, 0f,
				0f, 0f, 1f
			);
		}


		/// <summary>
		/// Creates a scale matrix.
		/// </summary>
		public static Matrix3x3 Scale( Vector2 scale )
		{
			return new Matrix3x3(
				scale.x, 0, 0f,
				0, scale.y, 0f,
				0f, 0f, 1f
			);
		}


        /// <summary>
        /// Creates a scale matrix.
        /// </summary>
        public static Matrix3x3 Scale( float scale )
        {
            return new Matrix3x3(
                scale, 0, 0f,
                0, scale, 0f,
                0f, 0f, 1f
            );
        }


        /// <summary>
        /// Creates a translation->rotation->scale matrix.
        /// </summary>
        public static Matrix3x3 TRS( Vector2 translation, float angleZ, Vector2 scale )
        {
			return Scale( scale ) * Rotate( angleZ ) * Translate( translation );
        }
    }
}