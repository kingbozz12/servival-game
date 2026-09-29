using System.Collections.Generic;
using UnityEngine;
using SurvivalGame.Core;

namespace SurvivalGame.Character
{
    public class ModularCharacterVisual : MonoBehaviour
    {
        [Header("Base bodies")]
        [SerializeField] private GameObject maleBody;
        [SerializeField] private GameObject femaleBody;

        [Header("Customization")]
        [SerializeField] private List<GameObject> maleHair = new();
        [SerializeField] private List<GameObject> femaleHair = new();
        [SerializeField] private List<GameObject> maleBeards = new();
        [SerializeField] private List<Material> skinMaterials = new();
        [SerializeField] private List<Material> hairMaterials = new();

        [Header("Renderers using skin material")]
        [SerializeField] private List<Renderer> skinRenderers = new();

        private CharacterProfile profile;

        public void Apply(CharacterProfile newProfile)
        {
            profile = newProfile;
            if (maleBody) maleBody.SetActive(profile.gender == CharacterGender.Male);
            if (femaleBody) femaleBody.SetActive(profile.gender == CharacterGender.Female);

            ApplyHair();
            ApplyBeard();
            ApplySkin();
        }

        private void ApplyHair()
        {
            SetOnlyActive(maleHair, profile.gender == CharacterGender.Male ? profile.hairstyle : -1);
            SetOnlyActive(femaleHair, profile.gender == CharacterGender.Female ? profile.hairstyle : -1);

            if (profile.hairColor < 0 || profile.hairColor >= hairMaterials.Count) return;
            var mat = hairMaterials[profile.hairColor];

            ApplyMaterialToActive(maleHair, mat);
            ApplyMaterialToActive(femaleHair, mat);
            ApplyMaterialToActive(maleBeards, mat);
        }

        private void ApplyBeard()
        {
            for (int i = 0; i < maleBeards.Count; i++)
                if (maleBeards[i]) maleBeards[i].SetActive(profile.gender == CharacterGender.Male && profile.hasBeard && i == 0);
        }

        private void ApplySkin()
        {
            if (profile.skinTone < 0 || profile.skinTone >= skinMaterials.Count) return;
            foreach (var r in skinRenderers)
                if (r) r.material = skinMaterials[profile.skinTone];
        }

        private static void SetOnlyActive(List<GameObject> list, int index)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i]) list[i].SetActive(i == index);
        }

        private static void ApplyMaterialToActive(List<GameObject> list, Material mat)
        {
            foreach (var go in list)
            {
                if (!go || !go.activeSelf) continue;
                foreach (var r in go.GetComponentsInChildren<Renderer>(true))
                    r.material = mat;
            }
        }
    }
}
