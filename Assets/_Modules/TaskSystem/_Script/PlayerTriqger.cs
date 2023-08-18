using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTriqger : MonoBehaviour
{
    public List<GameObject> colliders;
    IInteractable prev;
    InteractableObject col;
    bool checking;
    private void Awake()
    {
        GameManagerTest.Instance.AfterGameInitialized += TaskToDo;   //assigning the task to perform
    }
    // Update is called once per frame
    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), GetComponentInParent<CharacterController>());
        checking = false;
        
    }

    public void TaskToDo()
    {
        Debug.Log("Task is assigned");
        LevelController.Instance.ReturnCurrentTask().AssignTask(CheckClosest);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent(out col))
        {
            if (!colliders.Contains(other.gameObject))
            {
                colliders.Add(other.gameObject);
                Debug.Log("Count" + colliders.Count);
            }
        }
    }

    void CheckClosest()
    {
        Debug.Log("im heree");
        checking = true;
        colliders.Sort(ByDistance);
        if (prev != null)
            prev.Interact();
        var closest = colliders[0].GetComponent<IInteractable>();
        closest.Interact();
        prev = closest;
        checking = false;
    }
    public int ByDistance( GameObject a, GameObject b)
     {
        var dstToA = Vector3.Distance(transform.position, a.transform.position);
        var dstToB = Vector3.Distance(transform.position, b.transform.position);
        return dstToA.CompareTo(dstToB);
        
     }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out col))
        {
            colliders.Remove(other.gameObject);

        }
    }
}
