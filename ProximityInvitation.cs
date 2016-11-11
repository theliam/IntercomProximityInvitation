#define Test_IntercomProximityInvitation

using System;
#if Test_IntercomProximityInvitation
using System.Diagnostics;
#endif

namespace IntercomProximityInvitation
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length == 0) {
				throw new Exception("No input file given.");
			}
			IntercomProximityChecker proximityChecker = new IntercomProximityChecker (args [0]);
			proximityChecker.GetCustomersInRange ();

			#if Test_IntercomProximityInvitation
			proximityChecker = new IntercomProximityChecker (args [0]);
			proximityChecker.TestGetCustomersInRange ();
			proximityChecker = new IntercomProximityChecker ("../../test/baddata.txt");
			proximityChecker.TestGetCustomersInRange ();
			#endif
		}
	}
}
