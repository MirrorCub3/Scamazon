using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class EmissionsMeter : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    private FMOD.Studio.EventInstance ding;

    public FMODUnity.EventReference fmodEventDing;


    [SerializeField] [Range(0f,1f)]
    private float para;

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
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        ding = FMODUnity.RuntimeManager.CreateInstance(fmodEventDing);
        emissionsValue = 50;
        emissionsMeterSlider.value = emissionsValue;
        fill.color = emissionsMeterGradient.Evaluate(emissionsValue/100);
        CheckEmissionsMeterGradientAmount();
    }

    public void UpdateEmissionsMeter(float value)
    {
        emissionsValue += value;
        para = value > 0 ? 0 : 1;
        StartCoroutine("AdjustEmissionsMeter");
        CheckEmissionsMeterGradientAmount();
    }

    IEnumerator AdjustEmissionsMeter()
    {
        float value = emissionsMeterSlider.value;
        Color currentColor = fill.color;
        instance.start();
        float elapsedTime = 0f;
        while(elapsedTime < timeToAdjustMeter)
        {
            elapsedTime += Time.deltaTime;

            emissionsMeterSlider.value = Mathf.Lerp(value, emissionsValue, (elapsedTime / timeToAdjustMeter));

            fill.color = Color.Lerp(currentColor, newEmissionsMeterColor, (elapsedTime / timeToAdjustMeter));

            para += para == 0 ? Time.deltaTime : -Time.deltaTime;

            instance.setParameterByName("FillingUp",para);

            yield return null;
        }
        ding.start();
    }

    private void CheckEmissionsMeterGradientAmount()
    {
        newEmissionsMeterColor = emissionsMeterGradient.Evaluate(emissionsValue/100);
        

    }
}
