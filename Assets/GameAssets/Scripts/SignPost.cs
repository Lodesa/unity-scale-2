using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignPost : MonoBehaviour {
  public GameObject skeletonCounter;
  private SkeletonCounter _skeletonCounter = null;
  
  [TextAreaAttribute]
  public string message = "default message";
  public TextMeshProUGUI tmp;
  public GameObject canvas;
  public string skeletonId = null;
  private float _startTime;
  private float _endTime = -1;

  private void Start() {
    if (skeletonId == "FINAL") {
      _startTime = Time.time;
    }
  }

  private string FormatTime(float seconds) {
    TimeSpan time = TimeSpan.FromSeconds(seconds);
    return time.ToString(@"hh\:mm\:ss\:fff");
  }
  
  private void OnTriggerEnter2D(Collider2D other) {
    if (_skeletonCounter == null) {
      _skeletonCounter = skeletonCounter.gameObject.GetComponent<SkeletonCounter>();
    }

    if (other.CompareTag("Player")) {
      tmp.text = message;
      canvas.SetActive(true);

      if (skeletonId != null && _skeletonCounter != null) {
        _skeletonCounter.Add(skeletonId);

        if (skeletonId == "FINAL") {
          if (_endTime == -1) {
            _endTime = Time.time;
          }
          tmp.text = "Congratulations! Your completion time is:\n" + FormatTime(_endTime - _startTime);
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Player") && canvas) {
      tmp.text = "";
      canvas.SetActive(false);
    }
  }
}