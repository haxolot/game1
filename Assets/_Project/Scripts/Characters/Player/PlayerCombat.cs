using System;
using System.Collections;
using UnityEngine;
using Haxolot.Combat;
using Haxolot.Core;
using Haxolot.Feedback;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Composant majeur : Système de combat du joueur.
    /// Gère le combo à l'épée en 3 coups (avec buffer d'input) et la parade au timing strict (style Sekiro).
    /// </summary>
    [RequireComponent(typeof(PlayerController), typeof(PlayerMovementTopDown))]
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Réglages des Attaques (Combo en 3 temps)")]
        [Tooltip("Dégâts infligés par chacun des 3 coups")]
        [SerializeField] private float[] comboDamages = { 15f, 22f, 35f };

        [Tooltip("Dégâts de posture (Sekiro) pour chacun des 3 coups")]
        [SerializeField] private float[] comboPostureDamages = { 10f, 15f, 30f };

        [Tooltip("Coût en stamina pour chaque coup du combo")]
        [SerializeField] private float[] comboStaminaCosts = { 10f, 12f, 18f };

        [Tooltip("Force d'éjection (knockback) appliquée à la cible")]
        [SerializeField] private float[] comboKnockbacks = { 2f, 3f, 6f };

        [Tooltip("Délai maximal entre deux coups avant réinitialisation du combo")]
        [SerializeField] private float comboResetDelay = 0.8f;

        [Header("Zone de Frappe (Hitbox de l'épée)")]
        [Tooltip("Distance de la hitbox devant le joueur")]
        [SerializeField] private float attackOffset = 0.9f;

        [Tooltip("Rayon de la zone de frappe")]
        [SerializeField] private float attackRadius = 0.7f;

        [Tooltip("Calque des cibles touchables (ex: Entities)")]
        [SerializeField] private LayerMask targetLayers;

        [Header("Système de Parade (Déviation Sekiro)")]
        [Tooltip("Durée de la fenêtre de parade parfaite (en secondes)")]
        [SerializeField] private float parryWindowDuration = 0.15f;

        [Tooltip("Temps de recharge entre deux tentatives de parade")]
        [SerializeField] private float parryCooldown = 0.4f;

        [Tooltip("Coût en endurance pour tenter une parade")]
        [SerializeField] private float parryStaminaCost = 8f;

        // Références aux autres composants
        private PlayerController player;
        private PlayerMovementTopDown movement;
        private InputReader inputReader;

        // États de combat
        private int currentComboIndex = 0;
        private float comboTimer = 0f;
        private bool isAttacking = false;
        private bool hasBufferedAttack = false;

        private bool isParrying = false;
        private float parryCooldownTimer = 0f;

        // Propriétés publiques
        public bool IsAttacking => isAttacking;
        public bool IsParrying => isParrying;
        public int CurrentComboIndex => currentComboIndex;

        // Événements pour le son, les animations et les feedbacks visuels
        public event Action<int> OnAttackStarted;     // Renvoie l'index du coup (0, 1, 2)
        public event Action OnAttackEnded;
        public event Action OnParryStarted;
        public event Action OnParrySuccess;
        public event Action OnParryEnded;

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            movement = GetComponent<PlayerMovementTopDown>();
            inputReader = GetComponent<InputReader>();
        }

        private void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.OnAttackEvent += HandleAttackInput;
                inputReader.OnParryEvent += HandleParryInput;
            }
        }

        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.OnAttackEvent -= HandleAttackInput;
                inputReader.OnParryEvent -= HandleParryInput;
            }
        }

        private void Update()
        {
            // Décompte de la réinitialisation du combo
            if (comboTimer > 0f)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0f && !isAttacking)
                {
                    currentComboIndex = 0;
                }
            }

            // Décompte du cooldown de parade
            if (parryCooldownTimer > 0f)
            {
                parryCooldownTimer -= Time.deltaTime;
            }
        }

        #region Logique des Attaques
        private void HandleAttackInput()
        {
            // Ne pas attaquer pendant un dash
            if (movement != null && movement.IsDashing) return;

            // Si on est déjà en plein coup, on enregistre l'input pour enchaîner sans perte de fluidité
            if (isAttacking)
            {
                hasBufferedAttack = true;
                return;
            }

            ExecuteAttack();
        }

        private void ExecuteAttack()
        {
            float staminaCost = comboStaminaCosts[currentComboIndex];

            // Vérifier et consommer l'endurance
            if (!player.TryConsumeStamina(staminaCost))
            {
                currentComboIndex = 0;
                return;
            }

            isAttacking = true;
            hasBufferedAttack = false;

            OnAttackStarted?.Invoke(currentComboIndex);

            // Calcul du point d'impact selon la direction du joueur
            Vector2 facingDir = movement != null ? movement.FacingDirection : Vector2.down;
            Vector2 attackCenter = (Vector2)transform.position + facingDir * attackOffset;

            // Détection de toutes les cibles dans la zone de frappe
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackCenter, attackRadius, targetLayers);
            bool hitSomething = false;

            foreach (var col in hitColliders)
            {
                // Ne pas se frapper soi-même
                if (col.gameObject == gameObject) continue;

                if (col.TryGetComponent<IDamageable>(out var damageable))
                {
                    DamageData damage = new DamageData(
                        comboDamages[currentComboIndex],
                        comboPostureDamages[currentComboIndex],
                        facingDir,
                        comboKnockbacks[currentComboIndex],
                        gameObject
                    );

                    damageable.TakeDamage(damage);
                    hitSomething = true;
                }
            }

            // Déclencher le micro-gel d'impact (HitStop) et la secousse d'écran si on a touché une cible
            if (hitSomething)
            {
                HitStopManager.Instance?.TriggerHitStop(0.04f + (currentComboIndex * 0.02f));
                CameraShake.Instance?.Shake(0.08f + (currentComboIndex * 0.08f), 0.1f);
            }

            // Lancer la routine de récupération du coup
            StartCoroutine(AttackRecoveryRoutine());
        }

        private IEnumerator AttackRecoveryRoutine()
        {
            // Durée de récupération entre deux coups (légèrement plus longue pour le 3ème coup lourd)
            float attackDuration = currentComboIndex == 2 ? 0.32f : 0.22f;
            yield return new WaitForSeconds(attackDuration);

            isAttacking = false;
            OnAttackEnded?.Invoke();

            // Préparer le coup suivant du combo
            currentComboIndex = (currentComboIndex + 1) % comboDamages.Length;
            comboTimer = comboResetDelay;

            // Si le joueur a cliqué pendant l'animation, on enchaîne immédiatement le coup suivant
            if (hasBufferedAttack)
            {
                ExecuteAttack();
            }
        }
        #endregion

        #region Logique de Parade (Sekiro Déviation)
        private void HandleParryInput()
        {
            if (isParrying || parryCooldownTimer > 0f) return;
            if (movement != null && movement.IsDashing) return;

            // Consommation de stamina pour la parade
            if (!player.TryConsumeStamina(parryStaminaCost)) return;

            // Animation Cancelling : parer interrompt immédiatement un coup de base !
            if (isAttacking)
            {
                StopAllCoroutines();
                isAttacking = false;
                hasBufferedAttack = false;
                OnAttackEnded?.Invoke();
            }

            StartCoroutine(ParryWindowRoutine());
        }

        private IEnumerator ParryWindowRoutine()
        {
            isParrying = true;
            parryCooldownTimer = parryCooldown;
            OnParryStarted?.Invoke();

            // Fenêtre de timing active (0.15s)
            yield return new WaitForSeconds(parryWindowDuration);

            isParrying = false;
            OnParryEnded?.Invoke();
        }

        /// <summary>
        /// Appelé par le système de dégâts adverse lorsqu'un coup frappe le joueur pendant sa fenêtre de parade.
        /// </summary>
        public void NotifyParrySuccess()
        {
            OnParrySuccess?.Invoke();

            // HitStop prolongé pour savourer le contre (0.12s) et secousse d'impact
            HitStopManager.Instance?.TriggerHitStop(0.12f);
            CameraShake.Instance?.Shake(0.25f, 0.15f);

            Debug.Log("<color=cyan>[Sekiro Parry] DÉVIATION PARFAITE ! CLANG !</color>");
        }
        #endregion

        #region Visualisation dans l'Éditeur Unity (Gizmos)
        private void OnDrawGizmosSelected()
        {
            Vector2 facingDir = movement != null ? movement.FacingDirection : Vector2.down;
            Vector2 attackCenter = (Vector2)transform.position + facingDir * attackOffset;

            // Dessine la sphère d'attaque en rouge semi-transparent
            Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
            Gizmos.DrawSphere(attackCenter, attackRadius);

            // Contour rouge vif
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackCenter, attackRadius);
        }
        #endregion
    }
}
