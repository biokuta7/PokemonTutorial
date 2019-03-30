using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPooler : MonoBehaviour
{

    int amountPer = 10;

    [System.Serializable]
    public class PoolableParticleSystem
    {
        public string name;
        public GameObject particleSystemObject;

        public PoolableParticleSystem(string _name, GameObject _pso)
        {
            name = _name;
            particleSystemObject = _pso;
        }

    }

    public PoolableParticleSystem[] poolableParticleSystems;

    private Dictionary<string, List<ParticleSystem>> poolableParticleSystemDictionary;

    public static ParticleSystemPooler instance;

    private void Start()
    {
        instance = this;
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        poolableParticleSystemDictionary = new Dictionary<string, List<ParticleSystem>>();

        foreach(PoolableParticleSystem pps in poolableParticleSystems)
        {

            List<ParticleSystem> ppsList = new List<ParticleSystem>();
            
            for(int i = 0; i < amountPer; i++)
            {
                GameObject spawnedPS = Instantiate(pps.particleSystemObject, transform.position, Quaternion.identity);
                spawnedPS.transform.SetParent(transform);
                ppsList.Add(spawnedPS.GetComponent<ParticleSystem>());

            }

            poolableParticleSystemDictionary.Add(pps.name, ppsList);

        }

    }

    public void SpawnParticleSystem(string tag, Vector3 position)
    {

        List<ParticleSystem> psList;
        if (poolableParticleSystemDictionary.TryGetValue(tag, out psList))
        {
            ParticleSystem particleSystem = psList[0];

            particleSystem.transform.position = position;
            particleSystem.Play();

            psList.RemoveAt(0);
            psList.Add(particleSystem);

        }
    }

}
