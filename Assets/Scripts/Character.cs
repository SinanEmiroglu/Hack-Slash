using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : PooledMonoBehaviour, ITakeHit, IDie
{
    public static List<Character> All = new List<Character>();

    [SerializeField]
    private int maxHealth = 10;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private int damage = 1;

    private Animator animator;
    private int currentHealth;
    private Controller controller;
    private new Rigidbody rigidbody;

    public event Action<int, int> OnHealthChanged = delegate { };

    public event Action<IDie> OnDie = delegate { };

    public event Action OnHit = delegate { };

    public int Damage { get { return damage; } }
    public bool Alive { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    internal void SetController(Controller controller)
    {
        this.controller = controller;
        foreach (var ability in GetComponents<AbilityBase>())
        {
            ability.SetController(controller);
        }
    }

    private void Update()
    {
        Vector3 direction = controller.GetDirection();

        if (direction.magnitude > 0.1f)
        {
            var velocity = (direction * moveSpeed).With(y: rigidbody.velocity.y);
            rigidbody.velocity = velocity;
            transform.forward = direction * 360f;

            animator.SetFloat("Speed", direction.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        Alive = true;

        if (All.Contains(this) == false)
            All.Add(this);
    }

    protected override void OnDisable()
    {
        if (All.Contains(this))
            All.Remove(this);

        base.OnDisable();
    }

    public void TakeHit(IDamage hitBy)
    {
        currentHealth -= hitBy.Damage;

        OnHealthChanged(currentHealth, maxHealth);
        OnHit();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    private void Die()
    {
        Alive = false;
        OnDie(this);
    }
}