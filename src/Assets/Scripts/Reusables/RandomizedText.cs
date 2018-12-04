using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RandomizedText : MonoBehaviour
{
    public string[] randomTexts;
    public EventHandler OnMustChangeText;

    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        OnMustChangeText.OnInvoke.AddListener(ChangeText);
    }

    void ChangeText()
    {
        var idx = UnityHelpers.RandomIndex(0, randomTexts.Length - 1);
        text.text = randomTexts[idx];
    }
}
