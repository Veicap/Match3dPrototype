using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask layerMask;
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        Vector3 worldMousePos = GetMouseWorldPosition();
        offset = transform.position - worldMousePos;
        isDragging = true;
        rb.isKinematic = false;
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
                rb.isKinematic = true;
                
                transform.SetPositionAndRotation(hit.transform.position + new Vector3(0, 0.5f, 0), hit.transform.rotation);
            }
        }
        isDragging = false;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(screenMousePos);
    }
}
