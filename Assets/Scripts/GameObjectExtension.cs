using UnityEngine;
using System.Collections;

public static class GameObjectExtension {
    public static void DestroyAllChilds(this GameObject gameObject) {
        for (int i = gameObject.transform.childCount; i > 0; i--) {
            Object.Destroy(gameObject.transform.GetChild(i - 1).gameObject);
        }
    }

    public static void DestroyImmediateAllChilds(this GameObject gameObject) {
        for (int i = gameObject.transform.childCount; i > 0; i--) {
            Object.DestroyImmediate(gameObject.transform.GetChild(i - 1).gameObject);
        }
    }
}
