using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SingletonGameObject<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = default(T);

        private static object mLock = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (mLock)
                    {
                        if (instance == null)
                        {
                            instance = FindObjectOfType(typeof(T)) as T;
                            if (instance == null)
                            {
                                GameObject obj = new GameObject();
                                instance = obj.AddComponent(typeof(T)) as T;
                                obj.name = typeof(T).Name;
                            }
                        }
                    }
                }

                return instance;
            }
        }


    }


