using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjects : MonoBehaviour
{
    private GameController gameController;
    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.onObjectCreated.AddListener(CheckChildCount);
    }

    private void CheckChildCount()
    {
        if (transform.childCount > 15)
        {
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
        }
    }
    
}
