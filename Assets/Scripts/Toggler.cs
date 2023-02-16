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
            Debug.Log(isActive.ToString()+" First");
            base.Activate(entity);
        }
        else
        {
            Debug.Log(isActive.ToString() + " Second");
            base.Diactivate(entity);
        }
        isActive = !isActive;
    }
    public override void Diactivate(NewEnemyBace entity) { }
}
