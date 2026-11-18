using UnityEngine;
using Haxolot.Core;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Chef d'orchestre du joueur reliant les entrées (InputReader), le mouvement, la stamina et l'esquive.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovementTopDown), typeof(PlayerStamina))]
    [RequireComponent(typeof(PlayerDodge), typeof(InputReader))]
    public class PlayerController : MonoBehaviour
    {
        private InputReader inputReader;
        private PlayerMovementTopDown movementTopDown;
        private PlayerStamina stamina;
        private PlayerDodge dodge;

        public PlayerMovementTopDown Movement => movementTopDown;
        public PlayerStamina Stamina => stamina;
        public PlayerDodge Dodge => dodge;

        private void Awake()
        {
            inputReader = GetComponent<InputReader>();
            movementTopDown = GetComponent<PlayerMovementTopDown>();
            stamina = GetComponent<PlayerStamina>();
            dodge = GetComponent<PlayerDodge>();
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

        private void HandleMove(Vector2 input)
        {
            if (!dodge.IsDodging)
            {
                movementTopDown.SetMoveInput(input);
            }
        }

        private void HandleDash()
        {
            dodge.TryDodge(inputReader.MoveInput);
        }
    }
}
