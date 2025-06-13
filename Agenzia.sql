/*Traccia : creare la struttura db e un'app funzionante tramite console per un'agenzia di viaggi.
Gestire l'admin e l'utente con la possibilita di aggiungere luoghi/attrazioni.... e chi piu ne ha piu ne metta! 
Caricare il progetto su github e caricare tutte le query sql utilizzare per la creazione del db
*/

Create DATABASE agenzia;


Create table utente_password(
utente_id int unique,
username varchar(100) not null,
password_hash varchar(255) not null,
foreign key (utente_id) references utente(utente_id)
);


Create Table utente(
utente_id int Primary Key auto_increment,
nome varchar(100),
cognome varchar(100),
email varchar(100) UNIQUE NOT NULL,
telefono varchar(15) UNIQUE NOT NULL,
indirizzo_id int,
saldo decimal(10,2),
Foreign key (indirizzo_id) references indirizzo(indirizzo_id)
);

Create table indirizzo(
indirizzo_id int primary key auto_increment,
indirizzo varchar(100) unique Not null,
citta_id int unique,
postal_code varchar(10) Not null,
foreign key (citta_id) references citta(citta_id)
);

Create table citta(
citta_id int primary key auto_increment,
citta varchar(50) unique not null,
paese_id int,
foreign key (paese_id) references paese(paese_id)
);

Create table paese(
    paese_id int primary key auto_increment,
    paese varchar(100) not null
);

Create table prenotazione(
prenotazione_id int primary key auto_increment,
utente_id int,
destinazione_id int,
prenotazione_data date not null,
foreign key (utente_id) references utente(utente_id),
foreign key (destinazione_id) references destinazione(destinazione_id),
unique key unica_utente_data (utente_id,prenotazione_data)
);

Create table destinazione(
destinazione_id int primary key auto_increment,
paese_id int,
citta_id int,
descrizione_destinazione varchar(255),
prezzo decimal(10,2) not null,
disponibile bool default true,
citta varchar(50),
foreign key (citta) references citta(citta),
foreign key (citta_id) references citta(citta_id),
foreign key (paese_id) references citta(paese_id)
);




Create table attrazione(
attrazione_id int primary key auto_increment,
destinazione_id int,
attrazione_nome varchar(100) not null,
descrizione_attrazione varchar(255),
prezzo_attrazione decimal(10,2) not null,
foreign key (destinazione_id) references destinazione(destinazione_id)
);


Select*from destinazione;
Select*from citta;
Select*from paese