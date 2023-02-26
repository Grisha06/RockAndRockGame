using UnityEngine;

[AddComponentMenu("Triggers/Eater Trigger")]
public class EaterTrigger : MyTrigger
{
    [SerializeField] private EnemyGroundEater ege;
    public override void Activate(NewEnemyBace entity)
    {
        ege.StartCoroutine(ege.eat(entity));
    }
}