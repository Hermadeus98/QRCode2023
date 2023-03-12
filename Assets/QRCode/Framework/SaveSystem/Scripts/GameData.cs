namespace QRCode.Framework
{
    using System;
    using SerializedTypes;

    [Serializable]
    public class GameData
    {
        public int ValueTest;

        public SerializableDictionary<string, float> DictionaryTest = new SerializableDictionary<string, float>();

        //The values defined in this constructor will be default values.
        //The game starts with when there is no data to load.
        public GameData()
        {
            ValueTest = default;
            DictionaryTest = new SerializableDictionary<string, float>();
        }
    }
}