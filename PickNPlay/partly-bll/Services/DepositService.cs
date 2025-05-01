using AutoMapper;
using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_bll.Models.Deposit;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Repositories;

namespace PickNPlay.picknplay_bll.Services
{
    public class DepositService
    {
        private readonly IDepositRepository _depositRepository;
        private readonly IMapper mapper;

        public DepositService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _depositRepository = unitOfWork.DepositRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DepositGet>> GetAllAsync()
        {
            var deposits = await _depositRepository.GetAllAsync();
            return deposits.Select(mapper.Map<DepositGet>);
        }

        public async Task<DepositGet> GetByIdAsync(int id)
        {
            var deposit = await _depositRepository.GetByIdAsync(id);
            return mapper.Map<DepositGet>(deposit);
        }

        public async Task AddAsync(DepositPost depositDto)
        {
            var deposit = mapper.Map<Deposit>(depositDto);
            await _depositRepository.AddAsync(deposit);
        }

        public async Task UpdateAsync(int id, DepositUpdate depositDto)
        {
            var existingDeposit = await _depositRepository.GetByIdAsync(id);
            if (existingDeposit == null)
            {
                // Обработка ошибки: категория не найдена
                return;
            }

            mapper.Map(depositDto, existingDeposit);
            await _depositRepository.UpdateAsync(existingDeposit);
        }

        public async Task DeleteAsync(int id)
        {
            await _depositRepository.DeleteAsync(id);
        }

        public async Task<bool> TransferMoney(string sessionId)
        {
            return await _depositRepository.TransferMoney(sessionId);
        }
    }
}
