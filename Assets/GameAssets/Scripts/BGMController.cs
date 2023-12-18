using UnityEngine;

public class BGMController : MonoBehaviour {
  [SerializeField] private AudioSource[] songs;
  [SerializeField] private AudioSource foleyNight;
  private int _songIndex;

  private void Awake() {
    _songIndex = 0;
  }
  
  private void OnEnable() {
    Sky.DaylightChange += HandleDaylightChange;
  }

  private void HandleDaylightChange(string daylightChangeEventName) {
    if (daylightChangeEventName == "StartNight") {
      foleyNight.Play();
    }
    else if (daylightChangeEventName == "StartDay") {
      songs[_songIndex].Play();
      _songIndex = (_songIndex + 1) % songs.Length;
    }
  }
}