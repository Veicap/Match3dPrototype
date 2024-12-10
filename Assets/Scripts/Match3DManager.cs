using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3DManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseGameUI;

    private void Start()
    {
        HidePauseGameUI();
    }

    private void HidePauseGameUI()
    {
        PauseGameUI.SetActive(false);
    }
    private void ShowPauseGameUI()
    {
        PauseGameUI.SetActive(true);
    }
    public void PauseGame()
    {
        ShowPauseGameUI();
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
        HidePauseGameUI();
    }
    public void ReplayGame()
    {
        // load lai level
        Match3DManagerLevel.Instance.LoadLevel(Match3DManagerLevel.Instance.LevelIndex);
        LevelControl.Instance.SwitchState(LevelControl.GameState.LoadLevel);
        Time.timeScale = 1;
        HidePauseGameUI();
    }
}
