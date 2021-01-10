using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

}

