using ydb.Domain.Models;

namespace ydb.Domain.Interface
{
    public interface IUserProfileService
    {
        public ResponseModel SaveUserProfile(UserProfile userProfile);
        public ResponseModel GetUserProfile(UserProfile userProfile);
        public ResponseModel GetAutoHis(RouteQuery routeQuery);
    }
}
