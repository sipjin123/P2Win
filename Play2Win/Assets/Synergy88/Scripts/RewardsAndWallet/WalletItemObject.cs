using UnityEngine;
using System.Collections;

public class WalletItemObject : MonoBehaviour {

    [SerializeField]
    private tk2dSprite Image;

    [SerializeField]
    private tk2dTextMesh Name;

    private ItemRewards Item;

	[SerializeField]
	private tk2dSprite QRCode;
	[SerializeField]
	private tk2dSprite QRImage;

	private Animator QRAnim;


	void Start(){
		QRAnim = QRImage.gameObject.GetComponent<Animator> ();
	}

	void Update(){
		if (QRImage.transform.rotation.y == -1.0f) {
			QRCode.SortingOrder = 3;

		} 
		else {
			QRCode.SortingOrder = 0;
		}
	}


    public void SetItem(ItemRewards newItem) {
        Item = newItem;
        Image.SetSprite(Item.ImageName);
        Name.text = Item.Name;
    }

	void FlipQR(){
		QRAnim.SetBool ("FlipQR", QRAnim.GetBool("FlipQR") ? false : true);

	}

    public void OnItemClicked() {
        ConcreteSignalParameters param = new ConcreteSignalParameters();
        param.AddParameter("ItemRewards", Item);
        SignalManager.Instance.CallWithParam(SignalType.REWARD_ITEM_BOUGHT, param);
    }

}
