using UnityEngine;

namespace SurvivalGame.UI
{
    public class HudScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryScreen;
        [SerializeField] private GameObject craftingScreen;

        public void Configure(GameObject inventory, GameObject crafting)
        {
            inventoryScreen = inventory;
            craftingScreen = crafting;
            CloseAll();
        }

        public void ToggleInventory()
        {
            bool show = inventoryScreen && !inventoryScreen.activeSelf;
            CloseAll();
            if (inventoryScreen) inventoryScreen.SetActive(show);
        }

        public void ToggleCrafting()
        {
            bool show = craftingScreen && !craftingScreen.activeSelf;
            CloseAll();
            if (craftingScreen) craftingScreen.SetActive(show);
        }

        public void CloseAll()
        {
            if (inventoryScreen) inventoryScreen.SetActive(false);
            if (craftingScreen) craftingScreen.SetActive(false);
        }
    }
}
