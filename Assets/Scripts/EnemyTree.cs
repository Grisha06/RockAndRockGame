using System.Collections;
using UnityEngine;

[AddComponentMenu("Enemies/Tree")]
public class EnemyTree : EntityAttakable
{
    [SerializeField]
    protected TreeSpavnerObj[] treeSpavnerObj;
    private void SpawnEnemy(int i)
    {
        RaycastHit2D raycast= Physics2D.Raycast(treeSpavnerObj[i].MusicNoteSpavner.position, -tr.up, 100f, groundLayer);
        Entity mn = Instantiate(treeSpavnerObj[i].ToSpawn[Random.Range(0, treeSpavnerObj[i].ToSpawn.Length)], treeSpavnerObj[i].MusicNoteSpavner);
        mn.an.Play("grow");
        if (raycast)
        {
            Debug.Log(raycast.collider.gameObject.name);
            mn.tr.position = raycast.point;
        }
        mn.tr.rotation = Quaternion.identity;
        mn.tr.localScale = Vector3.one;
        mn.tr.SetParent(null);
    }
    protected override void NewFixedUpdate()
    {
        enemyBaceAction = attackIfRad && Vector2.Distance(PlayerMover.single.tr.position, tr.position) < attackRadius ? EnemyBaceActions.Attack : EnemyBaceActions.Run;
    }
    public override IEnumerator AttackingEnumerator()
    {
        yield return new WaitForSeconds(0.01f);
        while (true)
        {
            if (enemyBaceAction == EnemyBaceActions.Attack && hp > 0)
            {
                if (!sameTimeAttack)
                {
                    for (int i = 0; i < treeSpavnerObj.Length; i++)
                    {
                        SpawnEnemy(i);
                        if (hp > 0)
                            an.Play("spawnMN_nospawn");
                        yield return new WaitForSeconds(treeSpavnerObj[i].SpawnTime);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.25f);
                    if (hp > 0)
                        an.Play("spawnMN_nospawn");
                    for (int i = 0; i < treeSpavnerObj.Length && hp > 0; i++)
                    {
                        SpawnEnemy(i);
                    }
                }
            }
            yield return new WaitForSeconds(attackTime);
        }
    }
}