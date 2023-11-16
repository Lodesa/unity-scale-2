using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {
  [SerializeField] private bool debug;
  private Player _player;

  void Start() {
    _player = GetComponent<Player>();
  }

  void Update() {
    Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    _player.SetDirectionalInput(directionalInput);

    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))) {
      _player.OnJumpInputDown();
      DebugLog("Fire2");
    }

    if ((Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))) {
      _player.OnJumpInputUp();
    }

    if ((Input.GetKeyDown(KeyCode.I) || Input.GetButtonDown("Fire1"))) {
      DebugLog("Fire1");
    }

    if ((Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Jump"))) {
      _player.OnGrow();
      DebugLog("Jump");
    }

    if ((Input.GetKeyDown(KeyCode.O) || Input.GetButtonDown("Fire3"))) {
      _player.OnShrink();
      DebugLog("Fire3");
    }
  }

  void DebugLog(string message) {
    if (debug) {
      Debug.Log(message);
    }
  }
}