namespace MikeRostoli.Classes
{
    public class Staff
    {
        public string Name { get; set; }
        public List<UnavailableShift> UnavailableShifts { get; set; } = new List<UnavailableShift>();
        public List<Shift> AssignedShifts { get; set; } = new List<Shift>();
    }

    public class UnavailableShift
    {
        public string Day { get; set; }
        public string Time { get; set; }
    }

    public class Shift
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public List<Staff> AssignedStaff { get; set; } = new List<Staff>();
    }

    public class StaffRoster
    {
        public Config Config { get; set; }
        public List<Staff> Staff { get; set; }
    }

    public class Config
    {
        public List<string> Days { get; set; }
        public List<string> Times { get; set; }
        public int MinStaffPerShift { get; set; }
    }
}
