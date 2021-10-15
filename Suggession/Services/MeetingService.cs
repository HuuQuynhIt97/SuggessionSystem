using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Suggession.Services
{
    public interface IMeetingService: IServiceBase<PIC, PICDto>
    {
        Task<object> GetAllKPI();
        Task<object> GetAllKPIByPicAndLevel(int levelId , int PicId);
        Task<ChartDto> GetChart(int kpiId);
        Task<ChartDtoDateTime> GetChartWithDateTime(int kpiId, DateTime time);
        Task<ChartDto> GetDataTable(int kpiId);
    }
    public class MeetingService : ServiceBase<PIC, PICDto>, IMeetingService
    {
        private readonly IRepositoryBase<PIC> _repo;
        private readonly IRepositoryBase<Account> _repoAc;
        private readonly IRepositoryBase<KPINew> _repoKPINew;
        private readonly IRepositoryBase<Do> _repoDo;
        private readonly IRepositoryBase<Result> _repoResult;
        private readonly IRepositoryBase<ActionStatus> _repoAcs;
        private readonly IRepositoryBase<OC> _repoOC;
        private readonly IRepositoryBase<Types> _repoType;
        private readonly IRepositoryBase<Policy> _repoPo;
        private readonly IRepositoryBase<Target> _repoTarget;
        private readonly IRepositoryBase<TargetYTD> _repoTargetYTD;
        private readonly IRepositoryBase<Models.Action> _repoAction;
        private readonly IRepositoryBase<Models.Status> _repoStatus;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public MeetingService(
            IRepositoryBase<PIC> repo, 
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<Types> repoType,
            IRepositoryBase<Policy> repoPo,
            IRepositoryBase<Target> repoTarget,
            IRepositoryBase<TargetYTD> repoTargetYTD,
            IRepositoryBase<ActionStatus> repoAcs,
            IRepositoryBase<Account> repoAc,
            IRepositoryBase<KPINew> repoKPINew, 
            IRepositoryBase<Do> repoDo, 
            IRepositoryBase<Result> repoResult, 
            IRepositoryBase<Models.Action> repoAction, 
            IRepositoryBase<Models.Status> repoStatus,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoOC = repoOC;
            _repoAc = repoAc;
            _repoAcs = repoAcs;
            _repoPo = repoPo;
            _repoType = repoType;
            _repoTarget = repoTarget;
            _repoTargetYTD = repoTargetYTD;
            _repoKPINew = repoKPINew;
            _repoDo = repoDo;
            _repoResult = repoResult;
            _repoAction = repoAction;
            _repoStatus = repoStatus;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }


        public async Task<object> GetAllKPIByPicAndLevel(int levelId, int picId)
        {
            throw new NotImplementedException();
        }
        public async Task<object> GetAllKPI()
        {
            throw new NotImplementedException();
        }

       

        public async Task<ChartDto> GetChart(int kpiId)
        {
            List<string> listLabels = new List<string>();
            var dataTable = new List<UpdatePDCADto>();
            var data = _repoTarget.FindAll(x => x.KPIId == kpiId).ToList();
            var listLabel = data.OrderBy(x => x.TargetTime.Date.Month).Select(x => x.TargetTime.Date.Month).ToArray();
            foreach (var a in listLabel)
            {
                switch (a)
                {
                    case 1:
                        listLabels.Add("Jan");
                        break;
                    case 2:
                        listLabels.Add("Feb"); break;
                    case 3:
                        listLabels.Add("Mar"); break;
                    case 4:
                        listLabels.Add("Apr"); break;
                    case 5:
                        listLabels.Add("May");
                        break;
                    case 6:
                        listLabels.Add("Jun"); break;
                    case 7:
                        listLabels.Add("Jul"); break;
                    case 8:
                        listLabels.Add("Aug"); break;
                    case 9:
                        listLabels.Add("Sep");
                        break;
                    case 10:
                        listLabels.Add("Oct"); break;
                    case 11:
                        listLabels.Add("Nov"); break;
                    case 12:
                        listLabels.Add("Dec"); break;
                }
            }
            var listTarget = data.OrderBy(x => x.TargetTime.Date.Month).Select(x => x.Value).ToArray();
            var listPerfomance = data.OrderBy(x => x.TargetTime.Date.Month).Select(x => x.Performance).ToArray();
            var YTD = _repoTarget.FindAll().FirstOrDefault(x => x.KPIId == kpiId).YTD;
            foreach (var item in listLabel)
            {
                var model = from a in _repoAction.FindAll(x => x.KPIId == kpiId && x.CreatedTime.Month == item)
                            join b in _repoDo.FindAll() on a.Id equals b.ActionId into ab
                            from sub in ab.DefaultIfEmpty()
                            select new UpdatePDCADto
                            {
                                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item),
                                ActionId = a.Id,
                                DoId = sub == null ? 0 : sub.Id,
                                Content = a.Content,
                                DoContent = sub == null ? "" : sub.Content,
                                Achievement = sub == null ? "" : sub.Achievement,
                                Deadline = a.Deadline.HasValue ? a.Deadline.Value.ToString("MM/dd/yyyy") : "",
                                StatusId = a.StatusId,
                                StatusName = _repoStatus.FindAll().FirstOrDefault(x => x.Id == a.StatusId).Name.Trim(),
                                CContent = _repoResult.FindAll().FirstOrDefault(x => x.KPIId == kpiId).Content.Trim(),
                                Target = a.Target
                            };

                var datas = model.ToList();
                
                dataTable.AddRange(model);
            }
            return new ChartDto
            {
                labels = listLabels.ToArray(),
                perfomances = listPerfomance,
                targets = listTarget,
                YTD = YTD,
                DataTable = dataTable
            };
        }

        public Task<ChartDto> GetDataTable(int kpiId)
        {
            throw new NotImplementedException();
        }

        public async Task<ChartDtoDateTime> GetChartWithDateTime(int kpiId, DateTime currentTime)
        {
            throw new NotImplementedException();
        }
    }
}
