using App;
using Service_Locator;
using UnityEngine;
namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private AlphaTransition _alphaTransition;

        [SerializeField] private InGameUIView _gameUIView;
        private InGameUIPresenter _gameUIPresenter;

        public AlphaTransition AlphaTransition => _alphaTransition;

        private ILevel _level;
        public void Construct()
        {
            _gameUIPresenter = new(_gameUIView);

            ServiceProvider.Instance.Get(out _level);
            ServiceProvider.Instance.Register<IUIGoals>(_gameUIPresenter);
        }
        public void Init()
        {
            _gameUIPresenter.Bind();
            _gameUIPresenter.Init(_level.GetLevelData());
        }
        public void DeInit()
        {
            _gameUIPresenter.DeInit();
        }
    }
}

