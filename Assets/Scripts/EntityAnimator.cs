using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;



public enum AnimationStates {
    hurt,
    death,
    jump,
    hammer,
    axe,
    brush,
    hoe,
    milking,
    pickaxe,
    planting,
    shear,
    sickle,
    bow,
    spear,
    spear2,
    sword,
    sword2,
    crossbow,
    fishing_casting,
    fishing_catch,
    fishing_idle,
    fishing_reeling,
}

public class EntityAnimator : SerializedMonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    public EntityMotor Motor;
    
    [SerializeField] [FoldoutGroup("Settings")] [ListDrawerSettings(ListElementLabelName = "Anim")]
    private List<Animation> Anims = new();

    [SerializeField] [FoldoutGroup("Debug")]
    private AnimationStates state = AnimationStates.hurt;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private float RemainingTriggerAnimationDuration;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private TriggerAnimation CurrentAnimationClip;

    

    private void Awake() {
        foreach (var anim in Anims) {
            anim.Initialize(this, anim.IsDefault);
        }
    }

    private void Update() {
        DebugPrinter.Singleton.Print(state.ToString());
        DebugPrinter.Singleton.Print(CurrentAnimationClip?.GetClipLength().ToString());
        HandleDebugInput();
        HandleScale();
        if (RemainingTriggerAnimationDuration > 0) {
            RemainingTriggerAnimationDuration -= Time.deltaTime;
            return;
        }
        else if (RemainingTriggerAnimationDuration <= 0 && CurrentAnimationClip != null) {
            if (CurrentAnimationClip.Loops) {
                CurrentAnimationClip.RemainingLoops--;
                RemainingTriggerAnimationDuration = CurrentAnimationClip.GetClipLength();
                if (CurrentAnimationClip.RemainingLoops <= 0) {
                    HandleCurrentTriggerAnimationEnd();
                }
                return;
            }
            else {
                HandleCurrentTriggerAnimationEnd();
                return;
            }
        }
        foreach (var anim in Anims) {
            if (!anim.Evaluate()) continue;
            anim.Play();
            // break;
        }
    }

    private void HandleDebugInput() {
        if (Input.GetKeyDown(KeyCode.O))
            Trigger(state.ToString());
        if (Input.GetKeyDown(KeyCode.P))
            state++;
        if (Input.GetKeyDown(KeyCode.I))
            state--;
        if (Input.GetKeyDown(KeyCode.K))
            Motor.CurrentAnimationDirection = AnimationDirection.down;

        if (Input.GetKeyDown(KeyCode.I))
            Motor.CurrentAnimationDirection = AnimationDirection.up;

        if (Input.GetKeyDown(KeyCode.L))
            Motor.CurrentAnimationDirection = AnimationDirection.side;
    }

    private void HandleScale() {
        var scale = transform.localScale;
        scale.x = Motor.FacingRight ? 1 : -1;
        transform.localScale = scale;
    }

    private void HandleCurrentTriggerAnimationEnd() {
        if (!string.IsNullOrEmpty(CurrentAnimationClip.ChainAnimation)) {
            Trigger(CurrentAnimationClip.ChainAnimation);
        }
        else {
            CurrentAnimationClip.End();
            CurrentAnimationClip = null;
        }
    }

    public void Trigger(string triggerTag) {
        if (string.IsNullOrEmpty(triggerTag)) return;
        foreach (var anim in Anims) {
            if (!anim.Evaluate(triggerTag)) continue;
            anim.Play();
            if (anim is not TriggerAnimation) continue;
            CurrentAnimationClip = anim as TriggerAnimation;
            CurrentAnimationClip.RemainingLoops = CurrentAnimationClip.MaxLoops;
            RemainingTriggerAnimationDuration = anim.GetClipLength();
        }
    }
}

[Serializable]
public abstract class Animation {
    [SerializeField] [BoxGroup("Settings")]
    public bool IsDefault;

    [SerializeField] [BoxGroup("Dependencies")]
    protected Animator Anim;

    protected EntityAnimator EntityAnimator;

    public virtual void Initialize(EntityAnimator entityAnimator, bool isDefault) {
        EntityAnimator = entityAnimator;
        SetActive(isDefault);
    }

    public virtual void SetActive(bool state) {
        Anim.gameObject.SetActive(state);
    }

    public virtual bool Evaluate() {
        return false;
    }

    public virtual bool Evaluate<T>(T optional) {
        End();
        return false;
    }

    public virtual void Play() {
        // SetActive(true);
        Anim.Play(EntityAnimator.Motor.CurrentAnimationDirection.ToString());
    }

    public virtual void End() {
        SetActive(false);
    }

    public virtual float GetClipLength() {
        return Anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
}

public class MotorAnimation : Animation {
    [SerializeField] [BoxGroup("Settings")]
    private ComparisonOperator Operation;

    [SerializeField] [BoxGroup("Settings")]
    private float Threshold;

    public override bool Evaluate() {
        bool eval = Compare(Mathf.Abs(EntityAnimator.Motor.GetVelocity().magnitude), Threshold, Operation);
        SetActive(eval);
        return eval;
    }

    // Todo: move this somewhere that makes more sense
    public bool Compare(float current, float threshold, ComparisonOperator comparison) {
        switch (comparison) {
            case ComparisonOperator.GreaterThan:
                return current > threshold;
            case ComparisonOperator.LessThan:
                return current < threshold;
            case ComparisonOperator.EqualTo:
                return Mathf.Approximately(current, threshold);
            case ComparisonOperator.GreaterThanOrEqualTo:
                return current >= threshold;
            case ComparisonOperator.LessThanOrEqualTo:
                return current <= threshold;
            case ComparisonOperator.NotEqualTo:
                return !Mathf.Approximately(current, threshold);
            default:
                return false;
        }
    }
}

public class TriggerAnimation : Animation {
    [SerializeField] [BoxGroup("Settings")]
    public bool Loops;

    [SerializeField] [BoxGroup("Settings")] [ShowIf("Loops")]
    public int MaxLoops;

    [SerializeField] [BoxGroup("Settings")]
    public string ChainAnimation;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly] [ShowIf("Loops")]
    public int RemainingLoops;
    public override bool Evaluate<T>(T optional) {
        if (optional.GetType() != typeof(string)) return false;
        bool eval = optional.ToString() == Anim.gameObject.name;
        SetActive(eval);
        return eval;
    }
}