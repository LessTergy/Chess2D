using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessboard : MonoBehaviour {

    [SerializeField]
    private Vector2 offset;
    private const int CELL_COUNT = 8;

    [SerializeField]
    private ChessCell cellPrefab;
    private RectTransform rectT;

    private void Awake() {
        rectT = transform as RectTransform;

        InitCells();
    }

    private void InitCells() {
        float cellWidth = (rectT.rect.width - offset.x * 2) / CELL_COUNT;
        float cellHeight = (rectT.rect.height - offset.y * 2) / CELL_COUNT;

        for (int xIndex = 0; xIndex < CELL_COUNT; xIndex++) {
            for (int yIndex = 0; yIndex < CELL_COUNT; yIndex++) {
                ChessCell cell = Instantiate(cellPrefab, transform);
                cell.transform.localScale = Vector3.one;

                Vector3 position = new Vector3(xIndex * cellWidth, yIndex * cellHeight);
                position.x -= (rectT.rect.width * rectT.pivot.x - offset.x);
                position.y -= (rectT.rect.height * rectT.pivot.y - offset.y);
                cell.transform.localPosition = position;

                RectTransform cellRectT = cell.transform as RectTransform;
                cellRectT.sizeDelta = new Vector2(cellWidth, cellHeight);
            }
        }
    }


    #region Debug
    [HideInInspector]
    [SerializeField]
    private Texture debugGridTexture;

    private void OnDrawGizmosSelected() {
        //Look at grid interactive
        RectTransform rectT = transform as RectTransform;
        Rect rect = rectT.rect;

        rect.width = (rect.width - offset.x * 2) * transform.lossyScale.x;
        rect.height = (rect.height - offset.y * 2) * transform.lossyScale.y;
        rect.position = new Vector2(rectT.position.x - rect.width * rectT.pivot.x, rectT.position.y - rect.height * rectT.pivot.y);

        Gizmos.DrawGUITexture(rect, debugGridTexture);
    }
    #endregion
}
