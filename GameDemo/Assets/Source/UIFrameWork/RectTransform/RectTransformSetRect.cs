using UnityEngine;
using System.Collections;

public class RectTransformSetRect{

    public static void SetLeftMiddleRight( RectTransform rTrans, float posX, float posY, float width, float hight)
    {
        rTrans.anchoredPosition = new Vector2(posX, posY);
        rTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (int)width);
        rTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (int)hight);

		Debug.LogError(posX + "    " + posY + "   " + width + "      " + hight);

    }

	public static void SetStretchBottomMiddleTop( RectTransform rTrans, float left, float right, float posY, float hight)
	{
		rTrans.offsetMin = new Vector2((int)left, rTrans.offsetMin.y);
		rTrans.offsetMax = new Vector2(-(int)right, rTrans.offsetMax.y);
		rTrans.anchoredPosition = new Vector2(rTrans.anchoredPosition.x, posY);
		rTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (int)hight);
	}
	
    public static void SetStretchLeftCenterRight( RectTransform rTrans, float posX, float top, float width, float bottom)
    {
        rTrans.offsetMin = new Vector2((int)rTrans.offsetMin.x, (int)bottom);
        rTrans.offsetMax = new Vector2((int)rTrans.offsetMax.y, -(int)top);
        rTrans.anchoredPosition = new Vector2(posX, rTrans.anchoredPosition.y);
        rTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (int)width);
    }

    public static void SetStretchStretch( RectTransform rTrans, float left, float top, float right, float bottom)
    {
        rTrans.offsetMin = new Vector2((int)left, (int)bottom);
        rTrans.offsetMax= new Vector2(-(int)right, -(int)top);
    }
}