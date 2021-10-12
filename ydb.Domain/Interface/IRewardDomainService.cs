using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ydb.Domain
{
    public interface IRewardDomainService
    {
        public Task<ResponseModel> GetReward(string id, string startMonth, string endMonth);

        public Task<ResponseModel> GetTotal(string id);
        public Task<ResponseModel> GetSalesHospitals(string id, string productID, string startMonth, string endMonth);
        public Task<ResponseModel> GetSaleProducts(string id, string startMonth = "", string endMonth = "",string product="");
    }
}