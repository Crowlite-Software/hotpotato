using Sandbox;
using Sandbox.UI;

namespace HotPotato
{
	partial class HotPotatoHud : Hud
	{
		public HotPotatoHud()
		{
			if ( !IsClient )
				return;

			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<CrosshairCanvas>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<KillFeed>();
		}
	}
}
