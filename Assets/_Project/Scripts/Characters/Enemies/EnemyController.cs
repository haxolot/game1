using System.Collections;
using UnityEngine;
using Haxolot.Combat;
using Haxolot.Feedback;

namespace Haxolot.Characters.Enemies
{
    /// <summary>
    /// Contrôleur d'ennemi de base (Mob de test).
    /// Poursuit le joueur, annonce ses attaques avec une télégraphie visuelle (wind-up)
    /// et permet de tester en conditions réelles la parade Sekiro et le bris de posture !
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public enum EnemyState { Idle, Chase, Windup, Attack, Recovery, Stunned, Dead }

        [Header("Stats Vitales")]
        [SerializeField] private float maxHealth = 60f;
        private float currentHealth;

        [Header("Jauge de Posture (Sekiro)")]
        [SerializeField] private float maxPosture = 50f;
        [SerializeField] private float postureRecoveryDelay = 2f;
        [SerializeField] private float postureRecoveryRate = 15f;
        private float currentPosture;
        private float postureTimer;

        [Header("Déplacement & Détection")]
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float detectionRadius = 7f;
        [SerializeField] private float attackRange = 1.3f;

        [Header("Paramètres d'Attaque")]
        [SerializeField] private float attackDamage = 15f;
        [SerializeField] private float windupDuration = 0.55f;  // Durée de télégraphie visible avant le coup
        [SerializeField] private float recoveryDuration = 0.9f; // Temps de repos après le coup
        [SerializeField] private float knockbackOnPlayer = 5f;

        [Header("Visuel & Clignotement")]
        [SerializeField] private Color normalColor = new Color(0.9f, 0.3f, 0.3f); // Rougeâtre
        [SerializeField] private Color windupColor = Color.yellow;                // Jaune / Orange alerte
        [SerializeField] private Color stunnedColor = Color.gray;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private Transform playerTransform;
        private EnemyState currentState = EnemyState.Idle;

        public bool IsInvulnerable => currentState == EnemyState.Dead;
        public EnemyState CurrentState => currentState;
        public float CurrentHealth => currentHealth;
        public float CurrentPosture => currentPosture;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = normalColor;
            }

            currentHealth = maxHealth;
            currentPosture = 0f;
        }

        private void Start()
        {
            // Recherche du joueur dans la scène
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        private void Update()
        {
            if (currentState == EnemyState.Dead || currentState == EnemyState.Stunned) return;

            // Récupération naturelle de la posture
            if (postureTimer > 0f)
            {
                postureTimer -= Time.deltaTime;
            }
            else if (currentPosture > 0f)
            {
                currentPosture = Mathf.Max(0f, currentPosture - postureRecoveryRate * Time.deltaTime);
            }

            // Machine à états simple
            switch (currentState)
            {
                case EnemyState.Idle:
                case EnemyState.Chase:
                    HandleChase();
                    break;
            }
        }

        private void HandleChase()
        {
            if (playerTransform == null) return;

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // Joueur détecté dans le rayon de vision
            if (distanceToPlayer <= detectionRadius)
            {
                // À portée d'attaque : on lance la télégraphie du coup
                if (distanceToPlayer <= attackRange)
                {
                    rb.linearVelocity = Vector2.zero;
                    StartCoroutine(AttackSequenceRoutine());
                }
                else
                {
                    // Poursuite du joueur
                    currentState = EnemyState.Chase;
                    Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
                    rb.linearVelocity = direction * moveSpeed;
                }
            }
            else
            {
                // Joueur trop loin : arrêt
                currentState = EnemyState.Idle;
                rb.linearVelocity = Vector2.zero;
            }
        }

        private IEnumerator AttackSequenceRoutine()
        {
            // 1. TÉLÉGRAPHIE (Windup) : L'ennemi s'immobilise et change de couleur pour prévenir qu'il va frapper
            currentState = EnemyState.Windup;
            if (spriteRenderer != null) spriteRenderer.color = windupColor;

            yield return new WaitForSeconds(windupDuration);

            if (currentState == EnemyState.Dead || currentState == EnemyState.Stunned) yield break;

            // 2. ATTAQUE (Impact)
            currentState = EnemyState.Attack;
            if (spriteRenderer != null) spriteRenderer.color = Color.white;

            // Vérification si le joueur est toujours à portée
            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attackRange * 1.3f)
            {
                if (playerTransform.TryGetComponent<IDamageable>(out var damageable))
                {
                    Vector2 directionToPlayer = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
                    DamageData strike = new DamageData(attackDamage, 15f, directionToPlayer, knockbackOnPlayer, gameObject);

                    damageable.TakeDamage(strike);
                }
            }

            yield return new WaitForSeconds(0.1f);

            // 3. RÉCUPÉRATION : Pause après le coup avant de pouvoir réattaquer
            currentState = EnemyState.Recovery;
            if (spriteRenderer != null) spriteRenderer.color = normalColor;

            yield return new WaitForSeconds(recoveryDuration);

            if (currentState != EnemyState.Dead && currentState != EnemyState.Stunned)
            {
                currentState = EnemyState.Idle;
            }
        }

        #region Système de Dégâts (IDamageable)
        public void TakeDamage(DamageData damage)
        {
            if (currentState == EnemyState.Dead) return;

            // 1. Dégâts de Santé
            currentHealth = Mathf.Max(0f, currentHealth - damage.damageAmount);

            // 2. Dégâts de Posture
            if (currentState != EnemyState.Stunned)
            {
                currentPosture = Mathf.Min(maxPosture, currentPosture + damage.postureDamage);
                postureTimer = postureRecoveryDelay;

                if (currentPosture >= maxPosture)
                {
                    BreakPosture();
                    return;
                }
            }

            // 3. Recul subi (Knockback)
            rb.AddForce(damage.knockbackDirection * damage.knockbackForce, ForceMode2D.Impulse);

            // 4. Clignotement visuel
            StartCoroutine(DamageFlashRoutine());

            // 5. Mort
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        private void BreakPosture()
        {
            StopAllCoroutines();
            currentState = EnemyState.Stunned;
            rb.linearVelocity = Vector2.zero;

            if (spriteRenderer != null) spriteRenderer.color = stunnedColor;

            // Gros HitStop et secousse pour célébrer le bris de posture
            HitStopManager.Instance?.TriggerHitStop(0.15f);
            CameraShake.Instance?.Shake(0.3f, 0.2f);

            Debug.Log($"<color=yellow>[Sekiro] POSTURE DU MONSTRE BRISÉE ! ({gameObject.name})</color>");
            StartCoroutine(StunnedRecoveryRoutine());
        }

        private IEnumerator StunnedRecoveryRoutine()
        {
            // Étourdi pendant 2.5 secondes
            yield return new WaitForSeconds(2.5f);

            if (currentState != EnemyState.Dead)
            {
                currentPosture = 0f;
                currentState = EnemyState.Idle;
                if (spriteRenderer != null) spriteRenderer.color = normalColor;
            }
        }

        private IEnumerator DamageFlashRoutine()
        {
            if (spriteRenderer != null && currentState != EnemyState.Stunned)
            {
                Color prev = spriteRenderer.color;
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.06f);
                spriteRenderer.color = prev;
            }
        }

        private void Die()
        {
            StopAllCoroutines();
            currentState = EnemyState.Dead;
            rb.linearVelocity = Vector2.zero;

            Debug.Log($"<color=red>[Enemy] {gameObject.name} est vaincu !</color>");

            // Désactivation des collisions et disparition après un fondu
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Destroy(gameObject, 0.5f);
        }
        #endregion

        #region Gizmos dans l'Éditeur
        private void OnDrawGizmosSelected()
        {
            // Rayon de détection (Jaune)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            // Rayon d'attaque (Rouge)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion
    }
}
