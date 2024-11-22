using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragArea : MonoBehaviour
{
    public static DragArea Instance { get; private set; }
    private DragObject dragObjectLeft;
    private DragObject dragObjectRight;
    [SerializeField] GameObject dragAreaLeft;
    [SerializeField] GameObject dragAreaRight;

    private void Awake()
    {
        Instance = this;
    }
    public void SetDragObject(DragObject obj)
    {
        if(dragObjectLeft == null)
        {
            dragObjectLeft = obj;
        }
        else
        {
            dragObjectRight = obj;
        }
    }
    public void ClearDragObject()
    {
        if(dragObjectLeft != null && dragObjectRight != null)
        {
            dragObjectRight = null;
        }
        else if(dragObjectLeft != null && dragObjectRight == null)
        {
            dragObjectLeft = null;
        } 
    }
    public void CheckMatch()
    {
        if (dragObjectLeft != null && dragObjectRight != null)
        {
            if (dragObjectLeft.DragObjectSO().name == dragObjectRight.DragObjectSO().name)
            {
                Debug.Log("Match");
            }
            else
            {
                Debug.Log("Not match");
                
            }
        }
        
    }
    public GameObject DrageArea()
    {
        if(!dragObjectLeft)
        {
            return dragAreaLeft;
        }
        return dragAreaRight;
    }
}
