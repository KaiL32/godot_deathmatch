using Godot;
using System;

public partial class mainMenu : Control
{
    private const int PORT = 7777;

    private void ConnectToServer(string ip)
    {
        GD.Print($"Connecting to server {ip} ...");

        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        Error err = peer.CreateClient(ip, PORT);

        Hide();

        Multiplayer.MultiplayerPeer = peer;
    }
	private void _on_button_pressed()  {
		string ip = GetNode<LineEdit>("VBoxContainer/LineEdit").Text;
        ConnectToServer(ip);
	}
}
