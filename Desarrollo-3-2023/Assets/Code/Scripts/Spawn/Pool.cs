using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Spawn
{
    public class Pool : MonoBehaviour
    {
        [Serializable]
        public struct Object
        {
            public GameObject prefab;
            public float odds;
            public int typeId;
        }

        [SerializeField] protected Vector3 maxRotation = Vector3.zero;
        
        protected readonly List<Object> pool = new();
        
        /// <summary>
        /// Get if there is an unused object and return it
        /// </summary>
        /// <param name="id">type id of the object to find</param>
        /// <returns>Found game object (null if not found)</returns>
        protected GameObject FindUnusedObject(int id)
        {
            return (from obj in pool where !obj.prefab.activeSelf && obj.typeId == id select obj.prefab).FirstOrDefault();
        }
        
        /// <summary>
        /// Spawns the given object on scene
        /// </summary>
        /// <param name="obj">Object to spawn</param>
        /// <param name="pos">Position to spawn</param>
        protected void Instantiate(Object obj, Vector3 pos)
        {
            GameObject newObject = FindUnusedObject(obj.typeId);

            if (newObject) return;
            
            obj.prefab = Instantiate(obj.prefab, pos, Quaternion.identity);
            obj.prefab.transform.parent = transform;
            obj.prefab.transform.rotation = Quaternion.Euler(Random.Range(0f, maxRotation.x),
                Random.Range(0f, maxRotation.y), Random.Range(0f, maxRotation.z));
            pool.Add(obj);
        }
    }
}