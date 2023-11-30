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

  private void Start() {
    
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
      }
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      tmp.text = "";
      canvas.SetActive(false);
    }
  }
}