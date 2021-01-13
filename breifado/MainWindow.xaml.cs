using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace breifado
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static string stringconnection = "Data Source=desktop-oc51qh5;Initial Catalog=Client;Integrated Security=True";
        public static SqlConnection con = new SqlConnection(stringconnection);
        static SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        static SqlCommandBuilder build = new SqlCommandBuilder(adapter);
        DataRow CurrentRow;
        
       

        public MainWindow()
        {
            InitializeComponent();
            GetClient();
            getville();
            getvillesearch();




        }

        void vider()
        {
            txtadress.Text = "";
            txtfname.Text = "";
            txtlname.Text = "";
            boxville.Text = "";
            message.Text = "La Gestion des Clients";
        }

        void GetClient()
        {
            //Creation d'une SqlCommand
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM clientinfo",con);
            adapter.SelectCommand = selectCommand;
            //fill kat3amar la table o kat executer ACCEPTCHANGES()bach katkhali kolchi unchanged
            //adapter.Fill(ds, "clientinfo");
            //DataColumn[] pk = { ds.Tables["clientinfo"].Columns["id"]};
            //ds.Tables["Clientinfo"].PrimaryKey = pk;
            //ds.Tables["clientinfo"].Columns["id"].AutoIncrement = true;
            //ds.Tables["clientinfo"].Columns["id"].AutoIncrementStep = 1;
            //datagrid.ItemsSource = ds.Tables["clientinfo"].DefaultView;


            ////Creation de DeleteCommand
            //SqlCommand deleteCommand = new SqlCommand("delete from clientinfo where id=@id",con);
            //deleteCommand.Parameters.Add("@id", SqlDbType.Int, 0, "id");
            //adapter.DeleteCommand = deleteCommand;

            ////creation de updateCommand
            //SqlCommand updateCommand = new SqlCommand("update clientinfo  set firstname=@firstname, lastname=@lastname, adress=@adress, villeId=@villeId where id=@id", con);
            //updateCommand.Parameters.Add("@firstname", SqlDbType.Text,50,"firstname");
            //updateCommand.Parameters.Add("@lastname", SqlDbType.Text, 50, "lastname");
            //updateCommand.Parameters.Add("@adress", SqlDbType.Text, 50, "adress");
            //updateCommand.Parameters.Add("@villeId", SqlDbType.Int, 0, "villeId");
            //updateCommand.Parameters.Add("@id", SqlDbType.Int, 0, "id");
            //adapter.UpdateCommand = updateCommand;

            ////creation insertCommand

            //SqlCommand insertCommand = new SqlCommand("Insert into clientinfo values(@firstname, @lastname,@adress, @villeId)", con);
            //insertCommand.Parameters.Add("@id", SqlDbType.Int, 0, "id");
            //insertCommand.Parameters.Add("@firstname", SqlDbType.Text, 50, "firstname");
            //insertCommand.Parameters.Add("@lastname", SqlDbType.Text, 50, "lastname");
            //insertCommand.Parameters.Add("@adress", SqlDbType.Text, 50, "adress");
            //insertCommand.Parameters.Add("@villeId", SqlDbType.Int, 0, "villeId");
            //adapter.InsertCommand = insertCommand;


            SqlCommandBuilder bl = new SqlCommandBuilder(adapter);
            adapter.DeleteCommand = bl.GetDeleteCommand();
            adapter.UpdateCommand = bl.GetUpdateCommand();
            adapter.InsertCommand = bl.GetInsertCommand();
            adapter.Fill(ds, "clientinfo");
            DataColumn[] pk = { ds.Tables["clientinfo"].Columns["id"]};
            ds.Tables["Clientinfo"].PrimaryKey = pk;
            ds.Tables["clientinfo"].Columns["id"].AutoIncrement = true;
            datagrid.ItemsSource = ds.Tables["clientinfo"].DefaultView;








        }
        void getville()
        {
            SqlDataAdapter adapterville = new SqlDataAdapter("select * from ville", con);
            adapterville.Fill(ds, "ville");
            boxville.ItemsSource = ds.Tables["ville"].DefaultView;
            boxville.DisplayMemberPath = "name";
            boxville.SelectedValuePath = "id";
        }

        void getvillesearch()
        {

            SqlDataAdapter adapterville = new SqlDataAdapter("select * from ville", con);
            adapterville.Fill(ds, "ville");
            boxsearchc.ItemsSource = ds.Tables["ville"].DefaultView;
            boxsearchc.DisplayMemberPath = "name";
            boxsearchc.SelectedValuePath = "id";

        }
        private void dateset(object sender, EventArgs e)
        {

        }

      
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        //=======>
        // Serach By ID
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            int id = int.Parse(searchbyid.Text);
            //find id in the rows of the table clientinfo.
            SqlCommand query = new SqlCommand("SELECT name FROM ville INNER JOIN clientinfo ON clientinfo.villeId=ville.id where clientinfo.id ="+id, con);
            //Your table doit avoir un primary key sinon il peut pas travailler 
            CurrentRow = ds.Tables["clientinfo"].Rows.Find(id);

            if (CurrentRow != null)
            {
                searchbyid.Text = "";
                btnajout.IsEnabled = false;
                btnnouveau.IsEnabled = false;
                txtfname.Text = CurrentRow[1].ToString();
                txtlname.Text = CurrentRow[2].ToString();
                txtadress.Text = CurrentRow[3].ToString();
                //boxville.Text = query.ExecuteReader().ToString();
                con.Open();
                string count = query.ExecuteScalar().ToString();
                boxville.Text = count.ToString();
                con.Close();

            }
            else
            {
                message.Text = "Ce Client N'existe Pas";
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //=======>
        // Supprimer un Row 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(CurrentRow[0].ToString());
            CurrentRow.Delete();
            adapter.Update(ds.Tables["Clientinfo"]);
            MessageBox.Show("Le client dont le id: "+ id + " est supprimé");
            vider();
            btnajout.IsEnabled = true;
            btnupdate.IsEnabled = true;
            btnnouveau.IsEnabled = true;
        }
        

        //=======>
        //afficher Row state 
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            listBox.Items.Clear();
            foreach (DataRow r in ds.Tables["clientinfo"].Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    listBox.Items.Add("id:" + r[0, DataRowVersion.Original] + " " + r.RowState);

                }
                else
                {
                    listBox.Items.Add("id:" + r[0] + " " + r.RowState);
                }
                
            }

        }

      

        private void datagrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        //=======>
        //Datagrid Double click 
        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            btnajout.IsEnabled = false;
         
            btnnouveau.IsEnabled = false;
            var items = datagrid.SelectedItem as DataRowView;
            if (items != null)
            {
                int id = int.Parse(items.Row["id"].ToString());
                SqlCommand query = new SqlCommand("SELECT name FROM ville INNER JOIN clientinfo ON clientinfo.villeId=ville.id where clientinfo.id =" + id, con);
                CurrentRow = ds.Tables["clientinfo"].Rows.Find(id);
                txtfname.Text = CurrentRow[1].ToString();
                txtlname.Text = CurrentRow[2].ToString();
                txtadress.Text = CurrentRow[3].ToString();
                //boxville.Text = query.ExecuteReader().ToString();
                con.Open();
                string count = query.ExecuteScalar().ToString();
                boxville.Text = count.ToString();
                con.Close();


            }
             
        }
        //=======>
        //Modifier le row 
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
           
            if (CurrentRow !=null)
            {
                CurrentRow["firstname"] = txtfname.Text;
                CurrentRow["lastname"] = txtlname.Text;
                CurrentRow["adress"] = txtadress.Text;
                CurrentRow["villeId"] = int.Parse(boxville.SelectedValue.ToString());

                adapter.Update(ds.Tables["clientinfo"]);
                MessageBox.Show("DataBase updated !! ");
                vider();
                btnajout.IsEnabled = true;
                btndelete.IsEnabled = true;
                btnnouveau.IsEnabled = true;

            }
        }
        //=======>
        //ajouter new Row
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            
            DataRow newRow = ds.Tables["clientinfo"].NewRow();
            //newRow["id"] = int.Parse(txtid.Text);
            newRow["firstname"] = txtfname.Text;
            newRow["lastname"] = txtlname.Text;
            newRow["adress"] = txtadress.Text;
            newRow["villeId"] = int.Parse(boxville.SelectedValue.ToString());
            ds.Tables["clientinfo"].Rows.Add(newRow);
            adapter.Update(ds.Tables["clientinfo"]);
            MessageBox.Show("Succesfully Added ");
            vider();
            btndelete.IsEnabled = true;
            btnupdate.IsEnabled = true;

        }

        private void datagrid_CurrentCellChanged(object sender, EventArgs e)
        {
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            vider();
            txtfname.Focus();
            btndelete.IsEnabled = false;
            btnupdate.IsEnabled = false;
            
        }

        private void searchbyid_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void searchbycity_Click(object sender, RoutedEventArgs e)
        {
            retour.IsEnabled = true;
            string content = boxsearchc.Text;
            SqlCommand searchcity = new SqlCommand("Select * from clientinfo inner join ville ON clientinfo.villeId= ville.id  Where ville.name= '"+content+"' ", con);
            con.Open();
           
            SqlDataReader myReader = searchcity.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(myReader);
            datagrid.ItemsSource = dt.DefaultView;
            
            con.Close();



        }

        private void boxsearchc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void retour_Click(object sender, RoutedEventArgs e)
        {
            GetClient();
            boxsearchc.Text = "";
        }
    }
}
