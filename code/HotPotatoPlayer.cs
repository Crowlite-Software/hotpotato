using HotPotato.Cameras;
using HotPotato.Entities;
using Sandbox;
using System;

namespace HotPotato
{
	partial class HotPotatoPlayer : BasePlayer
	{
		private TimeSince timeSinceDropped;
		private TimeSince timeSinceJumpReleased;

		private DamageInfo lastDamage;

		private bool PlayerHasPotato;

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new WalkController();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use FirstPersonCamera (you can make your own Camera for 100% control)
			//
			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			PlayerHasPotato = true;

			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		protected override void Tick()
		{
			base.Tick();

			if ( Input.Pressed( InputButton.View ) )
			{
				if ( Camera is not FirstPersonCamera )
				{
					Camera = new FirstPersonCamera();
				}
				else
				{
					Camera = new ThirdPersonCamera();
				}
			}

			//
			// Shoot a ball when Mouse1 is pressed.
			//
			if ( IsServer && Input.Pressed( InputButton.Attack1 ) && PlayerHasPotato )
			{
				var potato = new BallEntity();
				potato.WorldPos = EyePos + EyeRot.Forward * 50;
				potato.Velocity = EyeRot.Forward * 1000;
				PlayerHasPotato = false; // We threw the potato, so we don't have it anymore.
			}
		}

		// Pick the potato back up.
		public override void StartTouch( Entity other )
		{
			if ( IsClient ) return;

			if ( other is BallEntity && !PlayerHasPotato ) // Don't pick up it up if we already have one.
			{
				PlayerHasPotato = true; // We picked the potato up.
				other.Delete();
			}
		}

#region Death Code
		public override void OnKilled()
		{
			base.OnKilled();

			BecomeRagdollOnClient( Velocity, lastDamage.Flags, lastDamage.Position, lastDamage.Force, GetHitboxBone( lastDamage.HitboxIndex ) );
			Camera = new RagdollCamera();

			Controller = null;

			EnableAllCollisions = false;
			EnableDrawing = false;
		}

		public override void TakeDamage( DamageInfo info )
		{
			lastDamage = info;

			TookDamage( lastDamage.Flags, lastDamage.Position, lastDamage.Force );

			base.TakeDamage( info );
		}

		[ClientRpc]
		public void TookDamage( DamageFlags damageFlags, Vector3 forcePos, Vector3 force )
		{
		}
#endregion

		// Remove before release
		public override bool HasPermission( string mode )
		{
			if ( mode == "noclip" ) return true;
			if ( mode == "devcam" ) return true;
			if ( mode == "suicide" ) return true;

			return base.HasPermission( mode );
		}
	}
}
