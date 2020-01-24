using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constant
{
    public static class Define
    {
        public const float VelocitySave = 0.3f;
    }
    public static class Enemy
    {
        //下までいって止まる位置
        public const float stopmove = -4;
        //上まで上がっていって止まる位置
        public const float startpos = 8;
        //横攻撃の止まる位置
        public const float sidestop = 12;
        //横攻撃の初期位置
        public const float sidestart = -12;
        //両手攻撃の止まる位置
        public const float purustartpos = -8;
        public const float attckstoppos = -1;
        //敵の移動スピード
        public const float attckspeed = 0.15f;        //上から下の攻撃
        public const float catchspeed = 0.15f;        //つかみ攻撃
        public const float backspeed = 0.075f;        //上に戻る速度
        public const float puruspeed = 0.3f;

        public const float redcolor = 0.55f;
    }
}
