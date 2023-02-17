using System.Collections.Generic;
using UnityEngine;

namespace QRCode.Framework
{
    using System;
    
    public class BlackBoard : MonoBehaviour, IBlackBoard
    {
        public Dictionary<string, BlackBoardElement> BoardElements { get; set; }
    }
    
    public interface IBlackBoard
    {
        public Dictionary<string, BlackBoardElement> BoardElements { get; set; }
    }

    public interface IBlackBoardElement<out T>
    {
        public string BlackBoardElementName { get; }
        public T BlackBoardElementValue { get; }
    }
    
    [Serializable]
    public class BlackBoardElement
    {
        
    }

    [Serializable]
    public class BoolBlackBoardElement : BlackBoardElement, IBlackBoardElement<bool>
    {
        [SerializeField] private string m_blackBoardElementName;
        [SerializeField] private bool m_blackBoardElementValue;
        
        public string BlackBoardElementName { get => m_blackBoardElementName; }
        public bool BlackBoardElementValue { get => m_blackBoardElementValue; }
    }

    [Serializable]
    public class FloatBlackBoardElement : BlackBoardElement, IBlackBoardElement<float>
    {
        [SerializeField] private string m_blackBoardElementName;
        [SerializeField] private float m_blackBoardElementValue;
        
        public string BlackBoardElementName { get => m_blackBoardElementName; }
        public float BlackBoardElementValue { get => m_blackBoardElementValue; }
    }
}
