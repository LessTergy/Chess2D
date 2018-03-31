using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesstergy.Chess2D {

    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour {

        public RectTransform rectT { get; private set; }

        private void Awake() {
            rectT = transform as RectTransform;
        }
    }
}
