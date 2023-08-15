namespace QRCode.Engine.Toolbox.GameConfigs
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GameConfigKeyAttribute : PropertyAttribute
    {
        public Type Type { get; }
        
        public GameConfigKeyAttribute(Type type)
        {
            Type = type;
        }
    }
}
