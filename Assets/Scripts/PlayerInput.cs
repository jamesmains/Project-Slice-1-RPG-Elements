using System;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    private Entity PlayerEntity;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private EntityAnimator PlayerAnimator;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private EntityMotor PlayerMotor;

    public static PlayerInput Singleton;
    [HideInInspector] public InputSystem_Actions InputSystem;

    private void OnEnable() {
        InputSystem ??= new InputSystem_Actions();
        InputSystem.Enable();
        InputSystem.Player.Attack.performed += UseAction;
        Singleton = this;
    }


    private void OnDisable() {
        InputSystem.Player.Attack.performed -= UseAction;
        InputSystem?.Disable();
    }

    private void UseAction(InputAction.CallbackContext obj) {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        PlayerAnimator.Trigger(PlayerEntity.UseItem());
    }

    private void Update() {
        PlayerMotor.SendInput(InputSystem.Player.Move.ReadValue<Vector2>());
    }
}