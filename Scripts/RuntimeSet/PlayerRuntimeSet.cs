using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRuntimeSet", menuName = "Scriptable Objects/RuntimeSets/PlayerRuntimeSet")]
public class PlayerRuntimeSet : RuntimeSet<PlayerCharacter>
{
    public RuntimeObject<PlayerCharacter> ActivePlayer;

    public event Action OnCharacterSwitched;

    private Dictionary<int, PlayerCharacter> _PlayerSet = new Dictionary<int, PlayerCharacter>();

    private int _activePlayerId;

    public void SetInitialCharacterById(int initialPlayerId)
    {
        MapPlayerSet();

        _activePlayerId = initialPlayerId;
        PlayerCharacter activeCharacter = GetPlayerById(_activePlayerId);

        SetActivePlayer(activeCharacter);
    }

    private void MapPlayerSet()
    {
        foreach (PlayerCharacter player in Items)
        {
            _PlayerSet.Add(player.CharacterData.Id, player);
        }
    }

    private PlayerCharacter GetPlayerById(int id)
    {
        if (_PlayerSet.TryGetValue(id, out PlayerCharacter playerCharacter))
        {
            return playerCharacter;
        }

        return playerCharacter;
    }

    /// <summary>
    /// Sets a specific character as the active character.
    /// Deactivates the previously active character and activates the new one.
    /// </summary>
    /// <param name="newActivePlayer">The character to activate.</param>
    private void SetActivePlayer(PlayerCharacter newActivePlayer)
    {
        if (newActivePlayer == null)
        {
            Debug.LogError("Attempted to set a null character as active.");
            return;
        }

        // Deactivate current character if one exists
        if (ActivePlayer.Object != null && ActivePlayer.Object != newActivePlayer)
        {
            ActivePlayer.Object.Deactivate();
        }

        ActivePlayer.Object = newActivePlayer;
        ActivePlayer.Object.Activate(); // Activate the new character

        // Update the active index
        for (int i = 0; i < Count(); i++)
        {
            if (Items[i] == ActivePlayer)
            {
                _activePlayerId = i;
                break;
            }
        }
    }

    /// <summary>
    /// Switches to the next character in the array.
    /// Cycles back to the first character if at the end of the array.
    /// </summary>
    public void SwitchToNextCharacter()
    {
        if (Count() <= 1)
        {
            Debug.Log("Only one character available, cannot switch.");
            return;
        }

        _activePlayerId = (_activePlayerId + 1) % Count();

        SetActivePlayer(GetPlayerById(_activePlayerId));
        OnCharacterSwitched?.Invoke();
    }

    public void SetActivePlayerToNull()
    {
        ActivePlayer.Object = null;
    }
}
