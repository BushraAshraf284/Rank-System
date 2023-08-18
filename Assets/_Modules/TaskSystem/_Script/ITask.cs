using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface ITask 
{

    public delegate void OnTask();
    public void AssignTask(OnTask method);    

    public void PerformTask();
    public void EndTask();

   
}
