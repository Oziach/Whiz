using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
    const string RUNNING = "Running";
    const string IDLE = "Idle";
    const string UP_ATTACK_TRIGGER = "UpAttackTrigger";
    const string DOWN_ATTACK_TRIGGER = "DownAttackTrigger";
    const string HORIZONTAL_ATTACK_TRIGGER = "HorizontalAttackTrigger";
    const string WAND_RECHARGING_BOOL = "Recharging";
    const string JUMPING_BOOL = "Jumping";
    const string HEAD_LOOK_DIRECTION_INT = "HeadLookDirection";


    [SerializeField] Player player;
    [SerializeField] PlayerCasting playerCasting;
    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator feetAnimator;
    [SerializeField] Animator handsAnimator;
    [SerializeField] Animator wandAnimator;
    [SerializeField] Animator headAnimator;

    [SerializeField] GameObject gravcastDirectionArrowPrefab;
    [SerializeField] GameObject gravcastDirectionArrowOrigin;

    Vector3 originalSpriteLocalPosition;


    // Start is called before the first frame update
    void Start()
    {
        playerCasting.OnSpellCasted += PlayerCasting_OnSpellCasted;
        playerCasting.OnGravcast += PlayerCasting_OnGravcast;

    }

    private void PlayerCasting_OnGravcast(object sender, PlayerCasting.SpellcastEventArgs e) {
        SoundManager.Instance?.PlayGravcastSound();
    }

    private void PlayerCasting_OnSpellCasted(object sender, PlayerCasting.SpellcastEventArgs e) {
        SoundManager.Instance?.PlaySpellcastSound();
        HandsAttackAnimation(e);
    }

    private void HandsAttackAnimation(PlayerCasting.SpellcastEventArgs e) {
        Vector2 direction = e.direction;

        if (direction == Vector2.down) { handsAnimator.SetTrigger(DOWN_ATTACK_TRIGGER); }
        else if (direction == Vector2.up) { handsAnimator.SetTrigger(UP_ATTACK_TRIGGER); }
        else { handsAnimator.SetTrigger(HORIZONTAL_ATTACK_TRIGGER); }
    }

    private void HeadLookDirection() {
        headAnimator.SetFloat(HEAD_LOOK_DIRECTION_INT, playerCasting.GetLookDirection());
    }

    // Update is called once per frame
    void Update()
    {
        bodyAnimator.SetBool(RUNNING, player.IsRunning());
        feetAnimator.SetBool(RUNNING, player.IsRunning());
        bodyAnimator.SetBool(JUMPING_BOOL, !player.IsGrounded());
        wandAnimator.SetBool(WAND_RECHARGING_BOOL, playerCasting.IsRecharging());
        HeadLookDirection();
    }

    private void OnDestroy() {
        playerCasting.OnSpellCasted -= PlayerCasting_OnSpellCasted;

    }
}
