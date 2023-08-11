namespace QRCode.Framework
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UI;

    public class InputHintBar : MonoBehaviour
    {
        [TitleGroup(K.InspectorGroups.References)] 
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
        public async void RefreshInputHintsReminders(DB_InputHintReminderDatabaseEnum inputHintReminderBatchEnum)
        {
            if (InputHintReminderDatabase.TryGetInDatabase(inputHintReminderBatchEnum.ToString(), out var inputHintReminderBatch))
            {
                ClearInputHintBar();

                var assetReferenceInputHintReminders = inputHintReminderBatch.AssetReferenceInputHintReminders;
                foreach (var inputHintReminder in assetReferenceInputHintReminders)
                {
                    await InstantiateAssetReferenceInputHintReminder(inputHintReminder);
                }

                foreach (var inputHintReminder in m_inputHintReminders)
                {
                    inputHintReminder.gameObject.SetActive(true);
                }

                await Task.Yield();
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            }
        }

        private async Task InstantiateAssetReferenceInputHintReminder(AssetReference assetReferenceInputHintReminder)
        {
            var handle = Addressables.InstantiateAsync(assetReferenceInputHintReminder, m_container);
            var assetReferenceInputHintReminderInstance =  await handle.Task;
            handle.BindTo(assetReferenceInputHintReminderInstance);
            var inputHintReminder = assetReferenceInputHintReminderInstance.GetComponent<InputHintReminder>();
            m_inputHintReminders.Add(inputHintReminder);
            inputHintReminder.gameObject.SetActive(false);
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
