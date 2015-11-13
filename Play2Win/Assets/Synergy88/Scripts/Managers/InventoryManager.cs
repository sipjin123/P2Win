using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ItemRewards {

    [SerializeField]
    private string id;

    [SerializeField]
    private string name;

    [SerializeField]
    private string description;

    [SerializeField]
    private string description2;

    [SerializeField]
    private int cost;

    [SerializeField]
    private string imageName;

    public string ID { get { return id; } }
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public string Description2 { get { return description2; } }
    public int Cost { get { return cost; } }
    public string ImageName { get { return imageName; } }
}

public class InventoryManager : MonoBehaviour {

    private static InventoryManager _instance;
    public static InventoryManager Instance { get { return _instance; } }

    [SerializeField]
    private ItemRewards[] Items;

    private Dictionary<string, ItemRewards> _itemMasterList;

    private List<ItemRewards> _shopItems;
    public List<ItemRewards> ShopItems { get { return _shopItems; } }

    private List<ItemRewards> _ownedItems;
    public List<ItemRewards> OwnedItems { get { return _ownedItems; } }

    void Awake() {
        _instance = this;
    }

    void Start() {
        _itemMasterList = new Dictionary<string, ItemRewards>();
        _shopItems = new List<ItemRewards>();
        _ownedItems = new List<ItemRewards>();

        for (int i = 0; i < Items.Length; i++) {
            _itemMasterList.Add(Items[i].ID, Items[i]);
            _shopItems.Add(Items[i]);
        }

        RefreshBoughtItems();
    }

    public void SetItemAsBought(ItemRewards item) {
        _shopItems.Remove(item);
        _ownedItems.Add(item);
        PlayerDataManager.Instance.SetInventoryItemBought(item.ID);
    }

    public void RefreshBoughtItems() {
        foreach (string id in _itemMasterList.Keys) {
            if (PlayerDataManager.Instance.CheckIfInventoryItemIsBought(id)) {
                if (!_ownedItems.Contains(_itemMasterList[id])) {
                    _ownedItems.Add(_itemMasterList[id]);
                }

                if (_shopItems.Contains(_itemMasterList[id])) {
                    _shopItems.Remove(_itemMasterList[id]);
                }
            } else {
                if (_ownedItems.Contains(_itemMasterList[id])) {
                    _ownedItems.Remove(_itemMasterList[id]);
                }

                if (!_shopItems.Contains(_itemMasterList[id])) {
                    _shopItems.Add(_itemMasterList[id]);
                }
            }
        }
    }

}
