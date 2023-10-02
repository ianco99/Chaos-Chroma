using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Controls the randomizer for attacks. (add different types of attacks to the list)
    /// </summary>
    public class HitsManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> hits;
        private int index;
        
        private void OnEnable()
        {
            index = Random.Range(0, hits.Count);
            
            hits[index].SetActive(true);
        }
        
        private void Update()
        {
            if (!hits[index].activeSelf)
                gameObject.SetActive(false);
        }
    }
}