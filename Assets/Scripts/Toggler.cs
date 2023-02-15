using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Triggers/Toggler")]
public class Toggler : Button
{
    protected bool isActive = false;
    protected override void Start()
    {
        activationAction.AddListener(() => { });
    }
    public override void Activate(NewEnemyBace entity)
    {
        isActive = !isActive;
        if (isActive)
        {
            base.Activate(entity);
            GetComponent<SpriteRenderer>().sprite = ButtonSprite1;
            activationAction?.Invoke();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = ButtonSprite0;
            diactivationAction?.Invoke();
            base.Diactivate(entity);
        }
    }
    public override void Diactivate(NewEnemyBace entity)
    {

    }
}
