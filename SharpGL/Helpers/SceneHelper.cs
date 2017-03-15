using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL
{

    /// <summary>
    /// The scene helper can be used to create scene presets, 
    /// such as designer, application, etc
    /// </summary>
    public class SceneHelper
    {
        /// <summary>
        /// Initialises a modeling scene. A modeling scene has:
        ///  - A 'Look At' camera targetting the centre of the scene
        ///  - Three gentle omnidirectional lights
        ///  - A design time grid and axis.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public static void InitialiseModelingScene(Scene scene)
        {
            //float x_largura = 10;
            //float y_profundidade = 9;
            //double distancia = Math.Sqrt((y_profundidade * y_profundidade) + (x_largura * x_largura));

            //  Create the 'Look At' camera
            var lookAtCamera = new LookAtCamera()
            {
                Position = new Vertex(-10f, -12f, 10f),
                Target = new Vertex(0, 0, 0),
                UpVector = new Vertex(0f, 0f, 1f)
            };

            //  Set the look at camera as the current camera.
            scene.CurrentCamera = lookAtCamera;

            //  Add some design-time primitives.
            var folder = new Folder() { Name = "Design Primitives" };
            folder.AddChild(new Grid());
            folder.AddChild(new Axies());
            scene.SceneContainer.AddChild(folder);

            //  Create some lights.
            Light light1 = new Light()
            {
                Name = "Light 1",
                On = true,
                Position = new Vertex(-9, -9, 11),
                GLCode = OpenGL.GL_LIGHT6
            };
            Light light2 = new Light()
            {
                Name = "Light 2",
                On = true,
                Position = new Vertex(9, -9, 11),
                GLCode = OpenGL.GL_LIGHT6
            };
            Light light3 = new Light()
            {
                Name = "Light 3",
                On = true,
                Position = new Vertex(0, 15, 15),
                GLCode = OpenGL.GL_LIGHT6
            };
            //  Add the lights.
            folder = new Folder() { Name = "Lights" };
            folder.AddChild(light1);
            folder.AddChild(light2);
            folder.AddChild(light3);
            scene.SceneContainer.AddChild(folder);

            //  Create a set of scene attributes.
            OpenGLAttributesEffect sceneAttributes = new OpenGLAttributesEffect()
            {
                Name = "Scene Attributes"
            };

            //  Specify the scene attributes.
            sceneAttributes.EnableAttributes.EnableDepthTest = true;
            sceneAttributes.EnableAttributes.EnableNormalize = true;
            sceneAttributes.EnableAttributes.EnableLighting = true;
            sceneAttributes.EnableAttributes.EnableTexture2D = true;
            sceneAttributes.EnableAttributes.EnableBlend = true;
            sceneAttributes.EnableAttributes.EnableAlphaTest = true;
            sceneAttributes.ColorBufferAttributes.BlendingSourceFactor = BlendingSourceFactor.SourceAlpha;
            sceneAttributes.ColorBufferAttributes.BlendingDestinationFactor = BlendingDestinationFactor.OneMinusSourceAlpha;
            sceneAttributes.LightingAttributes.TwoSided = true;
            scene.SceneContainer.AddEffect(sceneAttributes);
        }
    }
}
