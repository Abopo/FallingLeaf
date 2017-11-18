using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum SHOP_ITEMS { NOTHING = -1, WIND1 = 0, SHIELD, MAGNET };

public class Store : MonoBehaviour {
    public Text coinText;
    public Text itemName;
    public Text itemDescription;
    public Button buyButton;

    SHOP_ITEMS selectedItem;
    int curCoins;
    int curItemCost;
    

	// Use this for initialization
	void Start () {
        itemName.text = "";
        itemDescription.text = "";

        selectedItem = SHOP_ITEMS.NOTHING;

        curCoins = PlayerPrefs.GetInt("Coins");
        coinText.text = "$ " + curCoins;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButton() {
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

    public void BuyButton() {
        switch(selectedItem) {
            case SHOP_ITEMS.NOTHING:
                ;
                break;
            case SHOP_ITEMS.WIND1:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("UpdraftUnlocked", 1);
                UpdateBuyButton("UpdraftUnlocked");
                break;
            case SHOP_ITEMS.SHIELD:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("ShieldUnlocked", 1);
                UpdateBuyButton("ShieldUnlocked");
                break;
            case SHOP_ITEMS.MAGNET:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("MagnetUnlocked", 1);
                UpdateBuyButton("MagnetUnlocked");
                break;
        }

        PlayerPrefs.SetInt("Coins", curCoins);
        coinText.text = "$ " + curCoins;
    }

    public void WindBuff1Selected() {
        itemName.text = "Updraft - $500";
        itemDescription.text = "Increase the amount of wind you can use";
        selectedItem = SHOP_ITEMS.WIND1;
        curItemCost = 500;
        UpdateBuyButton("UpdraftUnlocked");
    }

    public void LeafShieldSelected() {
        itemName.text = "Leaf Shield - $1000";
        itemDescription.text = "Take an extra hit before dying";
        selectedItem = SHOP_ITEMS.SHIELD;
        curItemCost = 1000;
        UpdateBuyButton("ShieldUnlocked");
    }

    public void CoinMagnetSelected() {
        itemName.text = "Coin Magnet - $2000";
        itemDescription.text = "Pull in coins from a distance";
        selectedItem = SHOP_ITEMS.MAGNET;
        curItemCost = 2000;
        UpdateBuyButton("MagnetUnlocked");
    }

    void UpdateBuyButton(string item) {
        if (PlayerPrefs.GetInt(item) == 1) {
            buyButton.GetComponentInChildren<Text>().text = "Bought";
            buyButton.interactable = false;
        } else if (curCoins < curItemCost) {
            buyButton.GetComponentInChildren<Text>().text = "Buy";
            buyButton.interactable = false;
        } else {
            buyButton.GetComponentInChildren<Text>().text = "Buy";
            buyButton.interactable = true;
        }
    }

}
