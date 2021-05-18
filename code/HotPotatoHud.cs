using Sandbox;
using Sandbox.UI;

namespace HotPotato
{
	partial class HotPotatoHud : HudEntity<RootPanel>
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
