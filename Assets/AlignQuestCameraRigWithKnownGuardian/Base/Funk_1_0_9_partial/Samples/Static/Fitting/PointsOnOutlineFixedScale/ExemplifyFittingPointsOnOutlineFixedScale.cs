/*
	Copyright © Carl Emil Carlsen 2022
	http://cec.dk
*/

using System.Collections.Generic;
using UnityEngine;

namespace Funk.Samples
{
	public class ExemplifyFittingPointsOnOutlineFixedScale : MonoBehaviour
	{
		public int randomSeed = 0;
		[Range(0.1f,5f)] public float angularStep = 5;
		public bool performPostAdjustment = false;

		[Header("Source Outline")]
		[Range(3,100)] public int outlinePointCount = 10;
        [Range(0f,1f)] public float perlinNoiseOffset = 0.5f;
		[Range(0f,0.5f)] public float perlinNoiseFreq = 0f;
		public Vector2 outlineOffset = Vector2.zero;
        
		[Header( "Source Points" )]
        [Range(1,5)] public int pointsSubdivisionsPerSegment = 3;
		[Range(0f,0.5f)] public float pointsRandomOffset = 0f;
		public Vector2 pointsTranslation = new Vector2( 0.1f, 0.1f );
        [Range(0f,360f)] public float pointsAngle = 70;


        void OnDrawGizmos()
		{
			// Create input data.
			List<Vector2> outline = null;
			Generation.CirclePoints( Vector2.zero, 1, outlinePointCount, ref outline );
			for( int p = 0; p < outline.Count; p++ ) outline[ p ] += outlineOffset;
            Generation.AddPerlinNoise( outline, perlinNoiseOffset, perlinNoiseFreq, Vector2.right * randomSeed );
			List<Vector2> points = new List<Vector2>();
			Subdivision.OutlinePerSegment( outline, pointsSubdivisionsPerSegment, ref points );
			Generation.AddRandomOffset( points, randomSeed, pointsRandomOffset );
			Matrix3x3 pointsTransform = Matrix3x3.Translate( pointsTranslation ) * Matrix3x3.Rotate( pointsAngle * Mathf.Deg2Rad );
            for( int p = 0; p < points.Count; p++ ) points[ p ] = pointsTransform * points[ p ];

			// Compute.
			Vector2 translation;
			float rotation;
			Fitting.PointsOnOutlineFixedScale( points, outline, out translation, out rotation, angularStep * Mathf.Deg2Rad, performPostAdjustment );
            Matrix3x3 fittingTransform = Matrix3x3.Rotate( rotation ) * Matrix3x3.Translate( translation );
            //Matrix3x3 fittingTransform = Matrix3x3.Translate( translation ) * Matrix3x3.Rotate( rotation );
            List<Vector2> fittedPoints = new List<Vector2>( points.Count );
			foreach( Vector2 p in points ) fittedPoints.Add( fittingTransform * p );
            //foreach( Vector2 p in points ) fittedPoints.Add( Matrix3x3.Rotate( rotation ) * ( Matrix3x3.Translate( translation ) * p ) );

            // Draw.
            Gizmos.color = Color.white;
			Gizmotion.DrawOutline( outline );
            Gizmos.color = Color.yellow;
            Gizmotion.DrawOutline( points );
            Gizmotion.DrawPoints( points, 0.04f );
            Gizmos.color = Color.green;
            Gizmotion.DrawOutline( fittedPoints );
            Gizmotion.DrawPoints( fittedPoints, 0.04f );

            // Zero.
            Gizmos.color = new Color( 1, 1, 1, 0.1f );
			Gizmos.DrawLine( Vector2.up, Vector2.down );
            Gizmos.DrawLine( Vector2.left, Vector2.right );
        }
	}
}