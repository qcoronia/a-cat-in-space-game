using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ReferencedText : MonoBehaviour
{
    public GameState GameState;
    public string property;

    private Text Text;

    void Start()
    {
        Text = GetComponent<Text>();
    }

    void Update()
    {
        var sourceText = string.Empty;
        switch (property)
        {
            case "foundGems":
                sourceText += GameState.foundGems;
                break;
            case "goodieFound":
                sourceText += GameState.goodieFound;
                break;
            case "totalGems":
                sourceText += GameState.totalGems;
                break;
            default:
                break;
        }

        Text.text = sourceText;
    }
}
