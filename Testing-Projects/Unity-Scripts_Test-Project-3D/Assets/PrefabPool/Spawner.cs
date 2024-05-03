using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PrefabPool pool;
    [Min(0)] public float spawnDelay;

    private bool spawnNextObject = true;
    
    // Update is called once per frame
    void Update()
    {
        if (spawnNextObject)
        {
            spawnNextObject = false;
            StartCoroutine(SpawnObject(spawnDelay));
        }
    }

    private IEnumerator SpawnObject(float spawnDelay)
    {
        GameObject obj = pool.Next;
        obj.transform.position = this.transform.position;
        yield return new WaitForSeconds(spawnDelay);
        spawnNextObject = true;
    }
}
