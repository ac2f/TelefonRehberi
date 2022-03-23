using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelefonRehberi
{
    public partial class AnaForm : Form
    {
        AccessDB accdb;
        Dictionary<string, string> data = new Dictionary<string, string>() {};
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if (Control.MouseButtons != MouseButtons.Right)
                    {
                        if ((int)m.Result == HTCLIENT)
                            m.Result = (IntPtr)HTCAPTION;
                    }
                    return;
            }
            base.WndProc(ref m);
        }
        public AnaForm(AccessDB accdb)
        {
            InitializeComponent();
            this.accdb = accdb;
            dataGridView1.DataSource = accdb.GetRows("telefonlar");
            textChangedEvent(null, null);
        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var selected = dataGridView1.CurrentRow.Cells;
            try
            {
                txtTelefon.Text = selected[0].Value.ToString();
                txtAd.Text = selected[1].Value.ToString();
                txtSoyad.Text = selected[2].Value.ToString();
                dtDogumTarihi.Value = DateTime.Parse(selected[3].Value.ToString());
            }
            catch (Exception)
            {

            }

        }

        private void textChangedEvent(object sender, EventArgs e)
        {
            data["telefon"] = txtTelefon.Text;
            data["ad"] = txtAd.Text;
            data["soyad"] = txtSoyad.Text;
            data["dogumTarihi"] = dtDogumTarihi.Value.ToShortDateString();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = accdb.GetRows("telefonlar", accdb.CreateLikeCondition(new string[] { "telefon", "ad", "soyad" }, txtAra.Text));
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                accdb.ExecuteQuery(accdb.CreateInsertIntoQueryString("telefonlar", data));
                button5_Click(null, null);
            }
            catch (Exception err)
            {
                MessageBox.Show("Hata! " + err.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = accdb.GetRows("telefonlar");
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string telefonTMP = data["telefon"];
            data.Remove("telefon");
            try
            {
                accdb.ExecuteQuery(accdb.CreateUpdateQueryString("telefonlar", data, accdb.CreateExactCondition(new Dictionary<string, string>() { { "telefon", telefonTMP } })));
                button5_Click(null, null);
                MessageBox.Show("Başarıyla " + telefonTMP + "\" güncellendi!");
            }
            catch (Exception err)
            {
                MessageBox.Show("Hata! " + err.Message);
            }
            data["telefon"] = telefonTMP;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            accdb.ExecuteQuery(accdb.CreateDeleteQueryString("telefonlar", accdb.CreateExactCondition(new Dictionary<string, string>() { { "telefon", txtTelefon.Text } })));
            this.button5_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Rehber(accdb, data).ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
