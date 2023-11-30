using TMPro;
using UnityEngine;

public class PowerUpText : MonoBehaviour {
    [TextAreaAttribute]
    public string message = "default message";
    public string power;
    public TextMeshProUGUI tmp;
    public GameObject canvas;
    private Player _player;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _player = other.GetComponent<Player>();
            if (_player.IsEnabled(power)) {
                tmp.text = message;
                canvas.SetActive(true);
            }
        }    
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (_player != null && _player.IsEnabled(power)) {
            tmp.text = message;
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _player = null;
            tmp.text = "";
            canvas.SetActive(false);
        }
    }
}