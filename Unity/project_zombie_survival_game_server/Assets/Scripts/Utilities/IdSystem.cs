using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdSystem<TId, TValue> {

    private Dictionary<TId, TValue> collection;

    public IdSystem() {
        collection = new Dictionary<TId, TValue>();
    }

    public bool Register(TId aId, TValue aValue) {
        bool lRegistered = false;

        if (!collection.ContainsKey(aId)) {
            collection.Add(aId, aValue);
            lRegistered = true;
        }

        return lRegistered;
    }

    public bool Unregister(TId aId) {
        bool lUnregistered = false;

        if (collection.ContainsKey(aId)) {
            collection.Remove(aId);
            lUnregistered = true;
        }

        return lUnregistered;
    }

    public TValue GetValue(TId aId) {
        TValue lValue = default(TValue);

        if (collection.ContainsKey(aId)) {
            lValue = collection[aId];
        }

        return lValue;
    }

    public int Count() {

        return collection.Count;
    }
}
