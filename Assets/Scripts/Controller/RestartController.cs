using Chess2D.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartController : MonoBehaviour, IController {

    private Button restartButton;

    public void Construct(Button restartButton) {
        this.restartButton = restartButton;
    }

    public void Initialize() {
        restartButton.onClick.AddListener(RestartHandler);
    }

    private void RestartHandler() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

