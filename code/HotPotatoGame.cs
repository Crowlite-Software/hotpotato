using Sandbox;
using System;

namespace HotPotato
{
	[Library("hotpotato")]
	partial class HotPotatoGame : Game
	{
		public HotPotatoGame()
		{
			if ( IsServer )
				new HotPotatoHud();
		}

		public override Player CreatePlayer()
		{
			return new HotPotatoPlayer();
		}
	}
}
