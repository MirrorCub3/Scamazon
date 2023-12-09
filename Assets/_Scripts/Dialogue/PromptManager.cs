using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [Header("Audio Player")]
    [SerializeField]
    private FMOD.Studio.EventInstance source;

    [Header("Prompts")]
    [SerializeField] 
    private List<PromptTree> prompts = new List<PromptTree>();
    
    //Bookkeeping
    private int currPrompt = 0;

    private void Awake()
    {
        foreach(PromptTree p in prompts)
            p.SetUp();

        source.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        currPrompt = 0;
        source = FMODUnity.RuntimeManager.CreateInstance(prompts[currPrompt].GetOpeningPrompt());
    }

    public void NextPrompt() // only advances the prompt DOES NOT PLAY
    {
        source.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        currPrompt = (currPrompt + 1) % prompts.Count;
        prompts[currPrompt].Restart();
        source = FMODUnity.RuntimeManager.CreateInstance(prompts[currPrompt].GetOpeningPrompt());
    }

    public void Play() // plays current clip in general - will be used for starting the boss prompts
    {
        source.start();
    }

    public void Stop()
    {
        source.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlayGood() // Call when any "good option" is selected (player or rng result)
    {
        source.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODUnity.EventReference clip = prompts[currPrompt].SelectGood();
        if (clip.IsNull) 
           return; // end of tree
        else
        {
            source = FMODUnity.RuntimeManager.CreateInstance(clip);
            source.start();
        }    
    }

    public void PlayBad() // Call when any "bad option" is selected (player or rng result)
    {
        source.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODUnity.EventReference clip = prompts[currPrompt].SelectBad();
        if (clip.IsNull)
            return; // end of tree
        else
        {
            source = FMODUnity.RuntimeManager.CreateInstance(clip);
            source.start();
        }
    }

    public float GetOpeningPromptLength()
    {
       return prompts[currPrompt].GetOpeningPromptLength();
    }

    public float GetCurrNodeLength()
    {
        return prompts[currPrompt].GetCurrPromptLength();
    }

    #region Testing
    public int GetCurrPromptNum()
    {
        return source.isValid() ? currPrompt : -1;
    }

    public string GetClipName()
    {

        return source.isValid() ? GetInstantiatedEventName(source): "none active";
    }

    public void PauseToggle()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        source.getPlaybackState(out state);
        if (state== FMOD.Studio.PLAYBACK_STATE.PLAYING)
            source.setPaused(true);
        else
            source.setPaused(false);
    }
    public string GetInstantiatedEventName(FMOD.Studio.EventInstance instance)
{
    string result;
    FMOD.Studio.EventDescription description;

    instance.getDescription(out description);
    description.getPath(out result);

    return result; 

}
    #endregion
    
}
