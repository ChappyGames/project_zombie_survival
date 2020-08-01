using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    public static T Instance { get; protected set; }

    private void InitializeSingleton() {
        T lComponent = GetComponent<T>();
        if (Instance == null) {
            Instance = lComponent;
        } else if (Instance != lComponent) {
            Destroy(gameObject);
        }
    }

    protected virtual void Awake() {
        InitializeSingleton();
    }
}
