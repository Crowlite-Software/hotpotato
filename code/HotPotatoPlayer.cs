using HotPotato.Cameras;
using Sandbox;
using System;
namespace HotPotato
{
	partial class HotPotatoPlayer : BasePlayer
	{
		private TimeSince timeSinceDropped;
		private TimeSince timeSinceJumpReleased;

		private DamageInfo lastDamage;


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
			// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
			//
			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				var ragdoll = new ModelEntity();
				ragdoll.SetModel( "models/citizen/citizen.vmdl" );
				ragdoll.WorldPos = EyePos + EyeRot.Forward * 20;
				ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				ragdoll.Velocity = EyeRot.Forward * 5000;
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
