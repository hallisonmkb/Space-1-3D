using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SharpGL
{
    /// <summary>
    /// The Grid design time primitive is displays a grid in the scene.
    /// </summary>
    public class Grid : SceneElement, IRenderable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        public Grid()
        {
            Name = "Design Time Grid";
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            //  Design time primitives render only in design mode.
            if (renderMode != RenderMode.Design)
                return;

            //  If we do not have the display list, we must create it.
            //  Otherwise, we can simple call the display list.
            if (displayList == null)
                CreateDisplayList(gl, 0, 0, 0);
            else
                displayList.Call(gl);
        }

        /// <summary>
        /// Creates the display list. This function draws the
        /// geometry as well as compiling it.
        /// </summary>
        public void CreateDisplayList(OpenGL gl, int x_largura, int y_profundidade, int z_altura)
        {
            //if (x_largura == 0 && y_profundidade == 0 && z_altura == 0)
            //{
            //    x_largura = 10;
            //    y_profundidade = 7;
            //    z_altura = 4;
            //}

            //  Create the display list. 
            displayList = new DisplayList();

            //  Generate the display list and 
            displayList.Generate(gl);
            displayList.New(gl, DisplayList.DisplayListMode.CompileAndExecute);

            //  Push attributes, set the color.
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_ENABLE_BIT |
                OpenGL.GL_LINE_BIT);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.LineWidth(1.4f);

            //  Draw the grid lines.
            gl.Begin(OpenGL.GL_LINES);

            float x_inicial = x_largura / (float)2.0;
            float y_inicial = y_profundidade / (float)2.0;

            if (true)
            {
                //Base vertical
                for (int i = -(int)x_inicial; i <= x_largura - x_inicial; i++)
                {
                    try
                    {
                        float fcol = ((i % (x_largura / (float)2.0)) == 0) ? 0.3f : 0.15f;
                        gl.Color(fcol, fcol, fcol);
                    }
                    catch (Exception) { }

                    gl.Vertex(i, -y_inicial, 0);
                    gl.Vertex(i, y_profundidade - y_inicial, 0);
                }
                //Base horizontal
                for (int i = -(int)y_inicial; i <= y_profundidade - y_inicial; i++)
                {
                    try
                    {
                        float fcol = ((i % (x_largura / (float)2.0)) == 0) ? 0.3f : 0.15f;
                        gl.Color(fcol, fcol, fcol);
                    }
                    catch (Exception) { }

                    gl.Vertex(-x_inicial, i, 0);
                    gl.Vertex(x_largura - x_inicial, i, 0);
                }
                //Parede direita horizontal
                for (int i = 0; i <= z_altura; i++)
                {
                    try
                    {
                        float fcol = ((i % (z_altura / (float)2.0)) == 0) ? 0.3f : 0.15f;
                        gl.Color(fcol, fcol, fcol);
                    }
                    catch (Exception) { }

                    gl.Vertex(x_largura - x_inicial, -y_inicial, i);
                    gl.Vertex(x_largura - x_inicial, y_profundidade - y_inicial, i);
                }
                //Parede esquerda horizontal
                for (int i = 0; i <= z_altura; i++)
                {
                    try
                    {
                        float fcol = ((i % (z_altura / (float)2.0)) == 0) ? 0.3f : 0.15f;
                        gl.Color(fcol, fcol, fcol);
                    }
                    catch (Exception) { }

                    gl.Vertex(-x_inicial, y_profundidade - y_inicial, i);
                    gl.Vertex(x_largura - x_inicial, y_profundidade - y_inicial, i);
                }
                //Parede direita vertical
                for (int i = -(int)y_inicial; i <= y_profundidade - y_inicial; i++)
                {
                    try
                    {
                        float fcol = ((i % (x_largura / (float)2.0)) == 0) ? 0.3f : 0.15f;
                        gl.Color(fcol, fcol, fcol);
                    }
                    catch (Exception) { }

                    gl.Vertex(x_largura - x_inicial, i, 0);
                    gl.Vertex(x_largura - x_inicial, i, z_altura);
                }
                //Parede esquerda vertical
                for (int i = -(int)x_inicial; i <= x_largura - x_inicial; i++)
                {
                    try
                    {
                        float fcol = ((i % (x_largura / (float)2.0)) == 0) ? 0.3f : 0.15f;
                        gl.Color(fcol, fcol, fcol);
                    }
                    catch (Exception) { }

                    gl.Vertex(i, y_profundidade - y_inicial, 0);
                    gl.Vertex(i, y_profundidade - y_inicial, z_altura);
                }
            }
            else
            {
                int minimum_base_value = -7;
                int maximum_base_value = 7;
                int minimum_height_value = 0;
                int maximum_height_value = 4;

                for (int i = minimum_base_value; i <= maximum_base_value; i++)
                {
                    float fcol = ((i % maximum_base_value) == 0) ? 0.3f : 0.15f;
                    gl.Color(fcol, fcol, fcol);
                    gl.Vertex(i, minimum_base_value, 0);
                    gl.Vertex(i, maximum_base_value, 0);
                    gl.Vertex(minimum_base_value, i, 0);
                    gl.Vertex(maximum_base_value, i, 0);
                }
                for (int i = minimum_height_value; i <= maximum_base_value + maximum_base_value; i++)
                {
                    float fcol = ((i % maximum_base_value) == 0) ? 0.3f : 0.15f;
                    gl.Color(fcol, fcol, fcol);
                    gl.Vertex(maximum_base_value, i - maximum_base_value, minimum_height_value);
                    gl.Vertex(maximum_base_value, i - maximum_base_value, maximum_height_value);
                    if (i <= maximum_height_value)
                    {
                        gl.Vertex(maximum_base_value, minimum_base_value, i);
                        gl.Vertex(maximum_base_value, maximum_base_value, i);
                    }
                }
                for (int i = minimum_height_value; i <= maximum_base_value + maximum_base_value; i++)
                {
                    float fcol = ((i % maximum_base_value) == 0) ? 0.3f : 0.15f;
                    gl.Color(fcol, fcol, fcol);
                    gl.Vertex(i - maximum_base_value, maximum_base_value, minimum_height_value);
                    gl.Vertex(i - maximum_base_value, maximum_base_value, maximum_height_value);
                    if (i <= maximum_height_value)
                    {
                        gl.Vertex(minimum_base_value, maximum_base_value, i);
                        gl.Vertex(maximum_base_value, maximum_base_value, i);
                    }
                }
            }
            gl.End();

            //  Restore attributes.
            gl.PopAttrib();

            //  End the display list.
            displayList.End(gl);
        }

        /// <summary>
        /// The internal display list.
        /// </summary>
        [XmlIgnore]
        private DisplayList displayList;
    }
}
