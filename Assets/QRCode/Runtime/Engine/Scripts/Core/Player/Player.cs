namespace QRCode.Engine.Core.Player
{
    /// <summary>
    /// A player define the entity in game played by the user.
    /// </summary>
    public class Player
    {
        private int m_playerId = 0;
        private PlayerProfile m_playerProfile = null;

        public Player(int id)
        {
            m_playerId = id;
            m_playerProfile = new PlayerProfile();
        }
    }
}
