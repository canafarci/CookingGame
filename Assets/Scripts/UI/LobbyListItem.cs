using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;
    private CreateLobbyController _controller;
    private void Awake()
    {
        _controller = FindObjectOfType<CreateLobbyController>();
    }
    public void SetLobby(Lobby lobby)
    {
        _text.text = lobby.Name;
        _button.onClick.AddListener(() => _controller.OnLobbyButtonClicked(lobby.Id));
    }
}