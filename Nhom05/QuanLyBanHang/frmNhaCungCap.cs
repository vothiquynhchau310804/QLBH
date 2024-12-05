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
namespace QuanLyBanHang
{
    public partial class frmNhaCungCap : Form
    {
        string sCon = "Data Source=LAPTOP-3IP4RO36;Initial Catalog=QLBanhang;Integrated Security=True;TrustServerCertificate=True";
        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình kết nối DB");
            }
           

            //lấy dữ liệu về 
            string sQuerry = "Select * from NhaCungCap";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuerry, con);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "NhaCungCap");

            dataGridView1.DataSource = ds.Tables["NhaCungCap"];

            con.Close();

            try
            {
                con.Open();

                // Truy vấn lấy danh sách mã nhà cung cấp
                string sQuery = "SELECT MaNCC FROM NhaCungCap";
                SqlCommand cmd = new SqlCommand(sQuery, con);

                // Lấy dữ liệu vào DataTable
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter1.Fill(dt);

                // Điền các mã nhà cung cấp vào comboBox
                comboBoxMaNCC.DisplayMember = "MaNCC"; // Hiển thị tên cột mã nhà cung cấp
                comboBoxMaNCC.ValueMember = "MaNCC";   // Giá trị sử dụng là mã nhà cung cấp
                comboBoxMaNCC.DataSource = dt;         // Gán dữ liệu vào comboBox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình kết nối DB: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void frmNhaCungCap_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Xin chào, hẹn gặp lại lần sau!!!", "Thông báo");
        }

        private void btnLuuNCC_MouseClick(object sender, MouseEventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình kết nối DB");
            }
            //bước 2
            string sMaNCC = txtMaNCC.Text;
            string sTenNCC = txtTenNCC.Text;
            string sSdtNCC = txtSDTNCC.Text;
            string sDiaChiNCC = txtDiaChiNCC.Text;

            string sQuery = "insert into NhaCungCap values (@mancc, @tenncc, @sdtncc, @diachincc)";
            SqlCommand cmd = new SqlCommand(sQuery, con);
            cmd.Parameters.AddWithValue("@mancc", sMaNCC);
            cmd.Parameters.AddWithValue("@tenncc", sTenNCC);
            cmd.Parameters.AddWithValue("@sdtncc", sSdtNCC);
            cmd.Parameters.AddWithValue("@diachincc", sDiaChiNCC);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm mới thành công!!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình thêm mới!!!");
            }
            con.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Gán dữ liệu từ hàng được chọn vào các TextBox
                txtMaNCC.Text = row.Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value.ToString();
                txtSDTNCC.Text = row.Cells["SDTNCC"].Value.ToString();
                txtDiaChiNCC.Text = row.Cells["DiaChiNCC"].Value.ToString();
            }
            txtMaNCC.Enabled = false;
        }

        private void btbSuaNCC_Click(object sender, EventArgs e)
        {
            // Thu thập dữ liệu mới từ các TextBox
            string sMaNCC = txtMaNCC.Text;
            string sTenNCC = txtTenNCC.Text;
            string sSdtNCC = txtSDTNCC.Text;
            string sDiaChiNCC = txtDiaChiNCC.Text;

            // Chuỗi kết nối
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string sQuery = "UPDATE NhaCungCap SET TenNCC = @tenncc, SDTNCC = @sdtncc, DiaChiNCC = @diachincc WHERE MaNCC = @mancc";

                // Tạo command và thêm tham số
                SqlCommand cmd = new SqlCommand(sQuery, con);
                cmd.Parameters.AddWithValue("@mancc", sMaNCC);
                cmd.Parameters.AddWithValue("@tenncc", sTenNCC);
                cmd.Parameters.AddWithValue("@sdtncc", sSdtNCC);
                cmd.Parameters.AddWithValue("@diachincc", sDiaChiNCC);

                // Thực thi câu lệnh SQL
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật thông tin nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Gọi lại LoadData để làm mới DataGridView
                    frmNhaCungCap_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Không có thay đổi nào được thực hiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi khi cập nhật thông tin nhà cung cấp!\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnXoaNCC_Click(object sender, EventArgs e)
        {
            // Lấy mã nhà cung cấp cần xóa
            string sMaNCC = txtMaNCC.Text;

            // Hiển thị thông báo xác nhận
            DialogResult dialogResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhà cung cấp với mã '{sMaNCC}' không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                // Chuỗi kết nối
                SqlConnection con = new SqlConnection(sCon);
                try
                {
                    con.Open();

                    // Thực hiện câu lệnh DELETE
                    string sQuery = "DELETE FROM NhaCungCap WHERE MaNCC = @mancc";
                    SqlCommand cmd = new SqlCommand(sQuery, con);
                    cmd.Parameters.AddWithValue("@mancc", sMaNCC);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Gọi lại LoadData để làm mới DataGridView
                        frmNhaCungCap_Load(sender, e);

                        // Xóa thông tin trong TextBox
                        txtMaNCC.Clear();
                        txtTenNCC.Clear();
                        txtSDTNCC.Clear();
                        txtDiaChiNCC.Clear();
                        txtMaNCC.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi khi xóa nhà cung cấp!\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btnTimNCC_Click(object sender, EventArgs e)
        {
            if (comboBoxMaNCC.SelectedIndex >= 0)
            {
                string maNCC = comboBoxMaNCC.SelectedValue.ToString();
                SqlConnection con = new SqlConnection(sCon);

                try
                {
                    con.Open();

                    // Truy vấn để lấy thông tin chi tiết nhà cung cấp theo mã
                    string sQuery = "SELECT * FROM NhaCungCap WHERE MaNCC = @maNCC";
                    SqlCommand cmd = new SqlCommand(sQuery, con);
                    cmd.Parameters.AddWithValue("@maNCC", maNCC);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtMaNCC.Text = reader["MaNCC"].ToString();
                        txtTenNCC.Text = reader["TenNCC"].ToString();
                        txtSDTNCC.Text = reader["SDTNCC"].ToString();
                        txtDiaChiNCC.Text = reader["DiaChiNCC"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp với mã " + maNCC, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi trong quá trình tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn mã nhà cung cấp trước khi tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
