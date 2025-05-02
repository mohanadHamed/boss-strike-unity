using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerCharacter Player1Character { get; set; }
    public PlayerCharacter Player2Character { get; set; }

    public Dictionary<PlayerCharacter, Texture> PlayerCharacterTextureMap { get; private set; }

    [SerializeField]
    private Texture[] _playerCharacterTextures;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure this object is not destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);

        // Initialize the player character texture map
        PlayerCharacterTextureMap = new Dictionary<PlayerCharacter, Texture>
        {
            { PlayerCharacter.Blue, _playerCharacterTextures[(int) PlayerCharacter.Blue] },
            { PlayerCharacter.Green, _playerCharacterTextures[(int) PlayerCharacter.Green] },
            { PlayerCharacter.Orange, _playerCharacterTextures[(int) PlayerCharacter.Orange] },
            { PlayerCharacter.Red, _playerCharacterTextures[(int) PlayerCharacter.Red] },
            { PlayerCharacter.White, _playerCharacterTextures[(int) PlayerCharacter.White] },
            { PlayerCharacter.Yellow, _playerCharacterTextures[(int) PlayerCharacter.Yellow] }
        };

        Player1Character = PlayerCharacter.Blue; // Default character for Player 1
        Player2Character = PlayerCharacter.Green; // Default character for Player 2
    }
}
