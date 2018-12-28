using BT;
using UnityEngine;

public class EnemyWindownerAI : BTTree {
    protected override void Init () {
        base.Init ();

        BTConfiguration.ENABLE_BTACTION_LOG = true;
        BTConfiguration.ENABLE_DATABASE_LOG = true;

        // root 是优先选择器
        _root = new BTPrioritySelector ();
        // _root.interval = 0.1f;

        GameObject hero = GameObject.Find ("Game").GetComponent<Game> ().m_hero.gameObject;

        // 在射程就射击
        // _root.AddChild (new DoAttack (hero, new CheckInOrOutSight (90, hero, true)));
        _root.AddChild (new DoAttackEnemyWindowner (hero, this.GetComponent<EnemyWindowerBehavior> ().windownerDirection, new CheckInOrOutSight (100, hero, true)));

    }
}