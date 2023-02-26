using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[AddComponentMenu("Enemies/Ground Eater")]
public class EnemyGroundEater : NewEnemyBace
{
    [SerializeField] private Sprite EatSprite;
    [SerializeField] private Sprite UnEatSprite;
    [SerializeField, Min(0.1f)] private float EatTime = 1;
    [SerializeField, Min(0)] private float EatHp = 2;
    [SerializeField, Min(0)] private float DragForce = 2;
    private NewEnemyBace entit;

    public IEnumerator eat(NewEnemyBace entity)
    {
        entit = entity;
        entity.rb.isKinematic = true;
        entity.rb.velocity = Vector2.zero;
        entity.rb.angularVelocity = 0;
        entity.sr.enabled = false;
        sr.sprite = EatSprite;
        entity.tr.position = tr.position + tr.up;

        yield return new WaitForSeconds(EatTime);
        UnEat();
    }
    private void UnEat()
    {
        sr.sprite = UnEatSprite;
        entit.rb.isKinematic = false;
        entit.sr.enabled = true;
        entit.rb.AddForce(transform.up * DragForce, ForceMode2D.Impulse);
        entit.AddDamage(EatHp, true);
    }

    public override void SelfDestroy()
    {
        if(entit!=null)
            UnEat();
        base.SelfDestroy();
    }
}