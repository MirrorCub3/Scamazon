using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PromptDialogueDebugger : MonoBehaviour
{
    public TMP_Text voteNumber;
    public TMP_Text clipName;
    public PromptManager manager;
    private void Update()
    {
        voteNumber.text = "Vote: " + manager.GetCurrPromptNum();
        clipName.text = "Clip: " + manager.GetClipName();
    }
}
