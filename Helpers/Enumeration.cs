using System.ComponentModel;

namespace My.Web.Api.Project.Helpers
{
    public enum GenderType
    {
        None = 0,
        Male = 1,
        Female = 2,
    }

    public enum DbActionResult
    {
        Successful = 0,
        UnknownError = 1,
        DatabaseError = 2,
        NoItemFound = 3,
        AlreadyExists = 4,
    }

    public enum SortingOrder
    {
        [Description("ASC")]
        ASC,
        [Description("DESC")]
        DESC
    }
}