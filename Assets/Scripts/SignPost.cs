using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignPost : MonoBehaviour {
  [TextAreaAttribute]
  public string message = "default message";
  public TextMeshProUGUI tmp;
  public GameObject canvas;
      
  

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      tmp.text = message;
      canvas.SetActive(true);
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      tmp.text = "";
      canvas.SetActive(false);
    }
  }
}