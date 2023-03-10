using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using BossTraits;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Boss : Entity
{
    [SerializeField] private BossAttack[] bossAttacks;
    [SerializeField] private bool isSleeping = true;
    [SerializeField, Min(0)] private float attackDelay = 1;
    [HideInInspector] public UnityEvent OnWakeUp;
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
        object bt = bossAttacks[i].bossTrait.GetType().GetConstructor(Type.EmptyTypes).Invoke(null);
        IBossAttack bbt = (IBossAttack)bt;
        bbt.Attack(this);
        OnAttack?.Invoke(bbt);
        return i;
    }
}


namespace BossTraits
{
    public interface IBossAttack {
        public abstract void Attack(Boss entity);
    }

    public class Generic : IBossAttack 
    {
        public void Attack(Boss entity)
        {
            Debug.Log(entity.GetType().Name);
        }
    }
    public class Generic2 : IBossAttack
    {
        public void Attack(Boss entity)
        {
            Debug.Log(entity.GetType().Name);
        }
    }

    [Serializable]
    public class BossAttack
    {
        [Range(0f, 100f)]
        public float Chance;
        [Min(0)] 
        public float Delay = 1;
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
