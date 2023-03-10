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
    [SerializeField]
    private bool useSize = true;
    [SerializeField]
    private bool usePosition = true;
    private void Awake()
    {
        cm = Camera.main.GetComponent<Cameramower>();
    }
    public override void Activate(Entity entity)
    {
        if (useSize)
        {
            startSize = Camera.main.orthographicSize;
            cm.OrtSize = Size;
        }
        if (usePosition)
        {
            startPos = cm.lookTo;
            cm.lookTo = transform;
        }
    }
    public override void Diactivate(Entity entity)
    {
        if (usePosition)
            cm.lookTo = startPos;
        if (useSize)
            cm.OrtSize = 3;
    }
}