using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Triggers/Button")]
public class Button : MyTrigger
{
    [SerializeField] protected UnityEvent activationAction;
    [SerializeField] protected UnityEvent diactivationAction;
    [SerializeField] protected Sprite ButtonSprite0;
    [SerializeField] protected Sprite ButtonSprite1;
    protected virtual void Start()
    {
        activationAction.AddListener(() => { });
    }
    public override void Activate(NewEnemyBace entity)
    {
        base.Activate(entity);
        GetComponent<SpriteRenderer>().sprite = ButtonSprite1;
        activationAction?.Invoke();
    }
    public override void Diactivate(NewEnemyBace entity)
    {
        GetComponent<SpriteRenderer>().sprite = ButtonSprite0;
        diactivationAction?.Invoke();
        base.Diactivate(entity);
    }
}
