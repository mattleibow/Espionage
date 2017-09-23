using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Espionage
{
	/// <summary>
	/// Represents an enemy headquarters.
	/// </summary>
	public sealed class EnemyHeadquarters
	{
		private readonly ConcurrentBag<EnemyFacility> facilities;

		internal EnemyHeadquarters()
		{
			facilities = new ConcurrentBag<EnemyFacility>();
			facilities.Populate(World.Current.Random.Next(1, World.Current.MaximumEnemyHeadquarters), () => new EnemyFacility(this));
		}

		/// <summary>
		/// Gets a collection of the facilities that this headquarters runs.
		/// </summary>
		/// <value>The collection of facilities.</value>
		public IEnumerable<EnemyFacility> Facilities => facilities;
	}
}
