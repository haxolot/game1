using System;
using System.Collections;
using UnityEngine;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Gère l'esquive / dash avec frames d'invulnérabilité (i-frames) et coût en stamina.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerStamina))]
    public class PlayerDodge : MonoBehaviour
    {
        [Header("Réglages du Dash")]
        [Tooltip("Multiplicateur de vitesse pendant le dash")]
        [SerializeField] private float dodgeSpeedMultiplier = 2.5f;

        [Tooltip("Durée de la ruée")]
        [SerializeField] private float dodgeDuration = 0.2f;

        [Tooltip("Temps d'attente avant de pouvoir ré-esquiver")]
        [SerializeField] private float dodgeCooldown = 0.35f;

        [Tooltip("Consommation d'endurance")]
        [SerializeField] private float staminaCost = 25f;

        private Rigidbody2D rb;
        private PlayerStamina stamina;
        private IPlayerMovement movement;

        private bool isDodging;
        private bool isInvulnerable;
        private float cooldownTimer;

        public bool IsDodging => isDodging;
        public bool IsInvulnerable => isInvulnerable;

        public event Action OnDodgeStarted;
        public event Action OnDodgeEnded;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            stamina = GetComponent<PlayerStamina>();
            movement = GetComponent<IPlayerMovement>();
        }

        private void Update()
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }
        }

        public bool TryDodge(Vector2 currentInput)
        {
            if (isDodging || cooldownTimer > 0f)
            {
                return false;
            }

            if (!stamina.TryConsumeStamina(staminaCost))
            {
                return false;
            }

            Vector2 direction = currentInput.sqrMagnitude > 0.01f 
                ? currentInput.normalized 
                : (movement != null ? movement.FacingDirection : Vector2.down);

            StartCoroutine(DodgeRoutine(direction));
            return true;
        }

        private IEnumerator DodgeRoutine(Vector2 direction)
        {
            isDodging = true;
            isInvulnerable = true;
            cooldownTimer = dodgeCooldown;
            OnDodgeStarted?.Invoke();

            float baseSpeed = 6f;
            if (movement is PlayerMovementTopDown topDown)
            {
                baseSpeed = topDown.MoveSpeed;
            }

            Vector2 dodgeVelocity = direction * (baseSpeed * dodgeSpeedMultiplier);

            float elapsed = 0f;
            while (elapsed < dodgeDuration)
            {
                rb.linearVelocity = dodgeVelocity;
                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            isInvulnerable = false;
            isDodging = false;
            OnDodgeEnded?.Invoke();
        }
    }
}
