using UnityEngine;
using System.Collections;

public class ShopScene : MonoBehaviour, ISignalListener {

    [SerializeField]
    private ScrollManager _scrollManager;

    [SerializeField]
    private RewardItemObject _rewardItemPrefab;

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

    [SerializeField]
    private tk2dTextMesh _infoPrice;

    [SerializeField]
    private GameObject _infoNotAvailable;

    [SerializeField]
    private GameObject _infoAvailable;

    private ItemRewards _rewardItemToBuy;

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

        if (Input.GetKeyDown(KeyCode.Y)) {
            PlayerDataManager.Instance.AddPoints(1000);
        }
    }

    private void RefreshItemList() {
        foreach (ItemRewards item in InventoryManager.Instance.ShopItems) {
            RewardItemObject itemObject = (RewardItemObject)Instantiate(_rewardItemPrefab).GetComponent<RewardItemObject>();
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
		if (!_infoParent.activeSelf) {
			_rewardItemToBuy = itemToBuy;

			_infoImage.SetSprite (_rewardItemToBuy.ImageName);
			_infoName.text = _rewardItemToBuy.Name;
			_infoDescription.text = _rewardItemToBuy.Description;
			_infoDescription2.text = _rewardItemToBuy.Description2;
			_infoPrice.text = _rewardItemToBuy.Cost.ToString ();

			if (_rewardItemToBuy.Cost > PlayerDataManager.Instance.Points) {
				_infoNotAvailable.SetActive (true);
				_infoAvailable.SetActive (false);
			} else {
				_infoNotAvailable.SetActive (false);
				_infoAvailable.SetActive (true);
			}

			_infoParent.SetActive (true);
		}
    }

    public void OnBuyConfirmed() {
        if (PlayerDataManager.Instance.Points < _rewardItemToBuy.Cost) {
            Debug.LogWarning("Not enough gems");
            return;
        }

        PlayerDataManager.Instance.UsePoints(_rewardItemToBuy.Cost);
        InventoryManager.Instance.SetItemAsBought(_rewardItemToBuy);

        RefreshItemList();
        _infoParent.SetActive(false);
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
