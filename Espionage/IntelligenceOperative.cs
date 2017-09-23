using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espionage
{
	/// <summary>
	/// Represents an intelligence operative that can train and handle secret agents.
	/// </summary>
	public sealed class IntelligenceOperative
	{
		private readonly ConcurrentBag<SecretAgent> agents;
		private readonly ConcurrentBag<TopSecretDocument> documents;

		internal IntelligenceOperative(Headquarters headquarters)
		{
			agents = new ConcurrentBag<SecretAgent>();
			documents = new ConcurrentBag<TopSecretDocument>();
			Headquarters = headquarters;
		}

		/// <summary>
		/// Gets the headquarters that employs the operative.
		/// </summary>
		/// <value>The headquarters.</value>
		public Headquarters Headquarters { get; }

		/// <summary>
		/// Gets a collection of secret agents that this operative handles.
		/// </summary>
		/// <value>The collection of secret agents.</value>
		public IEnumerable<SecretAgent> Agents => agents;

		/// <summary>
		/// Gets a collection of top-secret documents that this operative has on his person.
		/// </summary>
		/// <value>The collection of documents.</value>
		public IEnumerable<TopSecretDocument> Documents => documents;

		/// <summary>
		/// Trains an new secret agent.
		/// </summary>
		/// <returns>The new secret agent.</returns>
		public async Task<SecretAgent> TrainAgentAsync()
		{
			await Task.Delay(World.Current.Random.Next(World.Current.ShortestTask, World.Current.LongestTask));

			var agent = new SecretAgent(this);
			agents.Add(agent);

			return agent;
		}

		/// <summary>
		/// Tries to deliver all documents to headquarters.
		/// </summary>
		/// <returns>Returns true if all documents were delivered, otherwise false.</returns>
		public async Task<bool> DeliverDocumentsAsync()
		{
			if (documents.IsEmpty)
				return false;

			await Task.Delay(World.Current.Random.Next(World.Current.ShortestTask, World.Current.LongestTask));

			var success = World.Current.Random.NextBoolean();

			if (success)
			{
				while (!documents.IsEmpty)
				{
					if (documents.TryTake(out TopSecretDocument doc))
					{
						Headquarters.ReceiveDocument(doc);
					}
				}
			}

			return success;
		}

		public void ReceiveDocument(TopSecretDocument doc)
		{
			doc.Location = DocumentLocation.OnIntelligenceOperative;
			documents.Add(doc);
		}
	}
}
