using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private List<Item> items = new();
    private List<Item> listItemSpawn;
    [SerializeField] private float levelTimer;
    [SerializeField] private TextMeshProUGUI textCounter;
    [SerializeField] private TextMeshProUGUI winStateText, loseStateText;

    [SerializeField] private Vector3 minBounds = new(-2.5f, 1f, -2.5f);
    [SerializeField] private Vector3 maxBounds = new(3f, 2f, 2.7f);
    [SerializeField] private int cellSize = 1;
    private Dictionary<Vector3, bool> occupicedCell = new();


    public enum GameState
    {
        Play,
        Lose,
        Win,
        LoadLevel
    }

    private GameState state;

    private void Start()
    {
        listItemSpawn = new List<Item>();
        Stage.Instance.OnCollect += Stage_OnCollect;
        state = GameState.LoadLevel;
        HideLoseStateText();
        HideWinStateText();
    }

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
            StartCoroutine(IELoadLevel());
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
                StartCoroutine(IESwitchStateLoadLevel());
                break;

            case GameState.Lose:
                ShowLoseStateText();
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
        SwitchState(GameState.LoadLevel);
    }

    private IEnumerator IELoadLevel()
    {

        Match3DManager.Instance.ChangeLevel();
        items = Match3DManager.Instance.ListItems;
        listItemSpawn.Clear();
        occupicedCell.Clear();
        HideWinStateText();
        yield return StartCoroutine(IELoadGame());
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

    private IEnumerator IELoadGame()
    {
        yield return StartCoroutine(IESpawnObjects());
    }
    private IEnumerator IESpawnObjects()
    {
        levelTimer = Match3DManager.Instance.LevelTimer;
        var shuffledItems = new List<Item>(items);
        Debug.Log(shuffledItems.Count);
        ShuffleList(shuffledItems);

        int maxAttempts = 100; // Giới hạn số lần thử spawn
        int attempts = 0;

        foreach (var item in shuffledItems)
        {
            bool spawned = false;

            while (!spawned && attempts < maxAttempts)
            {
                attempts++; // Tăng số lần thử spawn
                Vector3 pawnPosition = GetRandomPosition();
                Vector3Int cell = GetCell(pawnPosition);

                if (!occupicedCell.ContainsKey(cell))
                {
                    Item spawnItem = Instantiate(item, pawnPosition, Quaternion.identity);
                    listItemSpawn.Add(spawnItem);
                    occupicedCell[cell] = true;
                    spawned = true;
                }

                yield return null;
            }

            // Nếu đã thử quá nhiều lần mà không spawn được, bạn có thể dừng hoặc thông báo
            if (!spawned)
            {
                Debug.LogWarning("Không đủ không gian để spawn hết các đối tượng.");
                break; // Hoặc xử lý thêm nếu cần
            }
        }
    }


    private Vector3Int GetCell(Vector3 spawnPos)
    {
        return new Vector3Int(
            Mathf.FloorToInt(spawnPos.x /cellSize),
            Mathf.FloorToInt(spawnPos.y / cellSize),
            Mathf.FloorToInt(spawnPos.z / cellSize)
            );
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);
        return new Vector3(randomX, randomY, randomZ);
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
