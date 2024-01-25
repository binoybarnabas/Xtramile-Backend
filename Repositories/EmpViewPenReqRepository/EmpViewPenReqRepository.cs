using XtramileBackend.Data;
using XtramileBackend.Models.APIModels;

namespace XtramileBackend.Repositories.EmpViewPenReqRepository
{
    public class EmpViewPenReqRepository : Repository<PendingRequetsViewEmployee>, IEmpViewPenReqRepository        
    {
        public EmpViewPenReqRepository(AppDBContext dBContext) : base(dBContext) { }
    }
}
