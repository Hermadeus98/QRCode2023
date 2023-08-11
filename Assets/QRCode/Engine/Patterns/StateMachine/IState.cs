namespace QRCode.Framework
{
    public interface IState
    {
        public string StateName { get; set; }
        public void OnStateEnter();
        public void OnStateUpdate();
        public void OnStateExit();
    }
}
