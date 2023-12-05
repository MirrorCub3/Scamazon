using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class EmissionsMeter : MonoBehaviour
{
    [HideInInspector]
    public float emissionsValue;

    private Color newEmissionsMeterColor;

    [SerializeField] private Slider emissionsMeterSlider;
    [Tooltip("time (in seconds) it takes for the meter to increase or decrease")]
    [SerializeField] private float timeToAdjustMeter;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient emissionsMeterGradient;

    void Start()
    {
        emissionsValue = 50;
        emissionsMeterSlider.value = emissionsValue;
        fill.color = emissionsMeterGradient.Evaluate(emissionsValue/100);
        CheckEmissionsMeterGradientAmount();
    }

    public void UpdateEmissionsMeter(float value)
    {
        emissionsValue += value;
        StartCoroutine("AdjustEmissionsMeter");
        CheckEmissionsMeterGradientAmount();
    }

    IEnumerator AdjustEmissionsMeter()
    {
        float value = emissionsMeterSlider.value;
        Color currentColor = fill.color;

        float elapsedTime = 0f;
        while(elapsedTime < timeToAdjustMeter)
        {
            elapsedTime += Time.deltaTime;

            emissionsMeterSlider.value = Mathf.Lerp(value, emissionsValue, (elapsedTime / timeToAdjustMeter));

            fill.color = Color.Lerp(currentColor, newEmissionsMeterColor, (elapsedTime / timeToAdjustMeter));

            yield return null;
        }
    }

    private void CheckEmissionsMeterGradientAmount()
    {
        newEmissionsMeterColor = emissionsMeterGradient.Evaluate(emissionsValue/100);
    }
}
