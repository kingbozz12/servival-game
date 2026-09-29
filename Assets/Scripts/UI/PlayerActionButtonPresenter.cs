using UnityEngine;
using UnityEngine.UI;

namespace SurvivalGame.UI
{
    public class PlayerActionButtonPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerActionButtonState state;
        [SerializeField] private GameObject visualRoot;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private Text label;

        [Header("Approved button art")]
        [SerializeField] private Sprite neutralBackground;
        [SerializeField] private Sprite gatherFallbackButton;
        [SerializeField] private Sprite attackFallbackButton;

        private PlayerActionButtonMode lastMode = (PlayerActionButtonMode)(-1);
        private Sprite lastIcon;

        public void Configure(
            PlayerActionButtonState actionState,
            GameObject root,
            Image backgroundImage,
            Image iconImage,
            Text labelText,
            Sprite neutral,
            Sprite gatherFallback,
            Sprite attackFallback)
        {
            state = actionState;
            visualRoot = root;
            background = backgroundImage;
            icon = iconImage;
            label = labelText;
            neutralBackground = neutral;
            gatherFallbackButton = gatherFallback;
            attackFallbackButton = attackFallback;
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

            if (currentIcon)
            {
                if (background && neutralBackground)
                    background.sprite = neutralBackground;

                if (icon)
                {
                    icon.gameObject.SetActive(true);
                    icon.enabled = true;
                    icon.sprite = currentIcon;
                }
            }
            else
            {
                if (background)
                {
                    background.sprite = mode == PlayerActionButtonMode.Gather
                        ? gatherFallbackButton
                        : attackFallbackButton;
                }

                if (icon)
                {
                    icon.enabled = false;
                    icon.gameObject.SetActive(false);
                }
            }

            if (label)
                label.text = mode == PlayerActionButtonMode.Gather ? "ДОБЫТЬ" : "АТАКА";
        }
    }
}
