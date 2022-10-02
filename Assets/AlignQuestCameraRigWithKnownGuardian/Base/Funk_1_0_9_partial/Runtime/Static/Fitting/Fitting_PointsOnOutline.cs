/*
	Copyright © Carl Emil Carlsen 2022
	http://cec.dk
*/

using System.Collections.Generic;
using UnityEngine;

namespace  Funk
{
	public static partial class Fitting
	{
        /// <summary>
        /// Find the translation and rotation that minimizes the distance of a set of points to an outline without applying scaling.
        /// Angles in radians.
        /// </summary>
        public static void PointsOnOutlineFixedScale( IList<Vector2> points, IList<Vector2> outline, out Vector2 translation, out float angle, float angularStep = Trigonometry.Tau / 360 )
		{
			// Homebrewed algorithm, not an optimal solution.
			// 1) Place the points and outline on top of each other using center of mass of the convex hulls. 
			// 2) Rotate a full circle in steps while testing the points distance to the outline.

			angle = 0;

			// Find center of mass of convex hull for both points and outline.
			List<Vector2> tempPoints = new List<Vector2>();
			Analysis.ConvexHull( points, ref tempPoints );
			Vector2 pointsCenter = Analysis.PolygonCenterOfMass( tempPoints );
            Analysis.ConvexHull( outline, ref tempPoints );
			Vector2 outlineCenter = Analysis.PolygonCenterOfMass( tempPoints );

            // Center both points and the outline temporarily.
            for( int p = 0; p < points.Count; p++ ) points[ p ] -= pointsCenter;
            for( int p = 0; p < outline.Count; p++ ) outline[ p ] -= outlineCenter;

            // Find the best rotation.
            float minAccSqDist = float.MaxValue;
            int stepCount = Mathf.CeilToInt( Trigonometry.Tau / angularStep );
			angularStep = Trigonometry.Tau / stepCount; // Always divide full rotation in equal steps.
            for( int s = 0; s < stepCount; s++ )
			{
				float a = s * angularStep;
                Matrix3x3 testRotation = Matrix3x3.Rotate( a );
				float accSqDist = 0;
				foreach( Vector2 p in points )
				{
					accSqDist += Analysis.PointToOutlineSquareDistance( testRotation * p, outline );
				}
				if( accSqDist < minAccSqDist ){
					minAccSqDist = accSqDist;
					angle = a;
                }
			}

            // Compensate for rotation around outline pivot.
			translation = outlineCenter - pointsCenter;
            translation -= outlineCenter - Matrix3x3.Rotate( -angle ) * outlineCenter;

            // Translate points and outline back to where they were.
			// This may be more performant than creating and garbaging new arrays.
            for( int p = 0; p < points.Count; p++ ) points[ p ] += pointsCenter;
            for( int p = 0; p < outline.Count; p++ ) outline[ p ] += outlineCenter;
        }
    }
}