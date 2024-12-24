using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class EntityMotor : MonoBehaviour {
    [SerializeField, FoldoutGroup("Settings")]
    private float MoveSpeed = 5f;

    [SerializeField, FoldoutGroup("Settings")]
    private float Drag;

    [SerializeField, FoldoutGroup("Settings")]
    private float DirectionChangeThreshold = 0.25f;

    [SerializeField, FoldoutGroup("Settings")]
    private float FacingChangeThreshold = 0.03f;
    
    [SerializeField, FoldoutGroup("Settings")]
    private Vector2 ActionZoneOffset = new Vector2(0f, 0.25f);

    [SerializeField, FoldoutGroup("Dependencies")]
    private Rigidbody2D rb;

    [SerializeField, FoldoutGroup("Debug")]
    private Transform DebugActionZone;

    [SerializeField, FoldoutGroup("Debug")]
    private Vector2 DebugCacheActionZonePos;
    
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private Vector2 Input;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private Vector2 Velocity;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private Vector2 CachedAnimationVelocity;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private Vector2 RealDirection;
    
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    public AnimationDirection CurrentAnimationDirection = AnimationDirection.side;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    public AnimationDirection CachedAnimationDirection;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    public bool FacingRight = true;

    private void Update() {
        Vector2 tempPos;
        tempPos.x = CurrentAnimationDirection == AnimationDirection.side ? 1 : 0;
        tempPos.x = FacingRight ? tempPos.x : tempPos.x * -1;
        tempPos.y = CurrentAnimationDirection == AnimationDirection.up ? 1 : 
            CurrentAnimationDirection == AnimationDirection.down? -1 :0;
        var tempRot = 0f;
        if (tempPos.y != 0)
            tempRot = 90;
        
        if(DebugActionZone == null) return;
        DebugActionZone.localRotation = Quaternion.Euler(0, 0, tempRot);
        if(tempPos != Vector2.zero) DebugCacheActionZonePos = tempPos;
        
        // DebugActionZone.position = (Vector2)transform.position + DebugCacheActionZonePos;
    }

    private void FixedUpdate() {
        Velocity += Input * (MoveSpeed * Time.deltaTime);
        Velocity = Vector2.Lerp(Velocity, Vector2.zero, Time.fixedDeltaTime * Drag);
        SetDirections();
        ApplyVelocity();
    }

    public void SendInput(Vector2 input) {
        Input = input;
    }
    
    private void SetDirections() {
        
        
        
        if (Mathf.Abs(Input.x) > FacingChangeThreshold)
            FacingRight = Input.x > 0;

        var absHorizontal = Mathf.Abs(Input.x);
        if (CachedAnimationVelocity.magnitude <= Velocity.magnitude) {
            CachedAnimationVelocity = Velocity;
            if (absHorizontal < DirectionChangeThreshold && Velocity.y > DirectionChangeThreshold) {
                CachedAnimationDirection = CurrentAnimationDirection = AnimationDirection.up;
            } else if (absHorizontal < DirectionChangeThreshold && Velocity.y < -DirectionChangeThreshold) {
                CachedAnimationDirection = CurrentAnimationDirection = AnimationDirection.down;
            } else if (absHorizontal >= DirectionChangeThreshold) {
                CachedAnimationDirection = CurrentAnimationDirection = AnimationDirection.side;
            } else {
                CurrentAnimationDirection = CachedAnimationDirection;
            }
            if(Input != Vector2.zero) {
                RealDirection = Input;
                DebugActionZone.position = (Vector2)transform.position + RealDirection + ActionZoneOffset;
            }
        }
        else {
            CachedAnimationVelocity = Velocity;
        }
        
    }

    private void ApplyVelocity() {
        rb.linearVelocity = new Vector2(Velocity.x, Velocity.y);
    }

    public Vector2 GetVelocity() {
        return Velocity;
    }
}