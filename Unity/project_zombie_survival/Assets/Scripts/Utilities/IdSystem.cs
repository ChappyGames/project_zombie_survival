using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdSystem<TType, TId, TValue> {

    private Dictionary<TType, Dictionary<TId, TValue>> collection;

    public IdSystem() {
        collection = new Dictionary<TType, Dictionary<TId, TValue>>();
    }

    public bool Register(TType aType, TId aId, TValue aValue) {
        bool lRegistered = false;

        if (!collection.ContainsKey(aType)) {
            collection.Add(aType, new Dictionary<TId, TValue>());
        }

        if (!collection[aType].ContainsKey(aId)) {
            collection[aType].Add(aId, aValue);
            lRegistered = true;
        }

        return lRegistered;
    }

    public bool Unregister(TType aType, TId aId) {
        bool lUnregistered = false;

        if (collection.ContainsKey(aType)) {

            if (collection[aType].ContainsKey(aId)) {
                collection[aType].Remove(aId);
                lUnregistered = true;
            }
        }

        return lUnregistered;
    }

    public TValue GetValue(TType aType, TId aId) {
        TValue lValue = default(TValue);

        if (collection.ContainsKey(aType)) {
            if (collection[aType].ContainsKey(aId)) {
                lValue = collection[aType][aId];
            }
        }

        return lValue;
    }

    public int TypeCount(TType aType) {
        int lCount = 0;

        if (collection.ContainsKey(aType)) {
            lCount = collection[aType].Count;
        }

        return lCount;
    }

    public int Count() {
        int lCount = 0;

        foreach (KeyValuePair<TType, Dictionary<TId, TValue>> lCollections in collection) {
            lCount += lCollections.Value.Count;
        }

        return lCount;
    }
}
