using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelController : MonoBehaviour
{
    
    [SerializeField] List<Task> currentLevelTasks;
    [SerializeField] int currentTasksXP =0;
    [SerializeField] Task currentTask;
    private bool isTaskInProgress;

    public static LevelController Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Debug.Log("Mission Assigned");
        GameManagerTest.Instance.OnGameInitialized += InitializeGame;
        GameManagerTest.Instance.InitializeGame();
    }

    public void InitializeGame()
    {
        isTaskInProgress = false;
        Debug.Log("In Function");
        AssignMission();
        CreateTaskObjects();
    }
    /// <summary>
    /// Called After every interaction with interactive objects or sib task completion
    /// </summary>
    public void CheckTaskRequirement()
    {
        if(currentTasksXP> currentTask.totalXPs)
        {
            currentTasksXP = currentTask.totalXPs;
        }
        RankController.Instance.ShowProgress(currentTasksXP);
        if (currentTasksXP == currentTask.totalXPs)
        {           
            currentTask.EndTask();
            currentTasksXP = 0;           
            isTaskInProgress = false;
            
        }
    }
    /// <summary>
    /// Assigns tasks to the player
    /// </summary>
    public void AssignMission()
    {
        Debug.Log("Assign Task");
        if(!isTaskInProgress)
        {
            int taskIndex = 0;
            if (currentLevelTasks.Count > 1)
            {
                float MidPoint = Mathf.Round(currentLevelTasks.Count / 2);
                taskIndex = GetRandomTask((int)MidPoint, currentLevelTasks.Count);
                currentTask.SetTaskTotalXPs();
            }
            currentTask = currentLevelTasks[taskIndex];
           
            isTaskInProgress = true;
        }
      
    }
    

    /// <summary>
    /// Gets the index of the task with the less probability of previous tasks and more probability of recent tasks
    /// </summary>
    /// <param name="Mid">from where the tasks will be distinguished</param>
    /// <param name="Count">the total tasks available</param>
    /// <returns>The index of the task</returns>
    int GetRandomTask(int Mid, int Count)
    {
        float rand = Random.value;
        if (rand <= 0.6f)
            return Random.Range(Mid, Count);
        else
            return Random.Range(0, Mid);
    }
/*
    Vector3 GetRandomPosition(Vector3 reference)
    {
        float x = Random.Range(reference.x-5, reference.x + 5);
        float y = 1;
        float z = Random.Range(reference.z-5, reference.z +5);
        return new Vector3(x, y, z);
       
    }*/
    /// <summary>
    /// Function to perform on button click
    /// </summary>
    public void InteractWithObject()
    {
        currentTask.PerformTask();
        
    }
    /// <summary>
    /// Gives XP after sub-task completion
    /// </summary>
    /// <param name="xp"></param>
    public void GiveXP(int xp)
    {
        currentTasksXP += xp;
        Debug.Log(currentTasksXP);
        CheckTaskRequirement();

    }
    /// <summary>
    /// Instantiates interactable objects
    /// </summary>
    public void CreateTaskObjects()
    {
        Debug.Log("CreateObjects");
        if (currentTask.taskObject)
        {
            Debug.Log("Going to create");
            Instantiate(currentTask.taskObject.gameObject, currentTask.SpawnPosition, Quaternion.Euler(currentTask.SpawnRotation)).SetActive(true);
        }
    }

    public Task ReturnCurrentTask()
    {
        return currentTask;
    }
}

[System.Serializable]
public class Task : ITask
{
    public int totalXPs;
    public TaskObject taskObject;
    public ITask.OnTask onAssignTask; //Subscribe the task to perform to this
    public ITask.OnTask onEndTask;
    public Vector3 SpawnPosition;
    public Vector3 SpawnRotation;

    /// <summary>
    /// Call the function assigned to the onAssignTask delegate
    /// eg: Instatiate relevant objects
    /// </summary>
    public void PerformTask()
    {
        onAssignTask?.Invoke();
    }

    public void EndTask()
    {
        onEndTask?.Invoke();
    }

    public void AssignTask(ITask.OnTask method)
    {
        onAssignTask += method;
    }

    public void SetTaskTotalXPs()
    {
        totalXPs = taskObject.CalculateTaskTotalXP();
    }

   
}


