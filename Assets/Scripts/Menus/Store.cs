﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum SHOP_ITEMS { NOTHING = -1, WIND1 = 0, SHIELD, ROTATE, MAGNET, GALE, SQUALL, SAFEGUARD };

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
            case SHOP_ITEMS.ROTATE:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("RotationUnlocked", 1);
                UpdateBuyButton("RotationUnlocked");
                break;
            case SHOP_ITEMS.MAGNET:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("MagnetUnlocked", 1);
                UpdateBuyButton("MagnetUnlocked");
                break;
            case SHOP_ITEMS.GALE:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("GaleforceUnlocked", 1);
                UpdateBuyButton("GaleforceUnlocked");
                break;
            case SHOP_ITEMS.SQUALL:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("SquallUnlocked", 1);
                UpdateBuyButton("SquallUnlocked");
                break;
            case SHOP_ITEMS.SAFEGUARD:
                curCoins -= curItemCost;
                PlayerPrefs.SetInt("SafeguardUnlocked", 1);
                UpdateBuyButton("SafeguardUnlocked");
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
        itemName.text = "Leaf Guard - $1000";
        itemDescription.text = "Take an extra hit before dying";
        selectedItem = SHOP_ITEMS.SHIELD;
        curItemCost = 1000;
        UpdateBuyButton("ShieldUnlocked");
    }

    public void RotationSelected() {
        itemName.text = "Whirligig - $1000";
        itemDescription.text = "Increase the speed the leaf rotates";
        selectedItem = SHOP_ITEMS.ROTATE;
        curItemCost = 1000;
        UpdateBuyButton("RotationUnlocked");
    }

    public void CoinMagnetSelected() {
        itemName.text = "Vortex - $1500";
        itemDescription.text = "Pull in coins from a distance";
        selectedItem = SHOP_ITEMS.MAGNET;
        curItemCost = 1500;
        UpdateBuyButton("MagnetUnlocked");
    }

    public void GaleforceSelected() {
        itemName.text = "Galeforce - $2000";
        itemDescription.text = "Increase the strength of your wind gusts";
        selectedItem = SHOP_ITEMS.GALE;
        curItemCost = 1500;
        UpdateBuyButton("GaleforceUnlocked");
    }

    public void SquallSelected() {
        itemName.text = "Squall - $2500";
        itemDescription.text = "Maximize the amount of wind you can use";
        selectedItem = SHOP_ITEMS.SQUALL;
        curItemCost = 2500;
        UpdateBuyButton("SquallUnlocked");
    }

    public void SafeguardSelected() {
        itemName.text = "Safeguard - $3000";
        itemDescription.text = "Take yet another extra hit before dying";
        selectedItem = SHOP_ITEMS.SAFEGUARD;
        curItemCost = 3000;
        UpdateBuyButton("SafeguardUnlocked");
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
