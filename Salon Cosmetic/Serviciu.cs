public enum Procedura
{
    Coafor,
    Manichiura,
    Pedichiura,
    Machiaj,
    Epilare,
    Masaj
}

public class Serviciu
{
    public string Nume { get; set; }
    public decimal Pret { get; set; }
    public int Durata { get; set; }
    public Procedura ServiciuClient { get; set; }
    public Serviciu(string nume, decimal pret, int durata, Procedura procedura)
    {
        Nume = nume;
        Pret = pret;
        Durata = durata;
        ServiciuClient = procedura;
    }
    public override string ToString()
    {
        return $"Nume: {Nume}, Pret: {Pret}, Durata: {Durata}, Procedura: {ServiciuClient}";
    }
}
