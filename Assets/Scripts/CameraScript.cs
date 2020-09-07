using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform slicer;

    private float distance;

    private Vector3 targetPos;
    
    private bool onPlay = true;

    private GameController gameController;

    [SerializeField] private GameObject confettiLeft;
    [SerializeField] private GameObject confettiRight;
    
    // Start is called before the first frame update
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.onFail.AddListener(OnFail);
        gameController.crossedFinishLine.AddListener(OnFinishLine);
        gameController.crossedFinishLine.AddListener(ActivateConfetti);
    }

    private void ActivateConfetti()
    {
        confettiLeft.SetActive(true);
        confettiRight.SetActive(true);
    }
    void Start()
    {
        distance = slicer.position.z - transform.position.z;
    }

    private void OnFail()
    {
        onPlay = false;
    }

    private void OnFinishLine()
    {
        onPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onPlay)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, slicer.transform.position.z - distance);
        }
        else
        {
            targetPos = Vector3.Lerp(
                new Vector3(transform.position.x, transform.position.y, slicer.transform.position.z - distance),
                transform.position, 1);
        }
        transform.position = targetPos;
    }
}
