/*
	Copyright © Carl Emil Carlsen 2022
	http://cec.dk

	"GetGeometry is an OVR method, but OVR is deprecated as of v32-33 in favor of OpenXR. The  latest features (passthrough) are in openXR. OVR and OpenXR are incompatible."
	https://forums.oculusvr.com/t5/Quest-Development/Getting-boundary-points/td-p/883042
*/

// Disable warning 'Deprecated. This enum value will not be supported in OpenXR'
// Yes we know. But there is currently no other way to get the boundary. https://forum.unity.com/threads/can-we-reuse-user-s-vr-boundaries.818331/#post-8479355
#pragma warning disable CS0618

using Funk;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlignQuestCameraRigWithKnownGuardian : MonoBehaviour
{
	[SerializeField] OVRCameraRig _cameraRig = null;
	[SerializeField] Transform[] _expectedGuardianTransforms = null;

	[Header("PlayerGizmos")]
	public bool showExpectedGuardian = true;
	public bool showActualGuardian = true;

	bool _initiated;
	bool _aligned;

	Vector3[] _expectedPoints;
	Vector3[] _guardianPoints;

	const string logPrepend = "<b>[" + nameof( AlignQuestCameraRigWithKnownGuardian ) + "]</b> ";


    void Update()
	{

		if( _expectedGuardianTransforms != null ) ExtractPositions( _expectedGuardianTransforms, ref _expectedPoints );
		
		if( OVRManager.OVRManagerinitialized && _cameraRig )
		{
			if( !_initiated ) OverrideQuestInitialisation();
			if( !_aligned && OVRManager.boundary.GetConfigured() ) Align();
        }

		OnDrawPlayerGizmos();
    }

	
    void OverrideQuestInitialisation()
	{
		// Make sure that we realign if the user attempts to recenter pose.
		OVRManager.instance.AllowRecenter = false;
        //OVRManager.display.RecenteredPose += () => _aligned = false; // Just in case.

		// Disable OVRPlayerController to avoid conflict.
		OVRPlayerController playerController = FindObjectOfType<OVRPlayerController>();
		if( playerController ) playerController.enabled = false;

		// Force floor level.
		OVRManager.instance.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;

		_initiated = true;
	}


	void Align()
	{
		// Get Quest Guardian boundery points.
		_guardianPoints = OVRManager.boundary.GetGeometry( OVRBoundary.BoundaryType.OuterBoundary );

		// Find translation and rotation that will align the guardian with the expected points.
		Vector2[] guardianPoints2D = Vector3XZToVector2( _guardianPoints );
		Vector2[] expectedPoints2D = Vector3XZToVector2( _expectedPoints );
		Vector2 translation;
		float rotation;
		Fitting.PointsOnOutlineFixedScale( guardianPoints2D, expectedPoints2D, out translation, out rotation, Mathf.PI * 2 / 360f, performPostAdjustment: true );

		// Apply transformation.
		Transform camRigTransform = _cameraRig.transform;
		camRigTransform.position = new Vector3( translation.x, camRigTransform.position.y, translation.y );
		camRigTransform.RotateAround( Vector3.zero, Vector2.up, -rotation * Mathf.Rad2Deg );

        //camRigTransform.rotation = Quaternion.Euler( 0, -rotation * Mathf.Rad2Deg, 0 );

        _aligned = true;
	}


	void OnDrawPlayerGizmos()
	{
		if( _expectedPoints != null && _expectedPoints.Length >= 3 )
		{
			PlayerGizmos.color = Color.yellow;
			DrawPolyline( _expectedPoints );
		}

		if( _guardianPoints != null && _guardianPoints.Length > 3 )
		{
            PlayerGizmos.matrix = _cameraRig.transform.localToWorldMatrix;
			PlayerGizmos.color = Color.green;
			DrawPolyline( _guardianPoints );
            PlayerGizmos.matrix = Matrix4x4.identity;
		}
	}


	static void DrawPolyline( IList<Vector3> points )
	{
		int count = points.Count;
		Vector3 p0 = points[ count-1 ];
		for( int t = 0; t < count; t++ )
		{
			Vector3 p1 = points[ t ];
            PlayerGizmos.DrawLine( new Vector3( p0.x, 0, p0.z ), new Vector3( p1.x, 0, p1.z ) );
            p0 = p1;
		}
	}


	static void ExtractPositions( Transform[] transforms, ref Vector3[] positions )
	{
		if( positions == null || positions.Length != transforms.Length ) positions = new Vector3[ transforms.Length ];
		for( int t = 0; t < transforms.Length; t++ ) positions[ t ] = transforms[ t ].position;
	}


	static Vector2[] Vector3XZToVector2( IList<Vector3> vector3Ds )
	{
		Vector2[] vector2Ds = new Vector2[ vector3Ds.Count ];
		for( int v = 0; v < vector3Ds.Count; v++ ) vector2Ds[ v ] = new Vector2( vector3Ds[ v ].x, vector3Ds[ v ].z );
		return vector2Ds;
	}
}