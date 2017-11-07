using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.AI
{
    public enum CommonCode 
    {
        None,

        #region bool
        CatchEnemy,
        #endregion

        #region string
        BroadcastMessage,
        #endregion

        #region Vector3
        SoucePos,
        #endregion

        #region Transform
        ChaseEnemy,
        #endregion

        #region CountDownTimer
        BroadcastActionCD,
        FrozenActionCD,
        DelayDecisionCD,
        ScanDecisionCD
        #endregion

    }
}