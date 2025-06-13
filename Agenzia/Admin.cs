using MySql.Data.MySqlClient;
using System;

public sealed class Admin
{
    private string _username = "admin";
    public string Username
    {
        get { return _username; }
    }

    private string _password = "admin";
    public string Password
    {
        get { return _password; }
    }

    public void AggiungiDestinazione(MySqlConnection conn)
    {

        Console.Write("Inserisci la destinazione: ");
        string citta = Console.ReadLine();

        string sqlDestinazione = "select destinazione_id from destinazione where citta = @citta";
        MySqlCommand cmdDest = new MySqlCommand(sqlDestinazione, conn);
        cmdDest.Parameters.AddWithValue("@citta", citta);
        MySqlDataReader rdrDest = cmdDest.ExecuteReader();
        int destinazioneId = 0;
        if (rdrDest.Read())
        {
            destinazioneId = (int)rdrDest[0];
            rdrDest.Close();
            Console.WriteLine("La destinazione è già presente vuoi inserire delle attrazioni ?");
            bool risposta = Console.ReadLine()?.ToLower() == "si";
            if (risposta)
            {
                AggiungiAttrazione(conn, destinazioneId);
            }
            return;
        }
        rdrDest.Close();

        Console.Write("Inserisci il paese della destinazione: ");
        string paese = Console.ReadLine();
        Console.Write("Inserisi la descrizione della destinazione: ");
        string descrizione_destinazione = Console.ReadLine();
        Console.Write("Inserisci prezzo: ");
        decimal prezzo = decimal.Parse(Console.ReadLine());

        int cittaId = 0;
        int paeseId = 0;
        do
        {
            string sqlCitta = "Select citta_id from citta where citta=@citta";
            MySqlCommand cmdCitta = new MySqlCommand(sqlCitta, conn);
            cmdCitta.Parameters.AddWithValue("@citta", citta);
            MySqlDataReader rdrCitta = cmdCitta.ExecuteReader();

            if (rdrCitta.Read())
            {
                cittaId = (int)rdrCitta[0];
                rdrCitta.Close();
                break;
            }
            else
            {
                rdrCitta.Close();

                string sqlPaese = "Select paese_id from paese where paese=@paese";
                MySqlCommand cmdPaese = new MySqlCommand(sqlPaese, conn);
                cmdPaese.Parameters.AddWithValue("@paese", paese);
                MySqlDataReader rdrPaese = cmdPaese.ExecuteReader();
                if (rdrPaese.Read())
                {
                    paeseId = (int)rdrPaese[0];
                    rdrPaese.Close();
                }
                else
                {
                    rdrPaese.Close();
                    sqlPaese = "Insert into paese(paese) values (@paese)";
                    cmdPaese = new MySqlCommand(sqlPaese, conn);
                    cmdPaese.Parameters.AddWithValue("@paese", paese);
                    cmdPaese.ExecuteNonQuery();
                    paeseId = (int)cmdPaese.LastInsertedId;
                }

                sqlCitta = "Insert into citta(citta,paese_id) values (@citta,@paeseId)";
                cmdCitta = new MySqlCommand(sqlCitta, conn);
                cmdCitta.Parameters.AddWithValue("@citta", citta);
                cmdCitta.Parameters.AddWithValue("@paeseId", paeseId);
                cmdCitta.ExecuteNonQuery();
            }
        } while (true);

        string sqlGetCittaPaeseId = "SELECT paese_id FROM citta WHERE citta_id = @cittaId";
        MySqlCommand cmdGetPaeseId = new MySqlCommand(sqlGetCittaPaeseId, conn);
        cmdGetPaeseId.Parameters.AddWithValue("@cittaId", cittaId);
        int cittaPaeseId = (int)cmdGetPaeseId.ExecuteScalar();

        string sql = "Insert into destinazione(paese_id,citta,citta_id, descrizione_destinazione,prezzo) values (@paeseId,@citta,@cittaId, @descrizione,@prezzo);";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@cittaId", cittaId);
        cmd.Parameters.AddWithValue("@prezzo", prezzo);
        cmd.Parameters.AddWithValue("@descrizione", descrizione_destinazione);
        cmd.Parameters.AddWithValue("@paeseId", cittaPaeseId);
        cmd.Parameters.AddWithValue("@citta", citta);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Destinazione aggiunta con successo.");
    }

    public void ModificaDisponibilita(MySqlConnection conn)
    {
        Console.Write("Inserisci la destinazione: ");
        string citta = Console.ReadLine();

        string sqlDestinazione = "select destinazione_id from destinazione where citta = @citta";
        MySqlCommand cmdDest = new MySqlCommand(sqlDestinazione, conn);
        cmdDest.Parameters.AddWithValue("@citta", citta);
        MySqlDataReader rdrDest = cmdDest.ExecuteReader();
        if (!rdrDest.Read())
        {
            Console.WriteLine("Destinazione non trovata.");
            rdrDest.Close();
        }
        else
        {
            rdrDest.Close();
            sqlDestinazione = "update destinazione set disponibile=@disponibilita";
            Console.WriteLine("Vuoi rendere la destinazione non disponibile ?");
            bool risposta = Console.ReadLine()?.ToLower() == "no";
            cmdDest = new MySqlCommand(sqlDestinazione, conn);
            cmdDest.Parameters.AddWithValue("@disponibilita", risposta);
            cmdDest.ExecuteNonQuery();
            Console.WriteLine("Destinazione non più disponibile eliminato.");
        }

    }

    public void StampaInv(MySqlConnection conn)
    {

        string sql = "Select destinazione.citta,paese.paese, destinazione.descrizione_destinazionefrom destinazione join citta on destinazione.citta_id=citta.citta_id join paese on citta.paese_id=paese.paese_id where destinazione.disponibile=true;";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            Console.WriteLine(rdr[0] + " -- " + rdr[1] + " -- " + rdr[2]);
        }
        rdr.Close();
    }

    private void AggiungiAttrazione(MySqlConnection conn, int destID)
    { 

    }
}