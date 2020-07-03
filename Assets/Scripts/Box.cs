using System;
using UnityEngine;

public class Box : MonoBehaviour, ITakeHit
{
    private new Rigidbody rigidbody;

    [SerializeField]
    private float forceAmount = 5f;

    public bool Alive { get { return true; } }

    public event Action OnHit = delegate { };

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeHit(IDamage hitBy)
    {
        OnHit();

        var direction = Vector3.Normalize(transform.position - hitBy.transform.position);

        rigidbody.AddForce(direction * forceAmount, ForceMode.Impulse);
    }
}