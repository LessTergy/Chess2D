using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Chess2D.Controller
{
    public class RestartController : MonoBehaviour
    {
        private Button _restartButton;

        public void Construct(Button restartButton)
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

