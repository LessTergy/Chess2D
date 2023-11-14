using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Chess2D.Controller
{
    public class RestartController
    {
        private readonly Button _restartButton;

        public RestartController(Button restartButton)
        {
            _restartButton = restartButton;
        }

        public void Initialize()
        {
            _restartButton.onClick.AddListener(RestartHandler);
        }

        private void RestartHandler()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}

