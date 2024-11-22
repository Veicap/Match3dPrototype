using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        Vector3 worldMousePos = GetMouseWorldPosition();
        offset = transform.position - worldMousePos;
        isDragging = true;
       // transform.GetComponent<Collider>().enabled = false;
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
        var rayOrigin = mainCamera.transform.position;
        var rayDirection = GetMouseWorldPosition() - mainCamera.transform.position;
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("DropArea"))
            {
                transform.position = hit.transform.position;
            }
        }
        transform.GetComponent<Collider>().enabled = true;
        isDragging = false;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(screenMousePos);
    }
}
