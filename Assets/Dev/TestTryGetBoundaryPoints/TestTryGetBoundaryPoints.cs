using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class TestTryGetBoundaryPoints : MonoBehaviour
{
    Vector3[] points;


    void Start()
    {
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if( loader == null )
        {
            Debug.LogWarning( "Could not get active Loader." );
            return;
        }

        var inputSubsystem = loader.GetLoadedSubsystem<XRInputSubsystem>();
        inputSubsystem.boundaryChanged += InputSubsystem_boundaryChanged;

        Debug.Log( "READY!" );
    }


    void Update()
    {
        if( points != null ) DrawPolyline( points );
    }


    void InputSubsystem_boundaryChanged( XRInputSubsystem inputSubsystem )
    {
        List<Vector3> boundaryPoints = new List<Vector3>();
        if( inputSubsystem.TryGetBoundaryPoints( boundaryPoints ) )
        {
            foreach( var point in boundaryPoints ) Debug.Log( point );
            points = boundaryPoints.ToArray();
        }
        else
        {
            Debug.LogWarning( $"Could not get Boundary Points for Loader" );
        }
    }


    void DrawPolyline( IList<Vector3> points )
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
}