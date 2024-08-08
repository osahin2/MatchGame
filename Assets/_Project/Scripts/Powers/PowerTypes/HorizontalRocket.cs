using Item;
using UnityEngine;

namespace Power
{
    public class HorizontalRocket : ItemBase
    {
        [SerializeField] private Rocket _rocket;

        public override void Interact()
        {
            Conditions.DecreaseMove();
            InputSystem.SetInputActive(false);
            _rocket.UseRocket(GameBoard, GridSolver, _moveSettings, () =>
            {
                InputSystem.SetInputActive(true);
                Hide();
            });
        }

        public override void Pop()
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

