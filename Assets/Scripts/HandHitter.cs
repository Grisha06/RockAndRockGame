using UnityEngine;

[AddComponentMenu("Enemies/Hand Hitter")]
[RequireComponent(typeof(Collider2D))]
public class HandHitter : MonoBehaviour
{
    public int[] LayerToAttack;
    public int Damage = 1;
}
