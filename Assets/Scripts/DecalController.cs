using System;
using System.Collections.Generic;
using UnityEngine;

public enum DecalType
{
	woodType,
	waterType,
	fleshType,
	stoneType,
	metalType,
	defaultType
}

public class DecalController : MonoBehaviour
{
	[SerializeField]
	private GameObject	woodType,
						waterType,
						fleshType,
						stoneType,
						metalType,
						defaultType;

	private Dictionary<DecalType, CustomDecal> decalTypeToCustomDecal;
	private Dictionary<string, DecalType> materialNameToDecalType;

	[SerializeField]
	[Tooltip("The number of decals to keep alive at a time.  After this number are around, old ones will be replaced.")]
	private int maxConcurrentDecals = 10;

	private void Awake()
	{
		decalTypeToCustomDecal	= new Dictionary<DecalType, CustomDecal>();
		materialNameToDecalType = new Dictionary<string, DecalType>();

		InitializeDecals();
	}

	private void InitializeDecals()
	{
		// Define tipos
		insertDecalType(DecalType.defaultType,	defaultType);
		insertDecalType(DecalType.fleshType,	fleshType);
		insertDecalType(DecalType.stoneType,	stoneType);
		insertDecalType(DecalType.waterType,	waterType);
		insertDecalType(DecalType.woodType,		woodType);
		insertDecalType(DecalType.metalType, metalType);

		// Define relacao material tipo
		materialNameToDecalType["Meat"] = DecalType.fleshType;
		materialNameToDecalType["Wood"] = DecalType.woodType;
		materialNameToDecalType["WaterFilled"] = DecalType.waterType;
		materialNameToDecalType["Stone"] = DecalType.stoneType;
		materialNameToDecalType["Metal"] = DecalType.metalType;

	}

	private void insertDecalType(DecalType decalType, GameObject gameObjectType)
    {
		decalTypeToCustomDecal[decalType] = new CustomDecal(gameObjectType, maxConcurrentDecals, this.transform);

	}

	public void SpawnDecal(RaycastHit hit)
	{

		DecalType decalType;

		if (hit.collider.sharedMaterial is null)
			decalType = DecalType.defaultType;
		else if (!materialNameToDecalType.TryGetValue(hit.collider.sharedMaterial.name, out decalType))
        {
			decalType = DecalType.defaultType;
			materialNameToDecalType[hit.collider.sharedMaterial.name] = decalType;
		}

		decalTypeToCustomDecal[decalType].SpawnDecal(hit);

	}

}