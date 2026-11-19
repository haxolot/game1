using System;
using System.Collections;
using UnityEngine;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Composant majeur : Gère tous les déplacements Top-Down (Marche 8 directions + Dash avec i-frames).
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementTopDown : MonoBehaviour
    {
        [Header("Course & Marche")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float acceleration = 55f;
        [SerializeField] private float deceleration = 65f;

        [Header("Dash (Esquive)")]
        [SerializeField] private float dashSpeedMultiplier = 2.5f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 0.35f;
        [SerializeField] private float dashStaminaCost = 25f;

        private Rigidbody2D rb;
        private PlayerController player;

        private Vector2 moveInput;
        private Vector2 facingDirection = Vector2.down;
        private bool isDashing;
        private bool isInvulnerable;
        private float dashCooldownTimer;

        public Vector2 FacingDirection => facingDirection;
        public bool IsMoving => moveInput.sqrMagnitude > 0.01f;
        public bool IsDashing => isDashing;
        public bool IsInvulnerable => isInvulnerable;

        public event Action OnDashStarted;
        public event Action OnDashEnded;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerController>();

            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void Update()
        {
            if (dashCooldownTimer > 0f)
            {
                dashCooldownTimer -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (isDashing) return;

            Vector2 targetVelocity = moveInput * moveSpeed;
            float rate = moveInput.sqrMagnitude > 0.01f ? acceleration : deceleration;

            // Unity 6 linearVelocity
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        }

        public void SetMoveInput(Vector2 input)
        {
            if (isDashing) return;

            moveInput = input.sqrMagnitude > 1f ? input.normalized : input;

            if (moveInput.sqrMagnitude > 0.01f)
            {
                facingDirection = moveInput.normalized;
            }
        }

        public bool TryDash()
        {
            if (isDashing || dashCooldownTimer > 0f) return false;

            // Vérifier et consommer la stamina via le contrôleur
            if (player != null && !player.TryConsumeStamina(dashStaminaCost))
            {
                return false;
            }

            Vector2 dashDir = moveInput.sqrMagnitude > 0.01f ? moveInput.normalized : facingDirection;
            StartCoroutine(DashRoutine(dashDir));
            return true;
        }

        private IEnumerator DashRoutine(Vector2 direction)
        {
            isDashing = true;
            isInvulnerable = true;
            dashCooldownTimer = dashCooldown;
            OnDashStarted?.Invoke();

            Vector2 dashVelocity = direction * (moveSpeed * dashSpeedMultiplier);
            float elapsed = 0f;

            while (elapsed < dashDuration)
            {
                rb.linearVelocity = dashVelocity;
                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            isInvulnerable = false;
            isDashing = false;
            OnDashEnded?.Invoke();
        }

        public void StopImmediately()
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
