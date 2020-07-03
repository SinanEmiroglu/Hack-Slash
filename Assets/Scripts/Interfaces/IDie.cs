using System;
using UnityEngine;

public interface IDie
{
    event Action<IDie> OnDie;

    event Action<int, int> OnHealthChanged;

    GameObject gameObject { get; }
}