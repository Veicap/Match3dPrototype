using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    public static LevelControl Instance { get; private set; }
    [SerializeField] private List<Item> items = new();
    [SerializeField] private float levelTimer;
    [SerializeField] private TextMeshProUGUI textCounter;
    [SerializeField] private TextMeshProUGUI winStateText, loseStateText;
    [SerializeField] private TextMeshProUGUI levelText;
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
        if(Instance == null)
        {
            Instance = this;
        }
        Debug.Log(Instance.ToString());
        points = PointSpawn.GeneratePoints(radius, regionSize, rejectionSamples);
        Debug.Log("Radius: " + radius.ToString());
        Debug.Log("regionSize: " + regionSize.ToString());
        Debug.Log("rejectionSamples: " + rejectionSamples.ToString());
    }
    private void Start()
    {
        listItemSpawn = new List<Item>();
        Stage.Instance.OnCollect += Stage_OnCollect;
        HideLoseStateText();
        HideWinStateText();
        // start level 1
        Debug.Log("Game Start - Checking Match3DManagerLevel Instance...");
        if (Match3DManagerLevel.Instance == null)
        {
            Debug.LogError("Match3DManagerLevel.Instance is NULL at Start!");
            return;
        }

        Debug.Log("Calling ChangeLevel...");
        Match3DManagerLevel.Instance.ChangeLevel();
        Debug.Log("After ChangeLevel()");
        SwitchState(GameState.LoadLevel);
        Debug.Log("Number of points: " + points.Count.ToString());
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
        /*if(state == GameState.LoadLevel)
        {
            LoadLevel();
            OnLoadLevel?.Invoke(this, EventArgs.Empty);
            SwitchState(GameState.Play);

        }*/
        /*Debug.Log(state);*/
    }

    public void SwitchState(GameState newState)
    {
        state = newState;

        switch (state)
        {
            case GameState.Win:
                ShowWinStateText();
                Match3DManagerLevel.Instance.ChangeLevel(); // Change Index level
                StartCoroutine(IESwitchStateLoadLevel());
                break;

            case GameState.Lose:
                ShowLoseStateText();
                Match3DManagerLevel.Instance.LoadLevel(Match3DManagerLevel.Instance.LevelIndex); // Kepp index level
                StartCoroutine(IESwitchStateLoadLevel());
                break;

            case GameState.LoadLevel:
                LoadLevel();
                OnLoadLevel?.Invoke(this, EventArgs.Empty);
                break;
            case GameState.Play:
                HideLoseStateText();
                HideWinStateText();
                break;
        }

       // Debug.LogError(state);
    }

    private IEnumerator IESwitchStateLoadLevel()
    {
        yield return new WaitForSeconds(0.8f);
        SwitchState(GameState.LoadLevel);
    }

    private void LoadLevel()
    {
        foreach (var item in listItemSpawn)
        {
            Destroy(item.gameObject);
        }
        levelText.text = Match3DManagerLevel.Instance.Level.ToString();
        items = Match3DManagerLevel.Instance.ListItems;
        listItemSpawn.Clear();
        levelTimer = Match3DManagerLevel.Instance.LevelTimer;
        Debug.Log(levelTimer);
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


    void SpawnObjects()
    {
        Debug.Log("SpawnObjects called");

        if (items == null)
        {
            Debug.LogError("items is NULL!");
            return;
        }
        if (items.Count == 0)
        {
            Debug.LogError("items is EMPTY!");
            return;
        }

        
        var shuffledItems = new List<Item>(items);
        ShuffleList(shuffledItems);
        Debug.Log("Shuffled items count: " + shuffledItems.Count);

        // Kiểm tra points
        /*if (points == null)
        {
            Debug.LogError("points is NULL!");
            return;
        }
        if (points.Count < shuffledItems.Count)
        {
            Debug.LogError("points count is LESS than shuffledItems count!");
            return;
        }*/

        if (listItemSpawn == null)
        {
            Debug.LogWarning("listItemSpawn is NULL! Initializing it now.");
            listItemSpawn = new List<Item>();
        }

   
        int count = 0;
        foreach (var item in shuffledItems)
        {
            if (item == null)
            {
                Debug.LogError("Item at index " + count + " is NULL! Skipping...");
                continue;
            }

            Vector3 spawnPosition = points[count];
            Debug.Log("Spawning item " + count + " at position " + spawnPosition);

            Item spawnItem = Instantiate(item, spawnPosition, Quaternion.identity);
            listItemSpawn.Add(spawnItem);

            count++;
        }

        Debug.Log("SpawnObjects completed successfully.");
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
