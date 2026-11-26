using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Chess2D.Controller
{
    public class RestartController : IDisposable
    {
        private readonly Button.ButtonClickedEvent _onClick;

        public RestartController(Button restartButton)
        {
            _onClick = restartButton.onClick;
        }

        public void Initialize()
        {
            _onClick.AddListener(RestartHandler);
        }

        public void Dispose()
        {
            _onClick.RemoveListener(RestartHandler);
        }

        private void RestartHandler()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}

