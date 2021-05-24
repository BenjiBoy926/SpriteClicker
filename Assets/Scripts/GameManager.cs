using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("Reference to the text object that displays the number of clicks that the player has gotten")]
    public Text clicksText;
    [Tooltip("Prefix placed before the score of the player in the UI")]
    public string scorePrefix = "Score: ";

    // Total number of clicks
    private int totalClicks = 0;

    private void Awake()
    {
        clicksText.text = scorePrefix + totalClicks.ToString();
    }

    public void OnSpriteClicked()
    {
        totalClicks++;
        clicksText.text = scorePrefix + totalClicks.ToString();
    }
}
