using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {
  private CustomInput _input = null;
  private Player _player = null;

  
  private void Awake() {
    _input = new CustomInput();
    _player = GetComponent<Player>();
  }

  private void OnEnable() {
    _input.Enable();
    _input.Player.Movement.performed += OnMovementPerformed;
    _input.Player.Movement.canceled += OnMovementCancelled;
    _input.Player.Jump.performed += OnJumpPerformed;
    _input.Player.Jump.canceled += OnJumpCanceled;
  }

  private void OnDisable() {
    _input.Disable();
    _input.Player.Movement.performed -= OnMovementPerformed;
    _input.Player.Movement.canceled -= OnMovementCancelled;
    _input.Player.Jump.started += OnJumpPerformed;
    _input.Player.Jump.canceled += OnJumpCanceled;
  }

  private void OnMovementPerformed(InputAction.CallbackContext value) {
    _player.velocity = value.ReadValue<Vector2>();
  }

  private void OnMovementCancelled(InputAction.CallbackContext value) {
    _player.velocity = Vector2.zero;
  }

  private void OnJumpPerformed(InputAction.CallbackContext _) {
    
  }
  
  private void OnJumpCanceled(InputAction.CallbackContext _) {
    
  }
}