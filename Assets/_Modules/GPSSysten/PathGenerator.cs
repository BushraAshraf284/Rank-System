using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class PathGenerator : MonoBehaviour
{
    public LineRenderer path;
    public NavMeshAgent agent;
    public List<Vector3> point;
    public Transform target;
    public Transform player;
    NavMeshPath agentPath;
    private bool isNavigating;

    private void Start()
    {
        path = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agentPath = new NavMeshPath();
        isNavigating = false;
        path.startWidth = 5;
        path.endWidth = 5;
       
    }
    /// <summary>
    /// Starts Navigation towards the target
    /// </summary>
    public void StartNavigating()
    {
        agent.CalculatePath(target.position, agentPath);
        isNavigating = true;
    }
    /// <summary>
    /// Stops Navigation towards the target
    /// </summary>
    public void StopNavigation()
    {
        isNavigating = false;
        path.positionCount = 0;        
    }
    /// <summary>
    /// Sets the target for Navigation
    /// </summary>
    public void SetTarget(Transform transform)
    {
        target = transform;
        agent.CalculatePath(target.position, agentPath);
    }
   
    void Update()
    {
        if(isNavigating)
        {
            if (!agent.SetDestination(player.position) || Vector3.Distance(transform.position, player.position) > 1)
            {
                gameObject.transform.position = player.position;
                if(path.positionCount!=0)
                    path.SetPosition(0, player.position);
            }
            DisplayPath();
        }
       
    }
    /// <summary>
    /// Calculates the Path and displays it on the minimap
    /// </summary>
    public void DisplayPath()
    {
        agent.CalculatePath(target.position, agentPath);
        if (agentPath.corners.Length < 2) return;

        int i = 1;
        while(i < agentPath.corners.Length)
        {
            path.positionCount = agentPath.corners.Length;
            point = agentPath.corners.ToList();
            for(int j=0;j< point.Count;j++)
            {
                Vector3 pos = new Vector3(point[j].x, 1, point[j].z);
                path.SetPosition(j, pos);
            }

            i++;
        }
    }
}
