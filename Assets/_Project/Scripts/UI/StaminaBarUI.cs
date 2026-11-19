using UnityEngine;
using UnityEngine.UI;
using Haxolot.Characters.Player;

namespace Haxolot.UI
{
    /// <summary>
    /// Contrôle l'affichage de la jauge d'endurance dans le Canvas UI.
    /// S'abonne aux événements de PlayerController.
    /// </summary>
    public class StaminaBarUI : MonoBehaviour
    {
        [Header("Composants UI")]
        [Tooltip("Image configurée en Image Type: Filled")]
        [SerializeField] private Image staminaFillImage;

        [Header("Référence")]
        [SerializeField] private PlayerController player;

        private void Start()
        {
            if (player == null)
            {
                player = FindAnyObjectByType<PlayerController>();
            }

            if (player != null)
            {
                player.OnStaminaChanged += UpdateStaminaBar;
                UpdateStaminaBar(player.CurrentStamina, player.MaxStamina);
            }
        }

        private void OnDestroy()
        {
            if (player != null)
            {
                player.OnStaminaChanged -= UpdateStaminaBar;
            }
        }

        private void UpdateStaminaBar(float current, float max)
        {
            if (staminaFillImage != null && max > 0f)
            {
                staminaFillImage.fillAmount = Mathf.Clamp01(current / max);
            }
        }
    }
}
