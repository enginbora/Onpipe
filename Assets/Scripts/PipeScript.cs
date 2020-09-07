using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    public enum PipeType{Small, Medium, Big}

    public int index; //used when sorting different PipeTypes. e.g. Big Pipe > Small Pipe, Bigs Index > Smalls Index

    public PipeType pipeType;

    public Vector3 targetScale;
    // Start is called before the first frame update
    void Start()
    {
        if (pipeType == PipeType.Big)
        {
            index = 3;
            targetScale = new Vector3(1.12f, 1.12f, 0.3f);
        }
        else if (pipeType == PipeType.Medium)
        {
            index = 2;
            targetScale = new Vector3(0.87f, 0.87f, 0.3f);
        }
        else if (pipeType == PipeType.Small)
        {
            index = 1;
            targetScale = new Vector3(0.58f, 0.58f, 0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
