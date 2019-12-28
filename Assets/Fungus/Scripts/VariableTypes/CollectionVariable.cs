/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    /// <summary>
    /// Collection variable type.
    /// </summary>
    [VariableInfo("Other", "Collection")]
    [AddComponentMenu("")]
	[System.Serializable]
	public class CollectionVariable : VariableBase<Fungus.Collection>
	{ }

	/// <summary>
	/// Container for a Collection variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct CollectionData
	{
		[SerializeField]
		[VariableProperty("<Value>", typeof(CollectionVariable))]
		public CollectionVariable collectionRef;

		[SerializeField]
		public Fungus.Collection collectionVal;

		public static implicit operator Fungus.Collection(CollectionData CollectionData)
		{
			return CollectionData.Value;
		}

		public CollectionData(Fungus.Collection v)
		{
			collectionVal = v;
			collectionRef = null;
		}

		public Fungus.Collection Value
		{
			get { return (collectionRef == null) ? collectionVal : collectionRef.Value; }
			set { if (collectionRef == null) { collectionVal = value; } else { collectionRef.Value = value; } }
		}

		public string GetDescription()
		{
			if (collectionRef == null)
			{
				return collectionVal != null ? collectionVal.ToString() : string.Empty;
			}
			else
			{
				return collectionRef.Key;
			}
		}
	}
}