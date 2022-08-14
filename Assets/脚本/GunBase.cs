using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase
{
    public abstract string Name{ get; internal set; }
    public abstract string ID { get; internal set; }
    /// <summary>
    /// 弹夹容量
    /// </summary>
    public abstract int Volume { get; internal set; }

    /// <summary>
    /// 剩余子弹
    /// </summary>
    public abstract int Residue { get; internal set; }

    /// <summary>
    /// 装弹时间
    /// </summary>
    public abstract float KreloadTinm { get; set; }
    public abstract void InitData();
    public abstract void Fire();
    public abstract void OnShootBullet(int bulletCount);
}
