using Godot;
using System;

public partial class dedicated : Node
{
	private const int PORT = 7777;
	private const int MAX_PLAYERS = 32;
	public override void _Ready()
	{
		if (OS.HasFeature("dedicated_server"))
		{
			CreateServer();
		}
	}
	
	private void CreatePlayer(long id)
	{
		Node3D player = (Node3D)GD.Load<PackedScene>("res://scenes/player.tscn").Instantiate();

		player.Name = id.ToString();

		AddChild(player);
	}

	private void RemovePlayer(long id)
	{
		GetNode(id.ToString()).QueueFree();
	}   

	private void OnPeerDisconnected(long id)
	{
		GD.Print($"Peer {id} disconnected");
		RemovePlayer(id);
	}

	private void OnPeerConnected(long id)
	{
		GD.Print($"Peer {id} connected");
		CreatePlayer(id);
	}

	private void CreateServer()
	{
		ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
		var err = peer.CreateServer(PORT, MAX_PLAYERS);
		Multiplayer.PeerConnected += OnPeerConnected;
		Multiplayer.PeerDisconnected += OnPeerDisconnected;

		Multiplayer.MultiplayerPeer = peer;
		var success = err == Error.Ok;

		GD.Print(success
			? $"Dedicated server started on port {PORT} with {MAX_PLAYERS} players"
			: "Dedicated server failed to start");
	}
}
