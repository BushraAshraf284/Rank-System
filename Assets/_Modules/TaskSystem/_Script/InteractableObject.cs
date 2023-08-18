using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] int XpsToGive;
    [SerializeField] private bool On;
    [SerializeField] MeshRenderer Material;
    [SerializeField] Color onColor;
    [SerializeField] Color offColor;

    public int XPEarned { get => XpsToGive; set => XpsToGive = value; }

    public void Interact()
    {
        Debug.Log("Interacting");
        LevelController.Instance.GiveXP(XpsToGive);
        On = !On;
        ChangeColor();
    }    
    private void Start()
    {
        Material = GetComponent<MeshRenderer>();
    }
    public void ChangeColor()
    {
        Debug.Log("Changing color");
        if (On)
        {
            Material.material.color = onColor;
        }
        else
        {
            Material.material.color = offColor;
        }

    }

    

}
