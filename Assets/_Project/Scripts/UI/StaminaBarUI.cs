using UnityEngine;
using UnityEngine.UI;
using Haxolot.Characters.Player;

namespace Haxolot.UI
{
    /// <summary>
    /// Contrôle l'affichage de la jauge d'endurance dans le Canvas UI.
    /// S'abonne aux événements de PlayerStamina.
    /// </summary>
    public class StaminaBarUI : MonoBehaviour
    {
        [Header("Composants UI")]
        [Tooltip("Image configurée en Image Type: Filled")]
        [SerializeField] private Image staminaFillImage;

        [Header("Référence")]
        [SerializeField] private PlayerStamina playerStamina;

        private void Start()
        {
            if (playerStamina == null)
            {
                playerStamina = FindAnyObjectByType<PlayerStamina>();
            }

            if (playerStamina != null)
            {
                playerStamina.OnStaminaChanged += UpdateStaminaBar;
                UpdateStaminaBar(playerStamina.CurrentStamina, playerStamina.MaxStamina);
            }
        }

        private void OnDestroy()
        {
            if (playerStamina != null)
            {
                playerStamina.OnStaminaChanged -= UpdateStaminaBar;
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
