using UnityEngine;

namespace Chess2D.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class BoardView : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private BoardLayout _layout;
        public BoardLayout Layout => _layout;

        public RectTransform RectTransform { get; private set; }

        public void Awake()
        {
            RectTransform = transform as RectTransform;
        }

        public void SetInteractive(bool value)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = value;
        }
    }
}
