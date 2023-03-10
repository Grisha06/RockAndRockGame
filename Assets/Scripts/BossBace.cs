using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using BossAttacks;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Boss : Entity
{
    [SerializeField] private BossAttack[] bossAttacks;
    [SerializeField] private bool isSleeping = true;
    [SerializeField, Min(0)] private float attackDelay = 1;
    public UnityEvent OnWakeUp;
    [HideInInspector] public UnityEvent<IBossAttack> OnAttack;
    public override sealed void Awake()
    {
        StartCoroutine(Attacker());
        base.Awake();
    }
    private IEnumerator Attacker()
    {
        yield return null;
        while (true)
        {
            if (!isSleeping)
            {
                yield return new WaitForSeconds(attackDelay);
                yield return new WaitForSeconds(DoAttack());
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
    public void WakeUp()
    {
        OnWakeUp?.Invoke();
        isSleeping = false;
    }
    public float DoAttack()
    {
        int i = Random.Range(0, bossAttacks.Length);
        BossAttacks.IBossAttack bt = bossAttacks[i].bossTrait;
        bt.Attack(this);
        OnAttack?.Invoke(bt);
        return bossAttacks[i].Delay;
    }
}


namespace BossAttacks
{
    public interface IBossAttack
    {
        void Attack(Boss entity);
    }

    public class Generic : IBossAttack
    {
        public void Attack(Boss entity)
        {
            Debug.Log(entity.name + ": " + this.GetType().Name);
        }
    }
    public class Generic2 : IBossAttack
    {
        public void Attack(Boss entity)
        {
            Debug.Log(entity.name + ": " + this.GetType().Name);
        }
    }

    [Serializable]
    public struct BossAttack
    {
        [Range(0f, 100f)]
        public float Chance;
        [Min(0)]
        public float Delay;
        public enum BossTraitsEnum
        {
            Generic, Generic2
        }
        [SerializeField]
        private BossTraitsEnum bossTraits;
        public IBossAttack bossTrait
        {
            get
            {
                switch (bossTraits)
                {
                    case BossTraitsEnum.Generic:
                        return new Generic();
                    case BossTraitsEnum.Generic2:
                        return new Generic2();
                    default:
                        return null;
                }
            }
        }
    }
}
