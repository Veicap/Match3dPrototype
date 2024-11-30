using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask itemLayer, groundLayer, stageLayer;
    private Item itemSelecting;
    private Stage stage;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            itemSelecting = GetItemSelecting();
            if(itemSelecting != null )
            {
                itemSelecting.OnSelect();
            }
        }
        if(Input.GetMouseButton(0))
        {
            itemSelecting = GetItemSelecting();
            if (itemSelecting != null )
            {
                itemSelecting.OnMove(GetPointToItemFollow());
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            stage = GetStageArea();
            if(stage != null &&  itemSelecting != null)
            {

            }
            else if(itemSelecting != null)
            {
                itemSelecting.OnDrop();
            }
        }
    }

    private Item GetItemSelecting()
    {
        // can biet vi tri cua con chuot
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // neu cai ray bat trung object thi se tra ve selecting object;
        if(Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit ,100,  itemLayer ))
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
        // neu cai ray bat trung object thi se tra ve selecting object;
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
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100, groundLayer))
        {
            if (hit.collider != null)
            {
                float x = (ray.origin.y - 2) / ray.origin.y;
                float offset = x * Vector3.Distance(ray.origin, hit.point);
                return ray.origin + ray.direction * offset;
            }
        }
        return Vector3.zero;
    }
}
