using UnityEngine;

public class CameraController : MonoBehaviour {
  [SerializeField] private GameObject[] virtualCameras;

  void Start() {

  }

  void Update() {
 
  }

  public void SwitchCamera(int index) {
    for (int i = 0; i < virtualCameras.Length; i++) {
      virtualCameras[i].SetActive(index == i);
    }
  }
}