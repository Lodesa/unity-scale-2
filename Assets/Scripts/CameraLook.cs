using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class CameraLook : MonoBehaviour {
  private CameraController _cameraController;

  // Start is called before the first frame update
  void Awake() {
    _cameraController = GetComponent<CameraController>();
  }

  private void OnTriggerEnter2D(Collider2D other) {
    print("trigger entered");
    _cameraController.LookBelow();
  }

  private void OnTriggerExit2D(Collider2D other) {
    print("trigger exited");
    _cameraController.LookCenter();
  }
}