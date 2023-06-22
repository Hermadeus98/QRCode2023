namespace QRCode.Framework
{
    using UnityEngine.InputSystem;

    public interface IInputManagementService : IService
    {
        public PlayerInput GetPlayerInput();
        public void SetActionMapEnable(string actionMap);
        public void SetActionMapDisable(string actionMap);
        public void SetActionMapGroupEnable(DB_InputMapGroupEnum inputMapGroup);
        public void SetActionMapGroupDisable(DB_InputMapGroupEnum inputMapGroup);
    }
}