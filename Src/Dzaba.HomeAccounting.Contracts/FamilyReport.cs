namespace Dzaba.HomeAccounting.Contracts
{
    public class FamilyReport
    {
        public int FamilyId { get; set; }
        public string FamilyName { get; set; }
        public MonthReport[] Reports { get; set; }
    }
}
