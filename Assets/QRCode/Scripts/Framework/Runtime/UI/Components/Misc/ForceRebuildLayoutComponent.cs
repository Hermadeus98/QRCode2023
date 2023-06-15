namespace QRCode.Framework
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class ForceRebuildLayoutComponent : MonoBehaviour
    {
        private void Start()
        {
            ForceRebuildLayout();
        }

        [Preserve]
        public void ForceRebuildLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
