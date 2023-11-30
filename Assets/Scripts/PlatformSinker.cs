using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
  public Rigidbody2D _rb;

  void Awake() {
    _rb = GetComponent<Rigidbody2D>();
    _initialPosition = _rb.position;
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
      _rb.velocity = new Vector2(0, -sinkSpeedLarge);
      if (_initialPosition.y - _rb.position.y >= distanceY) {
        if (_player) {
          _player.unstable = false;
        }
        sunk = false;
      }      
    }
    else if (_boarded && _player) {
      _rb.velocity = Vector2.zero;
      if (_initialPosition.y - _rb.position.y < distanceY && !_player.isPinchedHorizontally) {
        _player.unstable = true;
        _rb.velocity = new Vector2(0, !_player.isSmall ? -sinkSpeedLarge : -sinkSpeed);
      }
      else {
        _player.unstable = false;
        sunk = false;
      }
    }
    else {
      if (_rb.position.y < _initialPosition.y) {
        if (Time.time - _unboardedTime > riseDelay) {
          _rb.velocity = new Vector2(0, riseSpeed);
        }
      }
      else {
        _rb.MovePosition(_initialPosition);
      }
    }
  }
}