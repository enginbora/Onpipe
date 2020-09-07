using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private GameController gameController;
    private Vector3 targetScale;    //vector we would scale if we hold
    private Vector3 startScale;     //original scale vector
    private Vector3 updateScale;    //vector we are currently scaling in update 
    private bool holding;
    private bool inCollision1; //currently in collision with an inner obstacle
    private bool inCollision2; //currently in collision with an outer obstacle
    private bool isAlive = true;
    private bool crossedLine;

    [SerializeField] private float speed;
    [SerializeField] private float scaleTime;
    [SerializeField] private GameObject shatteredSlicer;    
    
    private int lastPipeIndex;
    private PipeScript currentPipe;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.crossedFinishLine.AddListener(CrossedLine);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScale = transform.localScale;
        updateScale = startScale;
    }

    private void CrossedLine()
    {
        crossedLine = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            updateScale = currentPipe.targetScale;
            holding = true;
            
            if (lastPipeIndex == 0)
            {
                lastPipeIndex = currentPipe.index;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            updateScale = startScale;
            holding = false;
        }
        
        transform.localScale = Vector3.MoveTowards(transform.localScale, updateScale, scaleTime);

        if (isAlive)
        {
            if ((inCollision1 && holding) || (inCollision2 && !holding))
            {
                Fail();
            }
        }
    }

    void FixedUpdate()
    {
        if (isAlive && !crossedLine)
        {
            rb.velocity = Vector3.forward * (speed * Time.deltaTime);
        }
        else if (crossedLine)
        {
            speed = Mathf.Lerp(speed, 1000, 0.5f);
            rb.velocity = Vector3.forward * (speed * Time.deltaTime);
        }
        else if(!isAlive)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pipe"))
        {
            gameController.OnPipeExit();
            CalculateTargetScale(other.gameObject);
        }

        if (other.CompareTag("CornPiece") && holding && isAlive)
        {
            other.gameObject.SendMessage("OnContact");
            gameController.OnFever();
        }

        if (other.CompareTag("CornPiece") && !holding && isAlive)
        {
            gameController.ComboBreaker();
        }

        if (other.CompareTag("OuterObstacle"))
        {
            inCollision2 = true;
        }

        if (other.CompareTag("Obstacle"))
        {
            inCollision1 = true;
        }

        if (other.CompareTag("FinishLine"))
        {
            gameController.CrossedFinishLine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OuterObstacle"))
        {
            inCollision2 = false;
        }

        if (other.CompareTag("Obstacle"))
        {
            inCollision1 = false;
        }
    }

    private void CalculateTargetScale(GameObject pipe)
    {
        currentPipe = pipe.GetComponent<PipeScript>();
        
        if (holding)
        {
            if ((lastPipeIndex >= currentPipe.index))
            {
                updateScale = currentPipe.targetScale;
            }
            else
            {
                Fail();
            }
        }
        lastPipeIndex = currentPipe.index;
    }

    private void Fail()
    {
        isAlive = false;
        gameController.Failed();
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(shatteredSlicer, transform.position, Quaternion.identity);
    }
}