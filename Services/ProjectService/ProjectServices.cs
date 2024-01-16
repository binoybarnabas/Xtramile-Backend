using XtramileBackend.Models.EntityModels;
using XtramileBackend.UnitOfWork;

namespace XtramileBackend.Services.ProjectService
{
    public class ProjectServices : IProjectServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProjectServices(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TBL_PROJECT> GetAllProjects()
        {
            var ProjectData = _unitOfWork.ProjectRepository.GetAll();
            return ProjectData;
        }

        public void AddProject(TBL_PROJECT project) { 
            _unitOfWork.ProjectRepository.Add(project);
            _unitOfWork.Complete();
        }
    }
}
