using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sky : MonoBehaviour {
  public float durationSeconds;
  public Color[] colors;
  private Image _sky;
  private bool _skyInitialized;
  private float _currStartTime = 0;
  private float _currElapsed = 0;

  void Start() {
    _sky = GetComponent<Image>();
    if (_sky != null) {
      _skyInitialized = true;
    }
  }

  void Update() {
    if (_skyInitialized) {
      _currElapsed = Time.time - _currStartTime % durationSeconds;
      float scaledTime = _currElapsed / durationSeconds;
      Color oldColor = colors[(int) scaledTime % colors.Length];
      Color newColor = colors[(int) (scaledTime + 1f) % colors.Length];
      float newT = scaledTime - Mathf.Floor(scaledTime); 
      _sky.color = Color.Lerp(oldColor, newColor, newT);

    }
  }
}