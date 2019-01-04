using BT;
using UnityEngine;

public class Boss1AI : BTTree {
    protected override void Init () {
        base.Init ();

        BTConfiguration.ENABLE_BTACTION_LOG = true;
        BTConfiguration.ENABLE_DATABASE_LOG = true;

        // root 是优先选择器
        _root = new BTPrioritySelector ();

        GameObject hero = GameObject.Find ("Game").GetComponent<Game> ().m_hero.gameObject;

        // 在主角后面 则后退
        DoRunBack runback = new DoRunBack (20, new CheckBehindTarget (hero));
        _root.AddChild (runback);

        // 太近了保持距离 --> 并射击
        BTParallel keepDistanceAndShot = new BTParallel (BTParallel.ParallelFunction.Or, new CheckInOrOutSight (130, hero, true)); {
            // DoKeepDistanceAround keepDistance = new DoKeepDistanceAround (hero, 140);
            keepDistanceAndShot.AddChild ( new DoRunBack (20));
            keepDistanceAndShot.AddChild (new DoAttackDoAttackEnemy1 (hero));
            _root.AddChild (keepDistanceAndShot);
        }
        // 在射程就--> 边横移 边射击
        BTParallel runAndShoot = new BTParallel (BTParallel.ParallelFunction.Or, new CheckInOrOutSight (150, hero, true)); {
            DoRunLeftRight runLeftRight = new DoRunLeftRight ();
            runAndShoot.AddChild (runLeftRight);
            runAndShoot.AddChild (new DoAttackDoAttackEnemy1 (hero));
            _root.AddChild (runAndShoot);
        }

        // 太远了跑向主角
        DoRun findPlayer = new DoRun (hero, 140, new CheckInOrOutSight (140, hero, false));
        _root.AddChild (findPlayer);

    }
}