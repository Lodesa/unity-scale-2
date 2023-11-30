using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {
  [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
  [SerializeField] private float screenY;

  void Start() {
  }

  public void SwitchCamera(int index) {
    for (int i = 0; i < virtualCameras.Length; i++) {
      virtualCameras[i].gameObject.SetActive(index == i);
    }
  }

  public void LookBelow() {
    print("look below");
    for (int i = 0; i < virtualCameras.Length; i++) {
      CinemachineFramingTransposer framing = virtualCameras[i].GetCinemachineComponent<CinemachineFramingTransposer>();
      framing.m_ScreenY = screenY;
    }
  }

  public void LookCenter() {
    print("look center");
    for (int i = 0; i < virtualCameras.Length; i++) {
      CinemachineFramingTransposer framing = virtualCameras[i].GetCinemachineComponent<CinemachineFramingTransposer>();
      framing.m_ScreenY = 0.5f;
    }    
  }
}