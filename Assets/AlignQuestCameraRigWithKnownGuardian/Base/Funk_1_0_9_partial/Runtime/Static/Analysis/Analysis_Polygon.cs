/*
	Copyright © Carl Emil Carlsen 2021-2022
	http://cec.dk
*/


using System.Collections.Generic;
using UnityEngine;

namespace Funk
{
	public static partial class Analysis
	{
        /// <summary>
        /// Find the center of mass of a polygon. Polygon points must be ordered anti-clockwise.
		/// Also known as the centroid of a polygon.
        /// </summary>
        public static Vector2 PolygonCenterOfMass( IList<Vector2> polygon )
		{
            // Source: https://web.archive.org/web/20120229233701/http://paulbourke.net/geometry/polyarea/

            float x = 0;
			float y = 0;
			Vector2 p0 = polygon[ polygon.Count - 1 ];
			foreach( Vector2 p1 in polygon )
			{
				float t = p0.x * p1.y - p1.x * p0.y;
                x += (p0.x + p1.x) * t;
                y += (p0.y + p1.y) * t;
                p0 = p1;
			}
			float d = 1f / ( 6f *  PolygonArea( polygon ) );
			x *= d;
			y *= d;
			return new Vector2( x, y );
        }
    }
}