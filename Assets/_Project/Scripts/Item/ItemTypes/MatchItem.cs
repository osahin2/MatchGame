using Audio;
using Utilities;

namespace Item
{
    public class MatchItem : ItemBase
    {
        public override void Interact()
        {
            var gridPos = GameBoard.WorldToGrid(transform.position);
            if (!GridUtility.IsPositionOnGrid(gridPos, GameBoard.Width, GameBoard.Height))
                return;
            if (IsMoving)
                return;

            var slot = GameBoard[gridPos];

            if (GridSolver.HasMatch(slot))
            {
                Audio.Play(ClipType.CubeExplode);

                Conditions.DecreaseMove();
                GridSolver.Solve(slot);
                return;
            }

            ShakeAnim();
        }

        public override void Pop()
        {
            Audio.Play(ClipType.CubeExplode);
            foreach (var particle in Particles)
            {
                var main = particle.main;
                main.startColor = Data.ItemColor;
                particle.Play();
            }
        }
    }
}