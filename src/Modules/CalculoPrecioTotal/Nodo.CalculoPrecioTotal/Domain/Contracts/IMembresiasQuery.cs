using Nodo.SharedKernel;

namespace Nodo.CalculoPrecioTotal.Domain.Contracts;

public record PlanInfo(string PlanCodigo, int HorasIncluidas, decimal DescuentoPorcentaje);

public interface IMembresiasQuery
{
    Task<PlanInfo?> ObtenerPlanDelMiembroAsync(Guid clienteId, CancellationToken ct = default);
}
