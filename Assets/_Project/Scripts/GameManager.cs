using Audio;
using DG.Tweening;
using Inputs;
using Power;
using Service_Locator;
using UI;
using UnityEngine;

namespace App
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private Gameplay _gamePlay;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private AudioManager _audio;

        private IPowerTypeProvider _powerTypeProvider;
        private LevelConditionsProvider _levelConditions;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            DOTween.SetTweensCapacity(200, 200);

            _powerTypeProvider = new PowerTypeProvider();
            _levelConditions = new LevelConditionsProvider();
            _levelController.Construct();

            RegisterInstances();

            _uiManager.Construct();
            _gamePlay.Construct(_inputSystem, _levelController);


        }
        private void Start()
        {
            Init();
        }
        private void OnEnable()
        {
            _gamePlay.OnLevelFinished += OnLevelFinished;
        }
        private void OnDisable()
        {
            _gamePlay.OnLevelFinished -= OnLevelFinished;
        }
        private void Init()
        {
            _levelConditions.Init(_levelController.GetLevelData());
            _gamePlay.Init();
            _uiManager.Init();
        }
        private void DeInit()
        {
            _gamePlay.DeInit();
            _uiManager.DeInit();
            _levelConditions.DeInit();
        }
        private void RegisterInstances()
        {
            ServiceProvider.Instance
                .Register(_powerTypeProvider)
                .Register<ILevel>(_levelController)
                .Register<ILevelConditionsProvider>(_levelConditions)
                .Register<IInputSystem>(_inputSystem)
                .Register<IAudio>(_audio);
        }
        private void OnLevelFinished(bool isSuccess)
        {
            _inputSystem.SetActive(false);
            if (isSuccess)
            {
                _levelController.IncreaseLevel();
            }

            _uiManager.AlphaTransition.Transition(delay: 3f,onFaded:() =>
            {
                DeInit();
            }, onComplete: () =>
            {
                Init();
            });
        }
    }
}

