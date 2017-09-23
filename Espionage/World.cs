using System;
using System.Collections.Concurrent;

namespace Espionage
{
	public class World
	{
		static World()
		{
			LoadDefaultWorld();
		}

		public static World Current { get; private set; }

		/// <summary>
		/// Gets the current headquarters for this world.
		/// </summary>
		/// <value>The current headquarters.</value>
		public EnemyHeadquarters EnemyHeadquarters { get; private set; }


		/// <summary>
		/// Gets the current headquarters for this world.
		/// </summary>
		/// <value>The current headquarters.</value>
		public Headquarters Headquarters { get; private set; }

		public Random Random { get; private set; }

		public int ShortestTask { get; private set; }

		public int LongestTask { get; private set; }

		public int MaximumEnemyHeadquarters { get; private set; }

		public int MaximumEnemyFacilitiesPerHeadquarters { get; private set; }

		public static void LoadDefaultWorld()
		{
			Current = new World
			{
				ShortestTask = 1000,
				LongestTask = 3000,
				MaximumEnemyHeadquarters = 5,
				MaximumEnemyFacilitiesPerHeadquarters = 10,
				Random = new Random(),
			};
			Current.Headquarters = new Headquarters();
			Current.EnemyHeadquarters = new EnemyHeadquarters();
		}

		public static void LoadInstantWorld()
		{
			Current = new World
			{
				ShortestTask = 0,
				LongestTask = 0,
				MaximumEnemyHeadquarters = 5,
				MaximumEnemyFacilitiesPerHeadquarters = 10,
				Random = new Random(),
			};
			Current.Headquarters = new Headquarters();
			Current.EnemyHeadquarters = new EnemyHeadquarters();
		}
	}

	internal static class Extensions
	{
		public static bool NextBoolean(this Random random)
		{
			return random.Next(2) == 0;
		}

		public static void Populate<T>(this ConcurrentBag<T> collection, int count, Func<T> creator)
		{
			for (int i = 0; i < count; i++)
			{
				collection.Add(creator());
			}
		}
	}
}
