using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    [SerializeField]
    private GameObject audioSource;

    [SerializeField]
    private GameObject music;
    [SerializeField]
    private Sprite unMuteMusic;
    [SerializeField]
    private Sprite muteMusic;

    [SerializeField]
    private GameObject sound;
    [SerializeField]
    private Sprite unMuteSound;
    [SerializeField]
    private Sprite muteSound;
    public void ShowMenu() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
            ResumeGame();
        } else {
            gameObject.SetActive(true);
            PauseGame();
        }
    }

    public void Music() {
        if (audioSource.GetComponent<AudioSource>().mute) {
            audioSource.GetComponent<AudioSource>().mute = false;
            music.GetComponent<Image>().sprite = muteMusic;
        } else {
            audioSource.GetComponent<AudioSource>().mute = true;
            music.GetComponent<Image>().sprite = unMuteMusic;
        }  
    }
    public void Sound() {
        if (audioSource.GetComponent<AudioSource>().mute) {
            sound.GetComponent<Image>().sprite = muteSound;
        } else {
            sound.GetComponent<Image>().sprite = unMuteSound;
        }
    }

    [SerializeField]
    private GameObject trophyMenu;
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private GameObject settings;

    private GameObject currentMenu;

    public void SwapMenu(Button button) {
        GameObject targetMenu = GetTargetMenu(button.name);

        if (targetMenu != null && targetMenu != currentMenu) {
            DeactivateMenu(currentMenu);
            ActivateMenu(targetMenu);
            currentMenu = targetMenu;
        }
    }

    private GameObject GetTargetMenu(string buttonName) {
        switch (buttonName) {
            case "Trophy":
                return trophyMenu;
            case "Store":
                return shopMenu;
            case "Gear":
                return settings;
            default:
                return null;
        }
    }

    private void ActivateMenu(GameObject menu) {
        menu.SetActive(true);
    }

    private void DeactivateMenu(GameObject menu) {
        if (menu != null) {
            menu.SetActive(false);
        }
    }
    private void PauseGame() {
        Time.timeScale = 0f;
        // Add any additional pause-related logic here
    }
    private void ResumeGame() {
        Time.timeScale = 1f;
        // Add any additional resume-related logic here
    }



}
