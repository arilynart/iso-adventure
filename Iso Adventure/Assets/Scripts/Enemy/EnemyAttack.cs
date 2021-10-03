using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemyAttack
{
    /*    string Name
        {
            get;
            set;
        }
        string AnimationName
        {
            get;
            set;
        }

        int Damage
        {
            get;
            set;
        }
        float BoxStart
        {
            get;
            set;
        }
        float BoxEnd
        {
            get;
            set;
        }

        int NextAttack
        {
            get;
            set;
        }*/

    void InitializeAttack(string name, string anim, int dmg, float start, float end, int next);

    void ExecuteAttack();

    void ChooseNextAttack();
}
