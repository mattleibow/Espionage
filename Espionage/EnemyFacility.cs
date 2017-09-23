using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espionage
{
	/// <summary>
	/// Represents an enemy's facility that may contain top-secret documents.
	/// </summary>
	public sealed class EnemyFacility
	{
		private readonly ConcurrentBag<TopSecretDocument> documents;

		internal EnemyFacility(EnemyHeadquarters headquarters)
		{
			Headquarters = headquarters ?? throw new ArgumentNullException(nameof(headquarters));

			documents = new ConcurrentBag<TopSecretDocument>();
			documents.Populate(World.Current.Random.Next(1, World.Current.MaximumEnemyFacilitiesPerHeadquarters), () => new TopSecretDocument(this));
		}

		/// <summary>
		/// Gets a collection of documents currently at this facility.
		/// </summary>
		/// <value>The documents.</value>
		public IEnumerable<TopSecretDocument> Documents => documents;

		/// <summary>
		/// Gets the headquarters that runs this facility.
		/// </summary>
		/// <value>The headquarters.</value>
		public EnemyHeadquarters Headquarters { get; }

		/// <summary>
		/// Tries to compromise a top-secret document.
		/// </summary>
		/// <returns>Returns the document that was compromised, or null if the agent was unsuccessful.</returns>
		internal async Task<TopSecretDocument> CompromiseDocumentAsync()
		{
			await Task.Delay(World.Current.Random.Next(World.Current.ShortestTask, World.Current.LongestTask));

			var success = World.Current.Random.NextBoolean();

			if (success && documents.TryTake(out TopSecretDocument doc))
			{
				doc.Location = DocumentLocation.OnAgent;
				return doc;
			}
			return null;
		}
	}
}
