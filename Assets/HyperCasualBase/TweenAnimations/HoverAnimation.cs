using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum HoverDirection { Vertical, Horizontal }

public class HoverAnimation : MonoBehaviour
{
    public HoverDirection HoverDirection = HoverDirection.Vertical;
    public Ease Ease = Ease.InOutExpo;
    public float Duration = 2f;
    public float HoverDistance = -1f;

    private void Start()
    {
        switch (HoverDirection)
        {
            case HoverDirection.Vertical:
                transform.DOLocalMoveY(transform.localPosition.y + HoverDistance, Duration).SetEase(Ease).SetLoops(-1, LoopType.Yoyo);
                break;
            case HoverDirection.Horizontal:
                transform.DOLocalMoveX(transform.localPosition.x + HoverDistance, Duration).SetEase(Ease).SetLoops(-1, LoopType.Yoyo);
                break;
            default:
                transform.DOLocalMoveY(transform.localPosition.y + HoverDistance, Duration).SetEase(Ease).SetLoops(-1, LoopType.Yoyo);
                break;
        }
    }
}
