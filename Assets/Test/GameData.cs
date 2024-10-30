using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    [SerializedDictionary("ID", "Name")]
    public SerializedDictionary<int, string> redDataBlock, blueBlockData, greenBlockData;

    [SerializedDictionary("Request Name", "ID Order")]
    public SerializedDictionary<string, int[]> requestData;

    [SerializeField] Transform redBlocks, blueBlocks, greenBlocks;
    [SerializeField] TextMeshProUGUI requestText;
    [SerializeField] DropZone dropZone;

    public int[] requestedIDArray;

    void Start()
    {
        PopulateDataInBlocks();
    }

    void PopulateDataInBlocks()
    {
        int randomRequest = Random.Range(0, requestData.Count);
        List<string> requests = new(requestData.Keys);
        requestText.text = requests[randomRequest];
        requestedIDArray = requestData[requests[randomRequest]];

        int redKey = requestedIDArray[0];
        int blueKey = requestedIDArray[1];
        int greenKey = requestedIDArray[2];

        AssignDataToBlock(redKey, redDataBlock, redBlocks);
        AssignDataToBlock(blueKey, blueBlockData, blueBlocks);
        AssignDataToBlock(greenKey, greenBlockData, greenBlocks);
    }

    void AssignDataToBlock(int key, SerializedDictionary<int, string> dataBlock, Transform dataParent)
    {
        List<int> IDs = new() { key };
        List<int> availableKeys = new(dataBlock.Keys);
        availableKeys.Remove(key);
        for(int i = 0; i < dataParent.childCount - 1; i++)
        {
            int index = Random.Range(0, availableKeys.Count);
            int randomKey = availableKeys[index];
            IDs.Add(randomKey);
            availableKeys.Remove(randomKey);
        }
        Shuffle(IDs);
        for(int i = 0; i < dataParent.childCount; i++)
        {
            Block block = dataParent.GetChild(i).GetComponent<Block>();
            int keyIndex = IDs[i];
            block.ID = keyIndex;
            block.blockName.text = dataBlock[keyIndex];
        }
    }

    void Shuffle<T>(List<T> inputList)
    {
        for(int i = 0; i < inputList.Count - 1; i++)
        {
            int random = Random.Range(0, inputList.Count);
            (inputList[i], inputList[random]) = (inputList[random], inputList[i]);
        }
    }

    private bool CheckIDArray()
    {
        int[] IDArray = dropZone.IDArray();

        if(IDArray.Length != requestedIDArray.Length) return false;

        for(int i = 0; i < IDArray.Length; i++)
        {
            if(requestedIDArray[i] != IDArray[i]) return false;
        }

        return true;
    }

    public void CheckAnswer()
    {
        if(CheckIDArray())
        {
            dropZone.transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }
        else
        {
            dropZone.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }
    }
}

