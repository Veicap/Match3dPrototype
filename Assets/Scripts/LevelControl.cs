using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private List<Item> items = new();
    private List<Item> listItemSpawn;
    [SerializeField]
    private float
        levelTimer;
    [SerializeField] private TextMeshProUGUI textCounter;
    [SerializeField] private TextMeshProUGUI winStateText, loseStateText;
    private bool isLoad = false;
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
        items = Match3DManager.Instance.ListItems;
        Stage.Instance.OnCollect += Stage_OnCollect;
        state = GameState.Play;
        HideLoseStateText();
        HideWinStateText();
        LoadGame();
    }
    private void Update()
    {
        if (state == GameState.Play)
        {
            levelTimer -= Time.deltaTime;
            UpdateTimer(levelTimer);
        }
        if(levelTimer < 0)
        {
            state = GameState.Lose;
            ShowLoseStateText();
        }
        else if (levelTimer > 0 && listItemSpawn.Count == 0)
        {
            state = GameState.Win;
            ShowWinStateText();
        }
        if (state == GameState.Win)
        {
            state = GameState.LoadLevel;
            isLoad = true;
            StartCoroutine(IELoadLevel());
        }
        else if (state == GameState.LoadLevel)
        {
            state = GameState.Play;
        }

    }
    private IEnumerator IELoadLevel()
    {
        yield return new WaitForSeconds(1f);
        if(isLoad)
        {
            LoadLevel();
        }
    }
    private void LoadLevel()
    {
        Match3DManager.Instance.ChangeLevel();
        items = Match3DManager.Instance.ListItems;
        LoadGame();
        levelTimer = Match3DManager.Instance.LevelTimer;
        HideWinStateText();
        isLoad = false;
    }
    private void UpdateTimer(float timer)
    {
        timer += 1;
        float minute = Mathf.FloorToInt(timer / 60);
        float second = Mathf.FloorToInt(timer % 60);
        textCounter.text = string.Format("{0:00}:{1:00}", minute, second);
    }

    private void Stage_OnCollect(object sender, Stage.OnCollectChangedEventArgs e)
    {
        listItemSpawn.Remove(e.item1);
        listItemSpawn.Remove(e.item2);
    }
    private void ShowWinStateText()
    {
        winStateText.gameObject.SetActive(true);    
    }
    private void HideWinStateText()
    {
        winStateText.gameObject.SetActive(false);    
    }
    private void HideLoseStateText()
    {
        loseStateText.gameObject.SetActive(false);    
    }
    private void ShowLoseStateText()
    {
        loseStateText.gameObject.SetActive(true);    
    }
    private void LoadGame()
    {
        List<Item> shuffleItems = items;
        ShuffleList(shuffleItems);
        Vector3 spawnPosition = Vector3.zero;
        for (int i = 0; i < shuffleItems.Count; i++)
        {
            
        }
    }
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1); // ý đồ thuật toán là đưa hết các object về cuối list và sau mỗi lần update phần tử cuối mới
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }
}
