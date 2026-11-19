using System.Collections;
using UnityEngine;

namespace Haxolot.Combat
{
    /// <summary>
    /// Mannequin d'entraînement (Training Dummy) pour tester les coups, le combo et le bris de posture Sekiro.
    /// Implémente l'interface universelle IDamageable.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class TrainingDummy : MonoBehaviour, IDamageable
    {
        [Header("Points de Vie")]
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        [Header("Jauge de Posture (Sekiro / Nine Sols)")]
        [SerializeField] private float maxPosture = 80f;
        [SerializeField] private float postureRecoveryDelay = 2.0f;
        [SerializeField] private float postureRecoveryRate = 25f;
        private float currentPosture;
        private float postureTimer;
        private bool isPostureBroken;

        [Header("Feedback Visuel (Hit Flash)")]
        [SerializeField] private Color hitColor = Color.red;
        [SerializeField] private float flashDuration = 0.08f;

        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private Rigidbody2D rb;

        public bool IsInvulnerable => false;
        public float CurrentHealth => currentHealth;
        public float CurrentPosture => currentPosture;
        public bool IsPostureBroken => isPostureBroken;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            rb = GetComponent<Rigidbody2D>();

            currentHealth = maxHealth;
            currentPosture = 0f;
        }

        private void Update()
        {
            // Récupération naturelle de la posture avec le temps (comme dans Sekiro)
            if (postureTimer > 0f)
            {
                postureTimer -= Time.deltaTime;
            }
            else if (currentPosture > 0f && !isPostureBroken)
            {
                currentPosture = Mathf.Max(0f, currentPosture - postureRecoveryRate * Time.deltaTime);
            }
        }

        public void TakeDamage(DamageData damage)
        {
            // 1. Dégâts de Santé
            currentHealth = Mathf.Max(0f, currentHealth - damage.damageAmount);

            // 2. Dégâts de Posture
            if (!isPostureBroken)
            {
                currentPosture = Mathf.Min(maxPosture, currentPosture + damage.postureDamage);
                postureTimer = postureRecoveryDelay;

                if (currentPosture >= maxPosture)
                {
                    BreakPosture();
                }
            }

            // 3. Recul physique (Knockback)
            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                rb.AddForce(damage.knockbackDirection * damage.knockbackForce, ForceMode2D.Impulse);
            }

            // 4. Clignotement de couleur (Hit Flash)
            StartCoroutine(FlashRoutine());

            // 5. Affichage dans la console
            Debug.Log($"<color=orange>[Dummy Touche]</color> Dégâts: <b>{damage.damageAmount}</b> | Posture: <b>{currentPosture}/{maxPosture}</b> | PV: <b>{currentHealth}/{maxHealth}</b>");

            if (currentHealth <= 0f)
            {
                ResetDummy();
            }
        }

        private void BreakPosture()
        {
            isPostureBroken = true;
            Debug.Log("<color=yellow>[POSTURE BRISÉE !] Le mannequin est étourdi ! Ouverture pour un coup fatal !</color>");
            StartCoroutine(PostureBrokenStunRoutine());
        }

        private IEnumerator PostureBrokenStunRoutine()
        {
            // Couleur jaune étourdi pendant 3 secondes
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(3.0f);

            // Récupération
            isPostureBroken = false;
            currentPosture = 0f;
            spriteRenderer.color = originalColor;
            Debug.Log("<color=green>[Dummy] Posture régénérée.</color>");
        }

        private IEnumerator FlashRoutine()
        {
            if (!isPostureBroken)
            {
                spriteRenderer.color = hitColor;
                yield return new WaitForSeconds(flashDuration);
                spriteRenderer.color = originalColor;
            }
        }

        private void ResetDummy()
        {
            currentHealth = maxHealth;
            currentPosture = 0f;
            isPostureBroken = false;
            spriteRenderer.color = originalColor;
            Debug.Log("<color=white>[Dummy] Réinitialisation complète du mannequin.</color>");
        }
    }
}
