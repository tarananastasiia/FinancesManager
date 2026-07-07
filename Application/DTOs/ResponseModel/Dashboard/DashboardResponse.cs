namespace Application.DTOs.ResponseModel.Dashboard
{
    public class DashboardResponse
    {
        public decimal Balance { get; set; }

        public decimal SpentThisMonth { get; set; }

        public int Transactions { get; set; }

        public int Cards { get; set; }

        public List<MonthlySpendingDto> MonthlySpending { get; set; }
        public List<CategorySpendingDto> CategorySpending { get; set; }
    }

    public class CategorySpendingDto
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
    }

    public class MonthlySpendingDto
    {
        public string Month { get; set; }

        public decimal Amount { get; set; }
    }
}
