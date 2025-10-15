using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerHistoryService.Infrastructure.Persistence
{
    public class CustomerHistoryRepository : ICustomerHistoryRepository
    {
        private readonly CustomerHistoryDbContext _context;

        public CustomerHistoryRepository(CustomerHistoryDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerHistory?> GetByEmailAsync(string email)
        {
            return await _context.Histories
                .Include(h => h.Reservations)
                .Include(h => h.Payments)
                .Include(h => h.Notifications)
                .FirstOrDefaultAsync(h => h.CustomerEmail == email);
        }

        public async Task UpsertAsync(CustomerHistory history)
        {
            var existing = await GetByEmailAsync(history.CustomerEmail);
            if (existing == null)
            {
                _context.Histories.Add(history);
            }
            else
            {
                // Evita duplicação de reservas
                /* Se já existir:
                        Verifica se a reserva já está registrada (evita duplicação)
                        Adiciona apenas se for nova
                        Atualiza os dados principais do histórico (como CustomerEmail, se necessário)
                */
                foreach (var newReservation in history.Reservations)
                {
                    if (!existing.Reservations.Any(r => r.ReservationId == newReservation.ReservationId))
                    {
                        existing.Reservations.Add(newReservation);
                    }
                }

                // Mesma lógica pode ser aplicada para Payments e Notifications se necessário

                _context.Entry(existing).CurrentValues.SetValues(history);
            }

            await _context.SaveChangesAsync();
        }

    }

}
