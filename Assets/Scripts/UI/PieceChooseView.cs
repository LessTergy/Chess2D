using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesstergy.Chess2D {

    public class PieceChooseView : MonoBehaviour {

        public event Action<Piece.Type> OnPieceChoose = delegate { };

        [SerializeField]
        private List<Button> buttons;

        private Animator animator;

        public bool isOpen {
            get { return animator.GetBool("isOpen"); }
            set { animator.SetBool("isOpen", value); }
        }

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void SetContent(List<PiecePrefabPrefs> piecePrefsList, Color color) {
            for (int i = 0; i < piecePrefsList.Count; i++) {
                PiecePrefabPrefs piecePrefs = piecePrefsList[i];
                Button button = buttons[i];
                Image btImage = button.GetComponent<Image>();

                btImage.sprite = piecePrefs.sprite;
                btImage.color = color;

                button.onClick.AddListener(delegate { OnButtonClick(piecePrefs.type); });
            }
        }

        private void OnButtonClick(Piece.Type pieceType) {
            OnPieceChoose(pieceType);
        }

        public void Clear() {
            foreach (var button in buttons) {
                button.onClick.RemoveAllListeners();
            }
        }
    }

}
