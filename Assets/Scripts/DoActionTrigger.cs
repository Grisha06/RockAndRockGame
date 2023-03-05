using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DoActionTriggerAction : UnityEvent<Entity> { }

[AddComponentMenu("Triggers/Action Trigger")]
public class DoActionTrigger : MyTrigger
{
    [SerializeField] protected DoActionTriggerAction action;
    public override void Activate(Entity entity)
    {
        action.Invoke(entity);
    }
}