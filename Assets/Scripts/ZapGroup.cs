using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapGroup : Zap
{
    public float warningTime = 1f;
    public Zap[] warningZaps;
    public Zap[] mainZaps;

    public override void Awake()
    {
        base.Awake();

        foreach (Zap zap in warningZaps)
        {
            float randomNum = Random.Range(1f, 2f);
            float randomWeight = (randomNum * randomNum) / 4;
            zap.delay = delay - warningTime * randomWeight;
            zap.interval = interval;
        }
        foreach (Zap zap in mainZaps)
        {
            zap.delay = delay;
            zap.interval = interval;
        }
    }
}
