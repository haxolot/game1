using UnityEngine;

namespace Haxolot.Characters.Player
{
    /// <summary>
    /// Interface commune pour le déplacement du joueur (TopDown ou SideScroll).
    /// </summary>
    public interface IPlayerMovement
    {
        Vector2 FacingDirection { get; }
        bool IsMoving { get; }
        void SetMoveInput(Vector2 input);
    }
}
