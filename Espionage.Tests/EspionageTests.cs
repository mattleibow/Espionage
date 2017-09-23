using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

namespace Espionage.Tests
{
	[TestClass]
	public class EspionageTests
	{
		[TestInitialize]
		public void Initialize()
		{
			World.LoadInstantWorld();
		}

		[TestMethod]
		public async Task ChainOfCommandIsMaintained()
		{
			var hq = World.Current.Headquarters;
			var operative = await hq.TrainOperativeAsync();
			var agent = await operative.TrainAgentAsync();

			Assert.AreEqual(hq, operative.Headquarters);
			CollectionAssert.Contains(hq.Operatives.ToArray(), operative);

			Assert.AreEqual(operative, agent.Handler);
			CollectionAssert.Contains(operative.Agents.ToArray(), agent);
		}

		[TestMethod]
		public void EnemyHQHasFacilitiesWithDocuments()
		{
			var enemy = World.Current.EnemyHeadquarters;

			Assert.IsTrue(enemy.Facilities.Count() > 0);
			foreach (var facility in enemy.Facilities)
			{
				Assert.AreEqual(enemy, facility.Headquarters);
				Assert.IsTrue(facility.Documents.Count() > 0);

				foreach (var doc in facility.Documents)
				{
					Assert.AreEqual(facility, doc.Facility);
					Assert.AreEqual(DocumentLocation.AtEnemyFacility, doc.Location);
				}
			}
		}

		[TestMethod]
		public async Task AgentCanInfiltrateFacility()
		{
			var hq = World.Current.Headquarters;
			var operative = await hq.TrainOperativeAsync();
			var agent = await operative.TrainAgentAsync();

			var enemy = World.Current.EnemyHeadquarters;
			var facility = enemy.Facilities.First();

			var success = false;
			while (!success)
			{
				success = await agent.InfiltrateAsync(facility);
			}

			Assert.AreEqual(facility, agent.CurrentAssignment);
		}

		[TestMethod]
		public async Task AgentCanRetrieveDocuments()
		{
			var hq = World.Current.Headquarters;
			var operative = await hq.TrainOperativeAsync();
			var agent = await operative.TrainAgentAsync();
			var agentDocs = agent.Documents.ToArray();

			var enemy = World.Current.EnemyHeadquarters;
			var facility = enemy.Facilities.First();
			var facilityDocs = facility.Documents.ToArray();

			var success = false;
			while (!success)
			{
				success = await agent.InfiltrateAsync(facility);
			}

			TopSecretDocument doc = null;
			while (doc == null)
			{
				doc = await agent.RetrieveDocumentAsync();
			}

			Assert.IsNotNull(doc);
			Assert.AreEqual(facility, doc.Facility);
			Assert.AreEqual(DocumentLocation.OnAgent, doc.Location);

			CollectionAssert.Contains(facilityDocs, doc);
			CollectionAssert.DoesNotContain(facility.Documents.ToArray(), doc);

			CollectionAssert.DoesNotContain(agentDocs, doc);
			CollectionAssert.Contains(agent.Documents.ToArray(), doc);
		}

		[TestMethod]
		public async Task AgentCanDeliverDocuments()
		{
			var hq = World.Current.Headquarters;
			var operative = await hq.TrainOperativeAsync();
			var operativeDocs = operative.Documents.ToArray();
			var agent = await operative.TrainAgentAsync();
			var agentDocs = agent.Documents.ToArray();

			var enemy = World.Current.EnemyHeadquarters;
			var facility = enemy.Facilities.First();
			var facilityDocs = facility.Documents.ToArray();

			var success = false;
			while (!success)
			{
				success = await agent.InfiltrateAsync(facility);
			}

			TopSecretDocument doc = null;
			while (doc == null)
			{
				doc = await agent.RetrieveDocumentAsync();
			}

			success = false;
			while (!success)
			{
				success = await agent.DeliverDocumentsAsync();
			}

			Assert.IsNotNull(doc);
			Assert.AreEqual(facility, doc.Facility);
			Assert.AreEqual(DocumentLocation.OnIntelligenceOperative, doc.Location);

			CollectionAssert.DoesNotContain(agent.Documents.ToArray(), doc);
			CollectionAssert.Contains(operative.Documents.ToArray(), doc);
		}

		[TestMethod]
		public async Task OperativeCanDeliverDocuments()
		{
			var hq = World.Current.Headquarters;
			var operative = await hq.TrainOperativeAsync();
			var agent = await operative.TrainAgentAsync();
			var agentDocs = agent.Documents.ToArray();

			var enemy = World.Current.EnemyHeadquarters;
			var facility = enemy.Facilities.First();
			var facilityDocs = facility.Documents.ToArray();

			var success = false;
			while (!success)
			{
				success = await agent.InfiltrateAsync(facility);
			}

			TopSecretDocument doc = null;
			while (doc == null)
			{
				doc = await agent.RetrieveDocumentAsync();
			}

			success = false;
			while (!success)
			{
				success = await agent.DeliverDocumentsAsync();
			}

			success = false;
			while (!success)
			{
				success = await operative.DeliverDocumentsAsync();
			}

			Assert.IsNotNull(doc);
			Assert.AreEqual(facility, doc.Facility);
			Assert.AreEqual(DocumentLocation.AtHeadquarters, doc.Location);

			CollectionAssert.DoesNotContain(operative.Documents.ToArray(), doc);
			CollectionAssert.Contains(hq.Documents.ToArray(), doc);
		}
	}
}
