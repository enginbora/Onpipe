using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    
    [Header("Pipes")]
    [SerializeField] private GameObject bigPipe;

    [SerializeField] private GameObject medPipe;
    
    [SerializeField] private GameObject smallPipe;
    [Space(1)]
    [Header("Corn Rows")]
    [SerializeField] private GameObject bigCornRow;

    [SerializeField] private GameObject medCornRow;

    [SerializeField] private GameObject smallCornRow;
    [Space(1)]
    [Header("Obstacles")]
    [SerializeField] private GameObject smallObstacle;
    
    [SerializeField] private GameObject medObstacle;
    
    [SerializeField] private GameObject bigObstacle;
    
    [SerializeField] private GameObject outerObstacle;
    [Space(1)]
    [Header("Misc")]
    [SerializeField] private float rowDistance; //corn pieces fixed starting distance on pipes
    
    [SerializeField] private float rowCount; //how many corn rows do we want on a single pipe

    [SerializeField] private float distanceBetweenPipes;
    
    [SerializeField] private GameObject finishLine;

    private GameObject levelObjects;  //object on scene to hold every object in a level
    private GameObject lastPipe;
    private List<GameObject> pipeList; 
    private float pos2CreateZ;
    private GameController gameController;
    private PipeScript.PipeType ourType;
    private bool levelFinished = false;
    private bool createdFinishLine = false;


    // Start is called before the first frame update

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.onPipeExit.AddListener(GenerateLevel);
        gameController.onLevelComplete.AddListener(FinishLevel);
    }
    void Start()
    {
        levelObjects = GameObject.Find("Level Objects");
        pipeList = new List<GameObject>();
        pipeList.Add(bigPipe);
        pipeList.Add(medPipe);
        pipeList.Add(smallPipe);
        pos2CreateZ = -distanceBetweenPipes * 2; //creating the first pipe behind the camera so it looks seamless
        
        for (int i = 0; i < 3; i++)  //Calling GenerateLevel(); thrice at the start, so we can have three pipes ready
        {
            if (!levelFinished)
            {
                GenerateLevel();
            }
        } 
    }

    private void FinishLevel()
    {
        levelFinished = true;
        gameController.onPipeExit.RemoveListener(GenerateLevel);
        if (!createdFinishLine)
        {
            GenerateFinishLine();
            createdFinishLine = true;
        }
    }

    void GenerateLevel() //creates one single pipe and adds it right in front of the other one.
    {
        if (pipeList.Count < 1)
        {
            pipeList.Add(smallPipe);
            pipeList.Add(medPipe);
            pipeList.Add(bigPipe);
        }
        int random = Random.Range(0, pipeList.Count);
        GameObject pipeToCreate = pipeList[random];
        pipeList.Remove((pipeToCreate));
        Vector3 pos2Create = new Vector3(0, 0, pos2CreateZ + distanceBetweenPipes);
        GameObject newPipe = Instantiate(pipeToCreate, pos2Create, Quaternion.Euler(90, 0, 0));
        lastPipe = newPipe;
        AddCornRows(newPipe);
        float chanceToCreateObstacle = Random.Range(0, 100); //50% chance to create an obstacle on the pipe
        if (chanceToCreateObstacle < 50) //create obstacle
        {
            GameObject newObstacle;
            if (chanceToCreateObstacle < 25) //create inner obstacle
            {
                GameObject obs2Create;
                if (ourType == PipeScript.PipeType.Big)
                {
                    obs2Create = bigObstacle;
                }
                else if (ourType == PipeScript.PipeType.Medium)
                {
                    obs2Create = medObstacle;
                }
                else
                {
                    obs2Create = smallObstacle;
                }
                newObstacle = Instantiate(obs2Create, pos2Create + Vector3.forward * 8, Quaternion.identity);
                newPipe.transform.localScale += Vector3.up;
                pos2CreateZ += 2;
                newObstacle.transform.SetParent(levelObjects.transform);
                gameController.NewObjectCreated();
            }
            else //create outer obstacle
            {
                if (gameController.gameStarted)
                {
                    newObstacle = Instantiate(outerObstacle, new Vector3(0, 0, pos2CreateZ + distanceBetweenPipes / 2),
                        Quaternion.identity);    
                    newObstacle.transform.SetParent(levelObjects.transform);
                    gameController.NewObjectCreated();
                }
            }
        }
        newPipe.transform.SetParent(levelObjects.transform);
        pos2CreateZ += distanceBetweenPipes;
        gameController.NewObjectCreated();    
    }

    private void GenerateFinishLine()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos2Create = new Vector3(0, 0, pos2CreateZ + distanceBetweenPipes);
            GameObject newPipe = Instantiate(lastPipe, pos2Create, Quaternion.Euler(90, 0, 0));
            pos2CreateZ += distanceBetweenPipes;
            if (i == 0)
            {
                GameObject _finishLine = Instantiate(finishLine, pos2Create + (Vector3.forward * distanceBetweenPipes / 2), Quaternion.identity);
            }
        }
        
    }

    private void AddCornRows(GameObject pipe)
    {
        GameObject cornRow;
        float distanceBetweenRows;
        ourType = pipe.GetComponent<PipeScript>().pipeType;
        if (ourType == PipeScript.PipeType.Big)
        {
            cornRow = bigCornRow;
            distanceBetweenRows = 0.32f;
        }
        else if (ourType == PipeScript.PipeType.Medium)
        {
            cornRow = medCornRow;
            distanceBetweenRows = 0.31f;
        }
        else
        {
            cornRow = smallCornRow;
            distanceBetweenRows = 0.26f;
        }
        
        var newCornRow = new GameObject();
        newCornRow.transform.SetParent(levelObjects.transform);
        newCornRow.name = "New Corn Row";
        
        for (int i = 0; i < rowCount; i++)
        {
            Vector3 pos2Create = new Vector3(0, 0, pipe.transform.position.z + rowDistance + i * distanceBetweenRows);
            var newPiece = Instantiate(cornRow, pos2Create, Quaternion.identity);
            newPiece.transform.SetParent(newCornRow.transform);
        }
    }
}
