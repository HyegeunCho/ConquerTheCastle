using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HGPlugins.Singleton
{
    public class MonoSingleton<T> : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    string name = $"[SINGLETON] {typeof(T).Name}";
                    GameObject go = new GameObject(name, typeof(T));
                    _instance = go.GetComponent<T>();
                    // _instance = Instantiate(go).GetComponent<T>();
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }    
}

