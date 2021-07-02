using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinText;
    public Text livesText;
    public GameObject elevatorText;

    public void UpdateCoinText(int coinCount)
    {
        coinText.text = "Coins: " + coinCount.ToString();
        
    }

    public void UpdateLivesText(int livesCount)
    {
        livesText.text = "Lives: " + livesCount.ToString();
    }

    public void ActivateElevatorText()
    {
        elevatorText.SetActive(true);
    }

    public void DeactivateElevatorText()
    {
        elevatorText.SetActive(false);
    }
}
