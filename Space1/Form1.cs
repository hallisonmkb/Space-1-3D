using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Windows;
using System.Threading;
using SharpGL;
using System.IO;

namespace Space1
{
    public partial class Form1 : Form
    {
        private int curX;
        private int curY;
        private ArcBallEffect arcBallEffect = new ArcBallEffect();
        private SceneElement selectedSceneElement = null;
        private TreeNode selectedTreeNode = null;
        int x_largura;
        int y_profundidade;
        int z_altura;
        int tipo;
        Keys key_pressed;
        //Int64 time_elapsed;
        int casa_decimal = 2;
        List<Color> cor_todos;
        Color cor_anterior;
        ClienteCollection cliente_todos;

        enum TipoDdl
        {
            Pedido,
            Cliente,
            Produto
        };

        enum TipoTransformation
        {
            //Tamanho
            ScaleX,
            ScaleY,
            ScaleZ,
            //Posição
            TranslateX,
            TranslateY,
            TranslateZ,
            TopRadius,
            Height
        };

        public Form1()
        {
            InitializeComponent();

            x_largura = 0;
            y_profundidade = 0;
            z_altura = 0;

            Carregar_Lista_Cor();

            //Dimensionar_Scene();

            Carregar_Ddl_Estrutura();

            //TreeNode node_um;
            //TreeNode node_dois;

            ////Todos
            //treeView1.Nodes.Clear();
            //node_um = new TreeNode();
            //node_um.Checked = true;
            //node_um.Text = "Todos";
            //node_um.Tag = null;
            //treeView1.Nodes.Add(node_um);

            //node_dois = new TreeNode();
            //node_dois.Checked = true;
            //node_dois.ForeColor = cor_todos[0];
            //node_dois.Text = "João da Costa";
            //node_dois.Tag = null;
            //treeView1.Nodes[0].Nodes.Add(node_dois);

            ////Pedido
            //node_um = new TreeNode();
            //node_um.Checked = true;
            //node_um.ForeColor = node_dois.ForeColor;
            //node_um.Text = "47.891";
            //node_um.Tag = null;
            //node_dois.Nodes.Add(node_um);
            //AddElementToTree(node_um);

            ////Cliente
            //node_dois = new TreeNode();
            //node_dois.Checked = true;
            //node_dois.ForeColor = cor_todos[1];
            //node_dois.Text = "Maria Silveira";
            //node_dois.Tag = null;
            //treeView1.Nodes[0].Nodes.Add(node_dois);

            ////Pedido
            //node_um = new TreeNode();
            //node_um.Checked = true;
            //node_um.ForeColor = node_dois.ForeColor;
            //node_um.Text = "109.302";
            //node_um.Tag = null;
            //node_dois.Nodes.Add(node_um);
            //AddElementToTree(node_um);

            //treeView1.ExpandAll();

            //Montar_Produto();

            //Random randNum = new Random();

            Carregar_Ddl_Produto();

            splitContainer2.SplitterDistance = 1000;
            propertyGrid1.Visible = false;
            pictureBox1.Visible = true;
            treeView1.Visible = true;

            //tipo = randNum.Next(2);
            //Trocar_Imagem();

            sceneControl1.Scene.SceneContainer.AddEffect(arcBallEffect);
            sceneControl1.Scene.RenderBoundingVolumes = false;
            sceneControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.sceneControl1_MouseWheel);
        }

        private void OnSelectedSceneElementChanged()
        {
            propertyGrid1.SelectedObject = SelectedSceneElement;
        }

        public SceneElement SelectedSceneElement
        {
            get { return selectedSceneElement; }
            set
            {
                if (selectedTreeNode != null)
                {
                    //selectedTreeNode.NodeFont = new Font(Label.DefaultFont, FontStyle.Regular);
                    selectedTreeNode.ForeColor = cor_anterior;
                    Mudar_Cor(selectedSceneElement, cor_anterior);
                }
                if (value != null)
                {
                    bool possui = false;
                    foreach (var element in sceneControl1.Scene.SceneContainer.Children)
                    {
                        if (element.Codigo > 0 && element.IsEnabled == false)
                        {
                            possui = true;
                            break;
                        }
                    }
                    if (possui)
                    {
                        MessageBox.Show("Há produtos escondidos. Impossível selecionar produto!", "Space-1 3D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (value.Existe_Produto_Em_Cima == string.Empty)
                    {
                        selectedSceneElement = value;
                        Selecionar_TreeNode(treeView1.Nodes[0]);
                        
                        Mudar_Cor(value, Color.Red);
                    }
                }
                else
                {
                    selectedSceneElement = null;
                    selectedTreeNode = null;
                }
                OnSelectedSceneElementChanged();
            }
        }

        //private void toolStripButtonShowBoundingVolumes_Click(object sender, EventArgs e)
        //{
        //    sceneControl1.Scene.RenderBoundingVolumes = !sceneControl1.Scene.RenderBoundingVolumes;
        //    toolStripButtonShowBoundingVolumes.Checked = !toolStripButtonShowBoundingVolumes.Checked;
        //}

        private void Status_Produto(string produto, string pedido, string cliente)
        {
            statusProduto.Text = string.Empty;

            if (produto != string.Empty)
            {
                statusProduto.Text += "Produto: " + produto + "            |           ";
                statusProduto.Text += "Pedido: " + pedido + "          |           ";
                statusProduto.Text += "Cliente: " + cliente + "          |           ";
            }
        }

        private void Status_Mouse(int x, int y)
        {
            statusMouse.Text = string.Empty;
            statusMouse.Text += "Mouse X: " + x.ToString() + "          |           ";
            statusMouse.Text += "Mouse Y: " + sceneControl1.Scene.GetYPosition(y).ToString();
        }

        //private void Montar_Produto()
        //{
        //    float x_inicial = x_largura / (float)2.0;
        //    float y_inicial = y_profundidade / (float)2.0;

        //    Material materia = new Material();
        //    materia.Ambient = Color.FromArgb(0, Color.Blue);

        //    //Texture textura = new Texture();
        //    //textura.Create(sceneControl1.OpenGL, "ground_wheel.jpg");
        //    //materia.Texture = textura;

        //    //Sphere sphere = new Sphere();
        //    //sphere.Transformation.TranslateX = 2;
        //    //sphere.Transformation.TranslateY = 2;
        //    //sphere.Transformation.TranslateZ = 1;

        //    //Cylinder cone = new Cylinder() { Name = "Cone" };
        //    //cone.BaseRadius = 1.5;
        //    //cone.TopRadius = 0;
        //    //cone.Height = 2;
        //    //cone.Transformation.TranslateX = -2;
        //    //cone.Transformation.TranslateY = -2;
        //    //cone.Transformation.TranslateZ = 0;

        //    Cylinder cilindro = new Cylinder();
        //    cilindro.Codigo = 1;
        //    cilindro.Existe_Produto_Em_Cima = string.Empty;
        //    cilindro.Name = "cilindro";
        //    cilindro.Pedido = "109.302";
        //    cilindro.Cliente = "Maria Silveira";
        //    cilindro.Palet_Base = 0;
        //    cilindro.Empilhamento_Peso_Maximo = 0;
        //    cilindro.Empilhamento_Restricao = 0;
        //    cilindro.Tipo_Suporte = 0;
        //    cilindro.BaseRadius = 0.5;
        //    cilindro.TopRadius = 0.5;
        //    cilindro.Height = 3;
        //    cilindro.Transformation.TranslateX = 2 - x_inicial;
        //    cilindro.Transformation.TranslateY = (float)3.5 - y_inicial;
        //    cilindro.Transformation.TranslateZ = (float)0.5;
        //    cilindro.Transformation.RotateX = 270;
        //    cilindro.Material = materia;
        //    sceneControl1.Scene.SceneContainer.AddChild(cilindro);

        //    Cube cubo = new Cube();
        //    cubo.Codigo = 2;
        //    cubo.Existe_Produto_Em_Cima = string.Empty;
        //    cubo.Name = "quadrado";
        //    cubo.Pedido = "47.891";
        //    cubo.Cliente = "João da Costa";
        //    cubo.Palet_Base = 0;
        //    cubo.Empilhamento_Peso_Maximo = 0;
        //    cubo.Empilhamento_Restricao = 0;
        //    cubo.Tipo_Suporte = 0;
        //    cubo.Transformation.TranslateX = 4 - x_inicial;
        //    cubo.Transformation.TranslateY = 5 - y_inicial;
        //    cubo.Transformation.TranslateZ = 1;
        //    cubo.Material = materia;
        //    sceneControl1.Scene.SceneContainer.AddChild(cubo);

        //    Cube cubo2 = new Cube();
        //    cubo2.Codigo = 3;
        //    cubo2.Existe_Produto_Em_Cima = string.Empty;
        //    cubo2.Name = "retangulo";
        //    cubo2.Pedido = "109.302";
        //    cubo2.Cliente = "Maria Silveira";
        //    cubo2.Palet_Base = 0;
        //    cubo2.Empilhamento_Peso_Maximo = 0;
        //    cubo2.Empilhamento_Restricao = 0;
        //    cubo2.Tipo_Suporte = 0;
        //    cubo2.Transformation.TranslateX = 4 - x_inicial;
        //    cubo2.Transformation.TranslateY = 2 - y_inicial;
        //    cubo2.Transformation.TranslateZ = (float)0.5;
        //    cubo2.Transformation.ScaleX = (float)1.7;
        //    cubo2.Transformation.ScaleY = (float)0.6;
        //    cubo2.Transformation.ScaleZ = (float)0.5;
        //    cubo2.Material = materia;
        //    sceneControl1.Scene.SceneContainer.AddChild(cubo2);

        //    Cube cubo3 = new Cube();
        //    cubo3.Codigo = 4;
        //    cubo3.Existe_Produto_Em_Cima = string.Empty;
        //    cubo3.Name = "quadradinho";
        //    cubo3.Pedido = "47.891";
        //    cubo3.Cliente = "João da Costa";
        //    cubo3.Palet_Base = 0;
        //    cubo3.Empilhamento_Peso_Maximo = 0;
        //    cubo3.Empilhamento_Restricao = 0;
        //    cubo3.Tipo_Suporte = 0;
        //    cubo3.Transformation.TranslateX = 4 - x_inicial;
        //    cubo3.Transformation.TranslateY = 12 - y_inicial;
        //    cubo3.Transformation.TranslateZ = (float)1.2;
        //    cubo3.Transformation.ScaleX = (float)0.7;
        //    cubo3.Transformation.ScaleY = (float)0.3;
        //    cubo3.Transformation.ScaleZ = (float)1.2;
        //    cubo3.Material = materia;
        //    sceneControl1.Scene.SceneContainer.AddChild(cubo3);

        //    Disk disco = new Disk();
        //    disco.Codigo = 5;
        //    disco.Existe_Produto_Em_Cima = string.Empty;
        //    disco.Name = "octagno";
        //    disco.Pedido = "47.891";
        //    disco.Cliente = "João da Costa";
        //    disco.Palet_Base = 0;
        //    disco.Empilhamento_Peso_Maximo = 0;
        //    disco.Empilhamento_Restricao = 0;
        //    disco.Tipo_Suporte = 0;
        //    disco.Slices = 8;
        //    disco.Slices_Cylinder = 8;
        //    disco.Transformation.TranslateX = 5 - x_inicial;
        //    disco.Transformation.TranslateY = 9 - y_inicial;
        //    disco.Transformation.TranslateZ = (float)1.4;
        //    disco.Height = 1.4;
        //    disco.Transformation.ScaleX = (float)1;
        //    disco.Transformation.ScaleY = (float)1;
        //    disco.Transformation.RotateX = 180;
        //    disco.OuterRadius = 1;
        //    disco.BaseRadius = 1;
        //    disco.TopRadius = 1;
        //    disco.Material = materia;
        //    sceneControl1.Scene.SceneContainer.AddChild(disco);
        //}

        private void Carregar_Lista_Cor()
        {
            //17 cores
            cor_todos = new List<Color>();
            cor_todos.Add(Color.DarkGreen);
            cor_todos.Add(Color.BlueViolet);
            cor_todos.Add(Color.DarkMagenta);
            cor_todos.Add(Color.Blue);
            cor_todos.Add(Color.Brown);
            cor_todos.Add(Color.Chartreuse);
            cor_todos.Add(Color.Chocolate);
            cor_todos.Add(Color.Crimson);
            cor_todos.Add(Color.DarkRed);
            cor_todos.Add(Color.DarkSlateGray);
            cor_todos.Add(Color.DeepPink);
            cor_todos.Add(Color.ForestGreen);
            cor_todos.Add(Color.LightSalmon);
            cor_todos.Add(Color.MidnightBlue);
            cor_todos.Add(Color.Orange);
            cor_todos.Add(Color.Yellow);
            cor_todos.Add(Color.Aquamarine);
            cor_todos.Add(Color.Sienna);
        }

        private float Buscar_Dimensao_Produto(SceneElement element, TipoTransformation tipo, Keys KeyCode)
        {
            if (element != null)
            {
                if (element.GetType() == typeof(Cube))
                {
                    if (tipo == TipoTransformation.TranslateX)
                    {
                        return (float)Math.Round(((Cube)element).Transformation.TranslateX, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.TranslateY)
                    {
                        return (float)Math.Round(((Cube)element).Transformation.TranslateY, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.TranslateZ)
                    {
                        return (float)Math.Round(((Cube)element).Transformation.TranslateZ, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.ScaleZ)
                    {
                        return (float)Math.Round(((Cube)element).Transformation.ScaleZ, casa_decimal);
                    }
                    else if (((Cube)element).Transformation.RotateZ == 90 ||
                        ((Cube)element).Transformation.RotateZ == 270)
                    {
                        if (tipo == TipoTransformation.ScaleY)
                        {
                            return (float)Math.Round(((Cube)element).Transformation.ScaleX, casa_decimal);
                        }
                        else
                        {
                            return (float)Math.Round(((Cube)element).Transformation.ScaleY, casa_decimal);
                        }
                    }
                    else
                    {
                        if (tipo == TipoTransformation.ScaleY)
                        {
                            return (float)Math.Round(((Cube)element).Transformation.ScaleY, casa_decimal);
                        }
                        else
                        {
                            return (float)Math.Round(((Cube)element).Transformation.ScaleX, casa_decimal);
                        }
                    }
                }
                else if (element.GetType() == typeof(Cylinder))
                {
                    if (tipo == TipoTransformation.TranslateX)
                    {
                        return (float)Math.Round(((Cylinder)element).Transformation.TranslateX, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.TranslateY)
                    {
                        return (float)Math.Round(((Cylinder)element).Transformation.TranslateY, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.TranslateZ)
                    {
                        return (float)Math.Round(((Cylinder)element).Transformation.TranslateZ, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.ScaleZ)
                    {
                        return (float)Math.Round(((Cylinder)element).Transformation.ScaleZ, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.TopRadius)
                    {
                        return (float)Math.Round(((Cylinder)element).TopRadius, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.Height)
                    {
                        return (float)Math.Round(((Cylinder)element).Height, casa_decimal);
                    }
                    else if (((Cylinder)element).Transformation.RotateY == 0)
                    {
                        if (KeyCode != Keys.Down)
                        {
                            if (tipo == TipoTransformation.ScaleY)
                            {
                                if (KeyCode != Keys.None || ((Cylinder)element).Transformation.TranslateY > 0)
                                {
                                    return (float)Math.Round(((Cylinder)element).Height, casa_decimal);
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                            else
                            {
                                return (float)Math.Round(((Cylinder)element).TopRadius, casa_decimal);
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else if (((Cylinder)element).Transformation.RotateY == 180)
                    {
                        if (KeyCode != Keys.Up)
                        {
                            if (tipo == TipoTransformation.ScaleY)
                            {
                                if (KeyCode != Keys.None || ((Cylinder)element).Transformation.TranslateY > 0)
                                {
                                    return (float)Math.Round(((Cylinder)element).Height, casa_decimal);
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                            else
                            {
                                return (float)Math.Round(((Cylinder)element).TopRadius, casa_decimal);
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else if (((Cylinder)element).Transformation.RotateY == 90)
                    {
                        if (KeyCode != Keys.Left)
                        {
                            if (tipo == TipoTransformation.ScaleX)
                            {
                                if (KeyCode != Keys.None || ((Cylinder)element).Transformation.TranslateY > 0)
                                {
                                    return (float)Math.Round(((Cylinder)element).Height, casa_decimal);
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                            else
                            {
                                return (float)Math.Round(((Cylinder)element).TopRadius, casa_decimal);
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else if (((Cylinder)element).Transformation.RotateY == 270)
                    {
                        if (KeyCode != Keys.Right)
                        {
                            if (tipo == TipoTransformation.ScaleX)
                            {
                                if (KeyCode != Keys.None || ((Cylinder)element).Transformation.TranslateY > 0)
                                {
                                    return (float)Math.Round(((Cylinder)element).Height, casa_decimal);
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                            else
                            {
                                return (float)Math.Round(((Cylinder)element).TopRadius, casa_decimal);
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (element.GetType() == typeof(Disk))
                {
                    if (tipo == TipoTransformation.TranslateX)
                    {
                        return (float)Math.Round(((Disk)element).Transformation.TranslateX, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.TranslateY)
                    {
                        return (float)Math.Round(((Disk)element).Transformation.TranslateY, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.TranslateZ)
                    {
                        return (float)Math.Round(((Disk)element).Transformation.TranslateZ, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.ScaleX)
                    {
                        return (float)Math.Round(((Disk)element).Transformation.ScaleX, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.ScaleY)
                    {
                        return (float)Math.Round(((Disk)element).Transformation.ScaleY, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.ScaleZ && KeyCode == Keys.S)
                    {
                        return (float)Math.Round(((Disk)element).Height, casa_decimal);
                    }
                    else if (tipo == TipoTransformation.Height)
                    {
                        return (float)Math.Round(((Disk)element).Height, casa_decimal);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private void Mover_Produto(float valor, Keys KeyCode)
        {
            if (propertyGrid1.SelectedObject != null)
            {
                if (KeyCode == Keys.W)
                    KeyCode = Keys.Up;
                else if (KeyCode == Keys.S)
                    KeyCode = Keys.Down;
                else if (KeyCode == Keys.A)
                    KeyCode = Keys.Left;
                else if (KeyCode == Keys.D)
                    KeyCode = Keys.Right;

                LinearTransformation lt;
                float x_scale = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleX, KeyCode);
                float y_scale = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleY, KeyCode);
                //Altura do produto ao chão
                float z_scale = 0;
                float x_inicial = x_largura / (float)2.0;
                float y_inicial = y_profundidade / (float)2.0;
                float x_anterior = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TranslateX, KeyCode);
                float y_anterior = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TranslateY, KeyCode);

                if (propertyGrid1.SelectedObject.GetType() == typeof(Cube))
                {
                    lt = ((Cube)propertyGrid1.SelectedObject).Transformation;
                    z_scale = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleZ, KeyCode);
                }
                else if (propertyGrid1.SelectedObject.GetType() == typeof(Cylinder))
                {
                    lt = ((Cylinder)propertyGrid1.SelectedObject).Transformation;
                    z_scale = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TopRadius, KeyCode);
                }
                else if (propertyGrid1.SelectedObject.GetType() == typeof(Disk))
                {
                    lt = ((Disk)propertyGrid1.SelectedObject).Transformation;
                    z_scale = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.Height, KeyCode);
                }
                else
                {
                    lt = new LinearTransformation();
                }
                try
                {
                    if (KeyCode == Keys.Right || KeyCode == Keys.None)
                        if (lt.TranslateX + valor + x_scale <= x_largura - x_inicial)
                            lt.TranslateX += valor;
                        else
                            lt.TranslateX = x_largura - x_inicial - x_scale;

                    if (KeyCode == Keys.Left || KeyCode == Keys.None)
                        if (lt.TranslateX - valor - x_scale >= -x_inicial)
                            lt.TranslateX -= valor;
                        else
                            lt.TranslateX = x_scale - x_inicial;

                    if (KeyCode == Keys.Up || KeyCode == Keys.None)
                        if (lt.TranslateY + valor + y_scale <= y_profundidade - y_inicial)
                            lt.TranslateY += valor;
                        else
                            lt.TranslateY = y_profundidade - y_inicial - y_scale;

                    if (KeyCode == Keys.Down || KeyCode == Keys.None)
                        if (lt.TranslateY - valor - y_scale >= -y_inicial)
                            lt.TranslateY -= valor;
                        else
                            lt.TranslateY = y_scale - y_inicial;

                    //Depois da movimentação para obter a posição real, mas caso não tenha espaço é preciso recuperar a posição anterior
                    float z_inicial = Detectar_Altura_Produto(z_scale);

                    if (z_inicial + z_scale <= z_altura)
                    {
                        lt.TranslateZ = z_inicial;
                        lt.TranslateX = (float)Math.Round(lt.TranslateX, casa_decimal);
                        lt.TranslateY = (float)Math.Round(lt.TranslateY, casa_decimal);
                    }
                    else
                    {
                        lt.TranslateX = x_anterior;
                        lt.TranslateY = y_anterior;
                    }
                }
                catch (Exception) { }
            }
        }

        private float Detectar_Altura_Produto(float TranslateZ_Novo)
        {
            float X1_2_5 = 0;
            float Y2_3_6 = 0;
            float Y1_4_8 = 0;
            float X3_4_7 = 0;
            float Y5_7 = 0;
            float X6_8 = 0;
            int Codigo = 0;

            X1_2_5 = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TranslateX, Keys.None);
            X1_2_5 -= Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleX, Keys.None);
            X1_2_5 = (float)Math.Round(X1_2_5, casa_decimal);
            Y2_3_6 = Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TranslateY, Keys.None);
            Y2_3_6 -= Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleY, Keys.None);
            Y2_3_6 = (float)Math.Round(Y2_3_6, casa_decimal);

            if (propertyGrid1.SelectedObject.GetType() == typeof(Cylinder))
            {
                if (((Cylinder)selectedSceneElement).Transformation.RotateY == 90 ||
                    ((Cylinder)selectedSceneElement).Transformation.RotateY == 270)
                {
                    Y1_4_8 = Y2_3_6 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TopRadius, Keys.None) * (float)2.0;
                    Y1_4_8 = (float)Math.Round(Y1_4_8, casa_decimal);
                    X3_4_7 = X1_2_5 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.Height, Keys.None);
                    X3_4_7 = (float)Math.Round(X3_4_7, casa_decimal);
                    Y5_7 = Y2_3_6 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TopRadius, Keys.None);
                    Y5_7 = (float)Math.Round(Y5_7, casa_decimal);
                    X6_8 = X1_2_5 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.Height, Keys.None) * (float)0.5;
                    X6_8 = (float)Math.Round(X6_8, casa_decimal);
                }
                else
                {
                    Y1_4_8 = Y2_3_6 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.Height, Keys.None) * (float)0.5;
                    Y1_4_8 = (float)Math.Round(Y1_4_8, casa_decimal);
                    X3_4_7 = X1_2_5 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TopRadius, Keys.None);
                    X3_4_7 = (float)Math.Round(X3_4_7, casa_decimal);
                    Y5_7 = Y2_3_6 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.Height, Keys.None);
                    Y5_7 = (float)Math.Round(Y5_7, casa_decimal);
                    X6_8 = X1_2_5 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.TopRadius, Keys.None) * (float)2.0;
                    X6_8 = (float)Math.Round(X6_8, casa_decimal);
                }

                Codigo = ((Cylinder)propertyGrid1.SelectedObject).Codigo;
            }
            else
            {
                Y1_4_8 = Y2_3_6 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleY, Keys.None) * (float)2.0;
                Y1_4_8 = (float)Math.Round(Y1_4_8, casa_decimal);
                X3_4_7 = X1_2_5 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleX, Keys.None) * (float)2.0;
                X3_4_7 = (float)Math.Round(X3_4_7, casa_decimal);
                Y5_7 = Y2_3_6 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleY, Keys.None);
                Y5_7 = (float)Math.Round(Y5_7, casa_decimal);
                X6_8 = X1_2_5 + Buscar_Dimensao_Produto(selectedSceneElement, TipoTransformation.ScaleX, Keys.None);
                X6_8 = (float)Math.Round(X6_8, casa_decimal);

                if (propertyGrid1.SelectedObject.GetType() == typeof(Cube))
                {
                    Codigo = ((Cube)propertyGrid1.SelectedObject).Codigo;
                }
                else if (propertyGrid1.SelectedObject.GetType() == typeof(Disk))
                {
                    Codigo = ((Disk)propertyGrid1.SelectedObject).Codigo;
                }
            }

            //Coordenadas, pontos
            //              8         
            //1   ._________._________. 4
            //    |                   | 
            //    |                   |
            //5   .                   . 7
            //    |                   |
            //    |                   |
            //2   |.________.________.| 3
            //              6       

            foreach (var element in sceneControl1.Scene.SceneContainer.Children)
            {
                if (element.Codigo > 0)
                {
                    float XA_B = 0;
                    float YB_C = 0;
                    float YA_D = 0;
                    float XC_D = 0;

                    XA_B = Buscar_Dimensao_Produto(element, TipoTransformation.TranslateX, Keys.None);
                    XA_B -= Buscar_Dimensao_Produto(element, TipoTransformation.ScaleX, Keys.None);
                    XA_B = (float)Math.Round(XA_B, casa_decimal);
                    YB_C = Buscar_Dimensao_Produto(element, TipoTransformation.TranslateY, Keys.None);
                    YB_C -= Buscar_Dimensao_Produto(element, TipoTransformation.ScaleY, Keys.None);
                    YB_C = (float)Math.Round(YB_C, casa_decimal);

                    if (element.GetType() == typeof(Cylinder))
                    {
                        if (((Cylinder)element).Transformation.RotateY == 90 ||
                            ((Cylinder)element).Transformation.RotateY == 270)
                        {
                            YA_D = YB_C + Buscar_Dimensao_Produto(element, TipoTransformation.TopRadius, Keys.None) * (float)2.0;
                            YA_D = (float)Math.Round(YA_D, casa_decimal);
                            XC_D = XA_B + Buscar_Dimensao_Produto(element, TipoTransformation.Height, Keys.None);
                            XC_D = (float)Math.Round(XC_D, casa_decimal);
                        }
                        else
                        {
                            YA_D = YB_C + Buscar_Dimensao_Produto(element, TipoTransformation.Height, Keys.None);
                            YA_D = (float)Math.Round(YA_D, casa_decimal);
                            XC_D = XA_B + Buscar_Dimensao_Produto(element, TipoTransformation.TopRadius, Keys.None) * (float)2.0;
                            XC_D = (float)Math.Round(XC_D, casa_decimal);
                        }
                    }
                    else
                    {
                        YA_D = YB_C + Buscar_Dimensao_Produto(element, TipoTransformation.ScaleY, Keys.None) * (float)2.0;
                        YA_D = (float)Math.Round(YA_D, casa_decimal);
                        XC_D = XA_B + Buscar_Dimensao_Produto(element, TipoTransformation.ScaleX, Keys.None) * (float)2.0;
                        XC_D = (float)Math.Round(XC_D, casa_decimal);
                    }
                    if (element.Codigo > 0 && element.Codigo != Codigo)
                    {
                        //Apenas para testes
                        //textBox1.Text = "   X1_2_5=" + X1_2_5.ToString("0.00") + " - ";
                        //textBox1.Text += "   XA_B=" + XA_B.ToString("0.00") + "\r\n";
                        //textBox1.Text += "   Y2_3_6=" + Y2_3_6.ToString("0.00") + " - ";
                        //textBox1.Text += "   YB_C=" + YB_C.ToString("0.00") + "\r\n";
                        //textBox1.Text += "   Y1_4_8=" + Y1_4_8.ToString("0.00") + " - ";
                        //textBox1.Text += "   YA_D=" + YA_D.ToString("0.00") + "\r\n";
                        //textBox1.Text += "   X3_4_7=" + X3_4_7.ToString("0.00") + " - ";
                        //textBox1.Text += "   XC_D=" + XC_D.ToString("0.00") + "\r\n";

                        if (element.Existe_Produto_Em_Cima.Contains("|" + selectedSceneElement.Codigo + "|"))
                        {
                            element.Existe_Produto_Em_Cima = element.Existe_Produto_Em_Cima.Replace("|" + selectedSceneElement.Codigo + "|", "");
                        }
                        if (X3_4_7 > XA_B && X3_4_7 < XC_D && Y1_4_8 > YB_C && Y1_4_8 < YA_D ||
                            X1_2_5 > XA_B && X1_2_5 < XC_D && Y1_4_8 > YB_C && Y1_4_8 < YA_D ||
                            X3_4_7 > XA_B && X3_4_7 < XC_D && Y2_3_6 > YB_C && Y2_3_6 < YA_D ||
                            X1_2_5 > XA_B && X1_2_5 < XC_D && Y2_3_6 > YB_C && Y2_3_6 < YA_D ||
                            X3_4_7 > XA_B && X3_4_7 < XC_D && Y5_7 > YB_C && Y5_7 < YA_D ||
                            Y1_4_8 > YB_C && Y1_4_8 < YA_D && X6_8 > XA_B && X6_8 < XC_D ||
                            X1_2_5 > XA_B && X1_2_5 < XC_D && Y5_7 > YB_C && Y5_7 < YA_D ||
                            Y2_3_6 > YB_C && Y2_3_6 < YA_D && X6_8 > XA_B && X6_8 < XC_D ||
                            X1_2_5 <= XA_B && X3_4_7 >= XC_D && Y1_4_8 >= YA_D && Y2_3_6 <= YB_C)
                        {
                            TranslateZ_Novo += Retornar_Altura(element);
                            element.Existe_Produto_Em_Cima += "|" + selectedSceneElement.Codigo + "|";
                        }
                    }
                }
            }
            foreach (var element_um in sceneControl1.Scene.SceneContainer.Children)
            {
                if (element_um.Codigo > 0 && element_um.Codigo != Codigo && element_um.Existe_Produto_Em_Cima.Contains("|" + selectedSceneElement.Codigo + "|"))
                {
                    foreach (var element_dois in sceneControl1.Scene.SceneContainer.Children)
                    {
                        if (element_dois.Codigo > 0 && element_dois.Codigo != Codigo)
                        {
                            //Quando o objeto movimentado está em cima de dois ou mais objetos, porém esses não estão em cima uns dos outros
                            if (element_dois.Existe_Produto_Em_Cima.Contains("|" + selectedSceneElement.Codigo + "|") &&
                                element_um.Existe_Produto_Em_Cima.Contains("|" + element_dois.Codigo + "|") == false)
                            {
                                if (Retornar_Altura(element_um) < Retornar_Altura(element_dois) ||
                                    Retornar_Altura(element_um) == Retornar_Altura(element_dois) && element_um.Codigo < element_dois.Codigo)
                                {
                                    TranslateZ_Novo -= Retornar_Altura(element_um);
                                    TranslateZ_Novo = (float)Math.Round(TranslateZ_Novo, casa_decimal);
                                }
                            }
                            //Quando o objeto movimentado está em cima de um, mas esse está em cima de outro
                            else if (element_dois.Existe_Produto_Em_Cima.Contains("|" + selectedSceneElement.Codigo + "|") == false &&
                                element_dois.Existe_Produto_Em_Cima.Contains("|" + element_um.Codigo + "|"))
                            {
                                TranslateZ_Novo += Retornar_Altura(element_dois);
                                TranslateZ_Novo = (float)Math.Round(TranslateZ_Novo, casa_decimal);
                            }
                        }
                    }
                }
            }
            
            return TranslateZ_Novo;
        }

        private float Retornar_Altura(SceneElement element)
        {
            if (element.GetType() == typeof(Cube))
            {
                return (float)Math.Round(((Cube)element).Transformation.ScaleZ * (float)2.0, casa_decimal);
            }
            else if (element.GetType() == typeof(Cylinder))
            {
                return (float)Math.Round(((Cylinder)element).TopRadius * (float)2.0, casa_decimal);
            }
            else if (element.GetType() == typeof(Disk))
            {
                return (float)Math.Round(((Disk)element).Height, casa_decimal);
            }
            else
            {
                return 0;
            }
        }

        private void Carregar_Ddl_Produto()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Todos");
            comboBox1.SelectedIndex = 0;

            foreach (var element in sceneControl1.Scene.SceneContainer.Children)
            {
                if (element.Codigo > 0)
                {
                    comboBox1.Items.Add(element.Codigo.ToString("000") + " - " + element.Name);
                }
            }
        }

        private void Carregar_Ddl_Estrutura()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Cliente/Pedido/Produto");
            comboBox2.Items.Add("Produto/Cliente");
            comboBox2.SelectedIndex = 0;
        }

        private void Dimensionar_Scene()
        {
            foreach (var element in sceneControl1.Scene.SceneContainer.Children)
            {
                if (element.GetType() == typeof(Folder))
                {
                    Folder folder = (Folder)element;

                    foreach (var element_folder in folder.Children)
                    {
                        if (element_folder.GetType() == typeof(Grid))
                        {
                            Grid grid = (Grid)element_folder;
                            grid.CreateDisplayList(sceneControl1.OpenGL, x_largura, y_profundidade, z_altura);
                            break;
                        }
                    }
                    break;
                }
            }
            float x_inicial = x_largura / (float)2.0;
            float y_inicial = y_profundidade / (float)2.0;
            float z_inicial = z_altura / (float)2.0;

            Material materia1 = new Material();
            materia1.Texture = new Texture();
            materia1.Texture.Create(new OpenGL(), "ground_metal.jpg");

            Cube cubo = new Cube();
            cubo.Codigo = 0;
            cubo.Existe_Produto_Em_Cima = string.Empty;
            cubo.Name = "ground_1_sharpgl";
            cubo.Transformation.ScaleX = x_inicial;
            cubo.Transformation.ScaleY = y_inicial;
            cubo.Transformation.ScaleZ = (float)0.05;

            cubo.Transformation.TranslateX = 0;
            cubo.Transformation.TranslateY = 0;
            //O valor de 0.001 a mais para visualizar a grid
            cubo.Transformation.TranslateZ = (float)-0.051;
            cubo.Material = materia1;

            //Cube cubo2 = new Cube();
            //cubo2.Name = "ground_2_sharpgl";
            //cubo2.Transformation.ScaleX = x_inicial;
            //cubo2.Transformation.ScaleY = (float)0.05;
            //cubo2.Transformation.ScaleZ = z_inicial;

            //cubo2.Transformation.TranslateX = 0;
            //cubo2.Transformation.TranslateY = y_inicial + (float)0.05;
            //cubo2.Transformation.TranslateZ = z_inicial;
            //cubo2.Material = materia1;

            //Cube cubo3 = new Cube();
            //cubo3.Name = "ground_3_sharpgl";
            //cubo3.Transformation.ScaleX = (float)0.05;
            //cubo3.Transformation.ScaleY = y_inicial;
            //cubo3.Transformation.ScaleZ = z_inicial;

            //cubo3.Transformation.TranslateX = x_inicial + (float)0.05;
            //cubo3.Transformation.TranslateY = 0;
            //cubo3.Transformation.TranslateZ = z_inicial;
            //cubo3.Material = materia1;

            sceneControl1.Scene.SceneContainer.AddChild(cubo);
            //sceneControl1.Scene.SceneContainer.AddChild(cubo2);
            //sceneControl1.Scene.SceneContainer.AddChild(cubo3);
        }

        private void Inserir_Objeto(SceneElement element, int count_cor)
        {
            bool existe_cliente = false;
            bool existe_pedido = false;

            foreach (Cliente cliente in cliente_todos)
            {
                if (cliente.Descricao == element.Cliente)
                {
                    existe_cliente = true;
                    break;
                }
            }
            if (existe_cliente == false)
            {
                Cliente cliente_novo = new Cliente();
                cliente_novo.PedidoTodos = new PedidoCollection();
                cliente_novo.ForeColor = cor_todos[count_cor];
                cliente_novo.Descricao = element.Cliente;
                cliente_todos.Add(cliente_novo);
            }
            foreach (Cliente cliente in cliente_todos)
            {
                if (cliente.Descricao == element.Cliente)
                {
                    foreach (Pedido pedido in cliente.PedidoTodos)
                    {
                        if (pedido.Descricao == element.Pedido)
                        {
                            existe_pedido = true;
                            pedido.ElementoTodos.Add(element);
                            break;
                        }
                    }
                    if (existe_pedido == false)
                    {
                        Pedido pedido_novo = new Pedido();
                        pedido_novo.ElementoTodos = new List<SceneElement>();
                        pedido_novo.Descricao = element.Pedido;
                        pedido_novo.ElementoTodos.Add(element);
                        cliente.PedidoTodos.Add(pedido_novo);
                    }
                }
            }
        }

        private void Carregar_TreeNode()
        {
            TreeNode node_um;
            TreeNode node_dois;

            //Todos
            treeView1.Nodes.Clear();
            node_um = new TreeNode();
            node_um.Checked = true;
            node_um.Text = "Todos";
            node_um.Tag = null;
            treeView1.Nodes.Add(node_um);

            if (cliente_todos == null)
            {
                cliente_todos = new ClienteCollection();
            }
            if (comboBox2.SelectedIndex == 0)
            {
                foreach (Cliente cliente in cliente_todos)
                {
                    node_dois = new TreeNode();
                    node_dois.Checked = true;
                    node_dois.ForeColor = cliente.ForeColor;
                    node_dois.Text = cliente.Descricao;
                    node_dois.Tag = null;
                    treeView1.Nodes[0].Nodes.Add(node_dois);

                    foreach (Pedido pedido in cliente.PedidoTodos)
                    {
                        node_um = new TreeNode();
                        node_um.Checked = true;
                        node_um.ForeColor = cliente.ForeColor;
                        node_um.Text = pedido.Descricao;
                        node_um.Tag = null;
                        node_dois.Nodes.Add(node_um);
                        AddElementToTree(node_um);
                    }
                }
            }
            else
            {
                List<SceneElement> elements_includes = new List<SceneElement>();
                int count_cor = 0;

                foreach (var element in sceneControl1.Scene.SceneContainer.Children)
                {
                    if (element.Codigo > 0)
                    {
                        bool existe = false;
                        foreach (SceneElement element_include in elements_includes)
                        {
                            if (element.Codigo == element_include.Codigo)
                            {
                                existe = true;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            Mudar_Cor(element, cor_todos[count_cor]);
                            elements_includes.Add(element);

                            node_dois = new TreeNode();
                            node_dois.Checked = true;
                            node_dois.ForeColor = cor_todos[count_cor];
                            if (count_cor > 16)
                            {
                                count_cor = 0;
                            }
                            else
                            {
                                count_cor++;
                            }
                            node_dois.Text = element.Codigo.ToString("000") + " - " + element.Name;
                            node_dois.Tag = element;
                            node_um.Nodes.Add(node_dois);

                            foreach (Cliente cliente in cliente_todos)
                            {
                                if (cliente.Descricao == element.Cliente)
                                {
                                    foreach (Pedido pedido in cliente.PedidoTodos)
                                    {
                                        if (existe == false)
                                        {
                                            foreach (SceneElement element_object in pedido.ElementoTodos)
                                            {
                                                if (element.Codigo == element_object.Codigo)
                                                {
                                                    existe = true;
                                                    TreeNode node_tres = new TreeNode();
                                                    node_tres.Checked = true;
                                                    node_tres.ForeColor = node_dois.ForeColor;
                                                    node_tres.Text = cliente.Descricao; ;
                                                    node_tres.Tag = null;
                                                    node_dois.Nodes.Add(node_tres);
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            treeView1.ExpandAll();
        }

        private void AddElementToTree(TreeNode node_um)
        {
            foreach (var element in sceneControl1.Scene.SceneContainer.Children)
            {
                if (element.Codigo > 0 && (element.Cliente == node_um.Text || element.Pedido == node_um.Text || node_um.Text == "Todos"))
                {
                    TreeNode node = new TreeNode();
                    node.Checked = true;

                    node.ForeColor = node_um.ForeColor;
                    node.Text = element.Codigo.ToString("000") + " - " + element.Name;
                    node.Tag = element;
                    node_um.Nodes.Add(node);

                    Mudar_Cor(element, node_um.ForeColor);
                }
            }
        }

        private void Selecionar_TreeNode(TreeNode node_um)
        {
            foreach (TreeNode node_dois in node_um.Nodes)
            {
                if (statusProduto.Text.Substring(0, statusProduto.Text.IndexOf("Pedido:")).Contains(node_dois.Text))
                {
                    //node_dois.NodeFont = new Font(Label.DefaultFont, FontStyle.Bold);
                    cor_anterior = node_dois.ForeColor;
                    node_dois.ForeColor = Color.Red;
                    selectedTreeNode = node_dois;
                    break;
                }
                else
                {
                    Selecionar_TreeNode(node_dois);
                }
            }
        }

        private void CheckToTree(TreeNode node_um)
        {
            if (node_um.Tag != null)
            {
                ((SceneElement)node_um.Tag).IsEnabled = node_um.Checked;
            }
            foreach (TreeNode node_dois in node_um.Nodes)
            {
                node_dois.Checked = node_um.Checked;

                if (node_dois.Tag != null)
                {
                    ((SceneElement)node_dois.Tag).IsEnabled = node_um.Checked;
                }
                if (node_dois.Nodes.Count > 0)
                {
                    CheckToTree(node_dois);
                }
            }
        }

        private void Trocar_Imagem()
        {
            if (tipo == 0)
            {
                pictureBox1.Image = Space1.Properties.Resources.truck1;
            }
            else if (tipo == 1)
            {
                pictureBox1.Image = Space1.Properties.Resources.truck3;
            }
            else
            {
                pictureBox1.Image = Space1.Properties.Resources.container;
            }
        }

        private void Mudar_Cor(SceneElement element, Color cor)
        {
            Material materia = new Material();
            materia.Ambient = Color.FromArgb(0, cor);

            if (element.GetType() == typeof(Cube))
            {
                ((Cube)element).Material = materia;
            }
            else if (element.GetType() == typeof(Cylinder))
            {
                ((Cylinder)element).Material = materia;
            }
            else if (element.GetType() == typeof(Disk))
            {
                ((Disk)element).Material = materia;
            }
        }

        private void sceneControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (cb_mouse_mov_obj.Checked)
            {
                Status_Produto(string.Empty, string.Empty, string.Empty);
                SelectedSceneElement = null;
                comboBox1.SelectedIndex = 0;
            }
            var itemsHit = sceneControl1.Scene.DoHitTest(e.X, e.Y);
            LinearTransformation lt = new LinearTransformation();
            
            foreach (var item in itemsHit)
            {
                if (item.Codigo > 0)
                {
                    Status_Produto(item.Codigo.ToString("000") + " - " + item.Name, item.Pedido, item.Cliente);
                    SelectedSceneElement = item;

                    if (SelectedSceneElement != null)
                    {
                        comboBox1.Text = string.Empty;
                        comboBox1.SelectedText = item.Codigo.ToString("000") + " - " + item.Name;
                    }
                    else
                    {
                        Status_Produto(string.Empty, string.Empty, string.Empty);
                        comboBox1.SelectedIndex = 0;
                    }
                }
            }
            if (propertyGrid1.SelectedObject != null)
            {
                if (propertyGrid1.SelectedObject.GetType() == typeof(Cube))
                {
                    lt = ((Cube)propertyGrid1.SelectedObject).Transformation;

                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
	                {
                        if (lt.RotateZ != 270)
                        {
                            lt.RotateZ += 90;
                        }
                        else
                        {
                            lt.RotateZ = 0;
                        }
	                }
                }
                else if (propertyGrid1.SelectedObject.GetType() == typeof(Cylinder))
                {
                    lt = ((Cylinder)propertyGrid1.SelectedObject).Transformation;

                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (lt.RotateY == 0)
                        {
                            lt.RotateY += 90;
                            lt.TranslateX -= (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                            lt.TranslateY += (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                        }
                        else if (lt.RotateY == 90)
                        {
                            lt.RotateY += 90;
                            lt.TranslateX += (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                            lt.TranslateY += (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                        }
                        else if (lt.RotateY == 180)
                        {
                            lt.RotateY += 90;
                            lt.TranslateX += (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                            lt.TranslateY -= (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                        }
                        else
                        {
                            lt.RotateY = 0;
                            lt.TranslateX -= (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                            lt.TranslateY -= (float)((Cylinder)propertyGrid1.SelectedObject).Height / (float)2.0;
                        }
                    }
                }
                else if (propertyGrid1.SelectedObject.GetType() == typeof(Disk))
                {
                    lt = ((Disk)propertyGrid1.SelectedObject).Transformation;
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    Mover_Produto(0, Keys.None);
                }
            }
        }

        private void sceneControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            Vertex vertex = sceneControl1.Scene.CurrentCamera.Position;

            if (e.Delta < 0)
            {
                vertex.Z += (float)1;
                vertex.X -= (float)1;
                vertex.Y -= (float)1;
            }
            else
            {
                vertex.Z -= (float)1;
                vertex.X += (float)1;
                vertex.Y += (float)1;
            }
            sceneControl1.Scene.CurrentCamera.Position = vertex;
        }

        private void sceneControl1_MouseMove(object sender, MouseEventArgs e)
        {
            Status_Mouse(e.X, e.Y);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                arcBallEffect.ArcBall.MouseMove(e.X, e.Y, cb_camera_z.Checked);
            }
            else if (propertyGrid1.SelectedObject != null)
            {
                if (cb_mouse_mov_obj.Checked)
                {
                    LinearTransformation lt;

                    if (propertyGrid1.SelectedObject.GetType() == typeof(Cube))
                    {
                        lt = ((Cube)propertyGrid1.SelectedObject).Transformation;
                    }
                    else if (propertyGrid1.SelectedObject.GetType() == typeof(Cylinder))
                    {
                        lt = ((Cylinder)propertyGrid1.SelectedObject).Transformation;
                    }
                    else if (propertyGrid1.SelectedObject.GetType() == typeof(Disk))
                    {
                        lt = ((Disk)propertyGrid1.SelectedObject).Transformation;
                    }
                    else
                    {
                        lt = new LinearTransformation();
                    }
                    float x_dif = (float)x_largura / (float)sceneControl1.Width;
                    float y_dif = (float)y_profundidade / (float)sceneControl1.Height;
                    x_dif *= (float)e.X;
                    y_dif *= (float)e.Y;
                    //Ambiente começa em posição negativa para centralizar
                    float x_inicial = x_largura / (float)2.0;
                    float y_inicial = y_profundidade / (float)2.0;
                    x_dif -= x_inicial;
                    y_dif -= y_inicial;
                    //Acelerar
                    x_dif *= (float)1.5;
                    y_dif *= (float)1.5;

                    lt.TranslateX = x_dif;
                    lt.TranslateX = (float)Math.Round(lt.TranslateX, casa_decimal);
                    lt.TranslateY = -y_dif;
                    lt.TranslateY = (float)Math.Round(lt.TranslateY, casa_decimal);

                    Mover_Produto(0, Keys.None);
                }

                curX = e.X;
                curY = e.Y;
            }
        }

        private void sceneControl1_MouseDown(object sender, MouseEventArgs e)
        {
            arcBallEffect.ArcBall.SetBounds(sceneControl1.Width, sceneControl1.Height);
            arcBallEffect.ArcBall.MouseDown(e.X, e.Y, cb_camera_z.Checked);

            curX = e.X;
            curY = e.Y;
        }

        private void sceneControl1_MouseUp(object sender, MouseEventArgs e)
        {
            arcBallEffect.ArcBall.MouseUp(e.X, e.Y);
        }

        private void sceneControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!cb_mouse_mov_obj.Checked)
            {
                Mover_Produto((float)trackBar1.Value * (float)0.1, e.KeyCode);
            }
        }

        private void cb_textura_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var element in sceneControl1.Scene.SceneContainer.Children)
            {
                if (element.Codigo == 0 && element.GetType() == typeof(Cube))
                {
                    ((Cube)element).IsEnabled = cb_textura.Checked;
                    break;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                cb_textura.Checked = !cb_textura.Checked;
            }
            else if (e.KeyCode == Keys.F2)
            {
                cb_mouse_mov_obj.Checked = !cb_mouse_mov_obj.Checked;
            }
            else if (e.KeyCode == Keys.F3)
            {
                cb_camera_z.Checked = !cb_camera_z.Checked;
            }
            else if (e.KeyCode == Keys.D1 && trackBar1.Enabled)
            {
                trackBar1.Value = 1;
            }
            else if (e.KeyCode == Keys.D2 && trackBar1.Enabled)
            {
                trackBar1.Value = 3;
            }
            else if (e.KeyCode == Keys.D3 && trackBar1.Enabled)
            {
                trackBar1.Value = 5;
            }
            else if (e.KeyCode == Keys.D4 && trackBar1.Enabled)
            {
                trackBar1.Value = 7;
            }
            else if (e.KeyCode == Keys.D5 && trackBar1.Enabled)
            {
                trackBar1.Value = 9;
            }
            else if (e.KeyCode == Keys.D6 && trackBar1.Enabled)
            {
                trackBar1.Value = 11;
            }
            else if (trackBar1.Value > 1)
            {
                key_pressed = e.KeyCode;
                Application.DoEvents();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (trackBar1.Value > 1)
            {
                key_pressed = Keys.None;
                Application.DoEvents();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (key_pressed != Keys.None && trackBar1.Value > 1)
            {
                Mover_Produto((float)trackBar1.Value * (float)0.1, key_pressed);
                Application.DoEvents();
            }

            //time_elapsed += timer1.Interval;

            //if (time_elapsed == 10000)
            //{
            //    time_elapsed = 0;
            //    Trocar_Imagem();
            //}
        }

        private void sceneControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (trackBar1.Value > 1)
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Left:
                        key_pressed = e.KeyCode;
                        Application.DoEvents();
                        break;
                }
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            sceneControl1.Focus();
        }

        private void cb_mouse_mov_obj_CheckedChanged(object sender, EventArgs e)
        {
            trackBar1.Enabled = !cb_mouse_mov_obj.Checked;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckToTree(e.Node);

            if (e.Node.Checked == false)
            {
                Status_Produto(string.Empty, string.Empty, string.Empty);
                SelectedSceneElement = null;
                comboBox1.SelectedIndex = 0;
            }
            sceneControl1.Focus();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool selecionado = false;

            if (comboBox1.SelectedIndex == 0)
            {
                selecionado = true;
                Status_Produto(string.Empty, string.Empty, string.Empty);
                SelectedSceneElement = null;
            }
            else if (selectedTreeNode != null)
            {
                if (selectedTreeNode.Text == comboBox1.SelectedItem.ToString())
                {
                    selecionado = true;
                }
            }
            if (selecionado == false)
            {
                foreach (var element in sceneControl1.Scene.SceneContainer.Children)
                {
                    if (element.Codigo > 0)
                    {
                        if (element.Codigo.ToString("000") + " - " + element.Name == comboBox1.SelectedItem.ToString())
                        {
                            Status_Produto(element.Codigo.ToString("000") + " - " + element.Name, element.Pedido, element.Cliente);
                            SelectedSceneElement = element;

                            if (SelectedSceneElement == null)
                            {
                                Status_Produto(string.Empty, string.Empty, string.Empty);
                                comboBox1.SelectedIndex = 0;
                            }
                            break;
                        }
                    }
                }
            }
            sceneControl1.Focus();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                SceneElement element = ((SceneElement)e.Node.Tag);

                Status_Produto(element.Codigo.ToString("000") + " - " + element.Name, element.Pedido, element.Cliente);
                SelectedSceneElement = element;

                if (SelectedSceneElement != null)
                {
                    comboBox1.Text = string.Empty;
                    comboBox1.SelectedText = element.Codigo.ToString("000") + " - " + element.Name;
                }
                else
                {
                    Status_Produto(string.Empty, string.Empty, string.Empty);
                    comboBox1.SelectedIndex = 0;
                }
            }
            else
            {
                Status_Produto(string.Empty, string.Empty, string.Empty);
                SelectedSceneElement = null;
                comboBox1.SelectedIndex = 0;
            }
            sceneControl1.Focus();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Carregar_TreeNode();
            sceneControl1.Focus();
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] linha_todos = File.ReadAllLines(openFileDialog1.FileName.ToString());
                string[] linha;

                float x_inicial = 0;
                float y_inicial = 0;

                Material materia = new Material();
                materia.Ambient = Color.FromArgb(0, Color.Blue);

                bool existe = true;
                while (existe == true)
                {
                    SceneElement element = null;
                    existe = false;
                    foreach (SceneElement element_foreach in sceneControl1.Scene.SceneContainer.Children)
                    {
                        if (element_foreach.Codigo > 0 || element_foreach.Name == "ground_1_sharpgl")
                        {
                            element = element_foreach;
                            existe = true;
                        }
                    }
                    if (element != null)
                    {
                        sceneControl1.Scene.SceneContainer.RemoveChild(element);
                    }
                }

                cliente_todos = new ClienteCollection();
                int count_cor = 0;

                for (int i = 0; i < linha_todos.Length; i++)
                {
                    try
                    {
                        linha = linha_todos[i].Split('|');

                        if (i == 0) //Header
                        {
                            x_largura = int.Parse(linha[0]) / 100;
                            x_inicial = x_largura / (float)2.0;
                            y_profundidade = int.Parse(linha[1]) / 100;
                            y_inicial = y_profundidade / (float)2.0;
                            z_altura = int.Parse(linha[2]) / 100;

                            Dimensionar_Scene();

                            tipo = int.Parse(linha[3]);

                            Trocar_Imagem();
                        }
                        else if (linha.Length > 0 && linha[0] != string.Empty) //Carga
                        {
                            switch (int.Parse(linha[7]))
                            {
                                case 0:
                                    Cube cubo = new Cube();
                                    cubo.Cliente = linha[0];
                                    cubo.Pedido = linha[1];
                                    cubo.Codigo = int.Parse(linha[2]);
                                    cubo.Name = linha[3];
                                    cubo.Transformation.ScaleX = float.Parse(linha[4]) / float.Parse("100");
                                    cubo.Transformation.ScaleY = float.Parse(linha[5]) / float.Parse("100");
                                    cubo.Transformation.ScaleZ = float.Parse(linha[6]) / float.Parse("100");
                                    cubo.Palet_Base = 0;
                                    cubo.Empilhamento_Ilimitado = int.Parse(linha[9]);
                                    cubo.Empilhamento_Peso_Maximo = float.Parse(linha[10]);
                                    cubo.Empilhamento_Restricao = 0;
                                    cubo.Tipo_Suporte = 0;
                                    cubo.Transformation.TranslateX = (float.Parse(linha[13]) / float.Parse("100")) - x_inicial;
                                    cubo.Transformation.TranslateY = (float.Parse(linha[14]) / float.Parse("100")) - y_inicial;
                                    cubo.Transformation.TranslateZ = cubo.Transformation.ScaleZ + (float.Parse(linha[15]) / float.Parse("100"));

                                    cubo.Existe_Produto_Em_Cima = string.Empty;
                                    cubo.Material = materia;
                                    sceneControl1.Scene.SceneContainer.AddChild(cubo);

                                    Inserir_Objeto(cubo, count_cor);
                                    break;

                                case 1:
                                    Cylinder cilindro = new Cylinder();
                                    cilindro.Cliente = linha[0];
                                    cilindro.Pedido = linha[1];
                                    cilindro.Codigo = int.Parse(linha[2]);
                                    cilindro.Name = linha[3];
                                    cilindro.BaseRadius = float.Parse(linha[4]) / float.Parse("100");
                                    cilindro.Height = float.Parse(linha[5]) / float.Parse("100");
                                    cilindro.TopRadius = float.Parse(linha[6]) / float.Parse("100");
                                    cilindro.Palet_Base = 0;
                                    cilindro.Empilhamento_Ilimitado = int.Parse(linha[9]);
                                    cilindro.Empilhamento_Peso_Maximo = 0;
                                    cilindro.Empilhamento_Restricao = 0;
                                    cilindro.Tipo_Suporte = 0;
                                    cilindro.Transformation.TranslateX = (float.Parse(linha[13]) / float.Parse("100")) - x_inicial;
                                    cilindro.Transformation.TranslateY = (float)cilindro.Height + (float.Parse(linha[14]) / float.Parse("100")) - y_inicial;
                                    cilindro.Transformation.TranslateZ = (float)cilindro.BaseRadius + (float.Parse(linha[15]) / float.Parse("100"));

                                    cilindro.Existe_Produto_Em_Cima = string.Empty;
                                    cilindro.Material = materia;
                                    cilindro.Transformation.RotateX = 270;
                                    sceneControl1.Scene.SceneContainer.AddChild(cilindro);

                                    Inserir_Objeto(cilindro, count_cor);
                                    break;

                                case 2:
                                    Disk octagno = new Disk();
                                    octagno.Cliente = linha[0];
                                    octagno.Pedido = linha[1];
                                    octagno.Codigo = int.Parse(linha[2]);
                                    octagno.Name = linha[3];
                                    octagno.Transformation.ScaleX = float.Parse(linha[4]) / float.Parse("100");
                                    octagno.Transformation.ScaleY = octagno.Transformation.ScaleX;
                                    octagno.OuterRadius = (double)octagno.Transformation.ScaleX;
                                    octagno.BaseRadius = (double)octagno.Transformation.ScaleX;
                                    octagno.TopRadius = (double)octagno.Transformation.ScaleX;
                                    octagno.Height = (double)float.Parse(linha[5]) / float.Parse("100");
                                    octagno.Palet_Base = 0;
                                    octagno.Empilhamento_Ilimitado = int.Parse(linha[9]);
                                    octagno.Empilhamento_Peso_Maximo = 0;
                                    octagno.Empilhamento_Restricao = 0;
                                    octagno.Tipo_Suporte = 0;
                                    octagno.Transformation.TranslateX = (float.Parse(linha[13]) / float.Parse("100")) - x_inicial;
                                    octagno.Transformation.TranslateY = (float.Parse(linha[14]) / float.Parse("100")) - y_inicial;
                                    octagno.Transformation.TranslateZ = (float)octagno.Height + (float.Parse(linha[15]) / float.Parse("100"));

                                    octagno.Existe_Produto_Em_Cima = string.Empty;
                                    octagno.Slices = 8;
                                    octagno.Slices_Cylinder = 8;
                                    octagno.Transformation.RotateX = 180;
                                    octagno.Material = materia;
                                    sceneControl1.Scene.SceneContainer.AddChild(octagno);

                                    Inserir_Objeto(octagno, count_cor);
                                    break;
                            }
                            if (count_cor > 16)
                            {
                                count_cor = 0;
                            }
                            else
                            {
                                count_cor++;
                            }

                            Carregar_TreeNode();

                            Carregar_Ddl_Produto();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("O arquivo possui algum erro de estrutura na linha " + Convert.ToString(i + 1));
                        break;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Função não implementada.");
        }

        //private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    foreach (var element in sceneControl1.Scene.SceneContainer.Children)
        //    {
        //        if (element.Codigo > 0)
        //        {
        //            if (comboBox1.Items[e.Index].ToString() == element.Codigo.ToString("000") + " - " + element.Name)
        //            {
        //                e.DrawBackground();

        //                Brush brush = Brushes.Red;

        //                if (e.Index == 0)
        //                {
        //                    brush = Brushes.Red;
        //                }

        //                e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}