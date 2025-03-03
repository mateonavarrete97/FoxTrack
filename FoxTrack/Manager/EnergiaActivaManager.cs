using FoxTrack.DTO;

public class EnergiaActivaManager
{
    private EnergiaActivaDAO energiaActivaDAO;

    public EnergiaActivaManager(EnergiaActivaDAO energiaActivaDAO)
    {
        this.energiaActivaDAO = energiaActivaDAO;
    }

    public EnergiaActivaDTO CalcularEnergiaActiva()
    {
        decimal ea = energiaActivaDAO.GetCantidadEnergiaActiva();
        decimal cu = energiaActivaDAO.GetTarifaCU();
        decimal montoEA = ea * cu;

        return new EnergiaActivaDTO
        {
            EA = ea,
            CU = cu,
            MontoEA = montoEA
        };
    }
}

