using DG.Tweening;
using System.Collections;
using UnityEngine;

[AddComponentMenu("Enemies/Barnacle")]
public class EnemyPlantBarnacle : NewEnemyBace
{
    [SerializeField]
    private LayerMask layer;
    [Min(0.01f), SerializeField]
    private float grabDistance = 1;
    [Min(1f), SerializeField]
    private float grabTime = 10;
    private bool isGrabbing = false;
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private LineRenderer lr2;
    [SerializeField]
    private LineRenderer lr3;
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * grabDistance));
        if (isGrabbing)
            Gizmos.DrawLine(transform.position, PlayerMover.single.tr.position);
    }
    protected override void NewFixedUpdate()
    {
        lr.enabled = isGrabbing;
        lr2.enabled = isGrabbing;
        lr3.enabled = isGrabbing;
        if (!isGrabbing
            && Physics2D.Raycast(tr.position, transform.up, grabDistance, layer))
        {
            StartCoroutine(Grab());
            isGrabbing = true;
        }
    }
    protected override void NewLateUpdate()
    {
        if (isGrabbing)
        {
            lr.SetPosition(0, tr.position);
            lr.SetPosition(1, PlayerMover.single.sr.transform.position);
            lr2.SetPosition(0, lr2.transform.position);
            lr2.SetPosition(1, PlayerMover.single.sr.transform.position);
            lr3.SetPosition(0, lr3.transform.position);
            lr3.SetPosition(1, PlayerMover.single.sr.transform.position);
        }
    }
    private IEnumerator Grab()
    {
        PlayerMover.single.rb.simulated = false;

        PlayerMover.single.tr.DOMove(tr.position, grabTime);

        PlayerMover.single.rb.simulated = true;
        isGrabbing = false;
        yield return null;
    }
    public override void SelfDestroy()
    {
        if (isGrabbing)
        {
            PlayerMover.single.tr.DOKill();
            PlayerMover.single.rb.MovePosition(tr.position);
            PlayerMover.single.rb.simulated = true;
            isGrabbing = false;
        }
        base.SelfDestroy();
    }
}
