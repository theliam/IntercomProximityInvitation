#define Test_IntercomProximityInvitation

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
#if Test_IntercomProximityInvitation
using System.Diagnostics;
#endif

namespace IntercomProximityInvitation
{
	/// <summary>
	/// Takes customer data from an file and list customers within a certani range
	/// and displays them in order of user id ascending.
	/// </summary>
	public class IntercomProximityChecker
	{
		private string inputFilePath;
		private List<Customer> customers = new List<Customer>();
		private const int earthRadius = 6371;
		private const int customerRange = 100;
		private Coordinates intercomCoordinates = new Coordinates(53.3393, -6.2576841);

		/// <summary>
		/// Initializes a new instance of the <see cref="IntercomProximityInvitation.IntercomProximityChecker"/> class.
		/// </summary>
		/// <param name="_inputFilePath">Input file path.</param>
		public IntercomProximityChecker (string _inputFilePath)
		{
			inputFilePath = _inputFilePath;
			if (!File.Exists (inputFilePath)) {
				throw new Exception("The file specified does not exist.");
			} 
		}

		/// <summary>
		/// Main functionality of to get customers in range.
		/// </summary>
		public void GetCustomersInRange ()
		{
			ReadDataIntoCustomersList ();
			PruneCustomersFartherThanMaxDistance ();
			customers.Sort(Customer.CompareByUserId);
			PrintCustomers ();
		}

		/// <summary>
		/// Reads in and parses json into customers list.
		/// </summary>
		private void ReadDataIntoCustomersList ()
		{
			using (StreamReader reader = new StreamReader (inputFilePath)) {
				string line;
				while ((line = reader.ReadLine ()) != null) {
					try {
						// deserialise json into customer object.
						Customer newCustomer = JsonConvert.DeserializeObject<Customer> (line);
						if (newCustomer != null) {
							customers.Add (newCustomer);
						} else {
							throw new ArgumentNullException("customer", "Invalid json");
						}
					} catch (Exception e) {
						#if Test_IntercomProximityInvitation
						throw;
						#else
						Console.WriteLine ("Invalid or unexpected json. " + e.Message);
						#endif
					}
				}
			}
		}

		/// <summary>
		/// Prunes the customers from list that are farther than the max distance.
		/// </summary>
		private void PruneCustomersFartherThanMaxDistance ()
		{
			// loop customer list in reverse to safely remove customers not within 100km
			for (int i = customers.Count - 1; i >= 0; i--) {
				double temp = Coordinates.GreatCircleDistance (intercomCoordinates, customers [i].HomeCoordinates, earthRadius);
				if (Coordinates.GreatCircleDistance (intercomCoordinates, customers [i].HomeCoordinates, earthRadius) > customerRange) {
					customers.RemoveAt (i);
				}
			}
		}

		/// <summary>
		/// Prints the list of customers
		/// </summary>
		private void PrintCustomers ()
		{
			foreach (Customer customer in customers) {
				Console.WriteLine (customer.UserId + "\t\t" + customer.Name);
			}
			// keep console window open to view results
			Console.WriteLine ("Press any key to exit.");
			Console.ReadKey ();
		}

		#if Test_IntercomProximityInvitation
		public void TestGetCustomersInRange () 
		{
			TestReadDataIntoCustomersList ();
			TestPruneCustomersFartherThanMaxDistance ();
			TestCustomerSortById ();
			PrintCustomers ();
		}

		private void TestReadDataIntoCustomersList() 
		{
			try {
				ReadDataIntoCustomersList ();
			} catch (ArgumentNullException e) {
				Console.WriteLine ("Expected exception null argument. " + e.Message);
			} catch (JsonReaderException e) {
				Console.WriteLine ("Expected json exception." + e.Message);
			} catch (Exception e) {
				if (e.Message.ToLower().Contains("json")) {
					Console.WriteLine ("Expected json exception.");
				} else {
					Console.WriteLine ("Unexpected Exception.");
				} 
				Console.WriteLine (e.Message);	
			}
		}

		private void TestPruneCustomersFartherThanMaxDistance ()
		{
			PruneCustomersFartherThanMaxDistance ();
			foreach (Customer customer in customers) {
				Debug.Assert (Coordinates.TestGreatCircleDistance (intercomCoordinates, customer.HomeCoordinates, earthRadius) <= customerRange, 
							  "Customer remaining in list with distance greater than max.");
			} 
		}

		private void TestCustomerSortById ()
		{
			customers.Sort(Customer.CompareByUserId);
			for (int i = 0; i < customers.Count - 1; i++) {
				Debug.Assert (customers [i].UserId < customers [i + 1].UserId, "Customers not in ascending order.");
			}
		}
		#endif
	}
}

