using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using BossTraits;


[RequireComponent(typeof(Rigidbody2D))]
public abstract class Boss : Entity
{
    [SerializeField] private BossAttack[] bossAttacks;
    [SerializeField, Min(0)] private float attackDelay = 1;
    public override sealed void Awake()
    {
        StartCoroutine(Attacker());
        base.Awake();
    }
    private IEnumerator Attacker()
    {
        yield return null;
    }
}
namespace BossTraits
{
    abstract class BossTrait { }

    interface IBossAttack<T> where T : BossTrait { }

    class Generic : BossTrait { }
    class Generic2 : BossTrait { }

    static class BossTraits
    {
        public static void Attack(this IBossAttack<Generic> trait, Boss entity)
        {

        }
        public static void Attack(this IBossAttack<Generic2> trait, Boss entity)
        {

        }
    }

    [Serializable]
    struct BossAttack
    {
        [Range(0f, 100f)]
        public float chance;
        public enum BossTraitsEnum
        {
            Generic, Generic2
        }
        [SerializeField]
        private BossTraitsEnum bossTraits;
        public Type bossTrait
        {
            get
            {
                switch (bossTraits)
                {
                    case BossTraitsEnum.Generic:
                        return typeof(Generic);
                    case BossTraitsEnum.Generic2:
                        return typeof(Generic2);
                    default:
                        return null;
                }
            }
        }
    }
}