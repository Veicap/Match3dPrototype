using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    public static DropArea Instance { get; private set; }
    private DragObject dragObjectLeft;
    private bool hasDropObjectLeft = false;
    [SerializeField] GameObject dropAreaLeft;
    [SerializeField] GameObject dropAreaRight;
    private void Awake()
    {
        Instance = this;
    }
    public void SetHasDropObjectLeft(bool hasDropObjectLeft)
    {
        this.hasDropObjectLeft = hasDropObjectLeft;
    }
    public void SetDragObject(DragObject dragObject, RaycastHit hit)
    {
        if(!hasDropObjectLeft)
        {
            dragObjectLeft = dragObject;
            dragObject.transform.SetPositionAndRotation(dropAreaLeft.transform.position + new Vector3(0, 0.5f, 0), hit.transform.rotation);
            hasDropObjectLeft = true;
        }
        else
        {
            if(dragObject == dragObjectLeft)
            {
                dragObject.transform.SetPositionAndRotation(dropAreaLeft.transform.position + new Vector3(0, 0.5f, 0), hit.transform.rotation);
            }
            else
            {
                if (CheckMatch(dragObjectLeft, dragObject))
                {
                    Debug.Log("Match");
                    dragObject.transform.SetPositionAndRotation(dropAreaRight.transform.position + new Vector3(0, 0.5f, 0), hit.transform.rotation);
                }
                else
                {
                    Debug.Log("Not Match");
                    Vector3 direction = (dragObject.transform.position - dropAreaRight.transform.position).normalized;
                    float distance = 2.0f;
                    dragObject.transform.position += direction * distance;
                }
            }
            
        }
       
    }
    public void ClearDragObject(DragObject dragObject)
    {
        if(dragObjectLeft == dragObject)
        {
            dragObjectLeft = null;
            hasDropObjectLeft= false;
        }
    }
    private bool CheckMatch(DragObject left, DragObject right)
    {
        if(left.DragObjectSO().name == right.DragObjectSO().name)
        {
            return true;
        }
        return false;
    }
}
