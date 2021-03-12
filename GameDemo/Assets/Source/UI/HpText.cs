using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpText : MonoBehaviour {
    private Hp hp = null;
    private Text text;
    private float time = 0;
    private float releaseTime = 2;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        
	}

    private void OnEnable()
    {
        time = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup > (time + releaseTime) && (hp != null))
        {
            hp.ReleaseText(this);
        }
    }

    public void Init()
    {
        SetText(string.Empty);
    }

    public void SetHp(Hp hp)
    {
        this.hp = hp;
    }

    public void SetValue(int value)
    {
        text.gameObject.SetActive(true);
        SetText( string.Format("{0}{1}", GetSign(value), value));
    }

    private void SetText(string meg)
    {
        text.text = meg;
    }

    private string GetSign(int value)
    {
        return (value > 0) ? "-" : "+";
    }
}