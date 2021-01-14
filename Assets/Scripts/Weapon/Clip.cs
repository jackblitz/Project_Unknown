using System;
using UnityEngine;

[Serializable]
public class Clip
{
    /// <summary>
    /// How bullets can our bullet hold
    /// </summary>
    public int ClipSize = 30;

    /// <summary>
    /// How many bullets are left in clip
    /// </summary>
    public int BulletsLeft = 30;

    /// <summary>
    /// How many bullets can be fired in burst mode
    /// </summary>
    public int BurstRate = 15;

    /// <summary>
    /// How many bullets have been fired in burst round
    /// </summary>
    public int BurstCount = 0;

    /// <summary>
    /// Reference to Guns Magazine model
    /// </summary>
    public GameObject Magazine;
}

