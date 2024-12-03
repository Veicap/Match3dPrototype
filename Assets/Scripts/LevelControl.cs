using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private List<Item> items = new();
    //[SerializeField] private int level = 0;
    [SerializeField] private float levelTimer;
    [SerializeField] private TextMeshProUGUI textCounter;
    [SerializeField] private TextMeshProUGUI winStateText, loseStateText;
    public enum GameState
    {
        Play,
        Lose,
        Win
    }
    private GameState state;
    private void Start()
    {
        Stage.Instance.OnCollect += Stage_OnCollect;
        state = GameState.Play;
        HideLoseStateText();
        HideWinStateText();
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
        else if (levelTimer > 0 && items.Count == 0)
        {
            state = GameState.Win;
            ShowWinStateText();
        }
        Debug.Log(state);

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
        items.Remove(e.item1);
        items.Remove(e.item2);
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
}
