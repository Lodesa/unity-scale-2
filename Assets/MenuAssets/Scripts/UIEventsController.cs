using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIEventsController : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
  public AudioSource selectSound;
  public bool muteFirstSelect;
  private Button m_Button;
  private bool m_IsFirstSelect;

  private void Start() {
    m_Button = GetComponent<Button>();
    m_IsFirstSelect = muteFirstSelect;
  }

  public void OnPointerEnter(PointerEventData eventData) {
    m_Button.Select();
  }

  public void OnSelect(BaseEventData eventData) {
    if (muteFirstSelect && m_IsFirstSelect) {
      m_IsFirstSelect = false;
    }
    else {
      selectSound.Play();
    }
  }
}