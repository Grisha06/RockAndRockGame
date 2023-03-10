using UnityEngine;
using NTC.Global.Cache;

[RequireComponent(typeof(Collider2D))]
public abstract class MyTrigger : MonoCache
{
    public int[] LayerToActivate;
    [SerializeField]
    private bool diactivateOnDestroy = false;
    [SerializeField]
    private bool diactivateOnDisable = false;
    protected Entity neb = null;

    public virtual void Activate(Entity entity) { }
    public virtual void Diactivate(Entity entity) { }
    public void UpdateNebActivate(Entity entity) => neb = entity;
    public void UpdateNebDiactivate() => neb = null;
    private void OnDestroy()
    {
        if (diactivateOnDestroy && neb)
        {
            Diactivate(neb);
            UpdateNebDiactivate();
        }
        Destroy(gameObject);
    }
    protected override sealed void OnDisabled()
    {
        if (diactivateOnDisable && neb)
        {
            Diactivate(neb);
            UpdateNebDiactivate();
        }
    }

    [ContextMenu("Destroy To Event")]
    public void DestroyToEvent()
    {
        Destroy(gameObject);
    }
}