using UnityEngine;
using UnityEngine.UI;

namespace Chess2D.UI
{
    [RequireComponent(typeof(Button))]
    public class PieceViewButton : MonoBehaviour
    {
        public delegate void PieceButtonDelegate(PieceType pieceType);
        public event PieceButtonDelegate OnClick;
        
        [Header("Components")]
        [SerializeField] private Image _icon;

        private PieceType _type;

        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Button_onClick);
        }

        public void Construct(PieceType type, Sprite sprite)
        {
            _type = type;
            _icon.sprite = sprite;
        }

        public void SetColor(Color color)
        {
            _icon.color = color;
        }

        private void Button_onClick()
        {
            OnClick?.Invoke(_type);
        }
    }
}