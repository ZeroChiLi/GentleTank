using GameSystem.AI;
using Item.Tank;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastAction : Action
{
    public enum AcceptType { All, Teammates, Enemy }

    public float radius = 30f;
    public AcceptType acceptType;
    [Range(0.1f, 10f)]
    public float period = 3f;
    string messages;

    public override void Act(StateController controller)
    {
        controller.statePrefs.AddValueIfNotContains("BroadcastActionCD", new CountDownTimer(period, true));

        if (!((CountDownTimer)controller.statePrefs["BroadcastActionCD"]).IsTimeUp || string.IsNullOrEmpty(messages) || AllPlayerManager.Instance == null || controller.Team == null)
            return;
        for (int i = 0; i < AllPlayerManager.Instance.Count; i++)
        {
            if (AllPlayerManager.Instance[i] != controller.playerManager
                || AllPlayerManager.Instance[i].gameObject.activeInHierarchy
                || AllPlayerManager.Instance[i].Team == controller.Team
                || Vector3.Distance(controller.transform.position, AllPlayerManager.Instance[i].transform.position) > radius)
            {
                TankManager target = AllPlayerManager.Instance[i] as TankManager;
                if (target)
                    target.stateController.statePrefs.AddValueIfNotContains("ReceiveBroadcast", messages);
            }
        }
    }
}
