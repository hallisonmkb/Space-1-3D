using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SharpGL
{
    /// <summary>
    /// A particle system is, you guessed it, just a collection of particles.
    /// </summary>
	[Serializable()]
	public class ParticleSystem : SceneElement, IRenderable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSystem"/> class.
        /// </summary>
        public ParticleSystem()
        {
            //  Create the effect for the system.
            OpenGLAttributesEffect attributesEffect = new OpenGLAttributesEffect();
            attributesEffect.LightingAttributes.Enable = false;

            //  Add the effects.
            AddEffect(attributesEffect);
        }

		/// <summary>
		/// This function should create and initialise 'count' particles of the correct
		/// type. This is done automatically by default, only override if you want
		/// to change the standard behaviour.
		/// </summary>
		/// <param name="count"></param>
		public virtual void Initialise(int count)
		{
			//	Get rid of any old particles.
			particles.Clear();

			//	Add the particles.
			for(int i=0; i<count; i++)
			{
				//	Create a particle.
				Particle particle = new BasicParticle();

				//	Initialise it.
				particle.Intialise(rand);

				//	Add it.
				particles.Add(particle);
			}
		}

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {            
			foreach(Particle p in particles)
				p.Draw(gl);
        }

        /// <summary>
        /// This function ticks the particle system.
        /// </summary>
		public virtual void Tick()
		{
			foreach(Particle p in particles)
			{
				//	Tick the particle.
				p.Tick(rand);

			}
        }

        /// <summary>
        /// Pushes us into Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void PushObjectSpace(OpenGL gl)
        {
            //  Use the helper to push us into object space.
            hasObjectSpaceHelper.PushObjectSpace(gl);
        }

        /// <summary>
        /// Pops us from Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void PopObjectSpace(OpenGL gl)
        {
            //  Use the helper to pop us from object space.
            hasObjectSpaceHelper.PopObjectSpace(gl);
        }

        /// <summary>
        /// The IHasObjectSpace helper.
        /// </summary>
        private HasObjectSpaceHelper hasObjectSpaceHelper = new HasObjectSpaceHelper();

		protected Random rand = new Random();
        public List<Particle> particles = new List<Particle>();
        protected Type particleType = typeof(BasicParticle);

        /// <summary>
        /// Gets the transformation that pushes us into object space.
        /// </summary>
        [Description("The Quadric Object Space Transformation"), Category("Particle System")]
        public LinearTransformation Transformation
        {
            get { return hasObjectSpaceHelper.Transformation; }
        }
	}
}
