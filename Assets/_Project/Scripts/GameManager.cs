using Inputs;
using UnityEngine;

namespace App
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Gameplay _gamePlay;
        [SerializeField] private InputSystem _inputSystem;

        private void Awake()
        {
            _gamePlay.Construct(_inputSystem);
        }
    }
}

