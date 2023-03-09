using UnityEngine;

[AddComponentMenu("Triggers/Teleport Button")]
public class TeleportButton : Button
{
    [SerializeField] private Transform location;
    [SerializeField] private Entity pl;
    protected override void Start()
    {
        activationAction.AddListener(() => { pl.tr.position = location.position; Camera.main.transform.position = location.position; });
    }
}
