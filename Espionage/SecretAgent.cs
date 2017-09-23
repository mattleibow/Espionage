using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Espionage
{
	/// <summary>
	/// Represents a secret agent.
	/// </summary>
	public sealed class SecretAgent
	{
		private static int lastAgentNumber = 0;

		private readonly ConcurrentBag<TopSecretDocument> documents;

		internal SecretAgent(IntelligenceOperative handler)
		{
			documents = new ConcurrentBag<TopSecretDocument>();
			Handler = handler;
			Number = NextAgentNumber();
		}

		/// <summary>
		/// Gets the intelligence operative that handles this agent.
		/// </summary>
		/// <value>The intelligence operative.</value>
		public IntelligenceOperative Handler { get; }

		/// <summary>
		/// Gets the enemy facility that this agent is currently assigned to.
		/// </summary>
		/// <value>The currently assigned facility.</value>
		public EnemyFacility CurrentAssignment { get; private set; }

		/// <summary>
		/// Gets the agents' number.
		/// </summary>
		/// <value>The agents' number.</value>
		public int Number { get; }

		/// <summary>
		/// Gets the agent's name.
		/// </summary>
		/// <value>The agent's name.</value>
		public string Name => $"Agent {Number}";

		/// <summary>
		/// Gets a collection of the top-secret documents that this agent has on his person.
		/// </summary>
		/// <value>The collection of documents.</value>
		public IEnumerable<TopSecretDocument> Documents => documents;

		/// <summary>
		/// Tries to infiltrate an enemy facility.
		/// </summary>
		/// <param name="facility">The facility to infiltrate.</param>
		/// <returns>Returns true if the agent successfully infiltrated the facility, otherwise false.</returns>
		public async Task<bool> InfiltrateAsync(EnemyFacility facility)
		{
			if (facility == null)
				throw new ArgumentNullException(nameof(facility));

			await Task.Delay(World.Current.Random.Next(World.Current.ShortestTask, World.Current.LongestTask));

			var success = World.Current.Random.NextBoolean();

			if (success)
			{
				CurrentAssignment = facility;
			}

			return success;
		}

		/// <summary>
		/// Tries to retrieve a top-secret document from the current facility.
		/// </summary>
		/// <returns>Returns the document if the retrieval was successful, otherwise null.</returns>
		public async Task<TopSecretDocument> RetrieveDocumentAsync()
		{
			var assignment = CurrentAssignment;

			if (assignment == null)
				throw new InvalidOperationException("Agent cannot retrieve document without an assignment.");

			var doc = await assignment.CompromiseDocumentAsync();
			if (doc != null)
			{
				documents.Add(doc);
			}
			return doc;
		}

		private static int NextAgentNumber()
		{
			return Interlocked.Increment(ref lastAgentNumber);
		}

		/// <summary>
		/// Tries to deliver all the top-secret documents to this agent's handler.
		/// </summary>
		/// <returns>Returns true if the documents were delivered, otherwise false.</returns>
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
						Handler.ReceiveDocument(doc);
					}
				}
			}

			return success;
		}
	}
}
