using UnityEngine;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Contrôleur de déplacement 8 directions pour la phase d'exploration Top-Down.
    /// Utilise la physique 2D Unity 6 (linearVelocity) avec accélération et décélération réactives.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementTopDown : MonoBehaviour, IPlayerMovement
    {
        [Header("Réglages de Vitesse")]
        [Tooltip("Vitesse maximale de déplacement")]
        [SerializeField] private float moveSpeed = 6f;

        [Tooltip("Vitesse pour atteindre la vitesse max")]
        [SerializeField] private float acceleration = 55f;

        [Tooltip("Vitesse d'arrêt sans glissement")]
        [SerializeField] private float deceleration = 65f;

        private Rigidbody2D rb;
        private Vector2 moveInput;
        private Vector2 facingDirection = Vector2.down;

        public Vector2 FacingDirection => facingDirection;
        public bool IsMoving => moveInput.sqrMagnitude > 0.01f;
        public float MoveSpeed => moveSpeed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void SetMoveInput(Vector2 input)
        {
            moveInput = input.sqrMagnitude > 1f ? input.normalized : input;

            if (moveInput.sqrMagnitude > 0.01f)
            {
                facingDirection = moveInput.normalized;
            }
        }

        private void FixedUpdate()
        {
            Vector2 targetVelocity = moveInput * moveSpeed;
            float rate = moveInput.sqrMagnitude > 0.01f ? acceleration : deceleration;

            // En Unity 6, linearVelocity remplace velocity pour le Rigidbody2D
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        }

        public void StopImmediately()
        {
            moveInput = Vector2.zero;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
