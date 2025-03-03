using FoxTrack.DTO;

public class ECManager
{
    private ECDAO ecDAO;

    public ECManager(ECDAO ecDAO)
    {
        this.ecDAO = ecDAO;
    }

    public ECDTO CalcularEC()
    {
        decimal ec = ecDAO.GetCantidadEC();
        decimal c = ecDAO.GetTarifaC();
        decimal montoEC = ec * c;

        return new ECDTO
        {
            EC = ec,
            C = c,
            MontoEC = montoEC
        };
    }
}

