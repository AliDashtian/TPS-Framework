using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRuntimeSet", menuName = "Scriptable Objects/RuntimeSets/PlayerRuntimeSet")]
public class PlayerRuntimeSet : RuntimeSet<PlayerCharacter>
{
    public RuntimeObject<PlayerCharacter> ActivePlayer;

    public event Action OnCharacterSwitched;

    private int _activeCharacterId;

    public void SetInitialCharacterById(int initialPlayerId)
    {
        _activeCharacterId = initialPlayerId;
        // Pick the first player that has a matching Id with selected Id and set it as ActivePlayer
        PlayerCharacter activeCharacter = Items.Where(p => p.CharacterData.Id == _activeCharacterId).ToList()[0];
        SetActivePlayer(activeCharacter);
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
                _activeCharacterId = i;
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

        _activeCharacterId = (_activeCharacterId + 1) % Count();
        SetActivePlayer(Items[_activeCharacterId]);
        OnCharacterSwitched?.Invoke();
    }

    public void SetActivePlayerToNull()
    {
        ActivePlayer.Object = null;
    }
}
