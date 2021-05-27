using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app
{
    public partial class Form1 : Form
    {
        private PostInput _postInput = new PostInput { Id = -1, Title = "", Description = "", ImageUrl = "" };
        private BindingList<Post> _posts;
        public Form1()
        {
            InitializeComponent();
            button1.Visible = true;
            button2.Visible = button3.Visible = button4.Visible = false;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            _posts = new BindingList<Post>(await GetPostsAsync());
            dataGridView1.DataSource = _posts;
            _postInput.@Event += this.OnEvent;
        }

        public async Task<IList<Post>> GetPostsAsync()
        {
            using (var db = new DbEntities())
            {
                return await db.Posts.ToListAsync();
            }
        }

        public void OnEvent(object source, MyEventArgs args)
        {
            textBox1.Text = args.Title;
            textBox2.Text = args.Description;
            textBox3.Text = args.ImageUrl;
            if (args.Id == -1)
            {
                button1.Visible = true;
                button2.Visible = button3.Visible = button4.Visible = false;
            }
            else
            {
                button1.Visible = false;
                button2.Visible = button3.Visible = button4.Visible = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.ClearSelection();
            if (e.RowIndex != -1)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                _postInput.Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                _postInput.Title = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                _postInput.Description = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                _postInput.ImageUrl = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (_postInput.Id != -1)
                return;
            using (var db = new DbEntities())
            {
                var post = new Post { Title = _postInput.Title, Description = _postInput.Description, ImageUrl = _postInput.ImageUrl, Created = DateTime.Now };
                db.Posts.Add(post);
                if (await db.SaveChangesAsync() == 0)
                {
                    MessageBox.Show("something gone wrong");
                }
                else
                {
                    _posts.Add(post);
                    Clear();
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (_postInput.Id == -1)
                return;
            using (var db = new DbEntities())
            {
                var post = await db.Posts.FindAsync(_postInput.Id);
                post.Title = _postInput.Title;
                post.Description = _postInput.Description;
                post.ImageUrl = _postInput.ImageUrl;
                post.Updated = DateTime.Now;
                db.Entry(post).State = EntityState.Modified;
                if (await db.SaveChangesAsync() == 0)
                {
                    MessageBox.Show("something gone wrong");
                }
                else
                {
                    
                    var p = _posts.Single(x => x.Id == _postInput.Id);
                    var index = _posts.IndexOf(p);
                    _posts.Remove(p);
                    p.Title = post.Title;
                    p.Description = post.Description;
                    p.ImageUrl = post.ImageUrl;
                    p.Updated = post.Updated;
                    _posts.Insert(index, p);
                    Clear();
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (_postInput.Id == -1)
                return;
            using (var db = new DbEntities())
            {
                var todo = await db.Posts.FindAsync(_postInput.Id);
                db.Posts.Remove(todo);
                if (await db.SaveChangesAsync() == 0)
                {
                    MessageBox.Show("something gone wrong");
                }
                else
                {
                    var item = _posts.Single(x => x.Id == _postInput.Id);
                    _posts.Remove(item);
                    Clear();
                }
            }
        }

        private void Clear()
        {
            _postInput.Id = -1;
            _postInput.Title = "";
            _postInput.Description = "";
            _postInput.ImageUrl = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _postInput.Title = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            _postInput.Description = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            _postInput.ImageUrl = textBox3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
