using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerCharacter Player1Character { get; set; }
    public PlayerCharacter Player2Character { get; set; }

    [SerializeField]
    private Texture[] _playerCharacterTextures;

    [SerializeField]
    private Material[] _playerCharacterMaterials;


    private Dictionary<PlayerCharacter, Texture> _playerCharacterTextureMap;

    private Dictionary<PlayerCharacter, Material> _playerCharacterMaterialMap;

    private AudioSource _bgmAudioSource;

    public Texture GetTextureForPlayer(PlayerNumber playerNumber)
    {
        return playerNumber == PlayerNumber.PlayerOne
            ? _playerCharacterTextureMap[Player1Character]
            : _playerCharacterTextureMap[Player2Character];
    }

    public Material GetMaterialForPlayer(PlayerNumber playerNumber)
    {
        return playerNumber == PlayerNumber.PlayerOne
            ? _playerCharacterMaterialMap[Player1Character]
            : _playerCharacterMaterialMap[Player2Character];
    }

    public void UpdateBgmVolume()
    {
        _bgmAudioSource.volume = SaveSystem.Load().SoundMusicEnabled ? 0.5f : 0;
    }

    private void Awake()
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

        Initialize();
    }

    private void Initialize()
    {
        // Initialize the player character texture map
        _playerCharacterTextureMap = new Dictionary<PlayerCharacter, Texture>
        {
            { PlayerCharacter.Blue, _playerCharacterTextures[(int) PlayerCharacter.Blue] },
            { PlayerCharacter.Green, _playerCharacterTextures[(int) PlayerCharacter.Green] },
            { PlayerCharacter.Orange, _playerCharacterTextures[(int) PlayerCharacter.Orange] },
            { PlayerCharacter.Red, _playerCharacterTextures[(int) PlayerCharacter.Red] },
            { PlayerCharacter.White, _playerCharacterTextures[(int) PlayerCharacter.White] },
            { PlayerCharacter.Yellow, _playerCharacterTextures[(int) PlayerCharacter.Yellow] }
        };

        _playerCharacterMaterialMap = new Dictionary<PlayerCharacter, Material>
        {
            { PlayerCharacter.Blue, _playerCharacterMaterials[(int) PlayerCharacter.Blue] },
            { PlayerCharacter.Green, _playerCharacterMaterials[(int) PlayerCharacter.Green] },
            { PlayerCharacter.Orange, _playerCharacterMaterials[(int) PlayerCharacter.Orange] },
            { PlayerCharacter.Red, _playerCharacterMaterials[(int) PlayerCharacter.Red] },
            { PlayerCharacter.White, _playerCharacterMaterials[(int) PlayerCharacter.White] },
            { PlayerCharacter.Yellow, _playerCharacterMaterials[(int) PlayerCharacter.Yellow] }
        };

        Player1Character = PlayerCharacter.Blue; // Default character for Player 1
        Player2Character = PlayerCharacter.Yellow; // Default character for Player 2

        _bgmAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Play background music
        if (_bgmAudioSource != null)
        {
            _bgmAudioSource.loop = true;
            UpdateBgmVolume();
            _bgmAudioSource.Play();
        }
    }
}
