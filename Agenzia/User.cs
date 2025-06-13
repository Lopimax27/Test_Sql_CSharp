using MySql.Data.MySqlClient;

public class User
{
    private string _username;
    private MySqlConnection _connection;
    private int _userId;

    public User(MySqlConnection connection, int userId)
    {
        _connection = connection;
        _userId = userId;
    }

    // PROPERTY SALDO READ-ONLY - Si carica dal database ogni volta
    public float Saldo
    {
        get 
        { 
            return GetSaldoFromDatabase(); 
        }
        // NESSUN SET - SOLO LETTURA!
    }

    // Metodo privato per caricare il saldo dal database
    private float GetSaldoFromDatabase()
    {
        try
        {
            string sql = "SELECT saldo FROM utente WHERE utente_id = @userId";
            MySqlCommand cmd = new MySqlCommand(sql, _connection);
            cmd.Parameters.AddWithValue("@userId", _userId);
            
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                return Convert.ToSingle(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore lettura saldo: {ex.Message}");
        }
        return 0.0f;
    }

    // Metodo separato per aggiornare il saldo (solo se necessario)
    private bool AggiornaSaldo(float nuovoSaldo)
    {
        try
        {
            string sql = "UPDATE utente SET saldo = @saldo WHERE utente_id = @userId";
            MySqlCommand cmd = new MySqlCommand(sql, _connection);
            cmd.Parameters.AddWithValue("@saldo", nuovoSaldo);
            cmd.Parameters.AddWithValue("@userId", _userId);
            
            return cmd.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore aggiornamento saldo: {ex.Message}");
            return false;
        }
    }

    // Metodo per diminuire il saldo (per acquisti)
    public bool DiminuisciSaldo(float importo)
    {
        float saldoAttuale = Saldo;
        if (saldoAttuale >= importo)
        {
            return AggiornaSaldo(saldoAttuale - importo);
        }
        else
        {
            Console.WriteLine("Saldo insufficiente!");
            return false;
        }
    }
}