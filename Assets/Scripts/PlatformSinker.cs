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
    if (sunk) {
      transform.position += new Vector3(0, -sinkSpeedLarge * Time.deltaTime, 0);
      if (_initialPosition.y - transform.position.y >= distanceY) {
        if (_player) {
          _player.unstable = false;
        }
        sunk = false;
      }      
    }
    else if (_boarded && _player) {
      // if the platform hasn't bottomed out
      if (_initialPosition.y - transform.position.y < distanceY && !_player.isPinchedHorizontally) {
        _player.unstable = true;
        transform.position += new Vector3(0, (!_player.isSmall ? -sinkSpeedLarge : -sinkSpeed) * Time.deltaTime, 0);
      }
      // if the platform bottomed out, it's no longer unstable
      else {
        _player.unstable = false;
        sunk = false;
      }
    }
    // if not sunk or boarded by player, start rising up again
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