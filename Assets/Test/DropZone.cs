using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public List<Block> items;

    public void SortItems()
    {
        for(int i = 0; i < items.Count - 1; i++)
        {
            for(int j = i + 1; j < items.Count; j++)
            {
                if(items[i].GetComponent<RectTransform>().anchoredPosition.x > items[j].GetComponent<RectTransform>().anchoredPosition.x)
                {
                    (items[i], items[j]) = (items[j], items[i]);
                }
            }
        }
    }

    public int[] IDArray()
    {
        int[] IDArray = new int[items.Count];
        for(int i = 0; i < items.Count; i++)
        {
            IDArray[i] = items[i].ID;
        }
        return IDArray;
    }
}
