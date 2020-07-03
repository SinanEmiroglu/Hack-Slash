using Cinemachine;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    private CinemachineTargetGroup targetGroups;

    private void Awake()
    {
        targetGroups = GetComponent<CinemachineTargetGroup>();

        var players = FindObjectsOfType<Player>();

        foreach (var player in players)
        {
            player.OnCharacterChanged += (character) => Player_OnCharacterChanged(player, character);
        }
    }

    private void Player_OnCharacterChanged(Player player, Character character)
    {
        var playerIndex = player.PlayerNumber - 1;
        targetGroups.m_Targets[playerIndex].target = character.transform;
    }
}