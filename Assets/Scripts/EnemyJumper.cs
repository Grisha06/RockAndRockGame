using UnityEngine;

[AddComponentMenu("Enemies/Plant Jumper")]
public class EnemyJumper : Entity
{
    [SerializeField, Min(0)]
    private float jumpForce = 1;
    public void OnCollisionPlayerJump(Entity entity)
    {
        an.Play("damage");
        entity.rb.velocity = Vector2.zero;
        entity.rb.AddForce(tr.up * jumpForce, ForceMode2D.Impulse);
    }
}