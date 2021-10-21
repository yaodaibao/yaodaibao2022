namespace ydb.Domain
{
    /// <summary>
    /// 统一回复格式，结果默认是true
    /// </summary>
    public class ResponseModel
    {
        public bool Result { get; set; } = true;
        public string Description { get; set; } = "";
        public string DataRow { get; set; } = "";
        public string DataRows { get; set; } = "";
    }
}
