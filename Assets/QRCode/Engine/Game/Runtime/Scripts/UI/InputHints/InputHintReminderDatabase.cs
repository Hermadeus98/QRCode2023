namespace QRCode.Engine.Game.Inputs
{
    using UnityEngine;

    using Toolbox;
    using Toolbox.Database;

    [CreateAssetMenu(menuName = Constants.DatabasePath.BasePath + "Input Hint Reminder Database", fileName = "DB_InputHintReminderDatabase")]
    public class InputHintReminderDatabase : ScriptableObjectDatabase<InputHintReminderBatch>
    {
        protected override string m_databaseInformation { get => "This database must contains all inputs hint of the game."; }
    }
}
