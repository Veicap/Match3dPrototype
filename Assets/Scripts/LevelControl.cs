using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class LevelControl : MonoBehaviour
{
    public static LevelControl Instance { get; private set; }
    [SerializeField] private List<Item> items = new();
    [SerializeField] private float levelTimer;
    [SerializeField] private TextMeshProUGUI textCounter;
    [SerializeField] private TextMeshProUGUI winStateText, loseStateText;
    public event EventHandler OnLoadLevel;
    
    private List<Item> listItemSpawn;
    public float radius = 1f;
    public Vector3 regionSize = Vector3.one;
    public int rejectionSamples = 30;
    public float displayRadius = 0.1f;
    List<Vector3> points;


    public enum GameState
    {
        Play,
        Lose,
        Win,
        LoadLevel
    }

    private GameState state;
    public GameState State => state;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        listItemSpawn = new List<Item>();
        Stage.Instance.OnCollect += Stage_OnCollect;
        state = GameState.LoadLevel;
        HideLoseStateText();
        HideWinStateText();
        Match3DManager.Instance.ChangeLevel(); // start level 1
    }

    void OnValidate()
    {
        points = PointSpawn.GeneratePoints(radius, regionSize, rejectionSamples);
    }
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(regionSize / 2, regionSize);

        if (points != null)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }*/
    private void Update()
    {
        if (state == GameState.Play)
        {
            levelTimer -= Time.deltaTime;
            UpdateTimer(levelTimer);

            if (levelTimer <= 0)
            {
                SwitchState(GameState.Lose);
            }
            else if (listItemSpawn.Count == 0)
            {
                SwitchState(GameState.Win);
            }
        }
        if(state == GameState.LoadLevel)
        {
            OnLoadLevel?.Invoke(this, EventArgs.Empty);
            LoadLevel();
            
            SwitchState(GameState.Play);
        }
    }

    private void SwitchState(GameState newState)
    {
        state = newState;

        switch (state)
        {
            case GameState.Win:
                ShowWinStateText();
                Match3DManager.Instance.ChangeLevel(); // Change Index level
                StartCoroutine(IESwitchStateLoadLevel());
                break;

            case GameState.Lose:
                ShowLoseStateText();
                Match3DManager.Instance.LoadLevel(Match3DManager.Instance.LevelIndex); // Kepp index level
                StartCoroutine(IESwitchStateLoadLevel());
                break;

            case GameState.LoadLevel:
                break;

            case GameState.Play:
                HideLoseStateText();
                HideWinStateText();
                break;
        }
    }

    private IEnumerator IESwitchStateLoadLevel()
    {
        
        yield return new WaitForSeconds(0.8f);
        foreach (var item in listItemSpawn)
        {
            Destroy(item.gameObject);
        }
        SwitchState(GameState.LoadLevel);
    }

    private void LoadLevel()
    {
        items = Match3DManager.Instance.ListItems;
        listItemSpawn.Clear();
        HideWinStateText();
        SpawnObjects();
        SwitchState(GameState.Play);
    }

    private void UpdateTimer(float timer)
    {
        timer += 1;
        int minute = Mathf.FloorToInt(timer / 60);
        int second = Mathf.FloorToInt(timer % 60);
        textCounter.text = $"{minute:00}:{second:00}";
    }

    private void Stage_OnCollect(object sender, Stage.OnCollectChangedEventArgs e)
    {
        listItemSpawn.Remove(e.item1);
        listItemSpawn.Remove(e.item2);
    }

    private void ShowWinStateText() => winStateText.gameObject.SetActive(true);

    private void HideWinStateText() => winStateText.gameObject.SetActive(false);

    private void HideLoseStateText() => loseStateText.gameObject.SetActive(false);

    private void ShowLoseStateText() => loseStateText.gameObject.SetActive(true);

    
    private void SpawnObjects()
    {
        levelTimer = Match3DManager.Instance.LevelTimer;
        var shuffledItems = new List<Item>(items);
        Debug.Log(shuffledItems.Count);
        Debug.Log(shuffledItems.Count);
        ShuffleList(shuffledItems);
        int count = 0;
        foreach (var item in shuffledItems)
        {
            Vector3 spawnPosition = points[count];
            Item spawnItem = Instantiate(item, spawnPosition, Quaternion.identity);
            listItemSpawn.Add(spawnItem);
            count++;
           
        }
    }
    

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
