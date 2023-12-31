using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PromptTree : ScriptableObject
{
    [Header("Opening Prompt")]
    [SerializeField]
    private DialogueNode openingPrompt;

    [Header("Player Good Choice Branch")]
    [SerializeField]
    private DialogueNode goodChoice;
    [SerializeField]
    private DialogueNode good_GoodResult;
    [SerializeField]
    private DialogueNode good_BadResult;

    [Header("Player Bad Choice Branch")]
    [SerializeField]
    private DialogueNode badChoice;
    [SerializeField]
    private DialogueNode bad_GoodResult;
    [SerializeField]
    private DialogueNode bad_BadResult;

    private DialogueNode currNode;

    public void SetUp()
    {
        ConstructTree();
        currNode = openingPrompt;
    }

    public void Restart()
    {
        currNode = openingPrompt;
    }

    private void ConstructTree()
    {
        openingPrompt.SetNodes(goodChoice, badChoice);
        goodChoice.SetNodes(good_GoodResult, good_BadResult);
        badChoice.SetNodes(bad_GoodResult, bad_BadResult);
    }

    public FMODUnity.EventReference GetOpeningPrompt()
    {
        return openingPrompt.audioClip;
    }

    public float GetOpeningPromptLength()
    {
        return openingPrompt.length;
    }

    public float GetCurrPromptLength()
    {
        return currNode.length;
    }

    public FMODUnity.EventReference SelectGood() // returns the good option of the current node AND advances the current node to that option
    {
        // if(currNode == null || currNode.good == null) return null;
        if (currNode.good == null) return currNode.audioClip;

        currNode = currNode.good;
        return currNode.audioClip;
    }

    public FMODUnity.EventReference SelectBad() // returns the good option of the current node AND advances the current node to that option
    {
        // if (currNode == null || currNode.bad == null) return null;
        if (currNode.bad == null) return currNode.audioClip;

        currNode = currNode.bad;
        return currNode.audioClip;
    }
}

[Serializable]
public class DialogueNode
{
    public FMODUnity.EventReference audioClip;
    public float length = 1f;
    [HideInInspector]
    public DialogueNode good, bad;

    public void SetNodes(DialogueNode gNode, DialogueNode bNode)
    {
        good = gNode;
        bad = bNode;
    }
}
