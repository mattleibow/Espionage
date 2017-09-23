using System;

namespace Espionage
{
	/// <summary>
	/// Represents a top-secret document.
	/// </summary>
	public sealed class TopSecretDocument
	{
		internal TopSecretDocument(EnemyFacility facility)
		{
			Facility = facility ?? throw new ArgumentNullException(nameof(facility));
			Location = DocumentLocation.AtEnemyFacility;
		}

		/// <summary>
		/// Gets the current location status of the document.
		/// </summary>
		/// <value>The current location status.</value>
		public DocumentLocation Location { get; internal set; }

		/// <summary>
		/// Gets the facility that this document originated at.
		/// </summary>
		/// <value>The original facility.</value>
		public EnemyFacility Facility { get; }
	}
}
