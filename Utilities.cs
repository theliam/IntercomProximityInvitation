using System;

namespace IntercomProximityInvitation
{
	public static class Utilities
	{
		/// <summary>
		/// Converts degrees to radians
		/// </summary>
		/// <returns>Radian value.</returns>
		/// <param name="degrees">Degrees.</param>
		public static double DegreesToRads (double degrees)
		{
			return degrees * Math.PI / 180;
		}
	}
}

