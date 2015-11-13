using UnityEngine;
using System.Collections;

public class RewardItemObject : MonoBehaviour {

    [SerializeField]
    private tk2dSprite Image;

    [SerializeField]
    private tk2dTextMesh Name;

    [SerializeField]
    private tk2dTextMesh Cost;

    private ItemRewards Item;

    public void SetItem(ItemRewards newItem) {
        Item = newItem;
        Image.SetSprite(Item.ImageName);
        Name.text = Item.Name;
        Cost.text = Item.Cost.ToString();
    }

    public void OnBuyClicked() {
        ConcreteSignalParameters param = new ConcreteSignalParameters();
        param.AddParameter("ItemRewards", Item);
        SignalManager.Instance.CallWithParam(SignalType.REWARD_ITEM_BOUGHT, param);
    }
}
