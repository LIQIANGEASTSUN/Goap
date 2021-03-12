using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImage : MonoBehaviour {

    public string m_imagePath = string.Empty;

    private Image m_image = null;

    private bool m_isAvalid = false;

    private void Awake()
    {
        m_isAvalid = true;
        GetImage();
    }

    private void OnDestroy()
    {
        m_isAvalid = false;
    }

    public string ImageName
    {
        set
        {
            string name = string.Format("{0}/{1}", m_imagePath, value);
            AssetPool.Instance.UITexture.LoadAsset<Sprite>(name, LoadSpriteCallBack, null);
        }
    }

    private void GetImage()
    {
        if (gameObject && m_image == null)
        {
            m_image = GetComponent<Image>();
        }
    }

    private void SetImage(Sprite sprite)
    {
        GetImage();

        if (m_image != null)
        {
            m_image.overrideSprite = sprite;
        }
    }

    private void LoadSpriteCallBack(HandlerParam handlerParam)
    {
        if (handlerParam.assetObj == null || !m_isAvalid)
        {
            return;
        }

        Sprite sprite = handlerParam.assetObj as Sprite;
        SetImage(sprite);
    }
}