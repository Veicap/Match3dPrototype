using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    internal void OnMove(Vector2 pointToFllow)
    {
        rb.position = Vector3.MoveTowards(rb.position, pointToFllow, Time.deltaTime);
    }

    internal void OnSelect()
    {
        rb.useGravity = false;
    }

    internal void OnDrop()
    {
        rb.useGravity = true;
    }
}
