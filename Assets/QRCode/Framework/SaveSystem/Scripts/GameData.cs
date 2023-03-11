namespace QRCode.Framework
{
    using System;

    [Serializable]
    public class GameData
    {
        public int ValueTest;
 
        //The values defined in this constructor will be default values.
        //The game starts with when there is no data to load.
        public GameData()
        {
            ValueTest = default;
        }
    }
}