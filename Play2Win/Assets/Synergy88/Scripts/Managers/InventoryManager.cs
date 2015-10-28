using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

    [System.Serializable]
    public struct ItemRewards {

        [SerializeField]
        private string name;

        [SerializeField]
        private int cost;

        public string Name { get { return name; } }
        public int Cost { get { return cost; } }
    }

    [SerializeField]
    private ItemRewards[] Items;

    private Dictionary<string, ItemRewards> _itemMasterList;

    private List<ItemRewards> _shopItems;
    public List<ItemRewards> ShopItems { get { return _shopItems; } }

    private List<ItemRewards> _ownedItems;
    public List<ItemRewards> OwnedItems { get { return _ownedItems; } }

    void Start() {
        _itemMasterList = new Dictionary<string, ItemRewards>();
        _shopItems = new List<ItemRewards>();
        _ownedItems = new List<ItemRewards>();

        for (int i = 0; i < Items.Length; i++) {
            _itemMasterList.Add(Items[i].Name, Items[i]);
            _shopItems.Add(Items[i]);
        }
    }

    public void SetItemAsBought(ItemRewards item) {
        _shopItems.Remove(item);
        _ownedItems.Add(item);
    }

}
