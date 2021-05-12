using Sandbox;
using System;

namespace HotPotato.Entities
{
    [Library( "ent_ball", Title = "Bouncy Ball")]
    partial class BallEntity : Prop
    {
        public PickupTrigger PickupTrigger { get; protected set; }

        public override void Spawn()
        {
            base.Spawn();

            SetModel( "models/ball/ball.vmdl" );
            SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
            WorldScale = 1;
            RenderColor = Color.Random.ToColor32();

            PickupTrigger = new PickupTrigger
		    {
		    	Parent = this,
		    	WorldPos = WorldPos,
		    	EnableTouch = true
		    };
        }

        protected override void OnPhysicsCollision( CollisionEventData eventData )
        {
            var speed = eventData.PreVelocity.Length;
            var direction = Vector3.Reflect( eventData.PreVelocity.Normal, eventData.Normal.Normal ).Normal;
            Velocity = direction * (speed - 50f);
        }
    }
}