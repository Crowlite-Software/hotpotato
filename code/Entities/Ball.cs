
using Sandbox;
using System;
using HotPotato;

namespace HotPotato.Entities
{
    [Library("ent_ball", Title = "Bouncy Ball")]
    partial class BallEntity : Prop
    {
        public PickupTrigger PickupTrigger { get; protected set; }

        public override void Spawn()
        {
            base.Spawn();

            SetModel("models/potato/ball.vmdl");
            SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);
            WorldScale = 1;
            RenderColor = Color.Random.ToColor32();

            PickupTrigger = new PickupTrigger
            {
                Parent = this,
                WorldPos = WorldPos,
                EnableTouch = true
            };
        }

        protected override void OnPhysicsCollision(CollisionEventData eventData)
        {
            var speed = eventData.PreVelocity.Length;
            var direction = Vector3.Reflect(eventData.PreVelocity.Normal, eventData.Normal.Normal).Normal;
            Velocity = direction * (speed - 50f);
        }

        // Pick the potato back up.
        public override void StartTouch(Entity other)
        {
            if (IsClient) return;

            if (other is HotPotato.HotPotatoPlayer && !HotPotato.HotPotatoPlayer.PlayerHasPotato) // Don't pick up it up if we already have one.
            {
                HotPotato.HotPotatoPlayer.PlayerHasPotato = true; // We picked the potato up.
                Delete();
            }
        }
    }
}
