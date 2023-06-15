namespace QRCode.Framework
{
    using UnityEngine.InputSystem;

    public interface IInputManagementService : IService
    {
        public PlayerInput GetPlayerInput();
        public void SetActionMapEnable(string actionMap);
        public void SetActionMapDisable(string actionMap);
    }
}