﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using System;
using System.Threading.Tasks;
using Suggession.Helpers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ESS_API.Helpers;
using System.Linq;
using Suggession.Constants;
using NetUtility;
using System.IO;
using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;

namespace Suggession.Services
{
    public interface IH1H2Service
    {
      
        Task<object> GetH1H2Data();
        Task<object> GetReportInfo(int accountId);
        Task<Byte[]> ExportExcel(int accountId);
        Task<object> GetAllKPIScoreL0ByPeriod(int period);
    }
    public class H1H2Service : IH1H2Service
    {
        private OperationResult operationResult;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepositoryBase<ToDoList> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IRepositoryBase<Objective> _repoObjective;
        private readonly IRepositoryBase<ResultOfMonth> _repoResultOfMonth;
        private readonly IRepositoryBase<KPIScore> _repoKPIScore;
        private readonly IRepositoryBase<PeriodType> _repoPeriodType;
        private readonly IRepositoryBase<OCAccount> _repoOCAccount;
        private readonly IRepositoryBase<Contribution> _repoContribution;
        private readonly IRepositoryBase<PIC> _repoPIC;
        private readonly IRepositoryBase<OC> _repoOC;
        private readonly IRepositoryBase<Period> _repoPeriod;
        private readonly IRepositoryBase<AttitudeScore> _repoAttitudeScore;
        private readonly IRepositoryBase<SpecialContributionScore> _repoSpecial;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<Models.Comment> _repoComment;
        private readonly MapperConfiguration _configMapper;
        public H1H2Service(
            IRepositoryBase<ToDoList> repo,
            IRepositoryBase<Account> repoAccount,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
            IRepositoryBase<Objective> repoObjective,
            IRepositoryBase<ResultOfMonth> repoResultOfMonth,
            IRepositoryBase<KPIScore> repoKPIScore,
            IRepositoryBase<PeriodType> repoPeriodType,
            IRepositoryBase<OCAccount> repoOCAccount,
            IRepositoryBase<PIC> repoPIC,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<Contribution> repoContribution,
            IRepositoryBase<Period> repoPeriod,
            IRepositoryBase<Models.Comment> repoComment,
            IRepositoryBase<AttitudeScore> repoAttitudeScore,
            IRepositoryBase<SpecialContributionScore> repoSpecial,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoComment = repoComment;
            _repoContribution = repoContribution;
            _repoAccount = repoAccount;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _repoSpecial = repoSpecial;
            _repoObjective = repoObjective;
            _repoResultOfMonth = repoResultOfMonth;
            _repoKPIScore = repoKPIScore;
            _repoPeriodType = repoPeriodType;
            _repoOCAccount = repoOCAccount;
            _repoPIC = repoPIC;
            _repoOC = repoOC;
            _repoPeriod = repoPeriod;
            _repoAttitudeScore = repoAttitudeScore;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configMapper = configMapper;
        }


        public async Task<Byte[]> ExportExcel(int accountId)
        {
            var data = await GetReportInfo2(accountId);
            var H1 = data.H1;
            var H2 = data.H2;
            var avg = data.avg;
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\excelTemplate\\H1H2REPORT.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];
            ws.Name = "H1,H2 Report ";
            ws.Cells["J2"].PutValue("Dept. " + H1.OC);
            ws.Cells["L2"].PutValue("Name " + H1.FullName);
            //H1
            ws.Cells["D5"].PutValue("H1得分:  " + H1.A_total);
            ws.Cells["D7"].PutValue("H1:  " + H1.L0SelfScoreComment);
            ws.Cells["F5"].PutValue(H1.L1Score);
            ws.Cells["G5"].PutValue(H1.L2Score);
            ws.Cells["H5"].PutValue(H1.FLScore);
            ws.Cells["D8"].PutValue("Q1 KPI: " + H1.L1Q1Comment);
            ws.Cells["D9"].PutValue("Q2 KPI: " + H1.L1Q2Comment);
            ws.Cells["D10"].PutValue("H1 Attitude: " + H1.L1HComment);

            ws.Cells["D11"].PutValue("Q1 KPI: " + H1.L2Q1Comment);
            ws.Cells["D12"].PutValue("Q2 KPI: " + H1.L2Q2Comment);
            ws.Cells["D13"].PutValue("H1 Attitude: " + H1.L2HComment);

            ws.Cells["D14"].PutValue("H1 Attitude: " + H1.FLHComment);
            ws.Cells["D17"].PutValue("H1得分: " + H1.B_total);

            ws.Cells["F17"].PutValue(H1.selfScore);
            ws.Cells["H17"].PutValue(H1.L1);
            ws.Cells["I17"].PutValue(H1.L2);
            ws.Cells["J17"].PutValue(H1.Smart);

            ws.Cells["F18"].PutValue(H1.B_selfScore);
            ws.Cells["H18"].PutValue(H1.B_L1);
            ws.Cells["I18"].PutValue(H1.B_L2);
            ws.Cells["J18"].PutValue(H1.B_Smart);
            ws.Cells["D20"].PutValue("H1: " + H1.C_total);
            ws.Cells["D21"].PutValue("Comment: " + H1.SpecialComment);
            ws.Cells["F20"].PutValue(H1.SpecialScore);
            ws.Cells["E24"].PutValue("H1得分: " + H1.D_total);

            //endH1

            //H2
            ws.Cells["N5"].PutValue("H2得分:  " + H2.A_total);
            ws.Cells["P5"].PutValue(H2.L1Score);
            ws.Cells["Q5"].PutValue(H2.L2Score);
            ws.Cells["R5"].PutValue(H2.FLScore);
            ws.Cells["N7"].PutValue("H2: " + H2.L0SelfScoreComment);
            ws.Cells["N8"].PutValue("Q3 KPI: " + H2.L1Q1Comment);
            ws.Cells["N9"].PutValue("Q4 KPI: " + H2.L1Q2Comment);
            ws.Cells["N10"].PutValue("H2 Attitude: " + H2.L1HComment);

            ws.Cells["N11"].PutValue("Q3 KPI: " + H2.L2Q1Comment);
            ws.Cells["N12"].PutValue("Q4 KPI: " + H2.L2Q2Comment);
            ws.Cells["N13"].PutValue("H1 Attitude: " + H2.L2HComment);

            ws.Cells["N14"].PutValue("H2 Attitude: " + H2.FLHComment);
            ws.Cells["N17"].PutValue("H2得分: " + H2.B_total);

            ws.Cells["P17"].PutValue(H2.selfScore);
            ws.Cells["R17"].PutValue(H2.L1);
            ws.Cells["S17"].PutValue(H2.L2);
            ws.Cells["T17"].PutValue(H2.Smart);

            ws.Cells["P18"].PutValue(H2.B_selfScore);
            ws.Cells["R18"].PutValue(H2.B_L1);
            ws.Cells["S18"].PutValue(H2.B_L2);
            ws.Cells["T18"].PutValue(H2.B_Smart);
            ws.Cells["N20"].PutValue("H2: " + H2.C_total);
            ws.Cells["N21"].PutValue("Comment: " + H2.SpecialComment);
            ws.Cells["P20"].PutValue(H2.SpecialScore);
            ws.Cells["O24"].PutValue("H2得分: " + H2.D_total);

            //endH2
            ws.Cells["E26"].PutValue("年度得分: " + avg);
            var stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            Byte[] bin = stream.ToArray();
            //Lưu file lại
            return bin; ;
        }
        public async Task<DataReportDto> GetReportInfo2(int accountId)
        {
            throw new NotImplementedException();
        }
        public async Task<double> BSelfScore(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 10;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0  && number < 5)
                {
                    result = number * 10 / 5;
                }
                
                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
        public async Task<double> BL1(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 30;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0 && number < 5)
                {
                    result = number * 30 / 5;
                }

                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
        public async Task<double> BSmart(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 20;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0 && number < 5)
                {
                    result = number * 20 / 5;
                }

                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }

        public async Task<double> BL2(double number)
        {
            double result = 0;
            try
            {
                if (number == 5)
                {
                    result = 20;
                }
                if (number == 0)
                {
                    return result;
                }
                if (number > 0 && number < 5)
                {
                    result = number * 20 / 5;
                }

                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }

        public async Task<double> ConvertAtotal(double l1ScoreKPI, double l2ScoreKPI , double FLScore)
        {
             double result = 0;  
            try
            {
                if (FLScore == 0)
                {
                   
                    result = l1ScoreKPI + l2ScoreKPI;
                } else
                {
                    result = (l1ScoreKPI + l2ScoreKPI + FLScore) *20 / 30;
                }
                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }

        public async Task<double> MathL1(double l1Q1Score, double l1Q2Score)
        {
            double result = 0;
            try
            {
                if (l1Q1Score == 0)
                {
                 
                    result = l1Q2Score;
                }
                else
                {
                    result = Math.Round((l1Q1Score + l1Q2Score) / 2, 2); 
                }
                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
        public async Task<double> MathL2(double l2Q1Score, double l2Q2Score)
        {
            double result = 0;
            try
            {
                if (l2Q1Score == 0)
                {

                    result = l2Q2Score;
                }
                else
                {
                    result = Math.Round((l2Q1Score + l2Q2Score) / 2, 2); 
                }
                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
        public async Task<double> MathSmart(double GHRQ1ScoreKPI, double GHRQ2ScoreKPI)
        {
            double result = 0;
            try
            {
                if (GHRQ1ScoreKPI == 0)
                {

                    result = GHRQ2ScoreKPI;
                }
                else
                {
                    result = Math.Round((GHRQ1ScoreKPI + GHRQ2ScoreKPI) / 2, 2);
                }
                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }


        public async Task<object> GetH1H2Data()
        {
            //var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            //int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
            //// tim oc cua usser login
            //var ocuser = await _repoOCAccount.FindAll(x => x.AccountId == accountID).FirstOrDefaultAsync();
         
            //if (ocuser == null) return new List<dynamic>();
            //// Lay tat ca con cua oc
            //var oc = _repoOC.FindAll().AsHierarchy(x => x.Id, y => y.ParentId, ocuser.OCId).ToList();
            //var ocs = oc.Flatten(x => x.ChildNodes).Select(x => x.Entity.Id).ToList();
            //// vao ocUser tim theo ocId list 
            //var accountIds = await _repoOCAccount.FindAll(x => ocs.Contains(x.OCId)).Select(x => x.AccountId).Distinct().ToListAsync();
            //var pics = await _repoPIC.FindAll(x => accountIds.Contains(x.AccountId)).Select(x=>x.AccountId).Distinct().ToListAsync();

            //var model = await _repoOCAccount.FindAll(x => pics.Contains(x.AccountId))
            //    .Select(x => new
            //    {
            //        Id = x.AccountId,
            //        FullName = x.Account.FullName,
            //    }).ToListAsync();

            //return model;
            throw new NotImplementedException();
        }

        public Task<object> GetAllKPIScoreL0ByPeriod(int period)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetReportInfo(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
