using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 2;
    [SerializeField] private ItemType type;

    public ItemType Type => type;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    internal void OnMove(Vector3 pointToFllow)
    {
        rb.position = Vector3.MoveTowards(rb.position, pointToFllow, Time.deltaTime * speed);
    }
    public void OnMove(Vector3 targetPoint, Quaternion targetRot, float time)
    {
        StartCoroutine(IEOnMove(targetPoint, targetRot, time));
    }

    private IEnumerator IEOnMove(Vector3 targetPoint, Quaternion targetRot, float time)
    {
        float timeCount = 0;
        Vector3 startPoint = rb.position;
        Quaternion startRot = rb.rotation;

        while (timeCount < time)
        {
            timeCount += Time.deltaTime;
            rb.position = Vector3.Lerp(startPoint, targetPoint, timeCount / time);
            rb.rotation = Quaternion.Lerp(startRot, targetRot, timeCount / time);
            yield return null;
        }

    }
    internal void OffGravity()
    {
        rb.useGravity = false;
    }

    internal void OnGravity()
    {
        rb.useGravity = true;
    }

    internal void SetKinematic(bool v)
    {
        rb.isKinematic = v;
    }

    internal void Force(Vector3 force)
    {
        OnGravity();
        rb.velocity = Vector3.zero;
        rb.AddForce(force);
    }

    internal void Collect()
    {
        Destroy(gameObject);
        
    }
}
