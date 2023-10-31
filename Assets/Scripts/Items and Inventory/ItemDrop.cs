using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    [SerializeField] private bool tutorial = false; //確定ドロップのキャラクターを作るため

    public virtual void GenerateDrop()
    {
        if (possibleDrop.Length <= 0)
            return;


        for(int i = 0; i < possibleDrop.Length; i++)
        {   
            //チュートリアルの場合には確定でアイテムがドロップするようにする
            if(tutorial)
            {
                dropList.Add(possibleDrop[i]);
            }
            else
            {
                //乱数で決定した値よりもドロップ率が高かった場合、ドロップリストに追加する
                if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                {
                    dropList.Add(possibleDrop[i]);
                }
            }
            
        }

        for(int i = 0; i< possibleItemDrop; i++)
        {
            if (dropList.Count <= 0)
                return;
            
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);

            

        }
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
