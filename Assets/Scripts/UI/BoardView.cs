using UnityEngine;

namespace Chess2D.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class BoardView : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private BoardLayout _layout;

        public RectTransform RectTransform { get; private set; }
        public BoardLayout Layout => _layout;
        
        private CanvasGroup _canvasGroup;

        public void Awake()
        {
            RectTransform = transform as RectTransform;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetInteractive(bool value)
        {
            _canvasGroup.blocksRaycasts = value;
        }
    }
}