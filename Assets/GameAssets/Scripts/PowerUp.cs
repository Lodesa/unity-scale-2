using System;
using UnityEngine;

public class PowerUp : MonoBehaviour {

  public string powerName;
  public float rotationSpeed;
  public AudioSource audioPowerUp;

  private void Start() {}

  private void Update() {
    transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      Player player = other.GetComponent<Player>();
      player.Obtain(powerName);
      audioPowerUp.Play();
      Destroy(gameObject);
    }
  }
}