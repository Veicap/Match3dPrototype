using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private DragObjectSO dragObjectSO;
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        Vector3 worldMousePos = GetMouseWorldPosition();
        offset = transform.position - worldMousePos;
        isDragging = true;
    }
    private void OnMouseDrag()
    {
        if(isDragging)
        {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            transform.position = currentMousePosition + offset;
        }
    }
    private void OnMouseUp()
    {
        if(Physics.Raycast(transform.position, Vector3.down,out RaycastHit hit, 1f, layerMask))
        {
            if (hit.collider != null)
            {
                DropArea.Instance.SetDragObject(this, hit);
            }  
        }
        else
        {
            DropArea.Instance.ClearDragObject(this);
        }
        isDragging = false;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(screenMousePos);
    }
    public DragObjectSO DragObjectSO()
    {
        return dragObjectSO;
    }
}
