using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3DManager : MonoBehaviour
{
    public static Match3DManager Instance { get; private set; }
    public List<LevelScriptableObject> levelScriptableObjectList;
    private List<Item> items;
    private float levelTimer;
    private int levelIndex;
    private void Awake()
    {
        Instance = this;    
        levelIndex = 0;       
    }
    private void Start()
    {
        if(levelScriptableObjectList.Count != 0)
        {
            items = levelScriptableObjectList[levelIndex].itemList;
        }
    }
    public void ChangeLevel()
    {
        levelIndex = Mathf.Clamp(levelIndex +1, 0, levelScriptableObjectList.Count- 1);  
        items = levelScriptableObjectList[levelIndex].itemList;
        levelTimer = levelScriptableObjectList[levelIndex].levelTimer;
    }
    public float LevelTimer => levelTimer;
    public List<Item> ListItems => items;
  
}
