using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using BossAttacks;
using Random = UnityEngine.Random;
using OUTDATED_Traits;
using System.Collections.Generic;
using Unity.Mathematics;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Boss : Entity
{
    [SerializeField] private BossAttack[] bossAttacks;
    [SerializeField] private bool isSleeping = true;
    [SerializeField, Min(0)] private float attackDelay = 1;
    public UnityEvent OnWakeUp;
    [HideInInspector] public UnityEvent<BossAttack> OnAttack;
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
        List<BossAttack> ab = new List<BossAttack>();
        foreach (var item in bossAttacks)
        {
            if (Random.Range(0, 101) < item.Chance)
            {
                ab.Add(item);
            }
        }
        if (ab.Count > 0)
        {
            BossAttack i = ab[Random.Range(0, ab.Count)];

            IBossAttack bt = i.bossTrait;
            StartCoroutine(bt.Attack(this, i, OnAttack));
            return i.Delay;
        }
        else
            return 0.1f;
    }
}


namespace BossAttacks
{
    using UnityEngine;


    public interface IBossAttack
    {
        IEnumerator Attack(Boss entity, BossAttack attack, UnityEvent<BossAttack> onAttack = null);
    }

    public class GroundUpAttack : IBossAttack
    {
        public IEnumerator Attack(Boss entity, BossAttack attack, UnityEvent<BossAttack> onAttack = null)
        {
            yield return null;
            Debug.Log(entity.GetType().Name + " == " + nameof(BossGrave) + ": " + GetType().Name);

            if (entity.GetType().Name == nameof(BossGrave))
                entity.StartCoroutine(Attack((BossGrave)entity, attack, onAttack));

            yield return new WaitForSeconds(attack.Delay*5f);
        }
        public IEnumerator Attack(BossGrave entity, BossAttack attack, UnityEvent<BossAttack> onAttack = null)
        {
            Debug.Log(GetType().Name);
            for (int i = 0; i < 5; i++)
            {
                Transform ins = Object.Instantiate(entity.groundUpAttackPrefab).transform;
                ins.position = new Vector2(PlayerMover.single.tr.position.x, entity.tr.position.y);
                ins.SetParent(null);
                ins.rotation = Quaternion.identity;
                ins.localScale = Vector2.one;

                yield return new WaitForSeconds(attack.Delay);
            }
            onAttack?.Invoke(attack);
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
            GroundUpAttack
        }
        [SerializeField]
        private BossTraitsEnum bossTraits;
        public IBossAttack bossTrait
        {
            get
            {
                switch (bossTraits)
                {
                    case BossTraitsEnum.GroundUpAttack:
                        return new GroundUpAttack();
                    default:
                        return null;
                }
            }
        }
    }
}
