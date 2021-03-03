using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDecal
{

    private GameObject decalGameObject;

    private List<GameObject> listOfDecals;

    private int currentOffset = 0;

    private Transform parentTransform;
    private int maxDecalLimit;

    public CustomDecal(GameObject decalGameObject, int maxDecalLimit, Transform parentTransform)
    {
        this.decalGameObject = decalGameObject;
        this.parentTransform = parentTransform;
        this.maxDecalLimit = maxDecalLimit;

        listOfDecals = new List<GameObject>(maxDecalLimit);

        for (int i = 0; i < maxDecalLimit; ++i)
        {
            InstantiateDecal();
        }

    }

    private void InstantiateDecal()
    {
        var spawned = GameObject.Instantiate(decalGameObject);
        spawned.transform.SetParent(this.parentTransform);

        listOfDecals.Add(spawned);

        spawned.SetActive(false);
    }
        
    public void SpawnDecal(RaycastHit hit)
    {
        GameObject decal = GetNextAvailableDecal();
        if (decal != null)
        {
            decal.transform.position = hit.point;
            decal.transform.SetParent(hit.collider.transform);
            decal.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

            decal.SetActive(true);

        }
    }

    private GameObject GetNextAvailableDecal()
    {
        
        var oldestActiveDecal = listOfDecals[currentOffset];

        oldestActiveDecal.SetActive(false);

        currentOffset = (currentOffset + 1) % maxDecalLimit;

        return oldestActiveDecal;
    }

}
