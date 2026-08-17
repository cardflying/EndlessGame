using System;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private RawData rawData;

    public void ConvertRawData(string _data)
    {
        RawData data = JsonUtility.FromJson<RawData>(_data);
        this.rawData = data;
    }


    public RawData GetRawData()
    {
        return rawData;
    }   


    [Serializable]
    public class RawData
    {
        public float speed;
        public int timer;
        public int debug;
    }
}
