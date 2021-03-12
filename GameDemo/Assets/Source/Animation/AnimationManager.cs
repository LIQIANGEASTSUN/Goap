using UnityEngine;

public class AnimationManager  {

    private Animation animation;

    public AnimationManager(Transform target)
    {
        animation = target.GetComponent<Animation>();
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animName">动画名</param>
    /// <param name="fadeLength">融合时间</param>
    /// <param name="enterQueue">是否加入队列</param>
    /// <param name="isPlayNow">是否立即播放</param>
    public void CrossFade(string animName, float fadeLength = 0f, bool enterQueue = true, bool isPlayNow = true)
    {
        AnimationState animationState = GetAnimationState(animName);
        if (animationState == null)
        {
            return;
        }

        if (animation.IsPlaying(animName))
        {
            return;
        }

        if (enterQueue)
        {
            QueueMode queueMode = isPlayNow ? QueueMode.PlayNow : QueueMode.CompleteOthers;
            animation.CrossFadeQueued(animName, fadeLength, queueMode);
        }
        else
        {
            animation.CrossFade(animName, fadeLength);
        }
    }

    /// <summary>
    /// 获取动画时长
    /// </summary>
    /// <param name="animName">动画名</param>
    /// <returns></returns>
    public float GetAnimationLength(string animName)
    {
        AnimationState animationState = GetAnimationState(animName);
        if (animationState == null)
        {
            return 0;
        }
        // 返回动画时长 / 动画速度
        return animationState.length / animationState.speed;
    }

    /// <summary>
    /// 是否在播放动画
    /// </summary>
    /// <param name="animName"></param>
    /// <returns></returns>
    public bool IsPlaying(string animName)
    {
        if (animation == null)
        {
            return false;
        }

        return animation.IsPlaying(animName);
    }

    /// <summary>
    /// 根据动画名获得动画状态
    /// </summary>
    /// <param name="animName">动画名称</param>
    /// <returns></returns>
    public AnimationState GetAnimationState(string animName)
    {
        if (animation == null)
        {
            return null;
        }

        return animation[animName];
    }

    /// <summary>
    /// 获取动画状态
    /// </summary>
    /// <param name="animName">动画名</param>
    /// <returns></returns>
    public bool HasAnimationState(string animName)
    {
        if (animation == null)
        {
            return false;
        }

        return animation[animName] != null;
    }
}