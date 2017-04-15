#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HrUGUIShowRaycast : MonoBehaviour
{
    static Vector3[] ms_vecCornerArr = new Vector3[4];


    void OnDrawGizmos()
    {
        foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
        {
            if (g.raycastTarget)
            {
                RectTransform rectTransform = g.transform as RectTransform;
                rectTransform.GetWorldCorners(ms_vecCornerArr);

                const int nPixel = 5;
                //缩小下
                ms_vecCornerArr[0].x += nPixel; ms_vecCornerArr[0].y += nPixel;
                ms_vecCornerArr[1].x += nPixel; ms_vecCornerArr[1].y -= nPixel;
                ms_vecCornerArr[2].x -= nPixel; ms_vecCornerArr[2].y -= nPixel;
                ms_vecCornerArr[3].x -= nPixel; ms_vecCornerArr[3].y += nPixel;

                Gizmos.color = Color.blue;
                for (int i = 0; i < 4; i++)
                    Gizmos.DrawLine(ms_vecCornerArr[i], ms_vecCornerArr[(i + 1) % 4]);
            }
        }
    }
}
#endif
