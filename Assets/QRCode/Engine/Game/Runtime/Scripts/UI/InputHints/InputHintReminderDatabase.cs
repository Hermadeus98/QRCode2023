namespace QRCode.Engine.Game.Inputs
{
    using Toolbox;
    using Toolbox.Database;
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Input Hint Reminder Database", fileName = "DB_InputHintReminderDatabase")]
    public class InputHintReminderDatabase : ScriptableObjectDatabase<InputHintReminderBatch>
    {
        
    }
}
