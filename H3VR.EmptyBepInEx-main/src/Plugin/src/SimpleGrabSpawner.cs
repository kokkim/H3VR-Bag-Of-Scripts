using System;
using System.Collections;
using FistVR;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimpleGrabSpawner : FVRInteractiveObject
{
    [Header("Spawner params")]
    public string itemID;
    public bool isInfinite = true;
    [Tooltip("How many objects can be taken from this spawner before it runs out")]
    public int objectCapacity;
    int objectsRemaining;
    AnvilCallback<GameObject> itemLoader;

    public override void Awake()
    {
        base.Awake();
        itemLoader = IM.OD[itemID].GetGameObjectAsync();
        objectsRemaining = objectCapacity;
    }

    public override void BeginInteraction(FVRViveHand hand)
    {
        if (!isInfinite && objectsRemaining < 1) return;

        base.BeginInteraction(hand);
        if (!itemLoader.IsCompleted) itemLoader.CompleteNow();
        FVRPhysicalObject spawnedItem = Instantiate(itemLoader.Result, hand.transform.position, hand.transform.rotation).GetComponent<FVRPhysicalObject>();
        if (spawnedItem != null)
        {
            hand.ForceSetInteractable(spawnedItem);
            spawnedItem.BeginInteraction(hand);
        }

        objectsRemaining--;
        if (objectsRemaining < 1) enabled = false;
    }
}