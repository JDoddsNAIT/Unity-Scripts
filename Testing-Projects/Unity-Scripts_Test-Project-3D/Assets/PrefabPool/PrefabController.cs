using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    public string despawnTag;
    public float despawnDelay;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(despawnTag))
        {
            StartCoroutine(Despawn(despawnDelay));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
    }

    private IEnumerator Despawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
