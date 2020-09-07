using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class CornRowScript : MonoBehaviour
{
    private List<GameObject> children;

    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        children = new List<GameObject>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
    }

    public void OnContact()
    {
        foreach (GameObject child in children)
        {
            var rb = child.AddComponent<Rigidbody>();
            //rb.AddForce(new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(2, 3f), 0.1f) * 6, ForceMode.Impulse);
            rb.AddForce(new Vector3(0, Random.Range(2, 3), -0.4f) * 6, ForceMode.Impulse);
            rb.AddForce(rb.transform.up * 3, ForceMode.Impulse);
            rb.AddTorque(Vector3.one * Random.Range(-2, 2) * 60);
            Destroy(child, 1.5f);
        }
        gameController.AddScore(1);
    }
}