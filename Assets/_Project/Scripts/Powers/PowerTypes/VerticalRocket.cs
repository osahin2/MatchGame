using Item;
using UnityEngine;

namespace Power
{
    public class VerticalRocket : ItemBase
    {
        [SerializeField] private Rocket _rocket;
        public override void Interact()
        {
            Conditions.DecreaseMove();
            UseRocket();
        }

        public override void Pop()
        {
            UseRocket();
        }
        private void UseRocket()
        {
            InputSystem.SetInputActive(false);

            _rocket.UseRocket(GameBoard, GridSolver, _moveSettings, () =>
            {
                InputSystem.SetInputActive(true);
                Hide();
            });
        }
    }
}

