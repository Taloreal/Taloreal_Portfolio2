using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject Player;
    public PlayerController PlayerScript;

    private static GameManager _Instance;

    public GameObject PauseMenu;
    public GameObject PlayerDeadMenu;
    public GameObject PlayerDamageFlash;

    public Image Healthbar;

    public bool PlayerIsDead {
        get {
            return PlayerDeadMenu.activeSelf;
        }
    }

    public bool Paused {
        get {
            return PauseMenu.activeSelf;
        }
        set {
            bool state = value;
            PauseMenu.SetActive(state);
            Time.timeScale = state ? 0.0f : 1.0f;
            Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = state ? true : false;
        }
    }

    public static GameManager Instance {
        get {
            if (_Instance == null) {
                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
            }
            return _Instance;
        }
    }

    void Awake()
    {
        _Instance = this;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && PlayerIsDead == false) {
            Paused = !Paused;
        }
    }

    public void UpdateHealthbar(RangedInt health) {
        Healthbar.fillAmount = (float) health.Current / (float)health.GetMax();
    }

    public void PlayerDied() {
        PauseMenu.SetActive(false);
        PlayerDeadMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
