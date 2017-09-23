using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espionage
{
	/// <summary>
	/// Represents the friendly headquarters.
	/// </summary>
	public sealed class Headquarters
	{
		private readonly ConcurrentBag<TopSecretDocument> documents;
		private readonly ConcurrentBag<IntelligenceOperative> operatives;

		internal Headquarters()
		{
			operatives = new ConcurrentBag<IntelligenceOperative>();
			documents = new ConcurrentBag<TopSecretDocument>();
		}

		/// <summary>
		/// Gets the collection of the currently employed operatives.
		/// </summary>
		/// <value>The collection of operatives.</value>
		public IEnumerable<IntelligenceOperative> Operatives => operatives;

		/// <summary>
		/// Gets a collection of documents that have been delivered to the headquarters.
		/// </summary>
		/// <value>The documents.</value>
		public IEnumerable<TopSecretDocument> Documents => documents;

		/// <summary>
		/// Trains a new intelligence operative.
		/// </summary>
		/// <returns>The new intelligence operative.</returns>
		public async Task<IntelligenceOperative> TrainOperativeAsync()
		{
			await Task.Delay(World.Current.Random.Next(World.Current.ShortestTask, World.Current.LongestTask));

			var operative = new IntelligenceOperative(this);
			operatives.Add(operative);

			return operative;
		}

		public void ReceiveDocument(TopSecretDocument doc)
		{
			doc.Location = DocumentLocation.AtHeadquarters;
			documents.Add(doc);
		}
	}
}
