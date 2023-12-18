using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Sky : MonoBehaviour {
  public static event Action<string> DaylightChange;
  
  public float cycleDurationSeconds;
  public Color dayColor;
  public Color nightColor;
  public Color transitionColor;
  public int numDayCycles = 8;
  public int numNightCycles = 4;
  private Color[] _colors;
  private Color _prevOldColor;
  private Color _oldColor;
  private Color _newColor;
  private Image _sky;
  private bool _skyInitialized;
  private float _currStartTime = 0;
  private float _currElapsed = 0;

  void Start() {
    _sky = GetComponent<Image>();

    if (_sky != null) {
      int numCycles = numDayCycles + numNightCycles + 2;
      _colors = new Color[numCycles];

      for (int i = 0; i < numCycles; i++) {
        if (i < numDayCycles) {
          _colors[i] = dayColor;
        }
        else if (i == numDayCycles || i == numCycles - 1) {
          _colors[i] = transitionColor;
        }
        else {
          _colors[i] = nightColor;
        }
      }

      _skyInitialized = true;
    }
  }

  void Update() {
    if (_skyInitialized) {
      _currElapsed = Time.time - _currStartTime % cycleDurationSeconds;
      float scaledTime = _currElapsed / cycleDurationSeconds;
      _prevOldColor = _oldColor;
      _oldColor = _colors[(int)scaledTime % _colors.Length];
      _newColor = _colors[(int)(scaledTime + 1f) % _colors.Length];
      float newT = scaledTime - Mathf.Floor(scaledTime);
      _sky.color = Color.Lerp(_oldColor, _newColor, newT);

      if (_prevOldColor != _oldColor) {
        HandleSkyChange();
      }
    }
  }

  void HandleSkyChange() {
    string daylightEventName = "";
    if (_oldColor == dayColor) {
      daylightEventName = "StartDay";
    }
    else if (_prevOldColor == dayColor) {
      daylightEventName = "EndDay";
    }    
    else if (_oldColor == nightColor) {
      daylightEventName = "StartNight";
    }
    else if (_prevOldColor == nightColor) {
      daylightEventName = "EndNight";
    }
    DaylightChange?.Invoke(daylightEventName);
  }
}