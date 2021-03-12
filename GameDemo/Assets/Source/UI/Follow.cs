﻿using UnityEngine;

public class Follow : MonoBehaviour {

    private RectTransform rectTransform;

    private Transform target;

    //声明一个Camera 变量
    private Camera uICamera;
    // Use this for initialization
    void Start()
    {
        //获取血条的 RectTransform 组件
        rectTransform = GetComponent<RectTransform>();
        //在此为了省事我就用比较笨的方法获取UICamear了
        uICamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetPos(Vector3 pos)
    {
        rectTransform.anchoredPosition3D = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || rectTransform == null)
        {
            return;
        }

        //将角色的3D世界坐标转换为 屏幕坐标
        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(target.position);
        //将 屏幕坐标的 Y 轴加上 50， 提高一下 血条的位置
        targetScreenPosition.y += 0;

        //定义一个接收转换为 UI  2D 世界坐标的变量
        Vector3 followPosition;

        // 使用下面方法转换
        // RectTransformUtility.ScreenPointToWorldPointInRectangle（）
        // 参数1 血条的 RectTransform 组件；
        // 参数2 角色坐标转换的屏幕坐标
        // 参数3 目标摄像机，Canvas的 Render Mode 参数类型设置为 Screen Space - Camera 需要写摄像机参数
        //       将下面方法的第三个参数 写成 Canvas 参数 Render Camera 上挂的摄像机（UICamera）
        // 参数4 接收转换后的坐标，需要提前声明一个 Vector3 参数
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, targetScreenPosition, uICamera, out followPosition))
        {
            //将血条的坐标设置为 UI 2D 世界坐标
            transform.position = followPosition;
        }
    }
}