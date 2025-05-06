using UnityEngine;

public class FightManager : MonoBehaviour
{
    [Header("Character Prefabs")]
    // List of all possible characters. Order must match the character select screen.
    public GameObject[] characterPrefabs;

    [Header("Spawn Points")]
    // Where each player will appear in the fight scene.
    public Transform p1Spawn;
    public Transform p2Spawn;

    [Header("Player 1 Controls")]
    // Which movement keys and jump key Player 1 should use.
    public Enums.KeyGroups moveKeysP1 = Enums.KeyGroups.ArrowKeys;
    public KeyCode jumpKeyP1 = KeyCode.Return;
    public KeyCode specialKeyP1 = KeyCode.LeftShift;

    [Header("Player 2 Controls")]
    // Which movement keys and jump key Player 2 should use.
    public Enums.KeyGroups moveKeysP2 = Enums.KeyGroups.WASD;
    public KeyCode jumpKeyP2 = KeyCode.Space;
    public KeyCode specialKeyP2 = KeyCode.RightShift;

    void Start()
    {
        // Check if the character selection singleton exists
        if (SingletonCharacterSelection.Instance == null)
        {
            Debug.LogWarning("⚠️ FightManager: SingletonCharacterSelection not found. Spawning default characters.");
            SpawnDefaultCharacters(); // fallback if something goes wrong
            return;
        }

        // Get selected characters from the singleton
        int p1Index = SingletonCharacterSelection.Instance.selectedCharacterIndexP1;
        int p2Index = SingletonCharacterSelection.Instance.selectedCharacterIndexP2;

        // If Player 2 wasn't selected, pick a random one
        if (p2Index < 0)
        {
            p2Index = GetRandomCharacterIndexExcluding(p1Index);
        }

        // Create both characters in the scene
        GameObject p1 = Instantiate(characterPrefabs[p1Index], p1Spawn.position, Quaternion.identity);
        GameObject p2 = Instantiate(characterPrefabs[p2Index], p2Spawn.position, Quaternion.identity);

        // Assign control schemes for each player
        AssignPlaygroundControls(p1, moveKeysP1, jumpKeyP1, specialKeyP1);
        AssignPlaygroundControls(p2, moveKeysP2, jumpKeyP2, specialKeyP2);
    }

    // Fallback method if singleton fails: spawn the first two characters in the array
    void SpawnDefaultCharacters()
    {
        if (characterPrefabs.Length < 2)
        {
            Debug.LogError("❌ FightManager: Not enough character prefabs to spawn default characters.");
            return;
        }

        GameObject p1 = Instantiate(characterPrefabs[0], p1Spawn.position, Quaternion.identity);
        GameObject p2 = Instantiate(characterPrefabs[1], p2Spawn.position, Quaternion.identity);

        AssignPlaygroundControls(p1, moveKeysP1, jumpKeyP1, specialKeyP1);
        AssignPlaygroundControls(p2, moveKeysP2, jumpKeyP2, specialKeyP2);
    }

    // Choose a random index different from the one passed in
    int GetRandomCharacterIndexExcluding(int excludedIndex)
    {
        if (characterPrefabs.Length <= 1) return excludedIndex;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, characterPrefabs.Length);
        } while (newIndex == excludedIndex);

        return newIndex;
    }

    // Assign movement and jump controls to the character using Playground's Move and Jump scripts
    void AssignPlaygroundControls(GameObject character, Enums.KeyGroups moveKeys, KeyCode jumpKey, KeyCode specialKey)
    {
        Move move = character.GetComponent<Move>();
        if (move != null)
        {
            move.typeOfControl = moveKeys;
        }

        Jump jump = character.GetComponent<Jump>();
        if (jump != null)
        {
            jump.key = jumpKey;
        }

          Dash dash = character.GetComponent<Dash>();
      if (dash != null)
      {
        dash.typeOfControl = moveKeys; // Make sure control group matches movement
        dash.dashKey = specialKey;
        }
    }
}