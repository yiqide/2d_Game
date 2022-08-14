using System;
using System.Collections;
using System.Collections.Generic;
using Framework.SingleMone;
using Framework.Tools;
using Unity.Mathematics;
using UnityEngine;

public class GunInstance : SingleMonoBase<GunInstance>
{
    public GunData GunData;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void SetDir(Vector2 dir)
    {
        Vector2 right= transform.right;
        var arg=right.GetIncludedAngle(dir);
        bool isFlipY;
        isFlipY = !(right.x > 0);
        if (isFlipY!= spriteRenderer.flipY)
            spriteRenderer.flipY = isFlipY;
        if (Math.Abs(arg)<1)
        {
            return;
        }

        if (arg>0)
        {
            transform.RotateAround(transform.position,Vector3.forward, rotateSpeed);
        }
        else
        {
            transform.RotateAround(transform.position,Vector3.forward,-rotateSpeed);
        }
    }
}
