using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObject : MonoBehaviour
{
    public List<InteractableObject> interactableObjects;
    public List<GameObject> otherObjects;

    private void Start()
    {
        interactableObjects = new List<InteractableObject>(GetComponentsInChildren<InteractableObject>());
    }
    public int CalculateTaskTotalXP()
    {
        int total = 0;
        foreach(var obj in interactableObjects)
        {
            total += obj.XPEarned;
        }
        return total;
    }
}
