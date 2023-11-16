using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {
  private Player player;

  void Start() {
    player = GetComponent<Player>();
  }

  void Update() {
    Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    player.SetDirectionalInput(directionalInput);

    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))) {
      player.OnJumpInputDown();
      print("Fire2");
    }

    if ((Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))) {
      player.OnJumpInputUp();
    }

    if ((Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire1"))) {
      print("Fire1");
    }

    if ((Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Jump"))) {
      player.OnGrow();
      print("Jump");
    }

    if ((Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3"))) {
      player.OnShrink();
      print("Fire3");
    }
  }
}