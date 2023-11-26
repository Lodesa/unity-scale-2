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
    _input.Player.Movement.canceled += OnMovementCanceled;
    _input.Player.Jump.performed += OnJumpPerformed;
    _input.Player.Jump.canceled += OnJumpCanceled;
    _input.Player.ToggleSize.performed += OnToggleSizePerformed;
    _input.Player.Dash.performed += OnDashPerformed;
  }

  private void OnDisable() {
    _input.Disable();
    _input.Player.Movement.performed -= OnMovementPerformed;
    _input.Player.Movement.canceled -= OnMovementCanceled;
    _input.Player.Jump.started += OnJumpPerformed;
    _input.Player.Jump.canceled += OnJumpCanceled;
  }

  private void OnMovementPerformed(InputAction.CallbackContext value) {
    _player.movementInput = value.ReadValue<Vector2>();
  }
  private void OnMovementCanceled(InputAction.CallbackContext value) {
    _player.movementInput = Vector2.zero;
  }

  private void OnJumpPerformed(InputAction.CallbackContext _) {
    _player.OnJumpPerformed();
  }
  private void OnJumpCanceled(InputAction.CallbackContext _) {
    _player.OnJumpCanceled();
  }

  private void OnToggleSizePerformed(InputAction.CallbackContext _) {
    _player.OnToggleSizePerformed();
  }
  
  private void OnDashPerformed(InputAction.CallbackContext _) {
    _player.OnDashPerformed();
  }  
}