﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BukuTamu
{
    public partial class ad_alumni : UserControl
    {

        static MySqlConnection conn;
        static MySqlDataAdapter sdaUser;
        static DataSet dsUser;

        public ad_alumni()
        {
            InitializeComponent();
            queryTamu();
            photo();
        }

        public void cari()
        {
            try
            {
                queryCari();
                conn.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Information");
            }
        }

        public void queryCari()
        {
            string query = "SELECT * FROM tb_alumni WHERE " +
                                      " id_alumni LIKE '%" + tbCari.Text + "%'" +
                                      " OR nama_alumni LIKE '%" + tbCari.Text + "%'" +
                                      " OR tahun_lulus LIKE '%" + tbCari.Text + "%'" +
                                      " OR jurusan LIKE '%" + tbCari.Text + "%'" +
                                      " OR no_hp LIKE '%" + tbCari.Text + "%'" +
                                      " OR email LIKE '%" + tbCari.Text + "%'" +
                                      " OR keperluan LIKE '%" + tbCari.Text + "%'" +
                                      " OR ttd LIKE '%" + tbCari.Text + "%'" +
                                      " OR waktu LIKE '%" + tbCari.Text + "%'" +
                                      " ORDER BY waktu DESC";
            tampilTamu(query, "tb_ortu", dataGridView1);
        }

        public void queryTamu()
        {
            string query = "SELECT * FROM tb_alumni ORDER BY waktu DESC";
            tampilTamu(query, "tb_alumni", dataGridView1);
        }

        public static DataSet DataGrid(String query, String table)
        {
            conn = koneksi.getKoneksi();
            sdaUser = new MySqlDataAdapter(query, conn);
            dsUser = new DataSet();

            try
            {
                conn.Open();
                sdaUser.Fill(dsUser, table);
                return dsUser;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public void tampilTamu(string query, string table, DataGridView DG)
        {
            try
            {
                dataGridView1.DataSource = DataGrid(query, table);
                DG.DataMember = table;
                DG.Columns[0].HeaderText = "ID TAMU";
                DG.Columns[0].Width = 50;
                DG.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DG.Columns[1].HeaderText = "NAMA ALUMNI";
                DG.Columns[1].Width = 140;
                DG.Columns[2].HeaderText = "TAHUN LULUS";
                DG.Columns[2].Width = 50;
                DG.Columns[3].HeaderText = "JURUSAN";
                DG.Columns[3].Width = 70;
                DG.Columns[4].HeaderText = "NO HP";
                DG.Columns[4].Width = 90;
                DG.Columns[5].HeaderText = "EMAIL";
                DG.Columns[5].Width = 120;
                DG.Columns[6].HeaderText = "KEPERLUAN";
                DG.Columns[6].Width = 150;
                DG.Columns[7].HeaderText = "TTD";
                DG.Columns[7].Width = 180;
                DG.Columns[8].HeaderText = "WAKTU";
                DG.Columns[8].Width = 130;
                DG.Columns[9].HeaderText = "FOTO";
                DG.Columns[9].Width = 180;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (l_id.Text == "-")
            {
                MessageBox.Show("Silahkan pilih data untuk dihapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult dr = MessageBox.Show("Apakah Anda yakin ingin menghapus?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        MySqlConnection conn = koneksi.getKoneksi();
                        MySqlCommand command = conn.CreateCommand();
                        command.CommandText = "DELETE FROM tb_alumni WHERE id_alumni='" + l_id.Text + "'";
                        MySqlDataReader reader;
                        conn.Open();
                        reader = command.ExecuteReader();
                        MessageBox.Show("Data berhasil dihapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        l_id.Text = "-";
                        queryTamu();
                        conn.Close();
                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.Message, "Terjadi Kesalahan Query");
                    }
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                l_id.Text = row.Cells["id_alumni"].Value.ToString();
            }
            catch (Exception a)
            {

            }
        }

        private void tbCari_KeyPress(object sender, KeyPressEventArgs e)
        {
            cari();
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            queryTamu();
            l_id.Text = "-";
        }

        private void ad_alumni_Load(object sender, EventArgs e)
        {
            photo();
        }

        public void photo()
        {
            dataGridView1.RowTemplate.Height = 170;
            dataGridView1.AllowUserToAddRows = false;

            DataGridViewImageColumn imageColumns = new DataGridViewImageColumn();
            imageColumns.ImageLayout = DataGridViewImageCellLayout.Stretch;
               sdaUser.Dispose();
        }

        private void btCetak_Click(object sender, EventArgs e)
        {
            Report.cetak_lap_alumni alumni = new Report.cetak_lap_alumni();
            alumni.Show();
        }

    }
}
