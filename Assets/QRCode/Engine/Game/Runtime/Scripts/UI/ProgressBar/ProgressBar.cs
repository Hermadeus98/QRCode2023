namespace QRCode.Engine.Game.UI.ProgressBar
{
    using System;
    using DG.Tweening;
    using Engine.Core.UI;
    using Toolbox;
    using Sirenix.OdinInspector;
    using TMPro;
    using Toolbox.Extensions;
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.UI;

    public class ProgressBar : UIElement
    {
        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField]
        protected float m_minValue = 0f;
        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField]
        protected float m_maxValue = 1f;
        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField] [PropertyRange("@this.m_minValue", "@this.m_maxValue")]
        protected float m_startValue = 1f;
        [TitleGroup(Constants.InspectorGroups.Settings)] [SerializeField]
        protected FillModeEnum m_fillMode = FillModeEnum.ResizeRect; 
        
        [ToggleGroup("m_useTween", "Use Tween")] [SerializeField]
        protected bool m_useTween = true;
        [ToggleGroup("m_useTween")]
        [SuffixLabel("@m_fillTweenParameters.Duration")][SuffixLabel("Duration = ")]
        [SuffixLabel("@m_fillTweenParameters.Ease.ToString()")][SuffixLabel("Ease = ")]
        [SerializeField] private TweenParameters m_fillTweenParameters;
        
        [ToggleGroup("m_useValueText", "Use Value Text")] [SerializeField]
        protected bool m_useValueText = true;
        [ToggleGroup("m_useValueText")] [SerializeField] [Required]
        protected TextMeshProUGUI m_valueText = null;
        [ToggleGroup("m_useValueText")] [SerializeField]
        protected LocalizedString m_valueTextPrefix = null;
        [ToggleGroup("m_useValueText")] [SerializeField]
        protected LocalizedString m_valueTextSuffix = null;
        
        [TitleGroup(Constants.InspectorGroups.References)] [SerializeField][Required]
        protected Image m_background = null;
        [TitleGroup(Constants.InspectorGroups.References)] [SerializeField][Required]
        protected Image m_fill = null;
        [TitleGroup(Constants.InspectorGroups.References)] [SerializeField] [Required]
        protected Image m_mask = null;

        [TitleGroup(Constants.InspectorGroups.Cosmetics)] [SerializeField]
        protected Gradient m_fillColorGradient = null;

        [TitleGroup(Constants.InspectorGroups.Debugging)] [ReadOnly] [SerializeField]
        [ProgressBar("@this.m_minValue", "@this.m_maxValue")] [SuffixLabel("@this.m_maxValue")] [SuffixLabel("/")] [SuffixLabel("@this.m_currentValue")]
        protected float m_currentValue = 0f;
        [TitleGroup(Constants.InspectorGroups.Debugging)] [ReadOnly] [SerializeField]
        protected float m_currentValueInPercent = 0f;
        
        protected Tween m_fillTween = null;
        
        protected override void Start()
        {
            base.Start();

            if (m_useValueText == false)
            {
                m_valueText.gameObject.SetActive(false);
            }
            
            UpdateProgressBar(m_startValue);
            UpdateCosmetics();
        }

        public override void Delete()
        {
            if (m_fillTween != null)
            {
                m_fillTween.Kill();
            }
            
            base.Delete();
        }

        [Button]
        public void UpdateProgressBar(float currentValue, bool forceDontUseTween = false)
        {
            m_currentValue = currentValue;
            CalculateCurrentValue();
            SetFill(m_currentValueInPercent, m_useTween, forceDontUseTween);
        }

        private void SetFill(float percent, bool useTween, bool forceDontUseTween = false)
        {
            var useTweenForLoading = useTween && (!forceDontUseTween);
            m_fillTween?.Kill();
            
            switch (m_fillMode)
            {
                case FillModeEnum.ResizeRect:
                    var backgroundSizeDeltaX = RectTransform.sizeDelta.x;
                    var newFillSize = Mathf.Lerp(0f, backgroundSizeDeltaX, 1 - percent);
                    if (useTweenForLoading)
                    {
                        m_fillTween = m_fill.rectTransform.DoRight(newFillSize, m_fillTweenParameters.Duration)
                            .SetDelay(m_fillTweenParameters.Delay)
                            .SetEase(m_fillTweenParameters.Ease)
                            .OnUpdate(UpdateCosmetics);
                    }
                    else
                    {
                        m_fill.rectTransform.SetRight(newFillSize);   
                    }
                    break;
                case FillModeEnum.MaskFill:
                    var newFillAmount = percent;
                    if (useTweenForLoading)
                    {
                        m_fillTween = m_mask.DOFillAmount(newFillAmount, m_fillTweenParameters.Duration)
                            .SetDelay(m_fillTweenParameters.Delay)
                            .SetEase(m_fillTweenParameters.Ease)
                            .OnUpdate(UpdateCosmetics);
                    }
                    else
                    {
                        m_mask.fillAmount = newFillAmount;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            UpdateCosmetics();
        }

        private void CalculateCurrentValue()
        {
            m_currentValue = Mathf.Clamp(m_currentValue, m_minValue, m_maxValue);
            m_currentValueInPercent = Mathf.InverseLerp(m_minValue, m_maxValue, m_currentValue);
        }

        public override void UpdateCosmetics()
        {
            base.UpdateCosmetics();
            UpdateFillColor();
            UpdateValueText();
        }

        private void UpdateFillColor()
        {
            m_fill.color = m_fillColorGradient.Evaluate(m_currentValueInPercent);
        }

        private void UpdateValueText()
        {
            var prefix = m_valueTextPrefix.IsEmpty ? "" : m_valueTextPrefix.GetLocalizedString();
            var suffix = m_valueTextSuffix.IsEmpty ? "" : m_valueTextSuffix.GetLocalizedString();
            var valueText = $"{prefix} {m_currentValue}/{m_maxValue} {suffix}";
            m_valueText.SetText(valueText);
        }
    }

    public enum FillModeEnum
    {
        ResizeRect,
        MaskFill,
    }
}
