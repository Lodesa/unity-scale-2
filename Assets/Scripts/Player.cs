using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour {
  [SerializeField] private float moveSpeed = 7;
  [SerializeField] private float maxJumpHeight = 3.2f;
  [SerializeField] private float minJumpHeight = 1;
  [SerializeField] private float timeToJumpApex = .3f;
  [SerializeField] private float accelerationTimeGrounded = 0.1f;
  [SerializeField] private float accelerationTimeAirborne = 0.05f;
  [SerializeField] private float scaleTime = 0.3f;
  [SerializeField] private GameObject[] virtualCameras;
  

  private Controller2D controller;
  private Vector2 directionalInput;
  private Vector3 velocity;
  private float velocityXSmoothing = 0.1f;
  private bool grounded;
  private float gravity;
  private float maxJumpVelocity;
  private float minJumpVelocity;
  private int vCamIndex = 0;
  private int scaleLevel;
  private float maxScaleLevel = 2;
  private int scaleFactor = 4;
  private bool scalingInProgress = false;
  private Rigidbody2D rb;

  void Start() {
    controller = GetComponent<Controller2D>();
    calculateJumpPhysics();
    SwitchVCam(0);
    scaleLevel = 0;
    scalingInProgress = false;
  }

  void Update() {
    transform.rotation = Quaternion.identity;
    CalculateVelocity();
    controller.Move(velocity * Time.deltaTime);
    
    if (controller.collisions.above || controller.collisions.below) {
      if (controller.collisions.slidingDownMaxSlope) {
        velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
      }
      else {
        velocity.y = 0;
      }
    }    
  }

  void calculateJumpPhysics() {
    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
  }

  void CalculateVelocity() {
    float targetVelocityX = directionalInput.x * moveSpeed;
    velocity.x = scalingInProgress ? 0 : Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
      controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);
    velocity.y += gravity * Time.deltaTime;
  }

  public void SetDirectionalInput(Vector2 input) {
    directionalInput = input;
  }

  public void OnJumpInputDown() {
    if (controller.collisions.below) {
      velocity.y = maxJumpVelocity;
    }
  }

  public void OnJumpInputUp() {
    if (velocity.y > minJumpVelocity) {
      velocity.y = minJumpVelocity;
    }
  }

  public void OnGrow() {
    if (scaleLevel < maxScaleLevel && !scalingInProgress) {
      scaleLevel++;
      float targetScale = Mathf.Pow(scaleFactor, scaleLevel);
      StartCoroutine(MarkScalingInProgress());
      controller.Scale(new Vector3(targetScale, targetScale, 1), scaleTime);
      
      maxJumpHeight *= scaleFactor;
      minJumpHeight *= scaleFactor;
      moveSpeed *= scaleFactor;
      calculateJumpPhysics();
      vCamIndex++;
      SwitchVCam(vCamIndex);
    }
  }

  public void OnShrink() {
    if (scaleLevel > 0 && !scalingInProgress) {
      scaleLevel--;
      float factor = 1f / scaleFactor;
      float targetScale = Mathf.Pow(scaleFactor, scaleLevel);
      StartCoroutine(MarkScalingInProgress());
      controller.Scale(new Vector3(targetScale, targetScale, 1), scaleTime);
      maxJumpHeight *= factor;
      minJumpHeight *= factor;
      moveSpeed *= factor;
      calculateJumpPhysics();
      vCamIndex--;
      SwitchVCam(vCamIndex);
    }
  }

  IEnumerator MarkScalingInProgress() {
    scalingInProgress = true;
    yield return new WaitForSeconds(scaleTime);
    scalingInProgress = false;
  }
  
  private void SwitchVCam(int index) {
    for (int i = 0; i < virtualCameras.Length; i++) {
      virtualCameras[i].SetActive(index == i);
    }
  }
}