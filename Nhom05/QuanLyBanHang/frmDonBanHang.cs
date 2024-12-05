using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class frmDonBanHang : Form
    {
        string sCon = "Data Source=LAPTOP-3IP4RO36;Initial Catalog=QLBanhang;Integrated Security=True;TrustServerCertificate=True";
        public frmDonBanHang()
        {
            InitializeComponent();
        }

        private void frmDonBanHang_Load(object sender, EventArgs e)
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
            //lấy dữ liệu về 
            string sQuerry = "Select * from DonBanHang";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuerry, con);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "DonBanHang");

            dgvDanhSachDonHang.DataSource = ds.Tables["DonBanHang"];

            con.Close();

            string s1Querry = "Select * from DonBanHangChiTiet";
            SqlDataAdapter adapter1 = new SqlDataAdapter(s1Querry, con);

            DataSet ds1 = new DataSet();

            adapter1.Fill(ds1, "DonBanHangChiTiet");

            dgvDanhSachChiTiet.DataSource = ds1.Tables["DonBanHangChiTiet"];

            con.Close();

            // Load dữ liệu bảng DonBanHangChiTiet
            try
            {
                string sQueryDonHangChiTiet = "SELECT * FROM DonBanHangChiTiet";
                SqlDataAdapter adapterDonHangChiTiet = new SqlDataAdapter(sQueryDonHangChiTiet, con);
                DataSet dsDonHangChiTiet = new DataSet();
                adapterDonHangChiTiet.Fill(dsDonHangChiTiet, "DonBanHangChiTiet");
                dgvDanhSachChiTiet.DataSource = dsDonHangChiTiet.Tables["DonBanHangChiTiet"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu DonBanHangChiTiet: " + ex.Message);
            }

            // Load dữ liệu vào cmbMaDVGH từ DonViGiaoHang
            try
            {
                string sQueryDonViGiaoHang = "SELECT MaDV FROM DonViGiaoHang";
                SqlDataAdapter adapterDonViGiaoHang = new SqlDataAdapter(sQueryDonViGiaoHang, con);
                DataTable dtDonViGiaoHang = new DataTable();
                adapterDonViGiaoHang.Fill(dtDonViGiaoHang);

                // Kiểm tra nếu dữ liệu tồn tại
                if (dtDonViGiaoHang.Rows.Count > 0)
                {
                    cmbMaDVGH.DisplayMember = "MaDV"; // Hiển thị cột MaDV trong ComboBox
                    cmbMaDVGH.ValueMember = "MaDV";   // Gán giá trị thật là MaDV
                    cmbMaDVGH.DataSource = dtDonViGiaoHang; // Gắn dữ liệu vào ComboBox
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu mã đơn vị vận chuyển trong bảng DonViVanChuyen.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu DonViGiaoHang: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            // Load dữ liệu vào cmbMaSanPham từ bảng SanPham
            try
            {
                string sQuerySanPham = "SELECT MaSP FROM SanPham";  // Truy vấn lấy MaSP từ bảng SanPham
                SqlDataAdapter adapterSanPham = new SqlDataAdapter(sQuerySanPham, con);
                DataTable dtSanPham = new DataTable();
                adapterSanPham.Fill(dtSanPham);

                if (dtSanPham.Rows.Count > 0)
                {
                    cmbMaSanPham.DisplayMember = "MaSP";  // Hiển thị cột MaSP trong ComboBox
                    cmbMaSanPham.ValueMember = "MaSP";    // Gán giá trị thật là MaSP
                    cmbMaSanPham.DataSource = dtSanPham;  // Gắn dữ liệu vào ComboBox
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu mã sản phẩm trong bảng SanPham.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu SanPham: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        // Sự kiện khi chọn mã sản phẩm từ ComboBox
        private void cmbMaSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem ComboBox có dữ liệu hay không
            if (cmbMaSanPham.SelectedIndex == -1)
                return; // Nếu không có lựa chọn nào, thoát khỏi hàm

            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();

                // Lấy mã sản phẩm đã chọn từ ComboBox
                string maSP = cmbMaSanPham.SelectedValue.ToString();

                // Truy vấn để lấy tên sản phẩm theo mã sản phẩm
                string sQuery = "SELECT TenSP FROM SanPham WHERE MaSP = @MaSP";
                SqlCommand cmd = new SqlCommand(sQuery, con);
                cmd.Parameters.AddWithValue("@MaSP", maSP);

                // Thực thi truy vấn và lấy tên sản phẩm
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Hiển thị tên sản phẩm vào TextBox
                    txtTenSanPham.Text = reader["TenSP"].ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm với mã này.");
                    txtTenSanPham.Text = ""; // Nếu không có sản phẩm, xóa tên trong TextBox
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tên sản phẩm: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

    }
}
