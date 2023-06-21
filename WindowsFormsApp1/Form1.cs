using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.IO;
using System.Numerics;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {


        static String str = "Data Source=DESKTOP-M6F3TDU\\SQLEXPRESS;Initial Catalog=VT_OGRENCILER;Integrated Security=True";
        
        SqlConnection con = new SqlConnection(str);
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt = new DataTable();

        private string[] Ankara = { "Merkez", "Çankaya", "Beypazarı", "Polatlı", "Gölbaşı", "Sincan", "Mamak" };
        private string[] Eskisehir = { "Merkez", "Alpu", "Sivrihisar", "Odunpazarı", "Çifteler", "Seyitgazi", "Sarıcakaya" };
        private string[] Istanbul = { "Beşiktaş", "Bakırköy", "Beyoğlu", "Beylikdüzü", "Eyüp", "Kadıköy", "Şişli", "Üsküdar", "Zeytinburnu" };
        private string[] Izmir = { "Bornova", "Foça", "Karşıyaka", "Konak", "Urla", "Torbalı", "Çeşme", "Dikili" };


        public Form1(){InitializeComponent();}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    listBox1.Items.AddRange((object[])Ankara);
                    break;
                case 1:
                    listBox1.Items.AddRange((object[])Eskisehir);
                    break;
                case 2:
                    listBox1.Items.AddRange((object[])Istanbul);
                    break;
                default:
                    listBox1.Items.AddRange((object[])Izmir);
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            
            comboBox1.Items.AddRange((object[])new[] { "Ankara", "Eskisehir", "Istanbul", "Izmir" });

            comboBox2.Items.AddRange((object[])new[] { "buyuk ikon ", "kucuk ikon " });

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Tc", 50);
            listView1.Columns.Add("Adı", 50);
            listView1.Columns.Add("Soyadı", 50);
            listView1.Columns.Add("İli", 50);
            listView1.Columns.Add("İlçesi", 50);
            listView1.Columns.Add("Cinsiyat", 50);
            listView1.Columns.Add("İkon", 50);
            listView1.Columns.Add("Müzik Dinlemek", 50);
            listView1.Columns.Add("Kitap Okumak", 50);
            listView1.Columns.Add("Sinema", 50);

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
        }

        private void update(BigInteger tc, String newName, String newLName, String newprovince, String newDistrict, String newSex, int newIcon, Boolean newHobi1, Boolean newHobi2, Boolean newHobi3)
        {
            string sql = "UPDATE ogrenci SET  tc='" + tc + "',adi='" + newName + "',soyadi='" + newLName + "',ili='" + newprovince + "',ilcesi='" + newDistrict + "',cinsiyet='" + newSex + "',ikon='" + newIcon + "',muzik='" + newHobi1 + "',kitap='" + newHobi2 + "',sinema='" + newHobi3 + "' WHERE tc=" + tc + "";
            cmd = new SqlCommand(sql, con);
            try
            {
                con.Open();

                adapter = new SqlDataAdapter(cmd);
                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;
                if (MessageBox.Show("Guncellme islemi basari ile gerceklesti.", "Bligi", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                    {

                        userItemsDelete();
                    }
                }

                con.Close();

                retrieve();
            }
            catch(Exception err)
            {
                con.Close();
                MessageBox.Show(err.Message);
            }
        }

        private void delete(BigInteger tc)
        {
            String sql = "DELETE FROM ogrenci WHERE tc=" + tc + "";
            cmd = new SqlCommand(sql, con);

            try
            {
                con.Open();

                adapter = new SqlDataAdapter(cmd);
                adapter.DeleteCommand = con.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;

                if(MessageBox.Show(tc + " Tc kimlik numarali kaydi silmek istediğinize emin misin?", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        userItemsDelete();
                        
                    }
                }

                con.Close();
                retrieve();
            }catch(Exception err)
            {
                MessageBox.Show(err.Message);
                con.Close();
            }
        }

        private void populateLV(String tc, String adi, String soyadi, String ili, String ilcesi, String cinsiyet, String ikon, String muzik, String kitap, String sinema)
        {
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(15, 15);
            String[] paths = { };

            paths = Directory.GetFiles(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/small");
            try
            {
                foreach (String path in paths)
                {
                    imgs.Images.Add(Image.FromFile(path));
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            listView1.SmallImageList = imgs;

            String[] row = {tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema };
            int ik = 5;
            if (ikon == "1") ik = 0;
            else if (ikon == "2") ik = 1;
            else if (ikon == "3") ik = 2;
            else if (ikon == "4") ik = 3;
            listView1.Items.Add(new ListViewItem(row, ik));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked) { }; 
            string s1 = ""; 
            string s2 = ""; 
            if (radioButton1.Checked) s2 = radioButton1.Text; 
            else if (radioButton2.Checked) s2 = radioButton2.Text; 
            if (radioButton3.Checked) s1 = radioButton3.Text; 
            else if (radioButton4.Checked) s1 = radioButton4.Text; 
            else if (radioButton5.Checked) s1 = radioButton5.Text; 
            else if (radioButton6.Checked) s1 = radioButton6.Text; 
   

            try
            {
                con.Open();
                string qurey = "INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES (@tc, @adi, @soyadi, @ili, @ilcesi, @cinsiyet, @ikon, @muzik, @kitap, @sinema)";
                SqlCommand cmd = new SqlCommand(qurey, con);
                cmd.Parameters.AddWithValue("@tc", textBox3.Text);
                cmd.Parameters.AddWithValue("@adi", textBox1.Text);
                cmd.Parameters.AddWithValue("@soyadi", textBox2.Text);
                cmd.Parameters.AddWithValue("@ili", comboBox1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@ilcesi", listBox1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@cinsiyet", s2);
                cmd.Parameters.AddWithValue("@ikon", s1);
                cmd.Parameters.AddWithValue("@muzik", checkBox1.Checked ? true : false);
                cmd.Parameters.AddWithValue("@kitap", checkBox2.Checked ? true : false);
                cmd.Parameters.AddWithValue("@sinema", checkBox1.Checked ? true : false);
                cmd.ExecuteNonQuery();
               

                retrieve();
                con.Close();
                

            }
            catch(Exception err) {

                MessageBox.Show(err.Message, "!", MessageBoxButtons.OK);
                con.Close();
               
            };

            userItemsDelete();
            con.Close();

        }

        private void retrieve()
        {
            listView1.Items.Clear();
            String sql = "SELECT * FROM ogrenci";
            cmd = new SqlCommand(sql, con);

            try
            {

                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {   
                    string hobi1, hobi2, hobi3;
                    hobi1 = row[7].ToString() == "True" ? "Evet" : "Hayır";
                    hobi2 = row[8].ToString() == "True" ? "Evet" : "Hayır";
                    hobi3 = row[9].ToString() == "True" ? "Evet" : "Hayır";
                    populateLV(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), hobi1, hobi2, hobi3);
                }

                con.Close();
                dt.Rows.Clear();

            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message, "!", MessageBoxButtons.OK);
                con.Close();
            };

        }

        private void userItemsDelete()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            radioButton1.Checked = false;
            radioButton3.Checked = false;
            radioButton2.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
        }

        private void bilgileriTemizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userItemsDelete();
        }

        private void listeyiTemizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void cikisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            userItemsDelete();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            int HeaderWidth = (listView1.Parent.Width - 2) / listView1.Columns.Count;
            foreach (ColumnHeader header in listView1.Columns)
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    header.Width = HeaderWidth;
                }
                else { header.Width = -2; }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            retrieve();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox3.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text;
            comboBox1.Text = listView1.SelectedItems[0].SubItems[3].Text;
            listBox1.Text = listView1.SelectedItems[0].SubItems[4].Text;

            if(listView1.SelectedItems[0].SubItems[5].Text.ToLower() == "erkek")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else if(listView1.SelectedItems[0].SubItems[5].Text.ToLower() == "kadin")
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }

            if (listView1.SelectedItems[0].SubItems[6].Text == "1")
            {
                radioButton3.Checked = true;
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
            }
            else if (listView1.SelectedItems[0].SubItems[6].Text == "2")
            {
                radioButton3.Checked = false;
                radioButton4.Checked = true;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
            }
            else if (listView1.SelectedItems[0].SubItems[6].Text == "3")
            {
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = true;
                radioButton6.Checked = false;
            }
            else if (listView1.SelectedItems[0].SubItems[6].Text == "4")
            {
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = true;
            }
            if (listView1.SelectedItems[0].SubItems[7].Text.ToLower() == "evet")
            {
                checkBox1.Checked = true;
            }
            else checkBox1.Checked = false;

            if (listView1.SelectedItems[0].SubItems[8].Text.ToLower() == "evet")
            {
                checkBox2.Checked = true;
            }
            else checkBox2.Checked = false;

           if (listView1.SelectedItems[0].SubItems[9].Text.ToLower() == "evet")
            {
                checkBox3.Checked = true;
            }
            else checkBox3.Checked = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {

           var tc = BigInteger.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            String newName = textBox1.Text;
            String newLName = textBox2.Text;
            String newprovince = comboBox1.SelectedItem.ToString();
            String newDistrict = listBox1.SelectedItem.ToString();
            String newSex = radioButton1.Checked ? "erkek" : "kadin";


            int newIcon = 0 ;
            if (radioButton3.Checked) newIcon = 1;
            else if (radioButton4.Checked) newIcon = 2;
            else if (radioButton5.Checked) newIcon = 3;
            else if (radioButton6.Checked) newIcon = 4;

            Boolean newHobi1 = false;
            Boolean newHobi2 = false;
            Boolean newHobi3 = false;
         

            if (checkBox1.Checked) newHobi1 = true;
            if (checkBox2.Checked) newHobi2 = true;
            if (checkBox3.Checked) newHobi3 = true;

            update(tc, newName, newLName, newprovince, newDistrict, newSex, newIcon, newHobi1, newHobi2, newHobi3);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tc = BigInteger.Parse(listView1.SelectedItems[0].SubItems[0].Text);
                        
            delete(tc);
        }
    }
    }

