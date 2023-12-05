using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmissionsMeter : MonoBehaviour
{
    [HideInInspector]
    public float emissionsValue;

    [SerializeField] private Slider emissionsMeterSlider;
    [Tooltip("time (in seconds) it takes for the meter to increase or decrease")]
    [SerializeField] private float timeToAdjustMeter;

    void Start()
    {
        emissionsValue = 50;
        emissionsMeterSlider.value = emissionsValue;
    }

    void Update()
    {
        StartCoroutine("AdjustEmissionsMeter");
        emissionsMeterSlider.value = emissionsValue;
    }

    IEnumerator AdjustEmissionsMeter()
    {
        return null;
    }
}
