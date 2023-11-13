using System;
using System.Collections.Generic;
using Chess2D.Model;
using UnityEngine;

namespace Chess2D.UI
{
    [RequireComponent(typeof(Animator))]
    public class PawnPromotionPopup : MonoBehaviour
    {
        public event Action<PieceType> OnPieceChoose = delegate { };

        [Header("Components")]
        [SerializeField] private PieceViewButton _buttonPrefab;
        [SerializeField] private Transform _content;
        
        private Animator _animator;
        private static readonly int AnimatorIsOpen = Animator.StringToHash("isOpen");

        private PieceConfig _pieceConfig;
        private readonly List<PieceViewButton> _buttons = new();

        public bool IsOpen
        {
            get => _animator.GetBool(AnimatorIsOpen);
            set => _animator.SetBool(AnimatorIsOpen, value);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Construct(PieceConfig pieceConfig, IEnumerable<PieceType> pieceTypes)
        {
            _pieceConfig = pieceConfig;
            CreateButtons(pieceTypes);
        }

        private void CreateButtons(IEnumerable<PieceType> pieceTypes)
        {
            foreach (PieceType type in pieceTypes)
            {
                PieceViewButton button = Instantiate(_buttonPrefab, _content);
                _buttons.Add(button);
                
                Sprite sprite = _pieceConfig.GetSprite(type);
                button.Construct(type, sprite);
                button.OnClick += PieceButton_OnClick;
            }
        }

        public void SetContent(TeamType teamType)
        {
            Color color = _pieceConfig.GetColor(teamType);
            
            foreach (PieceViewButton button in _buttons)
            {
                button.SetColor(color);
            }
        }

        private void PieceButton_OnClick(PieceType pieceType)
        {
            OnPieceChoose(pieceType);
        }
    }
}