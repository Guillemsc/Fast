using System;

namespace Fast.Modules
{
    public class ParticlesModule : UpdatableModule
    {
        private readonly Fast.Particles.ParticlesController controller = new Particles.ParticlesController();

        public override void Update()
        {
            controller.Update();
        }

        public void PlayParticle(Fast.Particles.ParticlePlayer player, Action on_finish = null)
        {
            controller.PlayParticle(player, on_finish);
        }
    }
}
