using UnityEngine;

[AddComponentMenu("Triggers/Eater Trigger")]
public class EaterTrigger : MyTrigger
{
    [SerializeField] private EnemyGroundEater ege;
    public override void Activate(Entity entity)
    {
        ege.StartCoroutine(ege.eat(entity));
    }
}