using UnityEngine;

[AddComponentMenu("Triggers/Toggler")]
public class Toggler : Button
{
    [SerializeField]
    protected bool isActive = false;

    public override void Activate(NewEnemyBace entity)
    {
        if (!isActive)
        {
            base.Activate(entity);
        }
        else
        {
            base.Diactivate(entity);
        }
        isActive = !isActive;
    }
    public override void Diactivate(NewEnemyBace entity) { }
}
