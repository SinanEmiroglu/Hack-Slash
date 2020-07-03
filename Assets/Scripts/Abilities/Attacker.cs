using System.Collections;
using UnityEngine;

public class Attacker : AbilityBase, IAttack
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float attackOffset = 1f;

    [SerializeField]
    private float attackRadius = 1f;

    [SerializeField]
    private float attackImpactDelay = 1f;

    [SerializeField]
    private float attackRange = 2f;

    private Collider[] attackResult;
    private LayerMask layerMask;

    public int Damage { get { return damage; } }

    public void Attack()
    {
    }

    private void Awake()
    {
        string currentLayer = LayerMask.LayerToName(gameObject.layer);
        layerMask = ~LayerMask.GetMask(currentLayer);

        attackResult = new Collider[10];

        var animationImpactWatcher = GetComponentInChildren<AnimationImpactWatcher>();
        if (animationImpactWatcher != null)
        {
            animationImpactWatcher.OnImpact += AnimationImpactWatcher_OnImpact;
        }
    }

    public void Attack(Character target)
    {
        attackTimer = 0;
        StartCoroutine(DoAttack(target));
    }

    internal bool InAttackRange(ITakeHit target)
    {
        if (!target.Alive)
        {
            return false;
        }

        var distance = Vector3.Distance(transform.position, target.transform.position);
        return distance < attackRange;
    }

    private IEnumerator DoAttack(ITakeHit target)
    {
        yield return new WaitForSeconds(attackImpactDelay);
        if (target.Alive && InAttackRange(target))
        {
            target.TakeHit(this);
        }
    }

    /// <summary>
    /// Called by animation event via AnimationImpactWatcher
    /// </summary>
    private void AnimationImpactWatcher_OnImpact()
    {
        Vector3 position = transform.position + transform.forward * attackOffset;
        int hitCount = Physics.OverlapSphereNonAlloc(position, attackRadius, attackResult, layerMask);

        for (int i = 0; i < hitCount; i++)
        {
            var box = attackResult[i].GetComponent<ITakeHit>();
            if (box != null)
                box.TakeHit(this);
        }
    }

    protected override void OnUse()
    {
        Attack();
    }
}