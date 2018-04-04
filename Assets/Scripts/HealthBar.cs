using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public int TotalHp;
    public int CurrentHp;
    public Heart[] hearts;
    public int numberHearts; //+1 Since its used for an array.
    //Updates references to hearts
    public void TakeDamage(int damage)
    {
        //If the current hp of the heart isn't 0, and the damage isn't greater than it
        if (hearts[numberHearts].getCurrentHP() >= 0 && hearts[numberHearts].getCurrentHP() >= damage)
        {
            //Takes the damage
            hearts[numberHearts].takeDMG(damage);
        }
        //If its smaller
        else if (hearts[numberHearts].getCurrentHP() <= damage)
        {
            //Removes all of the hp of the current heart
            damage -= hearts[numberHearts].getCurrentHP();
            hearts[numberHearts].setEmpty();
            //And removes the rest of it to the next.
            numberHearts--;
            hearts[numberHearts].takeDMG(damage);
        }

    }
    public void IncreaseHealth(int amount)
    {
            print("Replenishing hp bar");
            if (hearts[numberHearts].getCurrentHP() >= hearts[numberHearts].getMaxHP())
            {
                numberHearts++;
                hearts[numberHearts].replenishHP(amount);
            }
            else
            {
                hearts[numberHearts].replenishHP(amount);
            }
    }
    public void setTotalHP(int hp)
    {
        TotalHp = hp;
        CurrentHp = hp;
    }
}
