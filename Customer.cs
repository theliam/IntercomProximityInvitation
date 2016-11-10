using System;
using Newtonsoft.Json;

namespace IntercomProximityInvitation
{
	/// <summary>
	/// Basic representation of a customer.
	/// </summary>
	public class Customer 
	{
		public string Name { get; set; }
		public int UserId { get; set; }
		public Coordinates HomeCoordinates { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IntercomProximityInvitation.Customer"/> class.
		/// </summary>
		/// <param name="_name">Name.</param>
		/// <param name="_userid">Userid.</param>
		/// <param name="_coordinates">Coordinates.</param>
		public Customer (string _name, int _userid, Coordinates _coordinates) 
		{
			Name = _name;
			UserId = _userid;
			HomeCoordinates = _coordinates;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IntercomProximityInvitation.Customer"/> class.
		/// </summary>
		/// <param name="name">Customer name.</param>
		/// <param name="user_id">User id.</param>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		/// <remarks>Parameter names match json format.</remarks>
		[JsonConstructor]
		public Customer (string name, int user_id, double latitude, double longitude)
		{
			Name = name;
			UserId = user_id;
			HomeCoordinates = new Coordinates (latitude, longitude);
		}

		/// <summary>
		/// Compares two customers by their user id.
		/// </summary>
		/// <returns>An indication of their relative value.</returns>
		/// <param name="a">Customer 1.</param>
		/// <param name="b">Customer 2.</param>
		public static int CompareByUserId (Customer a, Customer b)
		{
			return a.UserId.CompareTo (b.UserId);
		}
	}
}

