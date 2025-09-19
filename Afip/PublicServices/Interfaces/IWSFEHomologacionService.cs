using WSFEHomologacion;

namespace Afip.PublicServices.Interfaces
{
    public interface IWSFEHomologacionService
    {
        Task<FECAEResponse> SolicitarCAEAsync(FECAECabRequest fECAECab, FECAEDetRequest detalles);
        Task<int> GetLastCbteAsync(int ptoVta, int cbteTipo);
        Task<FECompConsultaResponse> ConsultarComprobanteAsync(FECompConsultaReq fEComp);
    }
}
