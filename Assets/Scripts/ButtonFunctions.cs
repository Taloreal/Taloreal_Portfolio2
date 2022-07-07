using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume_Clicked() {
        GameManager.Instance.Paused = false;
    }

    public void GiveHP_Clicked(int amount) {
        GameManager.Instance.PlayerScript.Heal(amount);
    }

    public void Quit_Clicked() { 
        Application.Quit();
    }

    public void Respawn_Clicked() { 
        GameManager.Instance.PlayerScript.Respawn();
        GameManager.Instance.PlayerDeadMenu.SetActive(false);
        GameManager.Instance.Paused = false;
    }
}
