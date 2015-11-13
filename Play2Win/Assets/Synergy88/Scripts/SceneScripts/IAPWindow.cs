﻿using UnityEngine;
using System.Collections;

public class IAPWindow : MonoBehaviour, ISignalListener {

    [SerializeField]
    private GameObject _windowParent;

    void Start() {
        SignalManager.Instance.Register(this, SignalType.BUTTON_MORE_COINS);

        Close();
    }

    void OnDestroy() {
        SignalManager.Instance.Remove(this, SignalType.BUTTON_MORE_COINS);
    }

    public void Show() {
        _windowParent.SetActive(true);
    }

    public void Close() {
        _windowParent.SetActive(false);
    }

    public void BuyItem1() {
        Buy(850000);
    }

    public void BuyItem2() {
        Buy(390000);
    }

    public void BuyItem3() {
        Buy(220000);
    }

    public void BuyItem4() {
        Buy(80000);
    }

    public void BuyItem5() {
        Buy(32500);
    }

    private void Buy(int amount) {
        PlayerDataManager.Instance.AddChips(amount);
    }

    public void Execute(SignalType type, ISignalParameters param) {
        if (type == SignalType.BUTTON_MORE_COINS) {
            Show();
        }
    }

}
