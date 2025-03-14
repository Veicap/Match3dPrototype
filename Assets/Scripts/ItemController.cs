using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask itemLayer, groundLayer, stageLayer;
    [SerializeField] private Stage stage;
    private Item itemSelecting;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            itemSelecting = GetItemSelecting();
            if(itemSelecting != null)
            {
                itemSelecting.OffGravity();
                stage.RemoveItem(itemSelecting);
            }
        }
        if(Input.GetMouseButton(0))
        {
            if (itemSelecting != null)
            {
                itemSelecting.OnMove(GetPointToItemFollow());
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (itemSelecting != null)
            {
                Stage stage = GetStageArea();
                if (stage != null)
                {
                   stage.AddItem(itemSelecting);
                }
                else
                {
                    itemSelecting.OnGravity();
                }
            }
        }
      
    }

    private Item GetItemSelecting()
    {
        // can biet vi tri cua con chuot
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // neu cai ray bat trung object thi se tra ve selecting object;
        if(Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit ,100,  itemLayer))
        {
            if(hit.collider !=  null)
            {
               
                return hit.collider.gameObject.GetComponent<Item>();
            }
        }
        return null;
    }
    private Stage GetStageArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // neu cai ray bat trung object thi se tra ve stage object;
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, stageLayer))
        {
            if (hit.collider != null)
            { 
                return hit.collider.gameObject.GetComponent<Stage>();
            }
        }
        return null;
    }
    private Vector3 GetPointToItemFollow()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // neu cai ray bat trung object thi se tra ve selecting object;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, groundLayer))
        {
            if (hit.collider != null)
            {
                float x = (ray.origin.y - 2) * Vector3.Distance(ray.origin, hit.point) / ray.origin.y;
                return ray.origin + ray.direction * x;
            }
        }
        return Vector3.zero;
    }
}
