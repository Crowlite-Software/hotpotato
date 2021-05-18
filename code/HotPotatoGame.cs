using Sandbox;
using System;

namespace HotPotato
{
	[Library( "hotpotato" )]
	partial class HotPotatoGame : Game
	{
		public HotPotatoGame()
		{
			if ( IsServer )
				new HotPotatoHud();
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new HotPotatoPlayer();
			cl.Pawn = player;

			player.Respawn();
		}
	}
}
