using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_IntroNarrative : MonoBehaviour
{
    public Model_Narrative narrative;

    private int _narrativeIndex;
    private bool _done;
    private float _timer;
    private int _stringIndex;
    private float _printDialogueTimer;

    public void Start()
    {
        CleanupNarrative();

        bool testFail = false;
        if (narrative.portraitSequence.Length != narrative.dialogue.Length) testFail = true;
        if (narrative.portraitSequence.Length != narrative.intervals.Length) testFail = true;

        if (testFail) Debug.LogError("ViewController_IntroNarrative: ERROR! Narrative arrays are not the same length.");
    }

    public void StartDialogue()
    {
        _ChangePortrait();
        _done = false;
    }

    public bool UpdateFromGameController()
    {
        _timer += Time.deltaTime;
        if (_timer >= narrative.intervals[_narrativeIndex])
        {
            _timer = 0;
            _narrativeIndex++;

            if (_narrativeIndex < narrative.portraitSequence.Length)
            {
                _ChangePortrait();
                _ChangeDialogue();
            }
        }

        _IncreaseDisplayedString();

        if (_narrativeIndex >= narrative.portraitSequence.Length) 
            _done = true;
        return _done;
    }

    private void _ChangePortrait()
    {
        for (int i = 0; i < narrative.portraits.Length; i++)
            narrative.portraits[i].enabled = (i == narrative.portraitSequence[_narrativeIndex]);   
    }
    private void _ChangeDialogue()
    {
        _stringIndex = 0;
        narrative.dialogueDisplay.text = "";
    }
    private void _IncreaseDisplayedString()
    {
        _printDialogueTimer += Time.deltaTime;
        if (_printDialogueTimer >= narrative.textInterval)
        {
            _printDialogueTimer -= narrative.textInterval;
            _stringIndex++;
            _stringIndex = Mathf.Clamp(_stringIndex, 0, narrative.dialogue[_narrativeIndex].Length);
            narrative.dialogueDisplay.text = narrative.dialogue[_narrativeIndex].Substring(0, _stringIndex);
        }
    }

    public void CleanupNarrative()
    {
        for (int i = 0; i < narrative.portraits.Length; i++)
            narrative.portraits[i].enabled = false;
        narrative.dialogueDisplay.text = "";
    }
}
