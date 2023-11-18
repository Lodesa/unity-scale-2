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
    _input.Player.Grow.performed += OnGrowPerformed;
    _input.Player.Shrink.performed += OnShrinkPerformed;
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

  private void OnGrowPerformed(InputAction.CallbackContext _) {
    _player.OnGrowPerformed();
  }

  private void OnShrinkPerformed(InputAction.CallbackContext _) {
    _player.OnShrinkPerformed();
  }
}