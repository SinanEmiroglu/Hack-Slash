using System.Collections;
using UnityEngine;

public class Knockback : AbilityBase, IDamage
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float forceAmount = 10f;

    [SerializeField]
    private float attackRadius = 2f;

    [SerializeField]
    private float impactDelay = 0.25f;

    private Collider[] attackResult;
    private LayerMask layerMask;
    public int Damage { get { return damage; } }

    private void Awake()
    {
        string currentLayer = LayerMask.LayerToName(gameObject.layer);
        layerMask = ~LayerMask.GetMask(currentLayer);

        attackResult = new Collider[10];
    }

    private void Attack()
    {
        StartCoroutine(DoAttack());
    }

    protected override void OnUse()
    {
        Attack();
    }

    private IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(impactDelay);

        Vector3 position = transform.position + transform.forward;
        int hitCount = Physics.OverlapSphereNonAlloc(position, attackRadius, attackResult, layerMask);

        for (int i = 0; i < hitCount; i++)
        {
            var takeHit = attackResult[i].GetComponent<ITakeHit>();
            if (takeHit != null)
            {
                takeHit.TakeHit(this);
            }
            var hitRigidbody = attackResult[i].GetComponent<Rigidbody>();

            if (hitRigidbody != null)
            {
                var direction = Vector3.Normalize(attackResult[i].transform.position - transform.position);
                hitRigidbody.AddForce(direction * forceAmount, ForceMode.Impulse);
            }
        }
    }
}