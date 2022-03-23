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
    public partial class Rehber : Form
    {
        Dictionary<string, string> data;
        AccessDB accdb;
        string telefon;
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
        public Rehber(AccessDB accdb, Dictionary<string, string> data)
        {
            InitializeComponent();
            telefon = data["telefon"];
            lblBaslik.Text = "\"" + telefon + "\"nin Rehberi";
            data.Remove("dogumTarihi");
            this.data = data;
            this.accdb = accdb;
            button5_Click(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = accdb.GetRows("baglantilar", accdb.CreateExactCondition(new Dictionary<string, string>() { { "kayitEdenTelefon", telefon } }));
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string telefonTMP = data["telefon"];
            data.Remove("telefon");
            try
            {

                accdb.ExecuteQuery(accdb.CreateUpdateQueryString("baglantilar", data, accdb.CreateExactCondition(new Dictionary<string, string>() { { "kayitEdenTelefon", telefon }, { "telefon", telefonTMP } })));
                button5_Click(null, null);
                MessageBox.Show("Başarıyla \"" + telefon + "\"nin rehberindeki \"" + telefonTMP + "\" güncellendi!");
            }
            catch (Exception err)
            {
                MessageBox.Show("Hata! " + err.Message);
            }
            data["telefon"] = telefonTMP;
        }

        private void textChangedEvent(object sender, EventArgs e)
        {
            data["telefon"] = txtTelefon.Text;
            data["ad"] = txtAd.Text;
            data["soyad"] = txtSoyad.Text;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var selected = dataGridView1.CurrentRow.Cells;
            try
            {
                txtTelefon.Text = selected[1].Value.ToString();
                txtAd.Text = selected[2].Value.ToString();
                txtSoyad.Text = selected[3].Value.ToString();
                dtDogumTarihi.Value = DateTime.Parse(selected[4].Value.ToString());
            }
            catch (Exception)
            {

            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                data["kayitTarihi"] = DateTime.Now.ToShortDateString();
                data["kayitEdenTelefon"] = telefon;
                accdb.ExecuteQuery(accdb.CreateInsertIntoQueryString("baglantilar", data));
                data.Remove("kayitEdenTelefon");
                button5_Click(null, null);
            }
            catch (Exception err)
            {
                MessageBox.Show("Hata! " + err.Message);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            accdb.ExecuteQuery(accdb.CreateDeleteQueryString("baglantilar", accdb.CreateExactCondition(new Dictionary<string, string>() { { "telefon", data["telefon"] }, { "kayitEdenTelefon", telefon } })));
            this.button5_Click(null, null);
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = accdb.GetRows("baglantilar", accdb.CreateExactCondition(new Dictionary<string, string>() { { "kayitEdenTelefon", telefon } }) + " and (" + accdb.SubString(accdb.CreateLikeCondition(new string[] { "telefon", "ad", "soyad" }, txtAra.Text), 6, 9999) + ")");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
