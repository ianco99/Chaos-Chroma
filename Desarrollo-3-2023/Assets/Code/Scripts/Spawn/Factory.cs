using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Spawn
{
    public class Factory : Pool
    {
        [Header("Spawn Settings:")] [SerializeField]
        protected List<Transform> spawns;

        [SerializeField] protected Object[] prefabs;
        [SerializeField] protected bool repeatSpawn;
        [SerializeField] protected bool keepSpawning;
        [SerializeField] protected float interval = 1f;

        protected virtual void Awake()
        {
            for (int i = 0; i < prefabs.Length; i++)
                prefabs[i].typeId = i;
        }

        protected virtual void Start()
        {
            do
            {
                StartCoroutine(SpawnLoop());
            } while (keepSpawning && spawns.Count > 0);
        }

        /// <summary>
        /// Spawn object, wait for interval, repeat if should
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator SpawnLoop()
        {
            SpawnRandomObject();
            yield return new WaitForSeconds(interval);
        }

        /// <summary>
        /// Randomize objects and spawn one
        /// </summary>
        public void SpawnRandomObject()
        {
            if (spawns.Count <= 0)
                return;

            float totalOdds = 0;
            float[] odds = new float[prefabs.Length];

            for (int i = 0; i < prefabs.Length; i++)
            {
                prefabs[i].prefab.SetActive(true);
                totalOdds += prefabs[i].odds;
                odds[i] = totalOdds;
            }

            float random = Random.Range(0f, totalOdds);

            for (int i = 0; i < prefabs.Length; i++)
            {
                if (!(random < odds[i])) continue;
                
                int randomSpawn = Random.Range(0, spawns.Count);

                Instantiate(prefabs[i], spawns[randomSpawn].position);

                if (repeatSpawn) return;
                
                GameObject spawn = spawns[randomSpawn].gameObject;
                spawns.Remove(spawns[randomSpawn]);
                Destroy(spawn);

                return;
            }
        }
    }
}