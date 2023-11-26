using UnityEngine;

public class PlatformSinker : MonoBehaviour {
  [SerializeField] private float sinkSpeed = 1;
  [SerializeField] private float sinkSpeedLarge = 1;
  [SerializeField] private float riseSpeed = 1;
  [SerializeField] private float riseDelay = 3;
  [SerializeField] private float distanceY = 1;

   public bool sunk;
  
  private Vector3 _initialPosition;
  private bool _boarded;
  private float _unboardedTime;
  private Player _player;

  void Awake() {
    _initialPosition = transform.position;
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (other.collider.CompareTag("Player")) {
      _player = other.gameObject.GetComponent<Player>();
      foreach (var contact in other.contacts) {
        if (contact.normal == new Vector2(0, -1)) {
          _boarded = true;
          _player.unstable = true;
          break;
        }
      }
    }
  }
  
  private void OnCollisionExit2D(Collision2D other) {
    if (other.collider.CompareTag("Player")) {
      _player.unstable = false;
      _player = null;
      _boarded = false;
      _unboardedTime = Time.time;
    }
  }

  private void FixedUpdate() {
    if (_boarded || sunk) {
      if (_initialPosition.y - transform.position.y < distanceY) {
        if (_player) {
          _player.unstable = true;
        }
        transform.position += new Vector3(0, ((_player && !_player.isSmall) || sunk ? -sinkSpeedLarge : -sinkSpeed) * Time.deltaTime, 0);
      }
      else {
        if (_player) {
          _player.unstable = false;
        }
        sunk = false;
      }
    }
    else {
      if (transform.position.y < _initialPosition.y) {
        if (Time.time - _unboardedTime > riseDelay) {
          transform.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
        }
      }
      else {
        transform.position = _initialPosition;
      }
    }
  }
}