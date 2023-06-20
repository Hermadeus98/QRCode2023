namespace QRCode.Framework
{
    using System.Collections.Generic;
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class InputHintBar : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.Settings)] 
        [SerializeField] private RectTransform m_container = null;

        private List<InputHintReminder> m_inputHintReminders = new List<InputHintReminder>();

        private InputHintReminderDatabase m_inputHintReminderDatabase;

        private InputHintReminderDatabase InputHintReminderDatabase
        {
            get
            {
                if (m_inputHintReminderDatabase == null)
                {
                    DB.Instance.TryGetDatabase<InputHintReminderDatabase>(DBEnum.DB_InputHintReminderDatabase, out m_inputHintReminderDatabase);
                }

                return m_inputHintReminderDatabase;
            }
        }
        
        [Button]
        public void RefreshInputHintsReminders(DB_InputHintReminderDatabaseEnum inputHintReminderBatchEnum)
        {
            if (InputHintReminderDatabase.TryGetInDatabase(inputHintReminderBatchEnum.ToString(), out var inputHintReminderBatch))
            {
                ClearInputHintBar();

                var assetReferenceInputHintReminders = inputHintReminderBatch.AssetReferenceInputHintReminders;
                foreach (var inputHintReminderReference in assetReferenceInputHintReminders)
                {
                    InstantiateAssetReferenceInputHintReminder(inputHintReminderReference);
                }
            }
        }

        private async void InstantiateAssetReferenceInputHintReminder(AssetReference assetReferenceInputHintReminder)
        {
            var handle = assetReferenceInputHintReminder.InstantiateAsync(m_container);
            var assetReferenceInputHintReminderInstance =  await handle.Task;
            handle.BindTo(assetReferenceInputHintReminderInstance);
            var inputHintReminder = assetReferenceInputHintReminderInstance.GetComponent<InputHintReminder>();
            m_inputHintReminders.Add(inputHintReminder);
        }

        private void ClearInputHintBar()
        {
            foreach (var inputHintReminder in m_inputHintReminders)
            {
                Destroy(inputHintReminder.gameObject);
            }
            
            m_inputHintReminders.Clear();
        }
    }
}
