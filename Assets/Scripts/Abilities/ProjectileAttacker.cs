using System.Collections;
using UnityEngine;

public class ProjectileAttacker : AbilityBase, IAttack
{
    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private float launchYOffset = 1f;

    [SerializeField]
    private float launchXOffset = 1f;

    [SerializeField]
    private float launchDelay = 0f;

    public int Damage { get { return 1; } }

    public void Attack()
    {
        attackTimer = 0;
        StartCoroutine(LaunchAfterDelay());
    }

    private IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(launchDelay);
        var projectile = projectilePrefab.Get<Projectile>((transform.position + Vector3.up * launchYOffset) + transform.forward * launchXOffset, transform.rotation);
    }

    protected override void OnUse()
    {
        Attack();
    }
}