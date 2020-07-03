using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Dictionary<PooledMonoBehaviour, Pool> pools = new Dictionary<PooledMonoBehaviour, Pool>();

    private Queue<PooledMonoBehaviour> objects = new Queue<PooledMonoBehaviour>();
    private PooledMonoBehaviour prefab;

    public static Pool GetPool(PooledMonoBehaviour prefab)
    {
        //if that specific prefab included in the dictionary, then just return.
        if (pools.ContainsKey(prefab))
        {
            return pools[prefab];
        }
        //Otherwise, create a new GameObject and add it to the dictionary
        var poolGameObject = new GameObject("Pool - " + prefab.name);
        var pool = poolGameObject.AddComponent<Pool>();
        pool.prefab = prefab;

        pools.Add(prefab, pool);
        return pool;
    }

    public T Get<T>() where T : PooledMonoBehaviour
    {
        if (objects.Count == 0)
        {
            GrowPool();
        }

        var pooledObject = objects.Dequeue();

        return pooledObject as T;
    }

    private void GrowPool()
    {
        for (int i = 0; i < prefab.InitialPoolSize; i++)
        {
            var pooledObject = Instantiate(prefab) as PooledMonoBehaviour;

            pooledObject.gameObject.name += " " + i;

            pooledObject.OnReturnToPool += AddObjectToAvailableQueue;

            pooledObject.transform.SetParent(transform);
            pooledObject.gameObject.SetActive(false);
        }
    }

    private void AddObjectToAvailableQueue(PooledMonoBehaviour pooledObject)
    {
        pooledObject.transform.SetParent(transform);
        objects.Enqueue(pooledObject);
    }
}