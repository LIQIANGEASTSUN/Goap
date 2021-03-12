using UnityEngine;
using System.Collections;

public enum ANCHORSTYPE
{
	TOP_LEFT = 0,
	TOP_CENTER,
	TOP_RIGHT,

	MIDDLE_LEFT,
	MIDDLE_CENTER,
	MIDDLE_RIGHT,

	BOTTOM_LEFT,
	BOTTOM_CENTER,
	BOTTOM_RIGHT,


	TOP_STRETCH,
	MIDDLE_STRETCH,
	BOTTOM_STRETCH,


	STRETCH_LEFT,
	STRETCH_CENTER,
	STRETCH_RIGHT,

	STRETCH_STRETCH,

	NONE,
}


public class RectTransformExtend : MonoBehaviour {

	public ANCHORSTYPE anchorsType = ANCHORSTYPE.NONE;

	public bool enableUse = false;

	public float posX = 0;
	public float posY = 0;

	public float width = 0;
	public float height = 0;

	public float left = 0;
	public float right = 0;

	public float top = 0;
	public float bottom = 0;

	private RectTransform _rectTransform;

	void Start()
	{
		if (!enableUse) 
		{
			return;
		}

		_rectTransform = GetComponent<RectTransform> ();
		if (_rectTransform == null)
		{
			return;
		}

		if ((int)anchorsType >= (int)ANCHORSTYPE.TOP_LEFT && (int)anchorsType <= (int)ANCHORSTYPE.BOTTOM_RIGHT) 
		{
			RectTransformSetRect.SetLeftMiddleRight( _rectTransform, posX, posY, width, height);
		}
		else if ((int)anchorsType <= (int)ANCHORSTYPE.BOTTOM_STRETCH)
		{
			RectTransformSetRect.SetStretchBottomMiddleTop( _rectTransform, left, right, posY, height);
		}
		else if ((int)anchorsType <= (int)ANCHORSTYPE.STRETCH_RIGHT)
		{
			RectTransformSetRect.SetStretchLeftCenterRight( _rectTransform, posX, top, width, bottom);
		}
		else if ((int)anchorsType <= (int)ANCHORSTYPE.STRETCH_STRETCH)
		{
			RectTransformSetRect.SetStretchStretch( _rectTransform, left, top, right, bottom);
		}
	}
}



























