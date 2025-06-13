using System;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

public class Program
{
    public static void Main()
    {
        string connStr = $"server=localhost;user=root;database=agenzia;port=3306;password=1234";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nMenù");
                Console.WriteLine("[1] Registrazione");
                Console.WriteLine("[2] Login");
                Console.WriteLine("[0] Esci");
                Console.Write("Scelta: ");
                int menuAction = int.Parse(Console.ReadLine());


                switch (menuAction)
                {
                    case 1:
                        Console.Write("Inserisci nome utente: ");
                        string username = Console.ReadLine();
                        Console.Write("Inserisci password: ");
                        string password = Console.ReadLine();
                        UserRegister(conn, username, password);
                        break;

                    case 2:

                        Console.Write("Inserisci nome utente: ");
                        username = Console.ReadLine();
                        Console.Write("Inserisci password: ");
                        password = Console.ReadLine();

                        Admin admin = new Admin();
                        User user = new User();
                        bool isAdmin = AdminLogin(admin, username, password);
                        bool isUser = UserLogin(conn, username, password);

                        if (isAdmin)
                        {
                            AdminMenu(admin, conn);
                        }
                        else if (isUser)
                        {

                            UserMenu(user, conn);
                        }
                        break;

                    case 0:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Scelta non valida.");
                        break;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        conn.Close();
        Console.WriteLine("Done.");
    }

    public static void AdminMenu(Admin admin, MySqlConnection conn)
    {

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nMenù admin");
            Console.WriteLine("[1] Aggiungi destinazione o attrazione");
            Console.WriteLine("[2] Rendi una destinazione non disponibile");
            Console.WriteLine("[3] Visualizza tutte le destinazioni disponibili");
            Console.WriteLine("[0] Esci");
            Console.Write("Scelta: ");
            int menuAction = int.Parse(Console.ReadLine());

            switch (menuAction)
            {
                case 1:
                    admin.AggiungiDestinazione(conn);
                    break;

                case 2:
                    admin.ModificaDisponibilita(conn);
                    break;

                case 3:
                    admin.StampaInv(conn);
                    break;

                case 0:
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }
        }

    }

    public static void UserMenu(User user, MySqlConnection conn)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nMenù user");
            Console.WriteLine("[1] Aggiungi al carrello");
            Console.WriteLine("[2] Rimuovi dal carrello");
            Console.WriteLine("[3] Concludi ordine");
            Console.WriteLine("[0] Esci");
            Console.Write("Scelta: ");
            int menuAction = int.Parse(Console.ReadLine());

            switch (menuAction)
            {
                case 1:
                    ;
                    break;

                case 2:
                    
                    break;

                case 3:
                    
                    break;

                case 0:
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }
        }
    }

    public static bool AdminLogin(Admin admin, string username, string password)
    {
        if (username == admin.Username && password == admin.Password)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool UserLogin(MySqlConnection conn, string username, string password)
    {
        string sqlUser = "Select u.username, u.password_hash from utente_password u where u.username=@username and u.password_hash=@password_u;";
        MySqlCommand cmdUser = new MySqlCommand(sqlUser, conn);
        cmdUser.Parameters.AddWithValue("@username", username);
        cmdUser.Parameters.AddWithValue("@password_u", password);
        MySqlDataReader rdrUser = cmdUser.ExecuteReader();

        if (rdrUser.Read())
        {
            rdrUser.Close();
            return true;
        }
        else
        {
            rdrUser.Close();
            return false;
        }
    }

    public static void UserRegister(MySqlConnection conn, string username, string password)
    {
        Console.Write("Inserisci nome: ");
        string nome = Console.ReadLine();
        Console.Write("Inserisci cognome: ");
        string cognome = Console.ReadLine();
        Console.Write("Inserisci email: ");
        string email = Console.ReadLine();
        Console.Write("Inserisci telefono: ");
        string telefono = Console.ReadLine();
        Console.Write("Inserisci indirizzo: ");
        string indirizzo = Console.ReadLine();
        Console.Write("Inserisci codice postale: ");
        string postalCode = Console.ReadLine();
        Console.Write("Inserisci città: ");
        string citta = Console.ReadLine();
        Console.Write("Inserisci il saldo: ");
        float saldo = float.Parse(Console.ReadLine());

        string sqlUser = "Select utente_id from utente where email=@email and telefono=@telefono;";
        MySqlCommand cmdUser = new MySqlCommand(sqlUser, conn);
        cmdUser.Parameters.AddWithValue("@email", email);
        cmdUser.Parameters.AddWithValue("@telefono", telefono);
        MySqlDataReader rdrUser = cmdUser.ExecuteReader();

        int userId = 0;
        if (rdrUser.Read())
        {
            rdrUser.Close();
            Console.WriteLine("Utente gia esistente.");
            return;
        }

        rdrUser.Close();

        string sqlCitta = "Select citta_id from citta where citta=@citta;";
        MySqlCommand cmdCitta = new MySqlCommand(sqlCitta, conn);
        cmdCitta.Parameters.AddWithValue("@citta", citta);
        MySqlDataReader rdrCitta = cmdCitta.ExecuteReader();
        int cittaId = 0;
        if (!rdrCitta.Read())
        {

            rdrCitta.Close();
            string sqlCitta2 = "Insert into citta(citta) values (@citta)";
            MySqlCommand cmdCitta2 = new MySqlCommand(sqlCitta2, conn);
            cmdCitta2.Parameters.AddWithValue("@citta", citta);
            cmdCitta2.ExecuteNonQuery();
            cittaId = (int)cmdCitta2.LastInsertedId;
        }
        else
        { 
            cittaId = (int)rdrCitta[0];
            rdrCitta.Close();
        }

        string sqlInd = "Select indirizzo_id from indirizzo where indirizzo=@indirizzo and postal_code=@postal_code";
        MySqlCommand cmdInd = new MySqlCommand(sqlInd, conn);
        cmdInd.Parameters.AddWithValue("@indirizzo", indirizzo);
        cmdInd.Parameters.AddWithValue("@postal_code", postalCode);
        MySqlDataReader rdrInd = cmdInd.ExecuteReader();
        int indId = 0;
        if (!rdrInd.Read())
        {
            rdrInd.Close();
            string sqlInd2 = "Insert into indirizzo(indirizzo, citta_id,postal_code) values (@indirizzo, @citta_id, @postal_code)";
            MySqlCommand cmdInd2 = new MySqlCommand(sqlInd2, conn);
            cmdInd2.Parameters.AddWithValue("@indirizzo", indirizzo);
            cmdInd2.Parameters.AddWithValue("@postal_code", postalCode);
            cmdInd2.Parameters.AddWithValue("@citta_id", cittaId);
            cmdInd2.ExecuteNonQuery();
            indId = (int)cmdInd2.LastInsertedId;
        }
        else
        { 
            indId = (int)rdrInd[0]; 
            rdrInd.Close();
        }



        string sqlUser2 = "Insert into utente (nome, cognome, email, telefono, indirizzo_id,saldo) values(@nome, @cognome, @email, @telefono, @indirizzo_id,@saldo)";
        MySqlCommand cmdUser2 = new MySqlCommand(sqlUser2, conn);
        cmdUser2.Parameters.AddWithValue("@nome", nome);
        cmdUser2.Parameters.AddWithValue("@cognome", cognome);
        cmdUser2.Parameters.AddWithValue("@email", email);
        cmdUser2.Parameters.AddWithValue("@telefono", telefono);
        cmdUser2.Parameters.AddWithValue("@indirizzo_id", indId);
        cmdUser2.Parameters.AddWithValue("@saldo", saldo);
        cmdUser2.ExecuteNonQuery();

        userId = (int)cmdUser2.LastInsertedId;

        sqlUser2 = "Insert into utente_password(utente_id,username,password_hash) values (@utente_id,@username,@password_u)";
        cmdUser2 = new MySqlCommand(sqlUser2, conn);
        cmdUser2.Parameters.AddWithValue("@utente_id", userId);
        cmdUser2.Parameters.AddWithValue("@username", username);
        cmdUser2.Parameters.AddWithValue("@password_u", password);
        cmdUser2.ExecuteNonQuery();
        
    }
}