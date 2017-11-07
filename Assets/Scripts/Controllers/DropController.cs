using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropController : MonoBehaviour
{

    // int[] drops = new int[3];
    [System.Serializable]
    public class DropCurrency {

        public GameObject items;
        public int dropRarity;
}

    public List<DropCurrency> LootTable = new List<DropCurrency>();
    public int dropChance;
    void calculateLoot()
    {
        int calc_dropChance = Random.Range(0, 101);

        if (calc_dropChance > dropChance)
        {
            return;
        }
        if (calc_dropChance <= dropChance)
        {
            int itemWeight = 0;

            for (int i = 0; i < LootTable.Count; i++)
            {
                itemWeight += LootTable[i].dropRarity;
            }
            Debug.Log("Itemweight=" + itemWeight);

            int randomValue = Random.Range(0, itemWeight);

            for (int j = 0; j < LootTable.Count; j++)
            {
                if (randomValue <= LootTable[j].dropRarity)
                {
                    Instantiate(LootTable[j].items, transform.position, Quaternion.identity);
                }
                    
            
            }
                    
         }
                    

      
     }
}


   
    
 