using UnityEngine;
using UnityEngine.UI;

namespace SurvivalGame.UI
{
    public class PlayerActionButtonPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerActionButtonState state;
        [SerializeField] private GameObject visualRoot;
        [SerializeField] private Image icon;
        [SerializeField] private Text label;
        [SerializeField] private Sprite fallbackGatherIcon;
        [SerializeField] private Sprite fallbackAttackIcon;

        private PlayerActionButtonMode lastMode = (PlayerActionButtonMode)(-1);
        private Sprite lastIcon;

        public void Configure(
            PlayerActionButtonState actionState,
            GameObject root,
            Image iconImage,
            Text labelText,
            Sprite gatherFallback,
            Sprite attackFallback)
        {
            state = actionState;
            visualRoot = root;
            icon = iconImage;
            label = labelText;
            fallbackGatherIcon = gatherFallback;
            fallbackAttackIcon = attackFallback;
            Refresh(true);
        }

        private void Update()
        {
            Refresh(false);
        }

        public void Press()
        {
            state?.Press();
        }

        private void Refresh(bool force)
        {
            var mode = state ? state.Mode : PlayerActionButtonMode.Hidden;
            var currentIcon = state ? state.Icon : null;

            if (!force && mode == lastMode && currentIcon == lastIcon)
                return;

            lastMode = mode;
            lastIcon = currentIcon;

            if (visualRoot)
                visualRoot.SetActive(mode != PlayerActionButtonMode.Hidden);

            if (mode == PlayerActionButtonMode.Hidden)
                return;

            if (icon)
            {
                icon.sprite = currentIcon
                    ? currentIcon
                    : mode == PlayerActionButtonMode.Gather
                        ? fallbackGatherIcon
                        : fallbackAttackIcon;

                icon.enabled = icon.sprite;
            }

            if (label)
                label.text = mode == PlayerActionButtonMode.Gather ? "ДОБЫТЬ" : "АТАКА";
        }
    }
}
