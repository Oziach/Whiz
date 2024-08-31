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
    

    [SerializeField] Player player;
    [SerializeField] PlayerCasting playerCasting;
    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator feetAnimator;
    [SerializeField] Animator handsAnimator;
    [SerializeField] Animator wandAnimator;

    Vector3 originalSpriteLocalPosition;


    // Start is called before the first frame update
    void Start()
    {
        playerCasting.OnSpellCasted += PlayerCasting_OnSpellCasted;
    }

    private void PlayerCasting_OnSpellCasted(object sender, PlayerCasting.SpellcastEventArgs e) {
        HandsAttackAnimation(e);
    }

    private void HandsAttackAnimation(PlayerCasting.SpellcastEventArgs e) {
        Vector2 direction = e.direction;

        if (direction == Vector2.down) { handsAnimator.SetTrigger(DOWN_ATTACK_TRIGGER); }
        else if (direction == Vector2.up) { handsAnimator.SetTrigger(UP_ATTACK_TRIGGER); }
        else { handsAnimator.SetTrigger(HORIZONTAL_ATTACK_TRIGGER); }
    }



    // Update is called once per frame
    void Update()
    {
        bodyAnimator.SetBool(RUNNING, player.IsRunning());
        feetAnimator.SetBool(RUNNING, player.IsRunning());
        wandAnimator.SetBool(WAND_RECHARGING_BOOL, playerCasting.IsRecharging());
    }

    private void OnDestroy() {
        playerCasting.OnSpellCasted -= PlayerCasting_OnSpellCasted;
    }
}
