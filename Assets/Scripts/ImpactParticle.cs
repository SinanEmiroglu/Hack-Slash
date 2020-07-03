using UnityEngine;

public class ImpactParticle : MonoBehaviour
{
    [SerializeField]
    private PooledMonoBehaviour impactParticle;

    private ITakeHit entity;

    private void Awake()
    {
        entity = GetComponent<ITakeHit>();
    }

    private void OnEnable()
    {
        entity.OnHit += HandleHit;
    }

    private void HandleHit()
    {
        impactParticle.Get<PooledMonoBehaviour>(transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
    }

    private void OnDisable()
    {
        entity.OnHit -= HandleHit;
    }
}