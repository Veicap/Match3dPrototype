using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Stage : MonoBehaviour
{
    public static Stage Instance { get; set; }
    private readonly List<Item> items = new();
    [SerializeField] Transform point1, point2;
    public event EventHandler<OnCollectChangedEventArgs> OnCollect;
    public class OnCollectChangedEventArgs
    {
        public Item item1;
        public Item item2;
    }
    private void Awake()
    {
        Instance = this;    
    }
    
    public void AddItem(Item item)
    {
        if(items.Count == 0)
        {
            items.Add(item);
            item.OnMove(point1.position, Quaternion.identity, 0.2f);
            item.SetKinematic(true);
        }
        else if(items.Count == 1)
        {
            if (item.Type == items[0].Type)
            {
                items.Add(item);
                item.OnMove(point2.position, Quaternion.identity, 0.2f);
                item.SetKinematic(true);
                Invoke(nameof(Collect), 0.5f);
                
            }
            else
            {
                item.Force(Vector3.up * 200 + Vector3.forward * 200);
            }
        }
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        item.SetKinematic(false);
    }
    private void Collect()
    {
        items[0].Collect();
        items[1].Collect();
        OnCollect?.Invoke(this, new OnCollectChangedEventArgs
        {
            item1 = items[0],
            item2 = items[1]
        });
        items.Clear();
    }
}
