using UnityEngine;
using System.Collections;

public class WalletItemObject : MonoBehaviour {

    [SerializeField]
    private tk2dSprite Image;

    [SerializeField]
    private tk2dTextMesh Name;

    private ItemRewards Item;

    public void SetItem(ItemRewards newItem) {
        Item = newItem;
        Image.SetSprite(Item.ImageName);
        Name.text = Item.Name;
    }

    public void OnItemClicked() {
        ConcreteSignalParameters param = new ConcreteSignalParameters();
        param.AddParameter("ItemRewards", Item);
        SignalManager.Instance.CallWithParam(SignalType.REWARD_ITEM_BOUGHT, param);
    }

}
