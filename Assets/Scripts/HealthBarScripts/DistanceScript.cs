using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceScript : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image content;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private bool lerpColors;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            //string[] tmp = valueText.text.Split(':');
            //valueText.text = tmp[0] + ": " + value;
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }

    }

    // Use this for initialization
    void Start()
    {
        if (lerpColors)
        {
            content.color = fullColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

        HandleBar();

    }

    private void HandleBar()
    {

        if (fillAmount != content.fillAmount)
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        if (lerpColors)
        {
            content.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }
    }

    //Value == HealthValue
    //inMn == 0 the health can have
    //inMax == MaxHealth the player can have the health
    //outMin == 0 fillAmounts
    //outMax == 1 fillAmount
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //ex: inMax = 100 inMin= 0 value = 80 outMini = 0 outMax = 1
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin; ;
    }
}

