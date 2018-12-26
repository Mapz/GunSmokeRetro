using BT;
using UnityEngine;

public class Enemy1AI : BTTree {
    protected override void Init () {
        base.Init ();

        BTConfiguration.ENABLE_BTACTION_LOG = true;
        BTConfiguration.ENABLE_DATABASE_LOG = true;

        // root 是优先选择器
        _root = new BTPrioritySelector ();
        // _root.interval = 0.1f;

        GameObject hero = GameObject.Find ("Game").GetComponent<Game> ().m_hero.gameObject;

        // 太近了就后退
        DoKeepDistanceAround keepDistance = new DoKeepDistanceAround (hero, 80, new CheckInOrOutSight (70, hero, true));
        _root.AddChild (keepDistance);
        // 在射程就射击
        _root.AddChild (new DoAttack (hero, new CheckInOrOutSight (90, hero, true)));
        // 太远了跑向主角
        DoRun findPlayer = new DoRun (hero, 80, new CheckInOrOutSight (80, hero, false));
        // findPlayer.interval = 0.1f;
        _root.AddChild (findPlayer);

    }
}