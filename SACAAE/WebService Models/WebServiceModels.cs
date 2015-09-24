namespace SACAAE.WebService_Models
{
    public class GroupScheduleWSModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Day { get; set; }
    }

    public class NameWSModel
    {
        public string Name { get; set; }
    }

    public class PeriodInformationWSModel
    {
        public int Level { get; set; }
        public int Course { get; set; }
        public int Number { get; set; }
        public int Day { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int Professor { get; set; }
    }

    public class PeriodWSModel
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public class BasicInfoWSModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class ProjectCommissionWSModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Day { get; set; }
    }

    public class GroupWSModel
    {
        public int Number { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Day { get; set; }
        public string Code { get; set; }
    }
}