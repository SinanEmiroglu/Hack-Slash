using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Attacker))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : PooledMonoBehaviour, ITakeHit, IDie
{
    [SerializeField]
    private int maxHealth = 3;

    private int currentHealth;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Attacker attacker;
    private Character target;

    public event Action<IDie> OnDie = delegate { };

    public event Action<int, int> OnHealthChanged = delegate { };

    public event Action OnHit = delegate { };

    public bool IsDead { get { return currentHealth <= 0; } }
    public bool Alive { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        attacker = GetComponent<Attacker>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        Alive = true;
    }

    private void Update()
    {
        if (IsDead)
        {
            return;
        }
        if (target == null || target.Alive == false)
        {
            AcquireTarget();
        }
        else
        {
            if (attacker.InAttackRange(target) == false)
            {
                FollowTarget();
            }
            else
            {
                TryAttack();
            }
        }
    }

    private void AcquireTarget()
    {
        target = Character.All.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
        animator.SetFloat("Speed", 0f);
    }

    private void TryAttack()
    {
        animator.SetFloat("Speed", 0f);
        navMeshAgent.isStopped = true;

        if (attacker.CanAttack)
        {
            animator.SetTrigger("Attack");
            attacker.Attack(target);
        }
    }

    private void FollowTarget()
    {
        animator.SetFloat("Speed", 1f);
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.transform.position);
    }

    public void TakeHit(IDamage hitBy)
    {
        currentHealth -= hitBy.Damage;

        OnHealthChanged(currentHealth, maxHealth);
        OnHit();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        navMeshAgent.isStopped = false;
        animator.SetTrigger("Die");

        Alive = false;
        OnDie(this);

        ReturnToPool(6f);
    }
}