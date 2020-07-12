using System;
using System.Collections.Generic;

[Serializable]
public class Trial
{
    public int trialId;
    public int maxColId;
    public int maxSz;
    public int maxRad;
    public List<Colony> colonies;
}
