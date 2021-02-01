using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable : IComponent
{
    void Damage(int damageAmount);
    void Dispose();
}
