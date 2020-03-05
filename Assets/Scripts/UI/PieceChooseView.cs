using System;
using System.Collections.Generic;
using Chess2D.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Chess2D.UI
{
    [RequireComponent(typeof(Animator))]
    public class PieceChooseView : MonoBehaviour
    {

        public event Action<Piece.Type> OnPieceChoose = delegate { };

        [SerializeField]
        private List<Button> buttons;

        private Animator _animator;
        private static readonly int AnimatorIsOpen = Animator.StringToHash("isOpen");

        public bool isOpen
        {
            get => _animator.GetBool(AnimatorIsOpen);
            set => _animator.SetBool(AnimatorIsOpen, value);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetContent(List<PiecePrefabPrefs> piecePrefsList, Color color)
        {
            for (int i = 0; i < piecePrefsList.Count; i++)
            {
                PiecePrefabPrefs piecePrefs = piecePrefsList[i];
                Button button = buttons[i];
                Image btImage = button.GetComponent<Image>();

                btImage.sprite = piecePrefs.sprite;
                btImage.color = color;

                button.onClick.AddListener(delegate { OnButtonClick(piecePrefs.type); });
            }
        }

        private void OnButtonClick(Piece.Type pieceType)
        {
            OnPieceChoose(pieceType);
        }

        public void Clear()
        {
            foreach (Button button in buttons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }

}