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
    [SerializeField] Transform middlePoint;
    [SerializeField] private float speed;
    public event EventHandler <OnCollectChangedEventArgs> OnCollect;
    public event EventHandler OnMatch;
    public class OnCollectChangedEventArgs
    {
        public Item item1;
        public Item item2;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
          
    }
    private void Start()
    {
        LevelControl.Instance.OnLoadLevel += LevelControl_OnLoadLevel;
    }

    private void LevelControl_OnLoadLevel(object sender, EventArgs e)
    {
        items.Clear();
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
                OnMatch?.Invoke(this, EventArgs.Empty);
                item.OnMove(point2.position, Quaternion.identity, 0.2f);
                item.SetKinematic(true);
                StartCoroutine(IEAnim(items[0], items[1]));
                Invoke(nameof(Collect), 0.8f);
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
        if(items.Count > 0)
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
    private void MoveObjectsToMiddle(Item item1, Item item2)
    {
        if(item1 == null || item2 == null) return;
        item1.transform.position = Vector3.MoveTowards(item1.transform.position, middlePoint.position, speed * Time.deltaTime);
        item2.transform.position = Vector3.MoveTowards(item2.transform.position, middlePoint.position, speed * Time.deltaTime);
    }
    private IEnumerator IEAnim(Item item1, Item item2)
    {
        yield return new WaitForSeconds(0.3f);
        float time = 0;
        while(time <= 1)
        {
            time += Time.deltaTime;
            MoveObjectsToMiddle(item1, item2);
            yield return null;
        }
    }
}
