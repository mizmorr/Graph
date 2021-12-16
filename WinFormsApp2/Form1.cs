using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Collections.Generic;


namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public bool indec;
        public Graph Graph = new Graph();
        public Form1()
        {
            InitializeComponent();
            move.LearnToMove(pictureBox1);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            Vertex.Width = 100;
            Vertex.Text = "Add Vertex";
            AddEdge.Top = 230;
            AddEdge.Width = 100;
            AddEdge.Text = "Add Edge";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }
        private void PaintGraph(Tuple<int,int> tuple, Graph.Node newnode)
        {
            try
            {
                if (tuple.Item1 != -1 && tuple.Item2 != -1) { var a = new Tuple<int, int>(tuple.Item1, tuple.Item2); Graph.AddEgde(a); }
                if (newnode.id == -1) Graph.InsertNewNode(newnode);
                Graph.y = pictureBox1.Height / 2;
                Graph.x = pictureBox1.Width / 2;
                Bitmap DoubleBuffer = new Bitmap(Width, Height);
                Graphics graphic = Graphics.FromImage(DoubleBuffer);
                SolidBrush myBrush = new SolidBrush(Color.Black);
                SolidBrush myBrush2 = new SolidBrush(Color.Green);
                Pen myPen = new Pen(Color.BlueViolet);
                Pen myPen2 = new Pen(Color.Green);
                Pen myPen3 = new Pen(Color.Brown, 2);
                graphic.Clear(Color.White);
                Font font = new Font("ArialBlack", 12, FontStyle.Regular);
                foreach (Graph.Node node in Graph.nodes)
                {
                    foreach (Graph.Node node1 in Graph.nodes)
                    {
                        var a = new Tuple<int, int>(node.id, node1.id);
                        if (Graph.listEgde.Contains(a))
                        {
                            double distance = PaintT.point_distance(node.x, node.y, node1.x, node1.y);
                            double direction = PaintT.point_direction(node.x, node.y, node1.x, node1.y);
                            if (Graph.NumberContains(a) > 1)
                                graphic.DrawLine(myPen3, new Point(node.x + (int)PaintT.lengthdir_x(Graph.roundsize / 2, direction), node.y + (int)PaintT.lengthdir_y(Graph.roundsize / 2, direction)), new Point(node.x + (int)PaintT.lengthdir_x(distance - (Graph.roundsize / 2), direction),
                                node.y + (int)PaintT.lengthdir_y(distance - (Graph.roundsize / 2), direction))); //кратность больше 1
                            else
                                graphic.DrawLine(myPen2, new Point(node.x + (int)PaintT.lengthdir_x(Graph.roundsize / 2, direction), node.y + (int)PaintT.lengthdir_y(Graph.roundsize / 2, direction)), new Point(node.x + (int)PaintT.lengthdir_x(distance - (Graph.roundsize / 2), direction),
                                node.y + (int)PaintT.lengthdir_y(distance - (Graph.roundsize / 2), direction)));//отрисовка ребра
                            graphic.FillEllipse(myBrush2, new Rectangle(node.x + (int)PaintT.lengthdir_x(distance - (Graph.roundsize / 2), direction) - 4, node.y + (int)PaintT.lengthdir_y(distance - (Graph.roundsize / 2), direction) - 4, 8, 8));
                        }
                    }
                }
                foreach (Graph.Node node2 in Graph.nodes)
                {
                    myBrush2.Color = Color.Coral;
                    graphic.FillEllipse(myBrush2, new Rectangle(node2.x - Graph.roundsize / 2, node2.y - Graph.roundsize / 2, Graph.roundsize, Graph.roundsize));
                    graphic.DrawEllipse(myPen, new Rectangle(node2.x - Graph.roundsize / 2, node2.y - Graph.roundsize / 2, Graph.roundsize, Graph.roundsize));
                    graphic.DrawString(node2.name, font, myBrush, new Point(node2.x - Graph.roundsize / 2 + 7, node2.y - Graph.roundsize / 2 + 5));
                }
                pictureBox1.Image = DoubleBuffer;
                myBrush.Dispose();
                myPen.Dispose();
                myBrush2.Dispose();
                myPen2.Dispose();
                myPen3.Dispose();
            }
            catch(Exception e) { textBox1.Text=e.Message;}
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Graph.Init();
            var a = new Tuple<int, int>(-1, -1);
            Graph.Node node = new();
            node.id = -1;
            PaintGraph(a,node);
            button1.Dispose();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(Graph.MaxNumberContains());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string vertex = textBox1.Text;
                Graph.AddNode(vertex);
                var a = Graph.nodes.Find(x => x.name == vertex);
                var a2 = new Tuple<int, int>(-1, -1);
                PaintGraph(a2, a);
                textBox1.Clear();
                progressBar1.Value += 10;
            }
            catch(Exception k) { textBox2.Text = k.Message; }
        }

        private void AddEdge_Click(object sender, EventArgs e)
        {
            try
            {
                string edge = textBox3.Text;
                textBox3.Clear();
                int[] k = new int[3];
                string[] edge1 = edge.Split(' ');
                for (var i = 0; i < 2; i++)
                    k[i] = Convert.ToInt32(edge1[i]);
                Graph.Node node = new();
                node.id = -1;
                var tuple = new Tuple<int, int>(k[0], k[1]);
                PaintGraph(tuple, node);
                progressBar1.Value += 10;
            }
            catch(Exception k) { textBox2.Text = k.Message; }
        }
        
    }
    public class move
    {
        static Graph.Node Node;
        static bool isPress = false;
        static Point startPst;
        private static void mDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;
            isPress = true;
            startPst = e.Location;
        }
        private static void mUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;
            isPress = false;
        }
        private static void mMove(object sender, MouseEventArgs e)
        {
            if (isPress)
            {
                foreach (Graph.Node n in Graph.nodes)
                {
                    if (PaintT.point_distance(n.x, n.y, e.X, e.Y) < Graph.roundsize / 2)
                    {
                
                        n.x += e.X-startPst.X; 
                        n.y = e.Y - startPst.Y;
                        Node = n;
                        break;
                    }
                }
            }
        }
        public static void LearnToMove(object sender)
        {
            Control control = (Control)sender;
            control.MouseDown += new MouseEventHandler(mDown);
            control.MouseUp += new MouseEventHandler(mUp);
            control.MouseMove += new MouseEventHandler(mMove);
        }
    }
    public class PaintT
    {
        
        public static double degtorad(double deg)
        {
            return deg * Math.PI / 180;
        }
        public static double radtodeg(double rad)
        {
            return rad / Math.PI * 180;
        }
        public static double lengthdir_x(double len, double dir)
        {
            return len * Math.Cos(degtorad(dir));
        }
        public static double lengthdir_y(double len, double dir)
        {
            return len * Math.Sin(degtorad(dir)) * (-1);
        }
        public static double point_direction(int x1, int y1, int x2, int y2) 
        {
            return 180 - radtodeg(Math.Atan2(y1 - y2, x1 - x2));
        }
        public static double point_distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
    }
    public class Graph
    {  
        static  public List<Node> nodes = new List<Node>();
        public List<Tuple<int, int>> listEgde = new List<Tuple<int, int>>();
        public class Node
        {
            public int id;
            public int x, y; 
            public string name;
        }
        private int maxid = 0;
        public int x = 0;
        public int y = 0;
        static public int roundsize = 32;
        public void AddEgde(Tuple<int,int> tuple)
        {
            if (Contain(tuple.Item1) && Contain(tuple.Item2)) listEgde.Add(new Tuple<int, int>(tuple.Item1, tuple.Item2));
            else return;
        }
        public void InsertNewNode(Node node)
        {
            Random random = new Random();
            foreach(Node node1 in nodes)
            {
                if (random.Next(0, 1) == 1) listEgde.Add(new Tuple<int, int>(node.id, node1.id));
            }
            foreach(Node node2 in nodes)
            {
                if (random.Next(0, 1) == 1) listEgde.Add(new Tuple<int, int>(node2.id, node.id));
            }
        }
        public void MakeNode(string name, int id,int x, int y)
        {
            Node node = new Node();
            if (id > maxid) maxid = id;
            node.id = id; node.x = x; node.y = y; node.name = name;
            nodes.Add(node);
            nodes.Sort((xx, yy) => xx.id.CompareTo(yy.id));
        }
        bool Contain(int item)
        {
            foreach(Node node in nodes)
            {
                if (node.id == item) return true;
            }
            return false;
        }
        public void Init()
        {
            MakeNode("A", 0, 163,150);
            MakeNode("B", 1, 334, 50);
            MakeNode("C", 2, 273, 20);
            MakeNode("D", 3, 25, 250);
            MakeNode("E", 4, 395,220);
            MakeNode("F", 5, 60, 100);
            BuildEdge();
        }
        public void AddNode(string name)
        {
            bool find = false;
            int id = 0;
            for (int i = 0; i < maxid; i++)
            {
                bool exist = false;
                foreach (Node nd in nodes)
                {
                    if (nd.id == i)
                    {
                        exist = true; break;
                    }
                }
                if (!exist)
                {
                    id = i;
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                id = maxid;
                maxid++;
            }
            Random rand = new Random();
            Node n = new Node();
            n.id = id;
            n.x =rand.Next(50,400);
            n.y =rand.Next(50,400);
            if (name != "")
                n.name = name;
            else
                n.name = id.ToString();
            nodes.Add(n);
            nodes.Sort((x, y) => x.id.CompareTo(y.id));
        }
      
        public int NumberContains(Tuple<int, int> a)
        {
            int count = listEgde.Count(x => x.Item1 == a.Item1 && x.Item2 == a.Item2);
            return count;
        }
        public int MaxNumberContains()
        {
            int max = 0, imax = 0;
            for (var i = 1; i < listEgde.Count; i++)
                if (NumberContains(listEgde[i]) > max) { max = NumberContains(listEgde[i]); imax = i; }
            return imax;
        }
        public static  int swap(int a,int k,int c)
        {
            a = a == k ? --a : a;
            a = a == c ? ++a : a;
            return a;
         }
        public void BuildEdge()
        {   Random random = new Random();
            Random random1 = new Random();
            foreach(Node node in nodes)
            {
                listEgde.Add(new Tuple<int, int>(node.id, swap(random.Next(0, nodes.Count), nodes.Count,node.id)));
            }
            foreach (Node node1 in nodes)
            {
                Random random2 = new Random();
                var k = random.Next(0, 2);
                switch (k)
                {
                    case 0: break;
                    case 1: listEgde.Add(new Tuple<int, int>(swap(random.Next(0, nodes.Count),nodes.Count, node1.id), node1.id)); break;
                    case 2: { var c = random.Next(0, nodes.Count); listEgde.Add(new Tuple<int, int>(swap(c, nodes.Count, node1.id), node1.id)); listEgde.Add(new Tuple<int, int>(swap(c, nodes.Count, node1.id), node1.id)); } break;
                }
            }
            var d = random1.Next(0, nodes.Count); var s = random.Next(0, nodes.Count); s = swap(s, nodes.Count, d); listEgde.Add(new Tuple<int, int>(d, s)); listEgde.Add(new Tuple<int, int>(d, s));
        }

}
}
