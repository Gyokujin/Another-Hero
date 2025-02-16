using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : AnimatorManager
{
    protected EnemyManager enemy;

    void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        enemy = GetComponentInParent<EnemyManager>();
    }

    void OnAnimatorMove()
    {
        if (enemy.onDamage || enemy.onDie)
            return;

        Vector3 deltaPoistion = new(animator.deltaPosition.x, 0, animator.deltaPosition.z);
        Vector3 velocity = deltaPoistion / Time.deltaTime;
        enemy.rigidbody.drag = 0;
        enemy.rigidbody.velocity = velocity;
    }

    public void AttackProcess()
    {
        enemy.enemyAudio.PlaySFX(enemy.enemyAudio.enemyClips[(int)EnemyAudio.EnemySound.Attack]);
    }

    public void AttackDelay()
    {
        animator.SetBool(enemy.characterAnimatorData.AttackParameter, false);
        animator.SetFloat(enemy.characterAnimatorData.VerticalParameter, enemy.characterAnimatorData.IdleParameterValue, enemy.characterAnimatorData.AnimationDampTime, Time.deltaTime);
        Invoke(nameof(AttackDelayProcess), enemy.enemyStatus.currentRecoveryTime);
    }

    void AttackDelayProcess()
    {
        enemy.isPreformingAction = false;
    }

    public void HitEnd()
    {
        enemy.onDamage = false;
        enemy.enemyStatus.damageAmount = 0;
    }
}