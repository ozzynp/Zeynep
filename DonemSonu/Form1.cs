using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;


namespace DonemSonu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection baglan = new OleDbConnection("Provider= Microsoft.ACE.OLEDB.12.0; Data Source = " + Application.StartupPath + "\\data.mdb");
        OleDbCommand komut = new OleDbCommand();
        BindingSource bindingSource1 = new BindingSource();
        deneme.Ortalama sonuc = new deneme.Ortalama();
        XmlDocument dosya = new XmlDocument();
        OleDbDataReader dr;

        private XmlElement CreateXmlElement(XmlDocument xmlDoc, string elementName, string value)
        {
            XmlElement element = xmlDoc.CreateElement(elementName);
            element.InnerText = value;
            return element;
        }
        public void VeriCek()
        {
            DataTable tablo = new DataTable();
            DataSet al = new DataSet();
            OleDbDataAdapter verial = new OleDbDataAdapter("select * from ogrenci", baglan);
            verial.Fill(al, "tablo");
            al.Tables.Add(tablo);
            bindingSource1.DataSource = al;
            bindingSource1.DataMember = al.Tables[0].TableName;
        }
        public void VeriCek2()
        {
         /*   VeriCek();
            dataGridView1.DataSource = bindingSource1;
           textBox1.DataBindings.Add("text", bindingSource1, "Kimlik");
            textBox2.DataBindings.Add("text", bindingSource1, "TC_Kimlik");
            textBox3.DataBindings.Add("text", bindingSource1, "Ad");
            textBox4.DataBindings.Add("text", bindingSource1, "Soyad");
            textBox5.DataBindings.Add("text", bindingSource1, "Adres");
            textBox6.DataBindings.Add("text", bindingSource1, "Ders_Adi");
            textBox7.DataBindings.Add("text", bindingSource1, "Vize_Notu");
            textBox8.DataBindings.Add("text", bindingSource1, "Final_Notu");
           textBox9.DataBindings.Add("text", bindingSource1, "Ortalama");  */

        }
        private void button2_Click(object sender, EventArgs e)
        {
            //xml'den dataGridView ' e ekle
            string path= "isim.xml";
            DataTable xmlTable = new DataTable();

            DataSet set = new DataSet();
            set.ReadXml(path);

            if (set.Tables.Count > 0)
            {
                xmlTable = set.Tables[0];
                MessageBox.Show("aktarıldı");
                dataGridView1.DataSource = xmlTable;
            }

            //   bindingSource1.AddNew();
            // textBox1.Focus();
        }
        public void list()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Kimlik", 65);
            listView1.Columns.Add("TC_Kimlik", 65);
            listView1.Columns.Add("Ad", 65);
            listView1.Columns.Add("Soyad", 65);
            listView1.Columns.Add("Adres", 65);
            listView1.Columns.Add("Ders_Adi", 65);
            listView1.Columns.Add("Vize_Notu", 65);
            listView1.Columns.Add("Final_Notu", 65);
            listView1.Columns.Add("Ortalama", 65);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            VeriCek();
            VeriCek2();
            list();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // girilen bilgiler buton tıklamasıyla veritabanına kaydediliyor
            try
            {
                baglan.Open();

                // Aynı Kimlik değerinin veritabanında var olup olmadığını kontrol et
                OleDbCommand komutara = new OleDbCommand("select count(*) from ogrenci where Kimlik like " + Int32.Parse(textBox1.Text) + "", baglan);
                komutara.ExecuteNonQuery();
                int mevcutSayi = (int)komutara.ExecuteScalar();

                if (mevcutSayi == 0)  // Aynı Kimlik değeri yoksa ekle
                {
                    //Ekleme İşlemi
                    if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox4.Text) &&
                       !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(textBox6.Text) &&
                       !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox8.Text) &&
                       !string.IsNullOrEmpty(textBox7.Text) && !string.IsNullOrEmpty(textBox9.Text))
                    {
                        OleDbCommand komutKaydet = new OleDbCommand("insert into ogrenci (Kimlik, TC_Kimlik , Ad , Soyad , Adres , Ders_Adi , Vize_Notu , Final_Notu , Ortalama) values (" + Int32.Parse(textBox1.Text) + "," + Int32.Parse(textBox2.Text) + ",'" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "'," + Int32.Parse(textBox7.Text) + "," + Int32.Parse(textBox8.Text) + "," + Double.Parse(textBox9.Text) + ")", baglan);
                        komutKaydet.ExecuteNonQuery();

                        MessageBox.Show("Kayıt Yapıldı");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        textBox8.Text = "";
                        textBox9.Text = "";
                        VeriCek();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen boşlukları doldurunuz");
                    }
                }
                else
                {
                    MessageBox.Show("Bu Kimlik değeri zaten var.Lütfen farklı bir değeri kullanın.");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglan.Close();
            }

        }


      

        private void button3_Click(object sender, EventArgs e)
        {
            double a = Double.Parse(textBox7.Text);
            double b = Double.Parse(textBox8.Text);
            textBox9.Text = sonuc.ort(a, b).ToString();
            //  bindingSource1.EndEdit();
            /*     baglan.Open();
                 komut.Connection = baglan;
                 komut.CommandType = CommandType.Text;

            
                         komut.CommandText = "insert into ogrenci (Kimlik, TC_Kimlik , Ad , Soyad, Adres , Ders_Adi, Vize_Notu , Final_Notu , Ortalama) values (" + Int32.Parse(textBox1.Text) + "," + Int32.Parse(textBox2.Text) + ",'" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "'," + Int32.Parse(textBox7.Text) + "," + Int32.Parse(textBox8.Text) + "," + Int32.Parse(textBox9.Text) + ")";
                         komut.ExecuteNonQuery();

                         MessageBox.Show("Kayıt Yapıldı");
                         textBox1.Text = "";
                         textBox2.Text = "";
                         textBox3.Text = "";
                         textBox4.Text = "";
                         textBox5.Text = "";
                         textBox6.Text = "";
                         textBox7.Text = "";
                         textBox8.Text = "";
                         textBox9.Text = "";
                         VeriCek();
              
                 baglan.Close */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //girilen bilgilerden kaydı silinmek istenilen bilginin id numarası textbox içine girildikten sonra buton tıklamasıyla veritabanından veri siliniyor
            baglan.Open();
            OleDbCommand komutDelete = new OleDbCommand("Delete from ogrenci where Kimlik = " + Int32.Parse(textBox11.Text) + "", baglan);
            komutDelete.ExecuteNonQuery();
          //  VeriCek();
            MessageBox.Show("Silindi");
            baglan.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //textbox lar içindeki veriler buton tıklaması ile xml dosyasına aktarılır.

            // dosya.Load("C:\\Users\\0zeyn\\OneDrive\\Masaüstü\\2.class\\nesne tabanlı\\DonemSonu\\DonemSonu\\bin\\Debug\\isim.xml");

            dosya.Load(Application.StartupPath + "\\isim.xml");
            XmlElement ogrenci = dosya.CreateElement("ogrenci");
            ogrenci.SetAttribute("Kimlik", textBox1.Text);

            XmlNode TC_Kimlik = dosya.CreateNode(XmlNodeType.Element, "TC_Kimlik", "");
            TC_Kimlik.InnerText = textBox2.Text;
            ogrenci.AppendChild(TC_Kimlik);

            XmlNode Ad = dosya.CreateNode(XmlNodeType.Element, "Ad", "");
            Ad.InnerText = textBox3.Text;
            ogrenci.AppendChild(Ad);

            XmlNode Soyad = dosya.CreateNode(XmlNodeType.Element, "Soyad", "");
            Soyad.InnerText = textBox4.Text;
            ogrenci.AppendChild(Soyad);

            XmlNode Adres = dosya.CreateNode(XmlNodeType.Element, "Adres", "");
            Adres.InnerText = textBox5.Text;
            ogrenci.AppendChild(Adres);

            XmlNode Ders_Adi = dosya.CreateNode(XmlNodeType.Element, "Ders_Adi", "");
            Ders_Adi.InnerText = textBox6.Text;
            ogrenci.AppendChild(Ders_Adi);

            XmlNode Vize_Notu = dosya.CreateNode(XmlNodeType.Element, "Vize_Notu", "");
            Vize_Notu.InnerText = textBox7.Text;
            ogrenci.AppendChild(Vize_Notu);

            XmlNode Final_Notu = dosya.CreateNode(XmlNodeType.Element, "Final_Notu", "");
            Final_Notu.InnerText = textBox8.Text;
            ogrenci.AppendChild(Final_Notu);

            XmlNode Ortalama = dosya.CreateNode(XmlNodeType.Element, "Ortalama", "");
            Ortalama.InnerText = textBox9.Text;
            ogrenci.AppendChild(Ortalama);

            dosya.DocumentElement.AppendChild(ogrenci);

            dosya.Save(Application.StartupPath + "\\isim.xml");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //dataGridView içinden xml e kaydet

            try
            {



                dosya.Load(Application.StartupPath + "\\isim.xml");

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    XmlElement ogrenci = dosya.CreateElement("ogrenci");
                    ogrenci.SetAttribute("Kimlik", dataGridView1.Rows[i].Cells[0].Value.ToString());

                    XmlNode TC_Kimlik = dosya.CreateNode(XmlNodeType.Element, "TC_Kimlik", "");
                    TC_Kimlik.InnerText = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    ogrenci.AppendChild(TC_Kimlik);

                    XmlNode Ad = dosya.CreateNode(XmlNodeType.Element, "Ad", "");
                    Ad.InnerText = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    ogrenci.AppendChild(Ad);

                    XmlNode Soyad = dosya.CreateNode(XmlNodeType.Element, "Soyad", "");
                    Soyad.InnerText = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    ogrenci.AppendChild(Soyad);

                    XmlNode Adres = dosya.CreateNode(XmlNodeType.Element, "Adres", "");
                    Adres.InnerText = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    ogrenci.AppendChild(Adres);

                    XmlNode Ders_Adi = dosya.CreateNode(XmlNodeType.Element, "Ders_Adi", "");
                    Ders_Adi.InnerText = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    ogrenci.AppendChild(Ders_Adi);

                    XmlNode Vize_Notu = dosya.CreateNode(XmlNodeType.Element, "Vize_Notu", "");
                    Vize_Notu.InnerText = dataGridView1.Rows[i].Cells[6].Value.ToString();
                    ogrenci.AppendChild(Vize_Notu);

                    XmlNode Final_Notu = dosya.CreateNode(XmlNodeType.Element, "Final_Notu", "");
                    Final_Notu.InnerText = dataGridView1.Rows[i].Cells[7].Value.ToString();
                    ogrenci.AppendChild(Final_Notu);

                    XmlNode Ortalama = dosya.CreateNode(XmlNodeType.Element, "Ortalama", "");
                    Ortalama.InnerText = dataGridView1.Rows[i].Cells[8].Value.ToString();
                    ogrenci.AppendChild(Ortalama);

                    dosya.DocumentElement.AppendChild(ogrenci);
                }
                dosya.Save(Application.StartupPath + "\\isim.xml");
                MessageBox.Show("Kayıt Başarılı");



            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata", ex.Message);
            }




        }

        private void button7_Click(object sender, EventArgs e)
        {
            //xml'den okunan veriler listView içine kaydedilecek
            try
            {



                string[] deneme = new string[] { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text };

                var item = new ListViewItem(deneme);

                listView1.Items.Add(item);
            }


            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //listView'den xml'e Kaydet
            try
            {
                dosya.Load(Application.StartupPath + "\\isim.xml");

                XmlElement ogrenci = dosya.CreateElement("ogrenci");

                foreach (ListViewItem item in listView1.Items)
                {

                    ogrenci.SetAttribute("Kimlik", item.SubItems[0].Text);

                    ogrenci.AppendChild(CreateXmlElement(dosya, "TC_Kimlik", item.SubItems[1].Text));
                    ogrenci.AppendChild(CreateXmlElement(dosya, "Ad", item.SubItems[2].Text));
                    ogrenci.AppendChild(CreateXmlElement(dosya, "Soyad", item.SubItems[3].Text));
                    ogrenci.AppendChild(CreateXmlElement(dosya, "Adres", item.SubItems[4].Text));
                    ogrenci.AppendChild(CreateXmlElement(dosya, "Ders_Adi", item.SubItems[5].Text));
                    ogrenci.AppendChild(CreateXmlElement(dosya, "Vize_Notu", item.SubItems[6].Text));
                    ogrenci.AppendChild(CreateXmlElement(dosya, "Final_Notu", item.SubItems[7].Text));
                    ogrenci.AppendChild(CreateXmlElement(dosya, "Ortalama", item.SubItems[8].Text));

                    dosya.DocumentElement.AppendChild(ogrenci);
                }
                dosya.Save(Application.StartupPath + " \\isim.xml");
                MessageBox.Show("XML dosyası başarıyla oluşturuldu");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex.Message);

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                dosya.Load(Application.StartupPath + "\\isim.xml");

                //Kök düğümü seçin
                XmlNode root = dosya.SelectSingleNode("ogrenci");

                if (root != null)
                {
                    //Kök düğüm altındaki tüm çocukları silin
                    root.RemoveAll();

                    //Değişikleri kaydedin
                    dosya.Save(Application.StartupPath + "\\isim.xml");

                    MessageBox.Show("XML dosyasının içeriği başarıyla silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Kök dizin bulunumadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dosya.Load(Application.StartupPath + "\\isim.xml");

            //textbox'tan gelen kimlik bilgisine göre düğümleri seç
            XmlNodeList nodes = dosya.SelectNodes("ogrenci[Kimlik='" + Int32.Parse(textBox10.Text) + "')");

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    //xml'den tek satır kayıt sil
                    node.ParentNode.RemoveChild(node);
                }
            }
            dosya.Save(Application.StartupPath + "\\isim.xml");

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                dosya.Load(Application.StartupPath + "\\isim.xml");

                //Kök düğümü seçin
                XmlNode root = dosya.SelectSingleNode("ogrenci");

                if (root != null)
                {
                    //Kök düğüm altındaki tüm çocukları silin
                    root.RemoveChild(root.FirstChild);

                    //Değişikleri kaydedin
                    dosya.Save(Application.StartupPath + "\\isim.xml");

                    MessageBox.Show("XML dosyasının içeriği başarıyla silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Kök dizin bulunumadı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // xml'den textboxlara kaydet

            dosya.Load(Application.StartupPath + "\\isim.xml");
            XmlNodeList liste = dosya.GetElementsByTagName("ogrenci");

            foreach (XmlNode ogr_liste in liste)
            {
                string kimlik = ogr_liste.Attributes["Kimlik"].Value;
                string tc_kimlik = ogr_liste["TC_Kimlik"].FirstChild.Value;
                string adi = ogr_liste["Ad"].FirstChild.Value;
                string soyadi = ogr_liste["Soyad"].FirstChild.Value;
                string adresi = ogr_liste["Adres"].FirstChild.Value;
                string ders_adi = ogr_liste["Ders_Adi"].FirstChild.Value;
                string vize = ogr_liste["Vize_Notu"].FirstChild.Value;
                string final = ogr_liste["Final_Notu"].FirstChild.Value;
                string sonuc = ogr_liste["Ortalama"].FirstChild.Value;

                textBox1.Text = kimlik;
                textBox2.Text = tc_kimlik;
                textBox3.Text = adi;
                textBox4.Text = soyadi;
                textBox5.Text = adresi;
                textBox6.Text = ders_adi;
                textBox7.Text = vize;
                textBox8.Text = final;
                textBox9.Text = sonuc;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                //xml'den listView ' e kaydet

                dosya.Load(Application.StartupPath + "\\isim.xml");

                XmlNodeList listee = dosya.GetElementsByTagName("ogrenci");

                foreach (XmlNode liste in listee)
                {
                    string Kimlik = liste.Attributes["Kimlik"].Value;

                    string TC_Kimlik = liste["TC_Kimlik"].FirstChild.Value;
                    string Ad = liste["Ad"].FirstChild.Value;
                    string Soyad = liste["Soyad"].FirstChild.Value;
                    string Adres = liste["Adres"].FirstChild.Value;
                    string Ders_Adi = liste["Ders_Adi"].FirstChild.Value;
                    string Vize_Notu = liste["Vize_Notu"].FirstChild.Value;
                    string Final_Notu = liste["Final_Notu"].FirstChild.Value;
                    string Ortalama = liste["Ortalama"].InnerText; 

                    ListViewItem item = new ListViewItem(Kimlik);
                //    item.Text = Kimlik;
                    item.SubItems.Add(TC_Kimlik);
                    item.SubItems.Add(Ad);
                    item.SubItems.Add(Soyad);
                    item.SubItems.Add(Adres);
                    item.SubItems.Add(Ders_Adi);
                    item.SubItems.Add(Vize_Notu);
                    item.SubItems.Add(Final_Notu);
                    item.SubItems.Add(Ortalama);
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex.Message);
            }

        }

   /*     private void button14_Click(object sender, EventArgs e)
        {
      
            baglan.Open();
            OleDbCommand cmd1 = new OleDbCommand("select * from ogrenci where Kimlik = "+Int32.Parse(textBox1.Text)+"", baglan);
            dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                getListView();
               
         
            }
            baglan.Close();
        }
        void getListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("Karenin Çevresi");
                listView1.Columns.Add("KareninAlani");
                listView1.Columns.Add("KupunHacmi");
            }
              
              ListViewItem item = new ListViewItem();
              item.Text = dr["KareninCevresi"].ToString();
              item.SubItems.Add(dr["KareninAlani"].ToString());
              item.SubItems.Add(dr["KupunHacmi"].ToString());
              listView1.Items.Add(item);
              
        }

        */
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                baglan.Open();
                OleDbCommand cmd1 = new OleDbCommand("select * from ogrenci where Kimlik = " + Int32.Parse(textBox1.Text) + "", baglan);
                dr = cmd1.ExecuteReader();

                // Döngü içinde değil, sadece bir kere çağrılıyor
                if (dr.Read())
                {
                    list();
                    ListViewItem item = new ListViewItem();
                    item.Text = dr.GetValue(0).ToString();
                    item.SubItems.Add(dr.GetValue(1).ToString());
                    listView1.Items.Add(item);
                }
                dr.Close(); // DataReader'ı kapat
                baglan.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {

        

         try
            {
                baglan.Open();
                OleDbCommand cmd1 = new OleDbCommand("select * from ogrenci where Kimlik = " + Int32.Parse(textBox1.Text) + "", baglan);
                dr = cmd1.ExecuteReader();
                dr.Read();
                // Döngü içinde değil, sadece bir kere çağrılıyor
                if (dr.HasRows == true)
                {

                    textBox1.Text = dr.GetValue(0).ToString();
                    textBox2.Text = dr.GetValue(1).ToString();
                    textBox3.Text = dr.GetValue(2).ToString();
                    textBox4.Text = dr.GetValue(3).ToString();
                    textBox5.Text = dr.GetValue(4).ToString();
                    textBox6.Text = dr.GetValue(5).ToString();
                    textBox7.Text = dr.GetValue(6).ToString();
                    textBox8.Text = dr.GetValue(7).ToString();
                    textBox9.Text = dr.GetValue(8).ToString();
                 //   textBox1.Text = dr.GetValue(9).ToString();
                   
                }
                dr.Close(); // DataReader'ı kapat
                baglan.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


    }

    }



