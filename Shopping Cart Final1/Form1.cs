using Microsoft.Data.SqlClient;
using Sunny.UI;
using System.Data;
using System.Drawing.Printing;

namespace Shopping_Cart_Final1
{
    public partial class Form1 : Form
    {
        private readonly (string Name, decimal Price)[] _menuItems = new[]
    {
            ("Burger",       5.00m),   // button1
            ("Pizza",       10.99m),   // button2
            ("Hotdog",       2.00m),   // button3
            ("Sandwich",     1.00m),   // button4
            ("Fries",        2.00m),   // button5
            ("Fries Chicken",3.00m),   // button6
            ("Sushi",       15.00m),   // button7
            ("Salad",        1.00m),   // button8
            ("Spaghetti",    2.00m),   // button9
            ("Noddle",       2.00m),   // button10
            ("Taco",         1.00m),   // button11
            ("Steak",        5.00m),   // button12
            ("CocaCola",     1.00m),   // button13
            ("Cappuccino",   1.50m),   // button14
            ("Matcha",       2.00m),   // button15
            ("Smoothie",     1.00m),   // button16
            ("GreenTea",     1.00m),   // button17
            ("Energy Drink", 1.00m),   // button18
            ("Water",        0.25m),   //button19
            ("Beer",         2.00m),   // button21
            ("Orange Juice", 1.00m),   // button24
        };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.WindowState = FormWindowState.Maximized;
            
            // Payment Method
            cboPaymentMethod.Items.Clear();
            cboPaymentMethod.Items.AddRange(new string[] { "Cash", "ABA Bank", "Wing", "Acleda X" });
            cboPaymentMethod.SelectedIndex = 0;

            // ភ្ជាប់ button1 → button21 ទៅ event handler តែមួយ
            AttachFoodButtonEvents();

            // Load data
            LoadStockData();
            LoadCustomerData();
        }

        private void AttachFoodButtonEvents()
        {
            // buttons ទាំងអស់ ដែល Name = button1 → button21
            Button[] foodButtons = new Button[]
            {
                button1,  button2,  button3,  button4,  button5,
                button6,  button7,  button8,  button9,  button10,
                button11, button12, button13, button14, button15,
                button16, button17, button18, button19, button20,
                button21, button22, button23, button24
            };

            for (int i = 0; i < foodButtons.Length; i++)
            {
                foodButtons[i].Tag = i; // រក្សា index
                foodButtons[i].Click += FoodButton_Click;
            }
        }

        /// <summary>
        /// Event Handler តែមួយ សម្រាប់ button ម្ហូប/ភេសជ្ជៈទាំងអស់
        /// </summary>
        private void FoodButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag == null) return;

            int index = (int)btn.Tag;
            if (index < 0 || index >= _menuItems.Length) return;

            AddItemToSaleCart(_menuItems[index].Name, _menuItems[index].Price);
        }

        /// <summary>
        /// បន្ថែមមុខទំនិញចូល Sale Cart
        /// ប្រសិនបើមុខនោះមានក្នុង Cart រួចហើយ ដំឡើង Qty ដោយស្វ័យប្រវត្តិ
        /// </summary>
        private void AddItemToSaleCart(string itemName, decimal price)
        {
            foreach (DataGridViewRow row in dgvOrderCart.Rows)
            {
                if (row.Cells[0].Value?.ToString() == itemName)
                {
                    int newQty = Convert.ToInt32(row.Cells[2].Value) + 1;
                    row.Cells[2].Value = newQty;
                    row.Cells[3].Value = price * newQty;
                    CalculateSaleTotal();
                    return;
                }
            }
            dgvOrderCart.Rows.Add(itemName, price, 1, price);
            CalculateSaleTotal();
        }

        private void CalculateSaleTotal()
        {
            decimal subTotal = 0;
            foreach (DataGridViewRow row in dgvOrderCart.Rows)
            {
                if (row.Cells[3].Value != null)
                    subTotal += Convert.ToDecimal(row.Cells[3].Value);
            }
            decimal tax = subTotal * 0.1m;
            decimal total = subTotal + tax;

            lblSubTotal.Text = "$" + subTotal.ToString("0.00");
            lblTax.Text = "$" + tax.ToString("0.00");
            lblTotal.Text = "$" + total.ToString("0.00");
        }

        private void btnCheackout_Click(object sender, EventArgs e)
        {
            if (dgvOrderCart.Rows.Count == 0)
            {
                MessageBox.Show("សូមជ្រើសរើសមុខម្ហូបជាមុនសិន!",
                    "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboPaymentMethod.SelectedIndex < 0)
            {
                MessageBox.Show("សូមជ្រើសរើសវិធីទូទាត់ប្រាក់!",
                    "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string customerName = string.IsNullOrWhiteSpace(txtCustomerName.Text)
                ? "General Guest" : txtCustomerName.Text.Trim();
            string paymentMethod = cboPaymentMethod.Text;

            if (!decimal.TryParse(lblSubTotal.Text.Replace("$", ""), out decimal subTotal) ||
                !decimal.TryParse(lblTax.Text.Replace("$", ""), out decimal tax) ||
                !decimal.TryParse(lblTotal.Text.Replace("$", ""), out decimal total))
            {
                MessageBox.Show("តម្លៃគណនាមានបញ្ហា!", "កំហុស");
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Orders (CustomerName, SubTotal, Tax, TotalAmount, PaymentMethod, OrderDate, Status)
                        OUTPUT INSERTED.OrderID
                        VALUES (@name, @sub, @tax, @total, @pay, @date, 'Completed')", conn, trans);
                    cmd.Parameters.AddWithValue("@name", customerName);
                    cmd.Parameters.AddWithValue("@sub", subTotal);
                    cmd.Parameters.AddWithValue("@tax", tax);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@pay", paymentMethod);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    int orderID = (int)cmd.ExecuteScalar();

                    foreach (DataGridViewRow row in dgvOrderCart.Rows)
                    {
                        if (row.Cells[0].Value == null) continue;
                        string item = row.Cells[0].Value.ToString();
                        decimal itemPrice = Convert.ToDecimal(row.Cells[1].Value);
                        int qty = Convert.ToInt32(row.Cells[2].Value);
                        decimal itemSum = Convert.ToDecimal(row.Cells[3].Value);

                        new SqlCommand(@"INSERT INTO OrderDetails (OrderID,ItemName,Qty,Price,SubTotal)
                            VALUES (@id,@item,@qty,@price,@sum)", conn, trans)
                        {
                            Parameters = {
                                new SqlParameter("@id",    orderID),
                                new SqlParameter("@item",  item),
                                new SqlParameter("@qty",   qty),
                                new SqlParameter("@price", itemPrice),
                                new SqlParameter("@sum",   itemSum)
                            }
                        }.ExecuteNonQuery();

                        new SqlCommand(@"UPDATE Products SET StockQty=StockQty-@qty WHERE ItemName=@item",
                            conn, trans)
                        {
                            Parameters = {
                                new SqlParameter("@qty",  qty),
                                new SqlParameter("@item", item)
                            }
                        }.ExecuteNonQuery();
                    }

                    trans.Commit();
                    MessageBox.Show($"ការទូទាត់តាម [{paymentMethod}] ជោគជ័យ!\nOrder ID: #{orderID}",
                        "ជោគជ័យ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearSaleForm();
                    LoadStockData();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ប្រតិបត្តិការបរាជ័យ៖ " + ex.Message, "កំហុស");
                }
            }
        }

        private void ClearSaleForm()
        {
            dgvOrderCart.Rows.Clear();
            txtCustomerName.Clear();
            cboPaymentMethod.SelectedIndex = 0;
            lblSubTotal.Text = "$0.00";
            lblTax.Text = "$0.00";
            lblTotal.Text = "$0.00";
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            dgvOrderCart.Rows.Clear();
            CalculateSaleTotal();
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dgvOrderCart.Rows.Count == 0)
            {
                MessageBox.Show("គ្មានទំនិញក្នុង Cart!", "ព្រមាន");
                return;
            }
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintReceiptPage;
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.ShowDialog();
        }

        private void PrintReceiptPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontTitle = new Font("Arial", 12, FontStyle.Bold);
            Font fontHeader = new Font("Arial", 10, FontStyle.Bold);
            Font fontBody = new Font("Arial", 9, FontStyle.Regular);
            Font fontBold = new Font("Arial", 9, FontStyle.Bold);
            const float x = 10f;
            float y = 10f;
            const float lineH = 20f;
            string sep = new string('-', 50);

            g.DrawString("YUMMY EXPRESS FOOD", fontTitle, Brushes.Black, x + 20, y); y += lineH + 5;
            g.DrawString(sep, fontBody, Brushes.Black, x, y); y += lineH;
            g.DrawString($"Date: {DateTime.Now:yyyy-MM-dd HH:mm}", fontBody, Brushes.Black, x, y); y += lineH;
            g.DrawString($"Customer: {(string.IsNullOrWhiteSpace(txtCustomerName.Text) ? "General Guest" : txtCustomerName.Text.Trim())}",
                fontBody, Brushes.Black, x, y); y += lineH;
            g.DrawString($"Payment : {cboPaymentMethod.Text}", fontBody, Brushes.Black, x, y); y += lineH;
            g.DrawString(sep, fontBody, Brushes.Black, x, y); y += lineH;

            g.DrawString("Item", fontHeader, Brushes.Black, x, y);
            g.DrawString("Qty", fontHeader, Brushes.Black, x + 130, y);
            g.DrawString("Price", fontHeader, Brushes.Black, x + 165, y);
            g.DrawString("Total", fontHeader, Brushes.Black, x + 215, y);
            y += lineH;
            g.DrawString(sep, fontBody, Brushes.Black, x, y); y += lineH;

            foreach (DataGridViewRow row in dgvOrderCart.Rows)
            {
                if (row.Cells[0].Value == null) continue;
                string name = row.Cells[0].Value.ToString();
                decimal price = Convert.ToDecimal(row.Cells[1].Value);
                int qty = Convert.ToInt32(row.Cells[2].Value);
                decimal rowTotal = Convert.ToDecimal(row.Cells[3].Value);
                if (name.Length > 16) name = name.Substring(0, 16) + "..";

                g.DrawString(name, fontBody, Brushes.Black, x, y);
                g.DrawString(qty.ToString(), fontBody, Brushes.Black, x + 130, y);
                g.DrawString($"${price:0.00}", fontBody, Brushes.Black, x + 165, y);
                g.DrawString($"${rowTotal:0.00}", fontBody, Brushes.Black, x + 215, y);
                y += lineH;
            }

            g.DrawString(sep, fontBody, Brushes.Black, x, y); y += lineH;
            g.DrawString("Sub Total:", fontBody, Brushes.Black, x + 110, y); g.DrawString(lblSubTotal.Text, fontBody, Brushes.Black, x + 215, y); y += lineH;
            g.DrawString("Tax (10%):", fontBody, Brushes.Black, x + 110, y); g.DrawString(lblTax.Text, fontBody, Brushes.Black, x + 215, y); y += lineH;
            g.DrawString("Total:", fontBold, Brushes.Black, x + 110, y); g.DrawString(lblTotal.Text, fontBold, Brushes.Black, x + 215, y); y += lineH + 15;
            g.DrawString("Thank You! Please Come Again.", fontHeader, Brushes.Black, x + 25, y);
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            CalcPurchaseItemTotal();
        }

        private void cboQty_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcPurchaseItemTotal();
        }

        private void CalcPurchaseItemTotal()
        {
            if (decimal.TryParse(txtUnitPrice.Text, out decimal p) &&
                decimal.TryParse(cboQty.Text, out decimal q))
                txtTotal.Text = (p * q).ToString("0.00");
            else
                txtTotal.Text = "0.00";
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboProduct.Text) ||
                string.IsNullOrEmpty(cboQty.Text) ||
                string.IsNullOrEmpty(txtUnitPrice.Text))
            {
                MessageBox.Show("សូមជ្រើសរើស ផលិតផល ចំនួន និងតម្លៃ!",
                    "ព្រមាន", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(cboQty.Text, out int qty) ||
                !decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) ||
                !decimal.TryParse(txtTotal.Text, out decimal total))
            {
                MessageBox.Show("ទិន្នន័យចំនួន ឬតម្លៃមិនត្រឹមត្រូវ!", "ព្រមាន");
                return;
            }

            dgvPurchaseCart.Rows.Add(cboProduct.Text, qty, unitPrice, total);
            CalcPurchaseInvoiceTotal();
        }

        private void CalcPurchaseInvoiceTotal()
        {
            decimal sub = 0;
            foreach (DataGridViewRow row in dgvPurchaseCart.Rows)
                if (row.Cells[3].Value != null)
                    sub += Convert.ToDecimal(row.Cells[3].Value);

            decimal tax = sub * 0.1m;
            decimal total = sub + tax;
            lblPSubTotal.Text = sub.ToString("0.00");
            lblPTax.Text = tax.ToString("0.00");
            lblPTotal.Text = total.ToString("0.00");
            CalcRemainingBalance();
        }

        private void CalcRemainingBalance()
        {
            decimal.TryParse(lblPTotal.Text, out decimal total);
            decimal.TryParse(lblPaidAmount.Text, out decimal paid);
            lblRemainingBalance.Text = (total - paid).ToString("0.00");
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            CalcRemainingBalance();
        }

        private void btnPSumitPurchase_Click(object sender, EventArgs e)
        {
            if (dgvPurchaseCart.Rows.Count == 0)
            {
                MessageBox.Show("កន្ត្រកទំនិញនៅទទេ!", "ព្រមាន");
                return;
            }

            if (!decimal.TryParse(lblPSubTotal.Text, out decimal sub) ||
                !decimal.TryParse(lblPTax.Text, out decimal tax) ||
                !decimal.TryParse(lblPTotal.Text, out decimal total) ||
                !decimal.TryParse(lblPaidAmount.Text, out decimal paid))
            {
                MessageBox.Show("តម្លៃទូទាត់មានបញ្ហា!", "ព្រមាន");
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction tr = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Purchases (SupplierName,InvoiceNo,Remarks,SubTotal,Tax,TotalAmount,PaidAmount,RemainingBalance,PurchaseDate)
                        OUTPUT INSERTED.PurchaseID
                        VALUES (@supplier,@invoice,@remarks,@sub,@tax,@total,@paid,@remain,@date)", conn, tr);
                    cmd.Parameters.AddWithValue("@supplier", cboSupplier.Text);
                    cmd.Parameters.AddWithValue("@invoice", txtInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@sub", sub);
                    cmd.Parameters.AddWithValue("@tax", tax);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@paid", paid);
                    cmd.Parameters.AddWithValue("@remain", total - paid);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    int pid = (int)cmd.ExecuteScalar();

                    foreach (DataGridViewRow row in dgvPurchaseCart.Rows)
                    {
                        if (row.Cells[0].Value == null) continue;
                        string name = row.Cells[0].Value.ToString();
                        int iqty = Convert.ToInt32(row.Cells[1].Value);
                        decimal price = Convert.ToDecimal(row.Cells[2].Value);
                        decimal iTotal = Convert.ToDecimal(row.Cells[3].Value);

                        new SqlCommand(@"INSERT INTO PurchaseDetails (PurchaseID,ItemName,Qty,UnitPrice,Total)
                            VALUES (@pid,@item,@qty,@price,@total)", conn, tr)
                        {
                            Parameters = {
                                new SqlParameter("@pid",   pid),
                                new SqlParameter("@item",  name),
                                new SqlParameter("@qty",   iqty),
                                new SqlParameter("@price", price),
                                new SqlParameter("@total", iTotal)
                            }
                        }.ExecuteNonQuery();

                        new SqlCommand("UPDATE Products SET StockQty=StockQty+@qty,CostPrice=@price WHERE ItemName=@item",
                            conn, tr)
                        {
                            Parameters = {
                                new SqlParameter("@qty",   iqty),
                                new SqlParameter("@price", price),
                                new SqlParameter("@item",  name)
                            }
                        }.ExecuteNonQuery();
                    }

                    tr.Commit();
                    MessageBox.Show("រក្សាទុកការទិញចូលជោគជ័យ!",
                        "ជោគជ័យ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearPurchaseForm();
                    LoadStockData();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show("មានបញ្ហា៖ " + ex.Message, "កំហុស");
                }
            }
        }

        private void ClearPurchaseForm()
        {
            dgvPurchaseCart.Rows.Clear();
            txtInvoiceNo.Clear();
            txtRemarks.Clear();
            txtUnitPrice.Clear();

            txtTotal.Text = "0.00";

            lblPSubTotal.Text = "0.00";
            lblPTax.Text = "0.00";
            lblPTotal.Text = "0.00";
            lblRemainingBalance.Text = "0.00";
        }

        private void btnPCancel_Click(object sender, EventArgs e)
        {
            ClearPurchaseForm();
        }

        private void btnPClearCart_Click(object sender, EventArgs e)
        {
            ClearPurchaseForm();
        }

        private void LoadStockData(string search = "", string category = "")
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string q = @"SELECT ItemName, Category, StockQty, ReorderLevel,
                    CASE WHEN StockQty<=0 THEN 'Out of Stock'
                         WHEN StockQty<=ReorderLevel THEN 'Low Stock'
                         ELSE 'Available' END AS Status
                    FROM Products WHERE 1=1";

                if (!string.IsNullOrEmpty(search)) q += " AND ItemName LIKE @s";
                if (!string.IsNullOrEmpty(category) && category != "All") q += " AND Category=@cat";
                q += " ORDER BY ItemName";

                SqlCommand cmd = new SqlCommand(q, conn);
                if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@s", "%" + search + "%");
                if (!string.IsNullOrEmpty(category) && category != "All") cmd.Parameters.AddWithValue("@cat", category);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                dgvStock.DataSource = dt;
                UpdateStockLabels(conn);
            }
        }

        private void UpdateStockLabels(SqlConnection conn)
        {
            int outCount = (int)new SqlCommand("SELECT COUNT(*) FROM Products WHERE StockQty<=0", conn).ExecuteScalar();
            int lowCount = (int)new SqlCommand("SELECT COUNT(*) FROM Products WHERE StockQty>0 AND StockQty<=ReorderLevel", conn).ExecuteScalar();
            lblOutOfStock.Text = outCount + " Items out of stock";
            lblLowStock.Text = lowCount + " Items with low stock";
        }

        private void txtSearchItems_TextChanged(object sender, EventArgs e)
        {
            LoadStockData(txtSearchItems.Text.Trim(), cboFilterByCategory.Text);
        }

        private void cboFilterByCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockData(txtSearchItems.Text.Trim(), cboFilterByCategory.Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboSelectItem.Text) || string.IsNullOrEmpty(cboStQty.Text))
            {
                MessageBox.Show("សូមជ្រើសរើសទំនិញ និងចំនួន!", "ព្រមាន");
                return;
            }
            if (!int.TryParse(cboStQty.Text, out int qty) ||
                !decimal.TryParse(cboCost.Text, out decimal cost))
            {
                MessageBox.Show("ចំនួន ឬតម្លៃមិនត្រឹមត្រូវ!", "ព្រមាន");
                return;
            }

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(@"UPDATE Products
                    SET StockQty=StockQty+@qty, CostPrice=@cost WHERE ItemName=@name", conn);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@cost", cost);
                cmd.Parameters.AddWithValue("@name", cboSelectItem.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("ធ្វើបច្ចុប្បន្នភាពស្តុកជោគជ័យ!", "ព័ត៌មាន");
            LoadStockData();
        }

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("មុខងារបើកផ្ទាំងបង្កើតទំនិញថ្មី!");
        }

        private void LoadCustomerData(string search = "", string group = "")
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string q = "SELECT CustomerName, Phone, Email, MemberType FROM Customers WHERE 1=1";
                if (!string.IsNullOrEmpty(search)) q += " AND (CustomerName LIKE @s OR Phone LIKE @s)";
                if (!string.IsNullOrEmpty(group) && group != "All") q += " AND MemberType=@g";
                q += " ORDER BY CustomerName";

                SqlCommand cmd = new SqlCommand(q, conn);
                if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@s", "%" + search + "%");
                if (!string.IsNullOrEmpty(group) && group != "All") cmd.Parameters.AddWithValue("@g", group);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                dgvCustomers.DataSource = dt;
            }
        }

        private void txtSearchCustomer_TextChanged(object sender, EventArgs e)
        {
            LoadCustomerData(txtSearchCustomer.Text.Trim(), cboFilterByGroup.Text);
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string name = dgvCustomers.Rows[e.RowIndex].Cells["CustomerName"].Value?.ToString();
            if (!string.IsNullOrEmpty(name)) ShowCustomerDetail(name);
        }

        private void ShowCustomerDetail(string custName)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Phone,Email,MemberType,TotalPurchase FROM Customers WHERE CustomerName=@n", conn);
                cmd.Parameters.AddWithValue("@n", custName);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;
                    lblPhone.Text = "Phone: " + r["Phone"];
                    lblEmail.Text = "Email: " + r["Email"];
                    lblTotalPurchase.Text = "Total Purchase: $" + Convert.ToDecimal(r["TotalPurchase"]).ToString("0.00");
                    bool isGold = r["MemberType"].ToString() == "Gold";
                    lblGoldMember.Visible = isGold;
                    lblGoldMember.Text = isGold ? "🏆 GOLD MEMBER" : "";
                }
                LoadPurchaseHistory(custName, conn);
            }
        }
        private void LoadPurchaseHistory(string custName, SqlConnection conn)
        {
            lstPurchaseHistory.Items.Clear();
            SqlCommand cmd = new SqlCommand(@"SELECT OrderID, OrderDate, TotalAmount
                FROM Orders WHERE CustomerName=@n ORDER BY OrderDate DESC", conn);
            cmd.Parameters.AddWithValue("@n", custName);
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string date = Convert.ToDateTime(r["OrderDate"]).ToString("yyyy-MM-dd");
                    decimal amt = Convert.ToDecimal(r["TotalAmount"]);
                    lstPurchaseHistory.Items.Add($"INV#{r["OrderID"]} | {date} | ${amt:0.00}");
                }
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("ឈ្មោះអតិថិជន:", "អតិថិជនថ្មី");
            if (string.IsNullOrWhiteSpace(name)) return;
            string phone = Microsoft.VisualBasic.Interaction.InputBox("Phone:", "Phone");
            string email = Microsoft.VisualBasic.Interaction.InputBox("Email:", "Email");

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(@"INSERT INTO Customers
                    (CustomerName,Phone,Email,MemberType,TotalPurchase)
                    VALUES (@n,@p,@e,'Normal',0)", conn);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@p", phone);
                cmd.Parameters.AddWithValue("@e", email);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("បន្ថែមអតិថិជនជោគជ័យ!", "ជោគជ័យ");
            LoadCustomerData();
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;
            string oldName = dgvCustomers.CurrentRow.Cells["CustomerName"].Value?.ToString();
            if (string.IsNullOrEmpty(oldName)) return;

            string newName = Microsoft.VisualBasic.Interaction.InputBox("ឈ្មោះ:", "កែប្រែ", oldName);
            if (string.IsNullOrWhiteSpace(newName)) return;
            string newPhone = Microsoft.VisualBasic.Interaction.InputBox("Phone:", "កែប្រែ",
                dgvCustomers.CurrentRow.Cells["Phone"].Value?.ToString());
            string newEmail = Microsoft.VisualBasic.Interaction.InputBox("Email:", "កែប្រែ",
                dgvCustomers.CurrentRow.Cells["Email"].Value?.ToString());

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(@"UPDATE Customers
                    SET CustomerName=@nn, Phone=@p, Email=@e WHERE CustomerName=@on", conn);
                cmd.Parameters.AddWithValue("@nn", newName);
                cmd.Parameters.AddWithValue("@p", newPhone);
                cmd.Parameters.AddWithValue("@e", newEmail);
                cmd.Parameters.AddWithValue("@on", oldName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("កែប្រែអតិថិជនជោគជ័យ!", "ជោគជ័យ");
            LoadCustomerData();
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;
            string name = dgvCustomers.CurrentRow.Cells["CustomerName"].Value?.ToString();
            if (string.IsNullOrEmpty(name)) return;

            if (MessageBox.Show($"លុប [{name}]?", "បញ្ជាក់",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Customers WHERE CustomerName=@n", conn);
                cmd.Parameters.AddWithValue("@n", name);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("លុបអតិថិជនជោគជ័យ!", "ព័ត៌មាន");
            LoadCustomerData();
        }

        private void btnImportCustomer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("មុខងារ Import Customer ពី Excel (Coming Soon)");
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime from = dtpDateFrom.Value.Date;
            DateTime to = dtpDateTo.Value.Date.AddDays(1).AddSeconds(-1);

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                decimal totalSales = GetScalar(conn, "SELECT ISNULL(SUM(TotalAmount),0) FROM Orders WHERE OrderDate BETWEEN @f AND @t AND Status='Completed'", from, to);
                decimal totalPurchase = GetScalar(conn, "SELECT ISNULL(SUM(TotalAmount),0) FROM Purchases WHERE PurchaseDate BETWEEN @f AND @t", from, to);
                decimal taxCollected = GetScalar(conn, "SELECT ISNULL(SUM(Tax),0) FROM Orders WHERE OrderDate BETWEEN @f AND @t AND Status='Completed'", from, to);
                decimal profit = totalSales - totalPurchase;

                lblTotalSale.Text = $"${totalSales:0.00}";
                lblRTotalPurchase.Text = $"${totalPurchase:0.00}";
                lblTaxCollected.Text = $"${taxCollected:0.00}";
                lblProfitLoss.Text = $"${profit:0.00}";
                lblProfitLoss.ForeColor = profit >= 0 ? Color.Green : Color.Red;

                string rq = @"SELECT CAST(D AS DATE) AS ReportDate,
                    SUM(Sales) AS SalesAmount, SUM(Pur) AS PurchaseAmount,
                    SUM(Tax) AS Tax, 0 AS Discount, SUM(Sales)-SUM(Pur) AS NetTotal
                    FROM (
                        SELECT OrderDate D, TotalAmount Sales, 0 Pur, Tax FROM Orders WHERE Status='Completed'
                        UNION ALL
                        SELECT PurchaseDate, 0, TotalAmount, 0 FROM Purchases
                    ) X WHERE D BETWEEN @f AND @t
                    GROUP BY CAST(D AS DATE) ORDER BY ReportDate";

                SqlCommand cr = new SqlCommand(rq, conn);
                cr.Parameters.AddWithValue("@f", from);
                cr.Parameters.AddWithValue("@t", to);
                DataTable dt = new DataTable();
                new SqlDataAdapter(cr).Fill(dt);
                dgvReport.DataSource = dt;

                LoadCharts(totalSales, totalPurchase, taxCollected);
            }
        }

        private decimal GetScalar(SqlConnection conn, string q, DateTime f, DateTime t)
        {
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@f", f);
            cmd.Parameters.AddWithValue("@t", t);
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
        private void LoadCharts(decimal sales, decimal purchase, decimal tax)
        {
            var bar = new UIBarOption();
            bar.Title = new UITitle { Text = "Sales vs Purchase" };
            var s1 = new UIBarSeries { Name = "Amount" };
            s1.AddData((double)sales);
            s1.AddData((double)purchase);
            bar.Series.Add(s1);
            bar.XAxis.Data.Add("Total Sales");
            bar.XAxis.Data.Add("Total Purchase");
            chartBar.SetOption(bar);

            var pie = new UIPieOption();
            pie.Title = new UITitle { Text = "Revenue Structure" };
            var s2 = new UIPieSeries { Name = "Amount" };
            s2.AddData("Net Revenue", (double)(sales - tax));
            s2.AddData("Tax Collected", (double)tax);
            s2.AddData("Purchase Cost", (double)purchase);
            pie.Series.Add(s2);
            chartPie.SetOption(pie);
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ប្រព័ន្ធកំពុងផ្ញើទៅម៉ាស៊ីនបោះពុម្ព...", "Print Report");
        }

        private void button32_Click(object sender, EventArgs e)
        {
            LoadOrderList(txtSearchOrder.Text.Trim(), cboStatus.Text);
        }

        private void LoadOrderList(string search = "", string status = "")
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string q = "SELECT OrderID, OrderDate, CustomerName, TotalAmount, Status FROM Orders WHERE 1=1";
                if (!string.IsNullOrEmpty(search)) q += " AND (CAST(OrderID AS VARCHAR) LIKE @s OR CustomerName LIKE @s)";
                if (!string.IsNullOrEmpty(status) && status != "All") q += " AND Status=@st";
                q += " ORDER BY OrderID DESC";

                SqlCommand cmd = new SqlCommand(q, conn);
                if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@s", "%" + search + "%");
                if (!string.IsNullOrEmpty(status) && status != "All") cmd.Parameters.AddWithValue("@st", status);

                DataTable dt = new DataTable();
                conn.Open();
                new SqlDataAdapter(cmd).Fill(dt);
                dgvOrderList.DataSource = dt;
            }
        }

        private void dgvOrderList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvOrderList.Rows[e.RowIndex].Cells["OrderID"].Value == null) return;
            DataGridViewRow row = dgvOrderList.Rows[e.RowIndex];
            int orderID = Convert.ToInt32(row.Cells["OrderID"].Value);
            lblOrderID.Text = "Order ID: #" + orderID;
            if (row.Cells["OrderDate"].Value != null)
                lblDate.Text = Convert.ToDateTime(row.Cells["OrderDate"].Value).ToString("dd/MM/yyyy HH:mm");
            LoadOrderDetail(orderID);
        }

        private void LoadOrderDetail(int orderID)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT ItemName AS [Items],Qty,Price,SubTotal FROM OrderDetails WHERE OrderID=@id", conn);
                cmd.Parameters.AddWithValue("@id", orderID);
                new SqlDataAdapter(cmd).Fill(dt);
                dgvOrderDetail.DataSource = dt;

                SqlCommand cs = new SqlCommand("SELECT SubTotal,Tax,TotalAmount FROM Orders WHERE OrderID=@id", conn);
                cs.Parameters.AddWithValue("@id", orderID);
                using (SqlDataReader r = cs.ExecuteReader())
                {
                    if (r.Read())
                    {
                        lblOSubTotal.Text = "$" + Convert.ToDecimal(r["SubTotal"]).ToString("0.00");
                        lblOTax.Text = "$" + Convert.ToDecimal(r["Tax"]).ToString("0.00");
                        lblOGrandTotal.Text = "$" + Convert.ToDecimal(r["TotalAmount"]).ToString("0.00");
                    }
                }
            }
        }

        private void btnOCancel_Click(object sender, EventArgs e)
        {
            if (dgvOrderList.CurrentRow == null ||
                dgvOrderList.CurrentRow.Cells["OrderID"].Value == null) return;

            int id = Convert.ToInt32(dgvOrderList.CurrentRow.Cells["OrderID"].Value);
            string status = dgvOrderList.CurrentRow.Cells["Status"].Value?.ToString();

            if (status == "Canceled")
            {
                MessageBox.Show("វិក្កយបត្រនេះត្រូវបានបោះបង់រួចហើយ!", "ព័ត៌មាន");
                return;
            }

            if (MessageBox.Show($"បោះបង់ Order #{id}? ស្តុកនឹងបូកចូលវិញ។",
                "បញ្ជាក់", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction tr = conn.BeginTransaction();
                try
                {
                    new SqlCommand("UPDATE Orders SET Status='Canceled' WHERE OrderID=@id", conn, tr)
                    {
                        Parameters = { new SqlParameter("@id", id) }
                    }.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    SqlCommand cg = new SqlCommand("SELECT ItemName,Qty FROM OrderDetails WHERE OrderID=@id", conn, tr);
                    cg.Parameters.AddWithValue("@id", id);
                    new SqlDataAdapter(cg).Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        new SqlCommand("UPDATE Products SET StockQty=StockQty+@qty WHERE ItemName=@item", conn, tr)
                        {
                            Parameters = {
                                new SqlParameter("@qty",  Convert.ToInt32(row["Qty"])),
                                new SqlParameter("@item", row["ItemName"].ToString())
                            }
                        }.ExecuteNonQuery();
                    }

                    tr.Commit();
                    MessageBox.Show("បោះបង់ Order ជោគជ័យ! ស្តុកបូកចូលវិញហើយ។", "ជោគជ័យ");
                    LoadOrderList();
                    dgvOrderDetail.DataSource = null;
                    lblOSubTotal.Text = lblOTax.Text = lblOGrandTotal.Text = "$0.00";
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show("មានកំហុស៖ " + ex.Message, "កំហុស");
                }
            }
        }

        private void btnSavePromotion_Click(object sender, EventArgs e)
        {
            MessageBox.Show("រក្សាទុក Promotion (Coming Soon)");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchOrder.Clear();
            dgvOrderList.DataSource = null;
            dgvOrderDetail.DataSource = null;
            lblOrderID.Text = "Order ID:";
            lblDate.Text = "Date:";
            lblOSubTotal.Text = "$0.00";
            lblOTax.Text = "$0.00";
            lblOGrandTotal.Text = "$0.00";
        }


        // ====================================================
        // MISC
        // ====================================================
        private void uiTabControl1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label90_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
