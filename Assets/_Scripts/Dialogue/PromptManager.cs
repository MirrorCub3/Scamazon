using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [Header("Audio Player")]
    [SerializeField]
    private AudioSource source;

    [Header("Prompts")]
    [SerializeField] 
    private List<PromptTree> prompts = new List<PromptTree>();
    
    //Bookkeeping
    private int currPrompt = 0;

    private void Awake()
    {
        foreach(PromptTree p in prompts)
            p.SetUp();

        source.Stop();
        currPrompt = 0;
        source.clip = prompts[currPrompt].GetOpeningPrompt();
    }

    public void NextPrompt() // only advances the prompt DOES NOT PLAY
    {
        source.Stop();
        currPrompt = (currPrompt + 1) % prompts.Count;
        source.clip = prompts[currPrompt].GetOpeningPrompt();
    }

    public void Play() // plays current clip in general - will be used for starting the boss prompts
    {
        source.Play();
    }

    public void PlayGood() // Call when any "good option" is selected (player or rng result)
    {
        source.Stop();
        AudioClip clip = prompts[currPrompt].SelectGood();
        if (!clip) 
           return; // end of tree
        else
        {
            source.clip = clip;
            source.Play();
        }    
    }

    public void PlayBad() // Call when any "bad option" is selected (player or rng result)
    {
        source.Stop();
        AudioClip clip = prompts[currPrompt].SelectBad();
        if (!clip)
            return; // end of tree
        else
        {
            source.clip = clip;
            source.Play();
        }
    }

    #region Testing
    public int GetCurrPromptNum()
    {
        return source.clip ? currPrompt : -1;
    }

    public string GetClipName()
    {
        return source.clip ? source.clip.name: "none active";
    }

    public void PauseToggle()
    {
        if (source.isPlaying)
            source.Pause();
        else
            source.UnPause();
    }
    #endregion
}
