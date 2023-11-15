using Chess2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess2D.Model
{
    [CreateAssetMenu(fileName = "PieceConfig", menuName = "Chess2D/Piece Config")]
    public class PieceConfig : ScriptableObject
    {
        [Header("Sprites")]
        [SerializeField] private List<PieceSpritePair> pieceSprites;

        [Space(10)]
        [Header("Colors")]
        [SerializeField] private Color whiteColor;
        [SerializeField] private Color blackColor;

        public Sprite GetSprite(PieceType type)
        {
            PieceSpritePair pair = pieceSprites.FirstOrDefault(p => p.type == type);
            return pair.sprite;
        }

        public Color GetColor(PlayerType playerType)
        {
            Color color = (playerType == PlayerType.White) ? whiteColor : blackColor;
            return color;
        }
    }

    [Serializable]
    public class PieceSpritePair
    {
        public PieceType type;
        public Sprite sprite;
    }
}