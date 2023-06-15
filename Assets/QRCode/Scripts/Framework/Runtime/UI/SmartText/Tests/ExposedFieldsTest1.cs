namespace QRCode.Framework
{
    using Singleton;
    using UnityEngine;

    public class ExposedFieldsTest1 : MonoBehaviourSingleton<ExposedFieldsTest1>
    {
        [SerializeField]  private float m_hp = 50f;
        [SerializeField]  private int m_mana = 25;
        [SerializeField]  private string m_name = "jean";

        [ExposedField("playerHP")] private static float HP2 => Instance.m_hp;
        [ExposedField("playerMana")] private static int Mana2 => Instance.m_mana;
        [ExposedField("playerName")] private static string PName2 => Instance.m_name;
    }
}
