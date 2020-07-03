using UnityEngine;

public class DeathParticle : MonoBehaviour
{
    [SerializeField]
    private PooledMonoBehaviour deathParticle;

    private IDie entity;

    private void Awake()
    {
        entity = GetComponent<IDie>();
    }

    private void OnEnable()
    {
        entity.OnDie += Character_OnDie;
    }

    private void Character_OnDie(IDie entity)
    {
        deathParticle.Get<PooledMonoBehaviour>(transform.position, Quaternion.identity);
        entity.OnDie -= Character_OnDie;
    }

    private void OnDisable()
    {
        entity.OnDie -= Character_OnDie;
    }
}