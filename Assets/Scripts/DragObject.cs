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
        rb.useGravity = false;
        isDragging = true;
        DeactivateKinematic(this);
    }
    private void OnMouseDrag()
    {
        if(isDragging)
        {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 cursorPosition = currentMousePosition + offset;
            rb.position = cursorPosition;
            rb.MovePosition(new Vector3(rb.position.x, 1f, rb.position.z));
        }
    }
    private void OnMouseUp()
    {
        /*if(Physics.Raycast(transform.position, Vector3.down,out RaycastHit hit, 2f, layerMask))
        {
            if (hit.collider != null)
            {
                DropArea.Instance.SetDragObject(this);
                Debug.Log("Collision with drop area");
            }  
        }
        else
        {
            DropArea.Instance.ClearDragObject(this);
        }*/
        rb.useGravity = true;
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
    public void ActivateKinematic(DragObject dragObject)
    {
        dragObject.rb.isKinematic = true;
    }
    public void DeactivateKinematic(DragObject dragObject)
    {
        dragObject.rb.isKinematic = false;
    }
}
