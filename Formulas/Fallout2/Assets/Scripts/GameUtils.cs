using System;
using UnityEngine;
using QuickDemo;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GameUtils
{
    public static readonly int LAYERMASK_WALL = 1 << LayerMask.NameToLayer("Wall");

    public static bool CheckHitWall(Vector3 pos, Vector3 dir, out Vector3 hitPoint, out Vector3 hitWallNormal)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, dir, out hit, 1f, LAYERMASK_WALL))
        {
            hitPoint = hit.point;
            hitWallNormal = hit.normal;
            return true;
        }
        hitPoint = Vector3.zero;
        hitWallNormal = Vector3.zero;
        return false;
    }

   public const int sortingOrderDefault = 5000;

    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null, 
        Vector3 localpos = default(Vector3), float scale = 0.3f, 
        int fontSize = 40, Color? color = null, 
        TextAnchor textAnchor = TextAnchor.MiddleCenter, 
        TextAlignment textAlignment = TextAlignment.Center, 
        int sortingOrder = sortingOrderDefault) 
    {
        if (color == null) color = Color.white;
        return CreateWorldText(text, parent, localpos, scale, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    
    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent, 
        Vector3 localpos, float scale, int fontSize, 
        Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) 
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localpos;
        transform.localScale = Vector3.one * scale;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        gameObject.transform.forward = Camera.main.transform.forward;
        return textMesh;
    }

    public static Color NewColorA(Color color, float a)
    {
        var n = new Color(color.r, color.g, color.b, a);
        return n;
    }

    public static void ShowTip(string text, Vector3 wpos, float duration = 1f, float scale = 0.1f)
    {
        var tt = CreateWorldText(text, null, wpos, scale);
        tt.color = NewColorA(tt.color, 0.4f);
        // UnityEditor.EditorApplication.isPaused = true;
        tt.transform.DOLocalMoveY(tt.transform.localPosition.y + 0.5f, duration);
        DOTween.To((x)=> tt.color = NewColorA(tt.color, x), tt.color.a, 1f, duration);
        CoroutineUtil.Inst.DelayTime(duration, ()=>{
            GameObject.Destroy(tt.gameObject);
        });
    }
}