using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Steak : MonoBehaviour
{
    [SerializeField] private Image steakImage;
    [SerializeField] private float steakTimer;
    private float counter = 0;
    private bool loadLevel = false;
    private void Start()
    {
        LevelControl.Instance.OnLoadLevel += LevelControl_OnLoadLevel;
        steakImage.fillAmount = 0;
        Stage.Instance.OnMatch += Stage_OnMatch;
    }

    private void LevelControl_OnLoadLevel(object sender, System.EventArgs e)
    {
        steakImage.fillAmount = 0;
        counter = -1;
    }

    private void Update()
    {
        if(counter > 0)
        {
            counter -= Time.deltaTime;
            steakImage.fillAmount = counter / steakTimer;
            // Debug.Log(counter);

        }
    }
    private void Stage_OnMatch(object sender, System.EventArgs e)
    {
        Debug.Log("Match");
        counter = steakTimer;
    }



}
