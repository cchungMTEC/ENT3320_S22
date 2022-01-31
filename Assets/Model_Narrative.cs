using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Model_Narrative : MonoBehaviour
{
    public Image[] portraits;
    public Text dialogueDisplay;

    public int[] portraitSequence;
    public string[] dialogue;
    public float[] intervals;

    public float textInterval = .04f;
}
