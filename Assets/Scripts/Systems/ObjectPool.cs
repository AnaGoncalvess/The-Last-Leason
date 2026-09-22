using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool genérico de componentes para evitar Instantiate/Destroy repetido (ex: projéteis).
/// Uso: var pool = new ObjectPool&lt;BolinhaDePapel&gt;(prefabComponent);
///      var item = pool.Get(posicao, rotacao);
///      pool.Release(item);
/// </summary>
public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Stack<T> available = new Stack<T>();

    public ObjectPool(T prefab, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        T instance;

        if (available.Count > 0)
        {
            instance = available.Pop();
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);
        }
        else
        {
            instance = Object.Instantiate(prefab, position, rotation, parent);
        }

        return instance;
    }

    public void Release(T instance)
    {
        if (instance == null) return;

        instance.gameObject.SetActive(false);
        available.Push(instance);
    }
}
