using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.PriorityService
{
    public class PriorityServices : IPriorityServices
    {

        private readonly IUnitOfWork _unitOfWork;
        public PriorityServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TBL_PRIORITY> GetPriorities()
        {
            var PriorityData = _unitOfWork.PriorityRepository.GetAll();
            return PriorityData;
        }

        public void AddPriority(TBL_PRIORITY priority)
        {
            _unitOfWork.PriorityRepository.Add(priority);
            _unitOfWork.Complete();
        }
    }
}
