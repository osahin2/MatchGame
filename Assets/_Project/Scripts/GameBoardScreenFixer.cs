using UnityEngine;

namespace App
{
    public class GameBoardScreenFixer : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private const float CONST_VALUE = (float)1920 / 1080;

        private void Awake()
        {
            var resolution = (float) Screen.height / Screen.width;
            var targetSize = resolution - CONST_VALUE;
            _camera.orthographicSize += targetSize;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var resolution = (float)Screen.height / Screen.width;
                var targetSize = resolution - CONST_VALUE;
                _camera.orthographicSize += targetSize;
            }
        }
    }
}

