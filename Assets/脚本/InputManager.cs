using System;
using System.Collections;
using System.Collections.Generic;
using Framework.SingleMone;
using UnityEngine;

public class InputManager : SingleMonoBase<InputManager>
{
    private CharacterControlManager characterControlManager;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform characterTransform;
    private GunInstance gunInstance;
    private void Start()
    {
        characterControlManager = CharacterControlManager.Instance;
        gunInstance = GunInstance.Instance;
    }

    public void Update()
    {
        if (UnityEngine.Input.GetKey(KeyCode.A))
        {
            characterControlManager.MoveLeft();
        }
        if (UnityEngine.Input.GetKey(KeyCode.D))
        {
            characterControlManager.MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterControlManager.Jump();
        }

        Vector2 dir=(camera.ScreenToWorldPoint(Input.mousePosition) - characterTransform.transform.position);
        characterControlManager.MoveDirection(dir);
        gunInstance.SetDir(dir);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(characterTransform.transform.position,camera.ScreenToWorldPoint(Input.mousePosition));
    }
#endif
}
