using System;
using UnityEngine;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Gestion de l'endurance du joueur (coût par dash/attaque, recharge avec délai).
    /// </summary>
    public class PlayerStamina : MonoBehaviour
    {
        [Header("Paramètres d'Endurance")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaRegenRate = 35f;
        [SerializeField] private float regenDelay = 0.5f;

        private float currentStamina;
        private float regenTimer;

        public float CurrentStamina => currentStamina;
        public float MaxStamina => maxStamina;
        public bool IsExhausted => currentStamina <= 0f;

        /// <summary>
        /// Événement diffusant (currentStamina, maxStamina) pour mettre à jour l'UI.
        /// </summary>
        public event Action<float, float> OnStaminaChanged;

        private void Awake()
        {
            currentStamina = maxStamina;
        }

        private void Start()
        {
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        private void Update()
        {
            if (regenTimer > 0f)
            {
                regenTimer -= Time.deltaTime;
                return;
            }

            if (currentStamina < maxStamina)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.deltaTime);
                OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            }
        }

        public bool TryConsumeStamina(float amount)
        {
            if (currentStamina < amount)
            {
                return false;
            }

            currentStamina = Mathf.Max(0f, currentStamina - amount);
            regenTimer = regenDelay;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            return true;
        }

        public void RestoreFullStamina()
        {
            currentStamina = maxStamina;
            regenTimer = 0f;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }
    }
}
