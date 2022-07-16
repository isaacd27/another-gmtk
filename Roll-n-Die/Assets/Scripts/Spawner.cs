using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject PrefabToSpawn;

    public void SpawnAt(Transform transform)
    {
        Instantiate(PrefabToSpawn, transform.position, PrefabToSpawn.transform.rotation);
    }
}
