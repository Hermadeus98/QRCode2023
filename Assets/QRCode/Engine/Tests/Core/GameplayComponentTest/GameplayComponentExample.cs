namespace QRCode.Engine.Core.Tests
{
    using UnityEngine;

    public class GameplayComponentExample : GameplayComponent
    {
        private void Awake()
        {
            Debug.Log("-> AWAKE");
        }

        private void Start()
        {
            Debug.Log("-> START");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Debug.Log("-> ON_ENABLE");
        }

        public override void OnGameInstanceIsReady()
        {
            base.OnGameInstanceIsReady();
            Debug.Log("-> ON_ENABLE");
        }
    }
}
