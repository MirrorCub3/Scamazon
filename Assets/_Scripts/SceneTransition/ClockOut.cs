using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockOut : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionManager sceneManager;
    [SerializeField]
    private string mainMenu;

    [Header("Stopping Audio")]
    [SerializeField]
    private ConveyorBeltSound[] conveyorSFX;
    [SerializeField]
    private PromptManager promptManager;
    [SerializeField]
    private Boss_Navigation bossDialogue;

    private Bus[] buses;
    private FMOD.RESULT busListOk;

    private void Start()
    {
        FMODUnity.RuntimeManager.StudioSystem.getBankList(out FMOD.Studio.Bank[] loadedBanks);
        foreach (FMOD.Studio.Bank bank in loadedBanks)
        {
            bank.getPath(out string path);
            busListOk = bank.getBusList(out buses);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Box box = other.GetComponent<Box>();
        if(box && box.IsPacked && box.itemName == "Clock")
        {
            print("going to main menu");
            sceneManager.GoToScene(mainMenu);
            foreach(var conveyor in conveyorSFX)
            {
                conveyor.switchClicked(false);
            }
            promptManager.Stop();
            bossDialogue.StopIntro();
        }
    }
    private void StopAllSFX()
    {
        foreach(Bus b in buses)
        {
            b.stopAllEvents(STOP_MODE.IMMEDIATE);
        }
    }

    private void OnDisable()
    {
        StopAllSFX();
    }
}
