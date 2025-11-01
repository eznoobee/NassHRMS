namespace HRMS.Domain.Enums
{
    public static class AllowanceRate
    {
        public const decimal NoDegree = 0.05m;
        public const decimal HighSchool = 0.10m;
        public const decimal Bachelor = 0.20m;
        public const decimal Master = 0.30m;
        public const decimal PhD = 0.40m;

        public static decimal GetRate(DegreeType degree) => degree switch
        {
            DegreeType.NoDegree => NoDegree,
            DegreeType.HighSchool => HighSchool,
            DegreeType.Bachelor => Bachelor,
            DegreeType.Master => Master,
            DegreeType.PhD => PhD,
            _ => 0m
        };
    }
}
