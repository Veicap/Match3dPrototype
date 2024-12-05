using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Steak : MonoBehaviour
{
    [SerializeField] private Image steakImage;
    [SerializeField] private float steakTimer;
    private void Start()
    {
        steakImage.fillAmount = 0;
        Stage.Instance.OnMatch += Stage_OnMatch;
    }

    private void Stage_OnMatch(object sender, System.EventArgs e)
    {
        steakImage.fillAmount = 1;
    }



}
