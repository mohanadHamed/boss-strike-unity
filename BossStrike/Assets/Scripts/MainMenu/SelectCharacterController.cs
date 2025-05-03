using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _player1NameInputField;
    [SerializeField]
    private TMP_InputField _player2NameInputField;
    [SerializeField]
    private RawImage _player1Image;
    [SerializeField] 
    private RawImage _player2Image;

    [SerializeField]
    private Button _player1NextButton;
    [SerializeField]
    private Button _player1PrevButton;
    [SerializeField]
    private Button _player2NextButton;
    [SerializeField]
    private Button _player2PrevButton;

    private PlayerCharacter[] _allCharacters;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _allCharacters = (PlayerCharacter[])System.Enum.GetValues(typeof(PlayerCharacter));

        _player1NameInputField.text = SaveSystem.Load().Player1Name;
        _player2NameInputField.text = SaveSystem.Load().Player2Name;
        _player1Image.texture = GameManager.Instance.GetTextureForPlayer(PlayerNumber.PlayerOne);
        _player2Image.texture = GameManager.Instance.GetTextureForPlayer(PlayerNumber.PlayerTwo);

        _player1NameInputField.onEndEdit.AddListener(OnPlayer1NameChanged);
        _player2NameInputField.onEndEdit.AddListener(OnPlayer2NameChanged);

        _player1NextButton.onClick.AddListener(OnPlayer1NextClick);
        _player1PrevButton.onClick.AddListener(OnPlayer1PrevClick);
        _player2NextButton.onClick.AddListener(OnPlayer2NextClick);
        _player2PrevButton.onClick.AddListener(OnPlayer2PrevClick);
    }

    private void OnPlayer1NameChanged(string playerName)
    {
        SaveSystem.SavePlayer1Name(playerName);
    }

    private void OnPlayer2NameChanged(string playerName)
    {
        SaveSystem.SavePlayer2Name(playerName);
    }

    public void OnPlayer1NextClick()
    {
        int currentIndex = (int)GameManager.Instance.Player1Character;
        int nextIndex = (currentIndex + 1) % _allCharacters.Length;
        GameManager.Instance.Player1Character = _allCharacters[nextIndex];
        _player1Image.texture = GameManager.Instance.GetTextureForPlayer(PlayerNumber.PlayerOne);
    }

    public void OnPlayer2NextClick()
    {
        int currentIndex = (int)GameManager.Instance.Player2Character;
        int nextIndex = (currentIndex + 1) % _allCharacters.Length;
        GameManager.Instance.Player2Character = _allCharacters[nextIndex];
        _player2Image.texture = GameManager.Instance.GetTextureForPlayer(PlayerNumber.PlayerTwo);
    }

    public void OnPlayer1PrevClick()
    {
        int currentIndex = (int)GameManager.Instance.Player1Character;
        int prevIndex = (currentIndex - 1 + _allCharacters.Length) % _allCharacters.Length;
        GameManager.Instance.Player1Character = _allCharacters[prevIndex];
        _player1Image.texture = GameManager.Instance.GetTextureForPlayer(PlayerNumber.PlayerOne);
        }

    public void OnPlayer2PrevClick()
    {
        int currentIndex = (int)GameManager.Instance.Player2Character;
        int prevIndex = (currentIndex - 1 + _allCharacters.Length) % _allCharacters.Length;
        GameManager.Instance.Player2Character = _allCharacters[prevIndex];
        _player2Image.texture = GameManager.Instance.GetTextureForPlayer(PlayerNumber.PlayerTwo);
    }
}
