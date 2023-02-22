using NTC.Global.Cache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoCache
{
    private Inventory inventory;
    [SerializeField]
    private Transform invPlane;
    [SerializeField]
    private ItemCanvasObj itemCanvasObj;
    [SerializeField]
    private GameObject MainObj;

    private void Start()
    {
        inventory = PlayerMover.single.GetComponent<Inventory>();
        PlayerMover.single.GetComponent<Inventory>().OnItemsChanged.AddListener(UpdateItems);
    }
    protected sealed override void Run()
    {
        if (Input.GetKeyDown(KeyObj.FindInKeysArr(PlayerMover.single.controls, "openInventory")))
        {
            PlayerMover.single.isInvOpen = !PlayerMover.single.isInvOpen;
            MainObj.gameObject.SetActive(PlayerMover.single.isInvOpen);
        }
    }
    private void UpdateItems()
    {
        foreach (Transform child in invPlane.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in inventory.items)
        {
            ItemCanvasObj i = Instantiate<ItemCanvasObj>(itemCanvasObj);
            i.gameObject.transform.SetParent(invPlane);
            i.transform.localPosition = Vector3.zero;
            i.Activate(item);
        }
    }
}
