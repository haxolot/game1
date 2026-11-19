using System;
using UnityEngine;
using Haxolot.Combat;
using Haxolot.Core;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Contrôleur central du joueur : Gère les stats vitales (Vie, Stamina), 
    /// les événements, la réception des dégâts (avec détection de parade Sekiro) et la coordination.
    /// Implémente l'interface universelle IDamageable.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovementTopDown), typeof(InputReader))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Stats Vitales : Santé")]
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        [Header("Stats Vitales : Endurance (Stamina)")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaRegenRate = 35f;
        [SerializeField] private float staminaRegenDelay = 0.5f;
        private float currentStamina;
        private float staminaRegenTimer;

        // Références aux gros modules
        private Rigidbody2D rb;
        private InputReader inputReader;
        private PlayerMovementTopDown movement;
        private PlayerCombat combat;

        // Propriétés publiques
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float CurrentStamina => currentStamina;
        public float MaxStamina => maxStamina;
        public bool IsExhausted => currentStamina <= 0f;
        public bool IsInvulnerable => movement != null && movement.IsInvulnerable;
        public PlayerMovementTopDown Movement => movement;
        public PlayerCombat Combat => combat;

        // Événements d'UI et de Game Feel
        public event Action<float, float> OnHealthChanged;   // (current, max)
        public event Action<float, float> OnStaminaChanged;  // (current, max)

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            inputReader = GetComponent<InputReader>();
            movement = GetComponent<PlayerMovementTopDown>();
            combat = GetComponent<PlayerCombat>();

            currentHealth = maxHealth;
            currentStamina = maxStamina;
        }

        private void Start()
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        private void OnEnable()
        {
            inputReader.OnMoveEvent += HandleMove;
            inputReader.OnDashEvent += HandleDash;
        }

        private void OnDisable()
        {
            inputReader.OnMoveEvent -= HandleMove;
            inputReader.OnDashEvent -= HandleDash;
        }

        private void Update()
        {
            RegenerateStamina();
        }

        private void HandleMove(Vector2 input)
        {
            movement.SetMoveInput(input);
        }

        private void HandleDash()
        {
            movement.TryDash();
        }

        #region Gestion de l'Endurance (Stamina)
        private void RegenerateStamina()
        {
            if (staminaRegenTimer > 0f)
            {
                staminaRegenTimer -= Time.deltaTime;
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
            staminaRegenTimer = staminaRegenDelay;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            return true;
        }
        #endregion

        #region Gestion de la Santé & Dégâts (IDamageable)
        /// <summary>
        /// Réception d'un coup complet : vérifie si le joueur pare au timing parfait (Sekiro) ou s'il prend le dégât.
        /// </summary>
        public void TakeDamage(DamageData damage)
        {
            // 1. Détection de la Parade Parfaite (Sekiro Déviation)
            if (combat != null && combat.IsParrying)
            {
                combat.NotifyParrySuccess();

                // Contre-choc de posture infligé à l'attaquant qui s'est fait dévier !
                if (damage.attacker != null && damage.attacker.TryGetComponent<IDamageable>(out var attackerDamageable))
                {
                    DamageData deflectCounter = new DamageData(0f, 25f, -damage.knockbackDirection, 3f, gameObject);
                    attackerDamageable.TakeDamage(deflectCounter);
                }
                return; // Les dégâts sont totalement annulés pour le joueur !
            }

            // 2. Vérification des i-frames pendant le dash
            if (IsInvulnerable) return;

            // 3. Application des dégâts
            TakeDamage(damage.damageAmount);

            // 4. Recul physique subi
            if (rb != null && damage.knockbackForce > 0f)
            {
                rb.AddForce(damage.knockbackDirection * damage.knockbackForce, ForceMode2D.Impulse);
            }
        }

        public void TakeDamage(float amount)
        {
            if (IsInvulnerable) return;

            currentHealth = Mathf.Max(0f, currentHealth - amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void Die()
        {
            movement.StopImmediately();
            Debug.Log("<color=red>[Player] Le joueur est mort !</color>");
        }
        #endregion
    }
}
