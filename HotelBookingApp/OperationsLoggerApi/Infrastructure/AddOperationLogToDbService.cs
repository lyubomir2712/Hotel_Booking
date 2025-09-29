using Microsoft.EntityFrameworkCore;
using OperationsLoggerApi.Data.Models;
using OperationsLoggerApi.Data.SeedOfWork.SeedWork;
using OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;
using OperationsLoggerApi.Interfaces;
using AutoMapper;

namespace OperationsLoggerApi.Infrastructure
{
    public class AddOperationLogToDbService : IAddOperationLogToDbService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddOperationLogToDbService> _logger;
        private readonly IMapper _mapper;

        public AddOperationLogToDbService(IUnitOfWork unitOfWork, ILogger<AddOperationLogToDbService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<int> AddOperationLogToDbAsync(OpsLogEntryDto entry, CancellationToken ct = default)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            
            var entity = _mapper.Map<OpsLogEntryModel>(entry);
            await _unitOfWork.Repository<OpsLogEntryModel>().AddAsync(entity, ct);

            try
            {
                await _unitOfWork.SaveChangesAsync(ct);
                _logger.LogInformation(
                    "Saved OpsLogEntry {EventId} for {EntityType}:{EntityId}",
                    entry.EventId,
                    entry.EntityType,
                    entry.EntityId);
                return entry.Id;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save OpsLogEntry {EventId}", entry.EventId);
                throw;
            }
        }
    }
}