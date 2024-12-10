using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Match3DManagerLevel : MonoBehaviour
{
    public static Match3DManagerLevel Instance { get; private set; }

    [SerializeField] private List<LevelScriptableObject> levelScriptableObjectList;
    
    private List<Item> items = new List<Item>();
    private float levelTimer;
    private int levelIndex;

    public int LevelIndex => levelIndex;    

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        levelIndex = -1;
    }

    public void LoadLevel(int index)
    {
        // Ensure index is within bounds
        if (index < 0 || index >= levelScriptableObjectList.Count)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        var currentLevel = levelScriptableObjectList[index];
        items = new List<Item>(currentLevel.itemList);
        levelTimer = currentLevel.levelTimer;
        
        Debug.Log($"Level {index} loaded with {items.Count} items and timer set to {levelTimer}.");
    }

    public void ChangeLevel()
    {
        levelIndex = (levelIndex + 1) % levelScriptableObjectList.Count;
        LoadLevel(levelIndex);
    }

    // Properties (Read-only access)
    public float LevelTimer => levelTimer;
    public List<Item> ListItems => items; // Expose as read-only list
    public int Level => levelIndex;
}
