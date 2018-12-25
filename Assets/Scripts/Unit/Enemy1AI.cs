using BT;
using UnityEngine;

public class Enemy1AI : BTTree {
    protected override void Init () {
        base.Init ();

        BTConfiguration.ENABLE_BTACTION_LOG = true;
        BTConfiguration.ENABLE_DATABASE_LOG = true;

        // root 是选择器
        _root = new BTPrioritySelector ();
        _root.interval = 0.1f;

        GameObject hero = GameObject.Find ("Game").GetComponent<Game> ().m_hero.gameObject;

        // 如果在视野内则攻击
        CheckInSight checkPlayerSight = new CheckInSight (80, hero);
        _root.AddChild (new DoAttack (hero, checkPlayerSight));

        // 不然跑向主角
        _root.AddChild (new DoRun (hero));

    }
}