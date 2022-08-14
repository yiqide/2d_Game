using System;
using System.Collections;
using System.Collections.Generic;
using Framework.SingleMone;
using Framework.Tools;
using UnityEngine;

public class CharacterControlManager : SingleMonoBase<CharacterControlManager>
{
    public float moveSpeed=1;
    public bool isTowJump=false;
    public float jumpHigh=1;
    [Header("判定区域")] 
    [SerializeField] private Vector2 frontAreaUpPoint;
    [SerializeField] private Vector2 frontAreaDowPoint;
    [SerializeField] private Vector2 rearAreaUpPoint;
    [SerializeField] private Vector2 rearAreaDowPoint;
    [SerializeField] private Vector2 groundArea;
    [SerializeField] private Vector2 groundOffset;
    [Header("组件")]
    [SerializeField] private Transform  characterTransform;
    [SerializeField] private Rigidbody2D characterRigidbody2D;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 frontSize;
    private Vector2 rearSize;
    private void Start()
    {
        frontSize = new Vector2(frontAreaUpPoint.x - frontAreaDowPoint.x, frontAreaUpPoint.y - frontAreaDowPoint.y);
        rearSize = new Vector2(rearAreaUpPoint.x - rearAreaDowPoint.x, rearAreaUpPoint.y - rearAreaDowPoint.y);
        Debug.Log(frontSize);
        Debug.Log(rearSize);
    }

    public void MoveRight()
    {
        if (frontBarrier) return;
        characterTransform.localPosition += Vector3.right*moveSpeed*Time.deltaTime;
    }
    public void MoveLeft()
    {
        if (rearBarrier) return;
        characterTransform.localPosition += Vector3.left*moveSpeed*Time.deltaTime;
    }

    private bool isStartCoroutine = false;
    public void Jump()
    {
        if (isStartCoroutine)return;
        if (isTowJump)
        {
            if (jumpCount>=2)return;
        }
        else
        {
            if (jumpCount>=1) return;
        }
        isStartCoroutine = true;
        if (jumpCount>=1)
        {
            float r = 1;
            float y=characterRigidbody2D.velocity.y;
            if (y>5)
            {
                r = 0.2f;
            }else if (y>=0)
            {
                r=0.2f+0.8f * (5 - y) / 5;
            }
            Debug.Log(r);
            characterRigidbody2D.AddForce(Vector2.up*jumpHigh*r, ForceMode2D.Impulse);
        }
        else
        {
            characterRigidbody2D.AddForce(Vector2.up*jumpHigh, ForceMode2D.Impulse);
        }

        CoroutineTools.Instance.StartCoroutine(JumpCountAdd());
        jumpCount++;
    }

    public void MoveDirection(Vector2 dir)
    {
        bool isFalse = false;
        if (dir.x>0) isFalse = false;
        else isFalse = true;
        if (isFalse!=spriteRenderer.flipX)
            spriteRenderer.flipX = isFalse;
    }

    public void Fire()
    {
        
    }


    private bool frontBarrier = false;
    private bool rearBarrier = false;
    private bool lastGround = false;
    private int jumpCount = 0;
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        frontBarrier = false;
        rearBarrier = false;
        bool isGround = false;
        var originFront=characterTransform.position +frontAreaDowPoint.ToVector3()+frontSize.ToVector3()/2;
        var originRear=characterTransform.position +rearAreaUpPoint.ToVector3()-rearSize.ToVector3()/2;
        
        var raycastFrontHit2Ds =Physics2D.BoxCastAll(originFront, frontSize,0, Vector2.zero);
        foreach (var item in raycastFrontHit2Ds)
        {
            if (item.rigidbody== characterRigidbody2D)
            {
                continue;
            }
            frontBarrier = true;
        }
        
        var raycastRearHit2Ds = Physics2D.BoxCastAll(originRear, rearSize, 0, Vector2.zero);
        foreach (var item in raycastRearHit2Ds)
        {
            if (item.rigidbody== characterRigidbody2D)
            {
                continue;
            }
            rearBarrier = true;
            
        }
        var raycastGroundHit2Ds = Physics2D.BoxCastAll(characterTransform.position+groundOffset.ToVector3(), groundArea, 0, Vector2.zero);
        
        foreach (var item in raycastGroundHit2Ds)
        {
            if (item.rigidbody== characterRigidbody2D)
            {
                continue;
            }
            isGround = true;
        }

        if (isGround!=lastGround)
        {
            isChangeGround?.Invoke(isGround);
        }
        
        lastGround = isGround;
    }

    private Action<bool> isChangeGround;
    private IEnumerator JumpCountAdd()
    {
        isChangeGround = null;
        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }

        isChangeGround += (b) =>
        {
            if (b)
            {
                jumpCount = 0;
            }
        };
        isStartCoroutine=false;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(characterTransform.localPosition+new Vector3(frontAreaDowPoint.x,frontAreaUpPoint.y,0),characterTransform.localPosition+frontAreaUpPoint.ToVector3());
        Gizmos.DrawLine(characterTransform.localPosition+frontAreaDowPoint.ToVector3(),characterTransform.localPosition+new Vector3(frontAreaDowPoint.x,frontAreaUpPoint.y,0));
        Gizmos.DrawLine(characterTransform.localPosition+frontAreaDowPoint.ToVector3(),characterTransform.localPosition+new Vector3(frontAreaUpPoint.x,frontAreaDowPoint.y));
        Gizmos.DrawLine(characterTransform.localPosition+new Vector3(frontAreaUpPoint.x,frontAreaDowPoint.y),characterTransform.localPosition+frontAreaUpPoint.ToVector3());
        Gizmos.color=Color.yellow;
        Gizmos.DrawLine(characterTransform.localPosition+new Vector3(rearAreaDowPoint.x,rearAreaUpPoint.y,0),characterTransform.localPosition+rearAreaUpPoint.ToVector3());
        Gizmos.DrawLine(characterTransform.localPosition+rearAreaDowPoint.ToVector3(),characterTransform.localPosition+new Vector3(rearAreaDowPoint.x,rearAreaUpPoint.y,0));
        Gizmos.DrawLine(characterTransform.localPosition+rearAreaDowPoint.ToVector3(),characterTransform.localPosition+new Vector3(rearAreaUpPoint.x,rearAreaDowPoint.y));
        Gizmos.DrawLine(characterTransform.localPosition+new Vector3(rearAreaUpPoint.x,rearAreaDowPoint.y),characterTransform.localPosition+rearAreaUpPoint.ToVector3());
        Gizmos.color=Color.green;
        var characterPos =characterTransform.localPosition+groundOffset.ToVector3();
        Gizmos.DrawLine(characterPos+groundArea.ToVector3()/2,characterPos+new Vector3(groundArea.x,-groundArea.y,0)/2);
        Gizmos.DrawLine(characterPos+groundArea.ToVector3()/2,characterPos+new Vector3(-groundArea.x,groundArea.y,0)/2);
        Gizmos.DrawLine(characterPos+new Vector3(groundArea.x,-groundArea.y,0)/2,characterPos+new Vector3(-groundArea.x,-groundArea.y,0)/2);
        Gizmos.DrawLine(characterPos+new Vector3(-groundArea.x,groundArea.y,0)/2,characterPos+new Vector3(-groundArea.x,-groundArea.y,0)/2);
    }
#endif
}
