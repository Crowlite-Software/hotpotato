using Sandbox;

namespace HotPotato.Cameras
{
	public class RagdollCamera : Camera
	{
		private Vector3 focusPoint;

		public override void Activated()
		{
			base.Activated();

			focusPoint = GetSpectatePoint();
			Pos = focusPoint + GetViewOffset();
			FieldOfView = CurrentView.FieldOfView;
		}

		public override void Update()
		{
			if ( Local.Pawn is not Player player ) return;

			focusPoint = GetSpectatePoint();
			Pos = focusPoint + GetViewOffset();

			var tr = Trace.Ray( focusPoint, Pos )
				.Ignore( player )
				.WorldOnly()
				.Radius( 4 )
				.Run();

			//
			// Doing a second trace at the half way point is a little trick to allow a larger camera collision radius
			// without getting initially stuck
			//
			tr = Trace.Ray( focusPoint + tr.Direction * (tr.Distance * 0.5f), tr.EndPos )
				.Ignore( player )
				.WorldOnly()
				.Radius( 8 )
				.Run();

			Pos = tr.EndPos;
			Rot = player.EyeRot;

			Viewer = null;

			ShowCorpse();
		}

		private void ShowCorpse()
		{
			if ( Local.Pawn is not Player player )
				return;

			if ( !player.Corpse.IsValid() || player.Corpse is not ModelEntity corpse )
				return;

			corpse.EnableDrawing = true;

			foreach ( var child in corpse.Children )
			{
				if ( child is ModelEntity e )
				{
					e.EnableDrawing = true;
				}
			}
		}

		public virtual Vector3 GetSpectatePoint()
		{
			if ( Local.Pawn is not Player player )
				return CurrentView.Position;

			if ( !player.Corpse.IsValid() || player.Corpse is not ModelEntity corpse )
				return player.GetBoneTransform( player.GetBoneIndex( "spine2" ) ).Position;

			return corpse.GetBoneTransform( corpse.GetBoneIndex( "spine2" ) ).Position;
		}

		public virtual Vector3 GetViewOffset()
		{
			if ( Local.Pawn is not Player player ) return Vector3.Zero;

			return player.EyeRot.Forward * -100 + Vector3.Up * 20;
		}
	}
}
