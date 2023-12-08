using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour {
  [SerializeField] Button startButton;
  [SerializeField] Button settingsButton;
  [SerializeField] Button quitButton;
  [SerializeField] AudioSource mainTheme;
  [SerializeField] AudioSource startGame;
  [SerializeField] AudioMixer mixer;
  [SerializeField] float initialAudioLevel = -7;
  [SerializeField] float startAudioFadeInTime = 1;
  [SerializeField] float startScreenFadeInTime = 3;
  [SerializeField] float startAudioFadeOutTime = 4;
  [SerializeField] float startScreenFadeOutTime = 3;
  [SerializeField] float startGameWaitTime = 5;
  [SerializeField] string gameSceneName;
  [SerializeField] Image crossfadeImage;

  private CanvasGroup fadeCanvasGroup;

  
  
  void Start() {
    crossfadeImage.enabled = true;
    fadeCanvasGroup = crossfadeImage.GetComponent<CanvasGroup>();
    
    StartCoroutine(ScreenFadeIn());
    if (mainTheme != null) {
      if (mixer) {
        mixer.SetFloat("MasterVolume", initialAudioLevel);
        StartCoroutine(FadeMixerGroup.StartFade(mixer, "MasterVolume", startAudioFadeInTime, 1));
      }

      mainTheme.loop = true;
      mainTheme.Play();
    }

    startButton.onClick.AddListener(StartClicked);
    settingsButton.onClick.AddListener(SettingsClicked);
    quitButton.onClick.AddListener(QuitClicked);
  }

  void SettingsClicked() { }

  void QuitClicked() {
    Application.Quit();
  }

  void StartClicked() {
    if (startGame) {
      startGame.Play();
    }
    StartCoroutine(StartGameAfterWaitTime());
    StartCoroutine(ScreenFadeOut());
    StartCoroutine(FadeMixerGroup.StartFade(mixer, "MasterVolume", startAudioFadeOutTime, 0));
  }

  IEnumerator ScreenFadeOut() {
    float elapsedTime = 0f;
    while (elapsedTime < startScreenFadeOutTime) {
      elapsedTime += Time.deltaTime;
      fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / startScreenFadeOutTime);
      yield return null;
    }
  }
  
  IEnumerator ScreenFadeIn() {
    float elapsedTime = 0f;
    while (elapsedTime < startScreenFadeInTime) {
      elapsedTime += Time.deltaTime;
      fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / startScreenFadeInTime);
      yield return null;
    }
  }
  
  IEnumerator StartGameAfterWaitTime() {
    yield return new WaitForSeconds(startGameWaitTime);
    SceneManager.LoadScene(gameSceneName);
  }
}