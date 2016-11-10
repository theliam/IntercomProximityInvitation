#define Test_IntercomProximityInvitation

using System;
#if Test_IntercomProximityInvitation
using System.Diagnostics;
#endif

namespace IntercomProximityInvitation
{
	/// <summary>
	/// Describes a coordinate as longitude and latitude.
	/// </summary>
	public class Coordinates
	{
		public double longitude;
		public double latitude;

		/// <summary>
		/// Initializes a new instance of the <see cref="IntercomProximityInvitation.Coordinate"/> class.
		/// </summary>
		/// <param name="_longitude">Longitude.</param>
		/// <param name="_latitude">Latitude.</param>
		public Coordinates (double _latitude, double _longitude)
		{
			latitude = _latitude;
			longitude = _longitude;
		}

		/// <summary>
		/// Finds the distance between two coordinates using 
		/// spherical law of cosines.
		/// </summary>
		/// <returns>Returns the distance in the same unit as the radius passed in.</returns>
		/// <param name="c1">Coordinate 1.</param>
		/// <param name="c2">Coordinate 2.</param>
		/// <param name="radius">Radius of sphere.</param>
		/// <remarks>
		/// cos(δ) = sin(lat1) * sin(lat2) + cos(lat1) * cos(lat2) * cos(long1 − long2)
		///	where lat and long are the latitudes and longitudes.
		/// distance is δ * radius.
		/// </remarks>
		public static double GreatCircleDistance (Coordinates c1, Coordinates c2, double radius)
		{
			double sinLatitude1 = Math.Sin (Utilities.DegreesToRads (c1.latitude));
			double sinLatitude2 = Math.Sin (Utilities.DegreesToRads (c2.latitude));
			double cosLatitude1 = Math.Cos (Utilities.DegreesToRads (c1.latitude));
			double cosLatitude2 = Math.Cos (Utilities.DegreesToRads (c2.latitude));

			double deltaLongitude = Math.Abs (Utilities.DegreesToRads (c1.longitude) - Utilities.DegreesToRads (c2.longitude));
			double deltaAngle = Math.Acos (sinLatitude1 * sinLatitude2 + cosLatitude1 * cosLatitude2 * Math.Cos (deltaLongitude));

			return radius * deltaAngle;
		}

		#if Test_IntercomProximityInvitation
		/// <summary>
		/// Basic test to ensure value is within the possible bounds
		/// </summary>
		/// <param name="c1">Coordinate 1.</param>
		/// <param name="c2">Coordinate 2.</param>
		/// <param name="radius">Radius.</param>
		public static double TestGreatCircleDistance (Coordinates c1, Coordinates c2, double radius)
		{
			double distance = GreatCircleDistance (c1, c2, radius);
			if (c1.latitude == c2.latitude && c1.longitude == c2.longitude) {
				Debug.Assert (Math.Abs (distance - 0) < 0.01, "Distance should be zero.");
			} else {
				double maxDistance = Math.PI * radius;
				Debug.Assert ((distance > 0 && distance <= maxDistance), "Distance must be greater than zero and less than or equal to half the circumference");
			}
			return distance;
		}
		#endif
	}
}

