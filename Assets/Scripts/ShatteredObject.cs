using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredObject : MonoBehaviour
{
    private List<GameObject> shards;
    // Start is called before the first frame update
    void Start()
    {
        shards = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            shards.Add(transform.GetChild(i).gameObject);
        }

        foreach (GameObject shard in shards)
        {
            var rb = shard.AddComponent<Rigidbody>();
            rb.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(1, 2), Random.Range(-1, 1)) * 10, ForceMode.Impulse);
            rb.AddTorque(Vector3.one * Random.Range(-2, 2) * 60);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}