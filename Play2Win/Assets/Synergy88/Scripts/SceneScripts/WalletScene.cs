using UnityEngine;
using System.Collections;

public class WalletScene : MonoBehaviour, ISignalListener {

    [SerializeField]
    private ScrollManager _scrollManager;

    [SerializeField]
    private WalletItemObject _rewardItemPrefab;

    [SerializeField]
    private Transform _rewardItemParent;

    // Info Details
    [SerializeField]
    private GameObject _infoParent;

    [SerializeField]
    private tk2dSprite _infoImage;

    [SerializeField]
    private tk2dTextMesh _infoName;

    [SerializeField]
    private tk2dTextMesh _infoDescription;

    [SerializeField]
    private tk2dTextMesh _infoDescription2;

    void Start() {
        ConcreteSignalParameters updateHudParam = new ConcreteSignalParameters();
        updateHudParam.AddParameter("ProfileUIType", ProfileUIType.GEM_SCENES);
        SignalManager.Instance.CallWithParam(SignalType.UPDATE_PROFILE_HUD, updateHudParam);

        SignalManager.Instance.Register(this, SignalType.REWARD_ITEM_BOUGHT);

        RefreshItemList();

        _infoParent.SetActive(false);
    }

    void OnDestroy() {
        SignalManager.Instance.Remove(this, SignalType.REWARD_ITEM_BOUGHT);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            ExitScene();
        }
    }

    private void RefreshItemList() {
        foreach (ItemRewards item in InventoryManager.Instance.OwnedItems) {
            WalletItemObject itemObject = (WalletItemObject)Instantiate(_rewardItemPrefab).GetComponent<WalletItemObject>();
            itemObject.name = item.ID;
            itemObject.transform.SetParent(_rewardItemParent);
            itemObject.SetItem(item);

            _scrollManager.AddObject(itemObject.gameObject);
        }

        _scrollManager.ActivateScrollList();
    }

    public void ExitScene() {
        GameManager.Instance.LoadScene(GameState.MAIN_MENU);
    }

    public void ShowItemInfo(ItemRewards itemToBuy) {
        _infoImage.SetSprite(itemToBuy.ImageName);
        _infoName.text = itemToBuy.Name;
        _infoDescription.text = itemToBuy.Description;
        _infoDescription2.text = itemToBuy.Description2;

        _infoParent.SetActive(true);
    }

    public void OnPopUpClosed() {
        _infoParent.SetActive(false);
    }

    public void Execute(SignalType type, ISignalParameters param) {
        if (type == SignalType.REWARD_ITEM_BOUGHT) {
            ItemRewards item = (ItemRewards)param.GetParameter("ItemRewards");
            ShowItemInfo(item);
        }
    }
}
