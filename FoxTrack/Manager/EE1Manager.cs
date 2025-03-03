using FoxTrack.DTO;

public class EE1Manager
{
    private EE1DAO ee1DAO;

    public EE1Manager(EE1DAO ee1DAO)
    {
        this.ee1DAO = ee1DAO;
    }

    public EE1DTO CalcularEE1()
    {
        decimal ee1 = ee1DAO.GetCantidadEE1();
        decimal negCU = ee1DAO.GetNegCU();
        decimal montoEE1 = ee1 * negCU;

        return new EE1DTO
        {
            EE1 = ee1,
            NegCU = negCU,
            MontoEE1 = montoEE1
        };
    }
}

