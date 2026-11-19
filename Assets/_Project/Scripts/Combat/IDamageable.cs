using UnityEngine;

namespace Haxolot.Combat
{
    /// <summary>
    /// Données transmises lors d'une attaque ou d'un impact.
    /// Contient les dégâts de PV, de Posture (style Sekiro) et les informations d'éjection (knockback).
    /// </summary>
    public struct DamageData
    {
        public float damageAmount;          // Dégâts bruts sur la jauge de vie
        public float postureDamage;         // Dégâts infligés à la jauge d'équilibre / posture
        public Vector2 hitPoint;            // Point de contact de l'impact
        public Vector2 knockbackDirection;  // Direction du recul infligé
        public float knockbackForce;        // Force de l'éjection
        public GameObject attacker;         // Entité qui a porté le coup
        public bool isPerilous;             // Attaque spéciale imparable au bouclier normal

        public DamageData(float damage, float posture, Vector2 direction, float force, GameObject attacker, bool isPerilous = false)
        {
            this.damageAmount = damage;
            this.postureDamage = posture;
            this.hitPoint = Vector2.zero;
            this.knockbackDirection = direction.normalized;
            this.knockbackForce = force;
            this.attacker = attacker;
            this.isPerilous = isPerilous;
        }
    }

    /// <summary>
    /// Interface universelle implémentée par toute entité pouvant recevoir des dégâts
    /// (Joueur, Ennemis, Boss, Mannequins d'entraînement, éléments destructibles).
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Indique si l'entité est temporairement invulnérable (pendant un dash/i-frames).
        /// </summary>
        bool IsInvulnerable { get; }

        /// <summary>
        /// Applique les dégâts, le recul et la posture à la cible.
        /// </summary>
        void TakeDamage(DamageData damage);
    }
}
