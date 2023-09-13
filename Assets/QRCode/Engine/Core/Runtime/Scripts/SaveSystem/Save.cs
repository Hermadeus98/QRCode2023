﻿namespace QRCode.Engine.Core.SaveSystem
{
    using System.Collections.Generic;

    public class Save
    {
        private List<ISavableObject> m_savableObjects = new List<ISavableObject>();
        private SaveManager SaveManager => SaveManager.Instance;
        
        private static Save m_current = null;
        public static Save Current
        {
            get
            {
                if (m_current == null)
                {
                    m_current = new Save();
                }

                return m_current;
            }
        }

        public void Register(ISavableObject savableObject)
        {
            if (!m_savableObjects.Contains(savableObject))
            {
                m_savableObjects.Add(savableObject);
            }
        }

        public void Unregister(ISavableObject savableObject)
        {
            if (m_savableObjects.Contains(savableObject))
            {
                m_savableObjects.Remove(savableObject);
            }
        }

        public void SaveObjects()
        {
            var gameData = SaveManager.GetGameData();
            foreach (var savableObject in m_savableObjects)
            {
                savableObject.SaveGameData(ref gameData);
            }
        }
    }

    public class Load
    {
        private List<ILoadableObject> m_loadableObjects = new List<ILoadableObject>();

        private SaveManager m_saveManager => SaveManager.Instance;

        private static Load m_current = null;
        public static Load Current
        {
            get
            {
                if (m_current == null)
                {
                    m_current = new Load();
                }

                return m_current;
            }
        }

        public void Register(ILoadableObject loadableObject)
        {
            if (!m_loadableObjects.Contains(loadableObject))
            {
                m_loadableObjects.Add(loadableObject);
            }
        }

        public void Unregister(ILoadableObject loadableObject)
        {
            if (m_loadableObjects.Contains(loadableObject))
            {
                m_loadableObjects.Remove(loadableObject);
            }
        }

        public void LoadObjects()
        {
            var gameData = m_saveManager.GetGameData();

            foreach (var loadableObject in m_loadableObjects)
            {
                loadableObject.LoadGameData(gameData);
            }
        }
    }
}