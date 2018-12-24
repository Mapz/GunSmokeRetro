using BT;
using UnityEngine;

public class Enemy1AI : BTTree {
    protected override void Init () {
        base.Init ();

        BTConfiguration.ENABLE_BTACTION_LOG = true;
        BTConfiguration.ENABLE_DATABASE_LOG = true;

        _root = new BTParallelFlexible ();
        _root.interval = 1.5f;

        CheckInSight checkPlayerSight = new CheckInSight (200, "Hero");
        // PlayAnimation();
    }
}