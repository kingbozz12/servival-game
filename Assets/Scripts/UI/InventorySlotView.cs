using UnityEngine;
using UnityEngine.UI;
using SurvivalGame.Inventory;

namespace SurvivalGame.UI
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text amountText;
        [SerializeField] private Text fallbackName;

        public void Configure(Image iconImage, Text amount, Text fallback)
        {
            icon = iconImage;
            amountText = amount;
            fallbackName = fallback;
        }

        public void Show(ItemStack stack)
        {
            bool hasItem = stack != null && !stack.IsEmpty && stack.item;

            if (icon)
            {
                icon.enabled = hasItem && stack.item.icon;
                icon.sprite = hasItem ? stack.item.icon : null;
            }

            if (fallbackName)
            {
                fallbackName.gameObject.SetActive(hasItem && !stack.item.icon);
                fallbackName.text = hasItem ? ShortName(stack.item.displayName) : string.Empty;
            }

            if (amountText)
            {
                bool showAmount = hasItem && stack.amount > 1;
                amountText.gameObject.SetActive(showAmount);
                amountText.text = showAmount ? stack.amount.ToString() : string.Empty;
            }
        }

        private static string ShortName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "?";
            value = value.Trim();
            return value.Length <= 7 ? value : value.Substring(0, 7);
        }
    }
}
