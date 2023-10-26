using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
public float volume;
public AudioMixer mixer;
public GameObject tutorialPage1;
public GameObject tutorialPage2;
public GameObject tutorialPage3;

   public void tutorial() {
       SceneManager.LoadSceneAsync("Tutorial");
    }

    public void playGame() {
       SceneManager.LoadSceneAsync("Level_2");
    }

    public void openSettings() {
       SceneManager.LoadSceneAsync("Settings");
    }

    public void quitGame() {
       SceneManager.LoadSceneAsync("MainMenu");
    }

    public void SetVolume(float volume) {
      mixer.SetFloat("Volume", volume);
    }
    public void Navegacao(string nomeDaCena) {
    SceneManager.LoadScene(nomeDaCena);
    }

    private int currentPage = 1;

public void nextPageTutorial() {
    // Desative a página atual
    switch (currentPage) {
        case 1:
            tutorialPage1.SetActive(false);
            break;
        case 2:
            tutorialPage2.SetActive(false);
            break;
        case 3:
            tutorialPage3.SetActive(false);
            break;
    }

    currentPage++;

    if (currentPage > 3) {
        currentPage = 1;
        SceneManager.LoadScene("Level_2");
    } else {
    switch (currentPage) {
        case 1:
            tutorialPage1.SetActive(true);
            break;
        case 2:
            tutorialPage2.SetActive(true);
            break;
        case 3:
            tutorialPage3.SetActive(true);
            break;
    }
    }
}

}
