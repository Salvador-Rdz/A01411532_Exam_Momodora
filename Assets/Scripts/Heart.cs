using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour {
    //Used for spriteswitching from an array of available heart sprites
    public Sprite[] hearts;
    public int maxHits;
    public int currentHP;

    private void Awake()
    {
        currentHP = maxHits;
    }
    //Decreases the hp of the heart and updates sprite.
    public void takeDMG(int damage)
    {
        currentHP -= damage;
        if (currentHP != 0) GetComponent<Image>().sprite = hearts[currentHP];
        else setEmpty();
    }
    public void replenishHP(int amount)
    {
        currentHP += amount;
        GetComponent<Image>().sprite = hearts[currentHP];
    }
    public int getMaxHP()
    {
        return maxHits;
    }
    public int getCurrentHP()
    {
        return currentHP;
    }
    //sets the heart to an empty sprite.
    public void setEmpty()
    {
        GetComponent<Image>().sprite = hearts[0];
    }
}
