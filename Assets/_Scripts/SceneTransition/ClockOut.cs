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
}
