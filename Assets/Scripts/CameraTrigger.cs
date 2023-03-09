using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

[AddComponentMenu("Triggers/Camera Trigger")]
public class CameraTrigger : MyTrigger
{
    [Min(0)]
    public float Size = 5;
    private float startSize = 3;
    private Transform startPos;
    private Cameramower cm;
    private void Awake()
    {
        cm = Camera.main.GetComponent<Cameramower>();
    }
    public override void Activate(Entity entity)
    {
        startSize = Camera.main.orthographicSize;
        startPos = cm.lookTo;
        cm.lookTo = transform;
        cm.OrtSize = Size;
    }
    public override void Diactivate(Entity entity)
    {
        cm.lookTo = startPos;
        cm.OrtSize = 3;
    }
}