namespace QRCode.Framework
{
    using Singleton;
    using UnityEngine;

    public class ExposedFieldsTest : MonoBehaviourSingleton<ExposedFieldsTest>
    {
        [SerializeField]  private float m_hp = 50f;
        [SerializeField]  private int m_mana = 25;
        [SerializeField]  private string m_name = "jean";

        [ExposedField("playerHP")] private static float HP => Instance.m_hp;
        [ExposedField("playerMana")] private static int Mana => Instance.m_mana;
        [ExposedField("playerName")] private static string PName => Instance.m_name;
    }
}
