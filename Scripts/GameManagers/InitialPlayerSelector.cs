using UnityEngine;

public class InitialPlayerSelector : MonoBehaviour
{
    [Tooltip("The initial character index to start with.")]
    public int InitialPlayerId;

    [Tooltip("A scriptableObject holding a list of all player characters available for switching.")]
    [SerializeField] 
    private PlayerRuntimeSet _playerRuntimeSet;

    private void Awake()
    {
        // Basic validation
        if (_playerRuntimeSet == null || _playerRuntimeSet.Items.Count == 0)
        {
            Debug.LogError("No player characters assigned to CharacterSwitcher!");
            enabled = false; // Disable script if no characters
            return;
        }

        // Ensure initial index is valid
        if (InitialPlayerId < 0 || InitialPlayerId >= _playerRuntimeSet.Count())
        {
            Debug.LogWarning($"Initial character index {InitialPlayerId} is out of bounds. Setting to 0.");
            InitialPlayerId = 0;
        }

        // Deactivate all characters initially
        foreach (var character in _playerRuntimeSet.Items)
        {
            if (character != null)
            {
                character.Deactivate(); // Ensure all are off
            }
        }

        _playerRuntimeSet.SetInitialCharacterById(InitialPlayerId);
    }

    private void OnDisable()
    {
        if (_playerRuntimeSet != null)
        {
            _playerRuntimeSet.SetActivePlayerToNull();
        }
    }
}
