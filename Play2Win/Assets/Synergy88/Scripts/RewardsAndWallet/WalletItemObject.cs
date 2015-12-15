using UnityEngine;
using System.Collections;

public class WalletItemObject : MonoBehaviour {

    [SerializeField]
    private tk2dSprite Image;

    [SerializeField]
    private tk2dTextMesh Name;

    private ItemRewards Item;

	[SerializeField]
	private GameObject myCamera;

	[SerializeField]
	private GameObject backDrop;

	[SerializeField]
	private tk2dSpriteAnimator QRFlip;


    public void SetItem(ItemRewards newItem) {
        Item = newItem;
        Image.SetSprite(Item.ImageName);
        Name.text = Item.Name;
    }

	void FlipQR(){
		this.gameObject.transform.parent.gameObject.GetComponent<CloseQR> ().OpenCode ();
	}

    public void OnItemClicked() {
        ConcreteSignalParameters param = new ConcreteSignalParameters();
        param.AddParameter("ItemRewards", Item);
        SignalManager.Instance.CallWithParam(SignalType.REWARD_ITEM_BOUGHT, param);
    }

}
